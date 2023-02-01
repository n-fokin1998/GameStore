using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class PlatformTypeFilter : IFilter
    {
        private readonly IEnumerable<int> _platformTypeList;

        public PlatformTypeFilter(IEnumerable<int> platformTypeList)
        {
            _platformTypeList = platformTypeList;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return _platformTypeList == null || !_platformTypeList.Any() ?
                input :
                input.Where(game => _platformTypeList
                .Any(platformType => game.PlatformTypes
                .Any(p => p.Id == platformType)));
        }
    }
}
