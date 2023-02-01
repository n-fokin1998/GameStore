using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class PlatformType : EntityBase
    {
        public PlatformType()
        {
            Games = new List<Game>();
        }

        public string TypeRu { get; set; }

        public string TypeEn { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}