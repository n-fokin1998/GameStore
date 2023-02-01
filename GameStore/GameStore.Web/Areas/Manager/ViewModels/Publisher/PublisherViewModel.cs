using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Manager.ViewModels
{
    public class PublisherViewModel
    {
        public int Id { get; set; }

        [Display(Name = "CompanyName", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string CompanyName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string DescriptionRu { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string DescriptionEn { get; set; }

        [Display(Name = "HomePage", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string HomePage { get; set; }
    }
}