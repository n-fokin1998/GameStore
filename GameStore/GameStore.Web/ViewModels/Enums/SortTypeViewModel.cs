using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Enums
{
    public enum SortTypeViewModel
    {
        [Display(Name = "SortTypePopular", ResourceType = typeof(GlobalRes))]
        MostPopular,
        [Display(Name = "SortTypeComment", ResourceType = typeof(GlobalRes))]
        MostCommented,
        [Display(Name = "SortTypePriceAsc", ResourceType = typeof(GlobalRes))]
        PriceAsc,
        [Display(Name = "SortTypePriceDesc", ResourceType = typeof(GlobalRes))]
        PriceDesc,
        [Display(Name = "SortTypeDate", ResourceType = typeof(GlobalRes))]
        Date
    }
}