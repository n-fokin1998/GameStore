using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Admin.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = _mapper.Map<RoleViewModel, RoleDTO>(model);
            var result = _roleService.Add(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(result.Property ?? nameof(role.Name), result.Message);

            return View(model);
        }

        public ActionResult RoleDetails(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(role);
        }

        [HttpGet]
        public ActionResult DeleteRole(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return RedirectToAction("RoleDetails", "Role", new { id });
            }

            var model = new DeleteViewModel { Id = id };

            return PartialView("DeleteRoleModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeleteRole(DeleteViewModel model)
        {
            var role = _roleService.GetById(model.Id);
            _roleService.Delete(role);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult EditRole(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return RedirectToAction("RoleDetails", "Role", new { id });
            }

            var model = _mapper.Map<RoleDTO, RoleViewModel>(role);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = _mapper.Map<RoleViewModel, RoleDTO>(model);
            var result = _roleService.Update(role);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleDetails", "Role", new { id = model.Id });
            }

            ModelState.AddModelError(result.Property ?? nameof(role.Name), result.Message);

            return View(model);
        }
    }
}