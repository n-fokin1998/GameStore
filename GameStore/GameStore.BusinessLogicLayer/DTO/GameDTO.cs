using System;
using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string NameEn { get; set; }

        public string NameRu { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int Popularity { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime UploadDate { get; set; }

        public int? PublisherId { get; set; }

        public DateTime PublishDate { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public List<PlatformTypeDTO> PlatformTypes { get; set; }

        public string ImageReference { get; set; }
    }
}