using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class OrderEntityConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderEntityConfiguration()
        {
            HasMany(o => o.OrderDetails).WithRequired(od => od.Order);
        }
    }
}
