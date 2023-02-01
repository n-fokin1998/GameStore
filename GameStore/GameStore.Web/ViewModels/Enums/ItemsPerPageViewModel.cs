using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Enums
{
    public enum ItemsPerPageViewModel
    {
        [Display(Name = "10")]
        Ten = 10,
        [Display(Name = "20")]
        Twenty = 20,
        [Display(Name = "50")]
        Fifty = 50,
        [Display(Name = "100")]
        OneHundred = 100,
        [Display(Name = "ItemsPerPageAll", ResourceType = typeof(GlobalRes))]
        All = -1
    }
}