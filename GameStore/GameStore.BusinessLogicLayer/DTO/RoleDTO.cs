using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public List<UserDTO> Users { get; set; }
    }
}