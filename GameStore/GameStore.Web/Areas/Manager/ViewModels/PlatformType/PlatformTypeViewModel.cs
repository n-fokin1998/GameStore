using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Manager.ViewModels
{
    public class PlatformTypeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Type", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string TypeRu { get; set; }

        [Display(Name = "Type", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string TypeEn { get; set; }
    }
}