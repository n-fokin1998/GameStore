using GameStore.BusinessLogicLayer.Domain.Enums;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class GameFilterDTO
    {
        public int[] GenreFilters { get; set; }

        public int[] PlatformTypeFilters { get; set; }

        public int[] PublisherFilters { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public DateFilterType DateFilter { get; set; }

        public string NameFilter { get; set; }

        public SortType? SortType { get; set; }

        public int? Page { get; set; }

        public ItemsPerPage? ItemsPerPage { get; set; }

        public int? CurrentPage { get; set; }
    }
}