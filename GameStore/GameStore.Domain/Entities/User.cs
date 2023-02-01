using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class User : EntityBase
    {
        public User()
        {
            Roles = new List<Role>();
        }

        public string Email { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}