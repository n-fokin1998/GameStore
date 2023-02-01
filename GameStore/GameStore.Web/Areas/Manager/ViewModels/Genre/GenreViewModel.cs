using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Areas.Manager.ViewModels
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string NameRu { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GlobalRes))]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string NameEn { get; set; }

        public int? ParentGenreId { get; set; }
    }
}