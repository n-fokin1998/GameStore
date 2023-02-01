using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class PlatformTypeDTO
    {
        public int Id { get; set; }

        public string TypeRu { get; set; }

        public string TypeEn { get; set; }

        public List<GameDTO> Games { get; set; }
    }
}