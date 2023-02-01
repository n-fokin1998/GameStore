using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CommentDTO GetById(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }

            var comment = _unitOfWork.Comments.GetItem(id.Value);
            comment = comment != null && !comment.IsDeleted ? comment : null;

            return _mapper.Map<Comment, CommentDTO>(comment);
        }

        public OperationDetails Add(CommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return new OperationDetails(false, BLLRes.CommentNotFound, null);
            }

            var game = _unitOfWork.Games.GetItem(commentDto.GameId);
            if (game == null)
            {
                return new OperationDetails(false, BLLRes.GameNotFound, null);
            }

            var comment = _mapper.Map<CommentDTO, Comment>(commentDto);
            comment.ParentComment = commentDto.ParentCommentId.HasValue
                ? _unitOfWork.Comments.GetItem(commentDto.ParentCommentId.Value)
                : null;
            if (commentDto.ParentCommentId != null && comment.ParentComment == null)
            {
                return new OperationDetails(false, BLLRes.ParentCommentNotFound, null);
            }

            comment.Game = game;
            _unitOfWork.Comments.Add(comment);
            _unitOfWork.Commit();
            return new OperationDetails(true);
        }

        public IEnumerable<CommentDTO> GetByGameKey(string key)
        {
            var commentList = _unitOfWork.Comments.GetList().Where(c => c.Game.Key == key).ToList();
            var comments = _mapper.Map<IEnumerable<Comment>, List<CommentDTO>>(commentList);

            return comments;
        }

        public OperationDetails Delete(CommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return new OperationDetails(false, BLLRes.CommentNotFound, null);
            }

            var comment = _unitOfWork.Comments.GetItem(commentDto.Id);
            if (comment == null)
            {
                return new OperationDetails(false, BLLRes.CommentNotFound, null);
            }

            comment.IsDeleted = true;
            _unitOfWork.Comments.Update(comment, comment.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Ban(CommentDTO comment, BanDuration duration)
        {
            throw new NotImplementedException();
        }
    }
}