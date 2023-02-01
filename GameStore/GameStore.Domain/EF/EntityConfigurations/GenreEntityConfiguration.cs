using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class GenreEntityConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreEntityConfiguration()
        {
            Property(p => p.NameEn).HasMaxLength(450).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index1") { IsUnique = true } }));
            Property(p => p.NameRu).HasMaxLength(450).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index2") { IsUnique = true } }));
        }
    }
}