using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF.EntityConfigurations
{
    public class PublisherEntityConfiguration : EntityTypeConfiguration<Publisher>
    {
        public PublisherEntityConfiguration()
        {
            Property(p => p.CompanyName).HasMaxLength(40).HasColumnAnnotation(
                "Index",
                new IndexAnnotation(new[] { new IndexAttribute("Index") { IsUnique = true } }));
            Property(p => p.DescriptionEn).HasColumnType("ntext");
            Property(p => p.DescriptionRu).HasColumnType("ntext");
            Property(p => p.HomePage).HasColumnType("ntext");
            HasMany(g => g.Games).WithOptional(p => p.Publisher);
        }
    }
}