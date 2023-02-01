using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Publisher : EntityBase
    {
        public Publisher()
        {
            Games = new List<Game>();
        }

        public string CompanyName { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public string HomePage { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}