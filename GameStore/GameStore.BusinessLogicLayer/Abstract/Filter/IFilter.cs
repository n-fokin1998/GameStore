using System.Linq;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Abstract.Filter
{
    public interface IFilter
    {
        IQueryable<Game> Execute(IQueryable<Game> input);
    }
}
