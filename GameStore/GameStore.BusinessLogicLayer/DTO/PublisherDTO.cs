using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class PublisherDTO
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public string HomePage { get; set; }

        public List<GameDTO> Games { get; set; }
    }
}