using System;
using System.Linq;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class DateFilter : IFilter
    {
        private readonly DateFilterType _filterType;

        public DateFilter(DateFilterType filterType)
        {
            _filterType = filterType;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            if (_filterType == DateFilterType.All)
            {
                return input;
            }

            var date = DateTime.Now.AddDays((int)_filterType * -1);

            return input.Where(g => g.PublishDate >= date);
        }
    }
}