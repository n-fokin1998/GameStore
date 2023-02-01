using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter
{
    public class GameSelectionPipeline
    {
        private readonly List<IFilter> _filters = new List<IFilter>();
        private readonly IUnitOfWork _database;

        public GameSelectionPipeline(IUnitOfWork database)
        {
            _database = database;
        }

        public GameSelectionPipeline Register(IFilter filter)
        {
            _filters.Add(filter);

            return this;
        }

        public IQueryable<Game> Process()
        {
            var games = _database.Games.GetList().Where(g => !g.IsDeleted && g.UnitsInStock > 0);
            games = _filters.Aggregate(games, (current, filter) => filter.Execute(current));

            return games;
        }
    }
}