using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Admin.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Role", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string Name { get; set; }
    }
}