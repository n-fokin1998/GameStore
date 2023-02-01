using System.Linq;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class GameSorting : IFilter
    {
        private readonly SortType _sortType;

        public GameSorting(SortType sortType)
        {
            _sortType = sortType;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            switch (_sortType)
            {
                case SortType.MostPopular: return input.OrderByDescending(g => g.Popularity).ThenBy(g => g.NameEn);
                case SortType.MostCommented: return input
                        .OrderByDescending(g => g.Comments.Count)
                        .ThenBy(g => g.NameEn);
                case SortType.PriceAsc: return input.OrderBy(g => g.Price).ThenBy(g => g.NameEn);
                case SortType.PriceDesc: return input.OrderByDescending(g => g.Price).ThenBy(g => g.NameEn);
                case SortType.Date: return input.OrderByDescending(g => g.UploadDate).ThenBy(g => g.NameEn);
            }

            return input;
        }
    }
}