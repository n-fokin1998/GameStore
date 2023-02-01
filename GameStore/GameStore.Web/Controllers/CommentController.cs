using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Infrastructure;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Controllers
{
    public class CommentController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(IGameService gameService, ICommentService commentService, IMapper mapper)
        {
            _gameService = gameService;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetCommentsByGameKey(string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var comments = _commentService.GetByGameKey(key).ToList();
            var commentViewModel = new CommentViewModel
            {
                Comments = comments,
                Game = game,
                IsGameDeleted = game.IsDeleted
            };

            return View(commentViewModel);
        }

        [CustomAuthorize]
        public ActionResult WriteComment(CommentViewModel model, string key)
        {
            var game = _gameService.GetByKey(key);
            if (game == null || game.IsDeleted)
            {
                throw new HttpException(404, ErrorMessage);
            }

            model.Game = game;
            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<CommentViewModel, CommentDTO>(model);
                var parentComment = _commentService.GetById(comment.ParentCommentId);
                if (parentComment != null && !comment.Body.StartsWith(parentComment.Name))
                {
                    comment.Body = comment.Body.Insert(0, parentComment.Name + ", ");
                }

                comment.GameId = game.Id;
                var result = _commentService.Add(comment);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetCommentsByGameKey", "Comment", new { key = game.Key });
                }

                ModelState.AddModelError("Name", result.Message);
            }

            model.Comments = _commentService.GetByGameKey(key);

            return View("GetCommentsByGameKey", model);
        }
    }
}