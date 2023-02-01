using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.BusinessLogicLayer.Domain.Enums;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface ICommentService
    {
        IEnumerable<CommentDTO> GetByGameKey(string key);

        OperationDetails Add(CommentDTO commentDto);

        CommentDTO GetById(int? id);

        OperationDetails Delete(CommentDTO commentDto);

        OperationDetails Ban(CommentDTO comment, BanDuration duration);
    }
}