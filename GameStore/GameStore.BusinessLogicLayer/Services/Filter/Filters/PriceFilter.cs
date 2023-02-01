using System.Linq;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class PriceFilter : IFilter
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public PriceFilter(decimal minPrice, decimal maxPrice)
        {
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return input.Where(g => g.Price >= _minPrice && g.Price <= _maxPrice);
        }
    }
}