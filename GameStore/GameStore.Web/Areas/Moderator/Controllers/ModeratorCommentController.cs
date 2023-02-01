using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.Web.Areas.Moderator.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ModeratorCommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public ModeratorCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public ActionResult DeleteComment(int id, string gameKey)
        {
            var comment = _commentService.GetById(id);
            if (comment == null)
            {
                return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = gameKey, area = string.Empty });
            }

            var model = new DeleteCommentViewModel { Id = comment.Id, GameKey = gameKey };

            return PartialView("DeleteCommentModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeleteComment(DeleteCommentViewModel model)
        {
            if (model.Id == null)
            {
                return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = model.GameKey, area = string.Empty });
            }

            _commentService.Delete(_commentService.GetById(model.Id));

            return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = model.GameKey, area = string.Empty });
        }

        public ActionResult BanComment(int id, string gameKey)
        {
            var comment = _commentService.GetById(id);
            if (comment == null)
            {
                return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = gameKey, area = string.Empty });
            }

            var model = new BanCommentViewModel { Id = comment.Id, GameKey = gameKey };

            return View(model);
        }

        [HttpPost]
        public ActionResult BanComment(BanCommentViewModel model)
        {
            _commentService.Ban(_commentService.GetById(model.Id), (BanDuration)model.Duration);

            return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = model.GameKey, area = string.Empty });
        }
    }
}