using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class RoleEntityConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleEntityConfiguration()
        {
            Property(p => p.Name).HasMaxLength(450).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index") { IsUnique = true } }));
        }
    }
}