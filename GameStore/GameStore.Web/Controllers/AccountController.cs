using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Account;

namespace GameStore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public ActionResult Register()
        {
            var model = new RegisterViewModel
            {
                RolesList = new MultiSelectList(_roleService.GetList(), nameof(RoleDTO.Id), nameof(RoleDTO.Name))
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RolesList = new MultiSelectList(_roleService.GetList(), nameof(RoleDTO.Id), nameof(RoleDTO.Name));

                return View(model);
            }

            var user = _mapper.Map<RegisterViewModel, UserDTO>(model);
            if (CurrentUser != null && CurrentUser.InRoles("Administrator"))
            {
                AttachRoles(model.Roles, user);
            }

            var result = _userService.Register(user);
            if (result.Succeeded && CurrentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (result.Succeeded && CurrentUser != null)
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            ModelState.AddModelError(result.Property ?? "Email", result.Message);
            model.RolesList = new MultiSelectList(_roleService.GetList(), nameof(RoleDTO.Id), nameof(RoleDTO.Name));

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Auth.Login(model.Email, model.Password, model.IsPersistent);
            if (user != null)
            {
                return RedirectToAction("GetAllGames", "Game");
            }

            ModelState.AddModelError("Email", GlobalRes.IncorrectLoginData);

            return View(model);
        }

        public ActionResult Logout()
        {
            Auth.LogOut();

            return RedirectToAction("GetAllGames", "Game");
        }

        private void AttachRoles(int[] roles, UserDTO user)
        {
            user.Roles = new List<RoleDTO>();
            if (roles == null)
            {
                return;
            }

            foreach (var g in roles)
            {
                var role = _roleService.GetById(g);
                if (role != null)
                {
                    user.Roles.Add(role);
                }
            }
        }
    }
}