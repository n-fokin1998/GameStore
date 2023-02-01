using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class GenreFilter : IFilter
    {
        private readonly IEnumerable<int> _genreList;

        public GenreFilter(IEnumerable<int> genreList)
        {
            _genreList = genreList;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return _genreList == null || !_genreList.Any()
                ? input
                : input.Where(game => _genreList.Any(genre => game.Genres.Any(g => g.Id == genre)));
        }
    }
}