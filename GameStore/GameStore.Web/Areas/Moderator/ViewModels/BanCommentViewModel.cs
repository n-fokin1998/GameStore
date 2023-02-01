using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Enums;

namespace GameStore.Web.Areas.Moderator.ViewModels
{
    public class BanCommentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(GlobalRes))]
        public BanDurationViewModel Duration { get; set; }

        public string GameKey { get; set; }
    }
}