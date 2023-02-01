using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Enums
{
    public enum BanDurationViewModel
    {
        [Display(Name = "BanDurationOneHour", ResourceType = typeof(GlobalRes))]
        OneHour,
        [Display(Name = "BanDurationOneDay", ResourceType = typeof(GlobalRes))]
        OneDay,
        [Display(Name = "BanDurationOneWeek", ResourceType = typeof(GlobalRes))]
        OneWeek,
        [Display(Name = "BanDurationOneMonth", ResourceType = typeof(GlobalRes))]
        OneMonth,
        [Display(Name = "BanDurationPermanent", ResourceType = typeof(GlobalRes))]
        Permanent
    }
}