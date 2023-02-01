using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BusinessLogicLayer.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public List<RoleDTO> Roles { get; set; }

        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }
           
            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return rolesArray.Select(role => Roles.Any(p => p.Name == role.Trim())).Any(hasRole => hasRole);
        }
    }
}