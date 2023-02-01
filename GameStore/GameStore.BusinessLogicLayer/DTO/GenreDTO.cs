using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string NameRu { get; set; }

        public string NameEn { get; set; }

        public int? ParentGenreId { get; set; }

        public GenreDTO ParentGenre { get; set; }

        public List<GameDTO> Games { get; set; }
    }
}