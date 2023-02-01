using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "RequiredField")]
        public string Body { get; set; }

        public IEnumerable<CommentDTO> Comments { get; set; }

        public GameDTO Game { get; set; }

        public int? ParentCommentId { get; set; }

        public int? QuoteCommentId { get; set; }

        public int? Seed { get; set; }

        public bool IsGameDeleted { get; set; }
    }
}