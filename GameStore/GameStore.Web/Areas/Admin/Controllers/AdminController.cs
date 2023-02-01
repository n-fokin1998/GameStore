using System.Linq;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.Web.Areas.Admin.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        private const string AdminRole = "Administrator";
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            var users = _userService.GetList();
            var model = new AdminPanelViewModel
            {
                Users = users.Where(u => u.Name != CurrentUser.Name),
                Roles = _roleService.GetList().Where(r => r.Name != AdminRole)
            };

            return View(model);
        }
    }
}