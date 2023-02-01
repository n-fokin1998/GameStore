using System;
using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string NameEn { get; set; }

        public string NameRu { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int PublisherId { get; set; }

        public bool IsDeleted { get; set; }

        public PublisherDTO Publisher { get; set; }

        public DateTime PublishDate { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<string> GenreNames { get; set; }

        public List<string> PlatformNames { get; set; }

        public string ImageReference { get; set; }
    }
}