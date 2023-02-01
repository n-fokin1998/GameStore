using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class OrderDetailsEntityConfiguration : EntityTypeConfiguration<OrderDetails>
    {
        public OrderDetailsEntityConfiguration()
        {
            Property(p => p.Price).HasColumnType("money");
        }
    }
}