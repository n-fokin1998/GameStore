using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Genre : EntityBase
    {
        public Genre()
        {
            Games = new List<Game>();
        }

        public string NameRu { get; set; }

        public string NameEn { get; set; }

        public int? ParentGenreId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Genre ParentGenre { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}