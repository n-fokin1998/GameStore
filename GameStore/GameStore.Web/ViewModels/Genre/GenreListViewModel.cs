using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.Web.ViewModels
{
    public class GenreListViewModel
    {
        public IEnumerable<GenreDTO> Genres { get; set; }

        public int? ParentGenreId { get; set; }

        public int? Seed { get; set; }
    }
}