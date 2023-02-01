using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class PublisherFilter : IFilter
    {
        private readonly IEnumerable<int> _publisherList;

        public PublisherFilter(IEnumerable<int> publisherList)
        {
            _publisherList = publisherList;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return _publisherList == null || !_publisherList.Any() ?
                input :
                input.Where(game => _publisherList.Any(publisher => game.Publisher.Id == publisher));
        }
    }
}