using System;
using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Game : EntityBase
    {
        public Game()
        {
            Comments = new List<Comment>();
            Genres = new List<Genre>();
            PlatformTypes = new List<PlatformType>();
        }

        public string Key { get; set; }

        public string NameEn { get; set; }

        public string NameRu { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int Popularity { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsDeleted { get; set; }

        public int? PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<PlatformType> PlatformTypes { get; set; }

        public string ImageReference { get; set; }
    }
}