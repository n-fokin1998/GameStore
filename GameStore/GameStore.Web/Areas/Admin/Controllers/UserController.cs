using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Admin.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public ActionResult UserDetails(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return RedirectToAction("UserDetails", "User", new { id });
            }

            var model = new DeleteViewModel { Id = id };

            return PartialView("DeleteUserModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteViewModel model)
        {
            var user = _userService.GetById(model.Id);
            _userService.Delete(user);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return RedirectToAction("UserDetails", "User", new { id });
            }

            var model = _mapper.Map<UserDTO, UserViewModel>(user);
            model.RolesList = new MultiSelectList(
                _roleService.GetList(),
                nameof(RoleDTO.Id),
                nameof(RoleDTO.Name),
                user.Roles.Select(r => r.Id).ToList());

            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RolesList = new MultiSelectList(
                    _roleService.GetList(),
                    nameof(RoleDTO.Id),
                    nameof(RoleDTO.Name),
                    _userService.GetById(model.Id).Roles.Select(r => r.Id).ToList());

                return View(model);
            }

            var user = _mapper.Map<UserViewModel, UserDTO>(model);
            AttachRoles(model.Roles, user);
            var result = _userService.Update(user);
            if (result.Succeeded)
            {
                return RedirectToAction("UserDetails", "User", new { id = model.Id });
            }

            ModelState.AddModelError(result.Property ?? nameof(user.Name), result.Message);

            return View(model);
        }

        private void AttachRoles(int[] roles, UserDTO user)
        {
            user.Roles = new List<RoleDTO>();
            if (roles == null)
            {
                return;
            }

            foreach (var r in roles)
            {
                var role = _roleService.GetById(r);
                if (role != null)
                {
                    user.Roles.Add(role);
                }
            }
        }
    }
}