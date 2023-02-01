using System.Collections.Generic;
using GameStore.BusinessLogicLayer.Domain.Enums;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.ViewModels.Enums;

namespace GameStore.Web.ViewModels
{
    public class CatalogViewModel
    {
        public List<GameDTO> Games { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public List<PlatformTypeDTO> PlatformTypes { get; set; }

        public List<PublisherDTO> Publishers { get; set; }

        public int[] GenreFilters { get; set; }

        public int[] PlatformTypeFilters { get; set; }

        public int[] PublisherFilters { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public DateFilterType DateFilter { get; set; }

        public string NameFilter { get; set; }

        public SortTypeViewModel? SortType { get; set; }

        public int? Page { get; set; }

        public ItemsPerPageViewModel? ItemsPerPage { get; set; }

        public int? CurrentPage { get; set; }
    }
}