using System.Linq;
using System.Threading;
using GameStore.BusinessLogicLayer.Abstract.Filter;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Filter.Filters
{
    public class NameFilter : IFilter
    {
        private const string EnglishCulture = "en";
        private readonly string _pattern;

        public NameFilter(string pattern)
        {
            _pattern = pattern;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            if (_pattern == null)
            {
                return input;
            }

            return Thread.CurrentThread.CurrentCulture.Name.StartsWith(EnglishCulture) ?
                input.Where(g => g.NameEn.Contains(_pattern)) :
                input.Where(g => g.NameRu.Contains(_pattern));
        }
    }
}