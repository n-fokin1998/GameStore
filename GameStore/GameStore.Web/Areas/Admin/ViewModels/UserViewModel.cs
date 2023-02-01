using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(GlobalRes))]
        public int[] Roles { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}