using System.Collections.Generic;

namespace GameStore.Domain.Entities
{
    public class Role : EntityBase
    {
        public Role()
        {
            Users = new List<User>();
        }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}