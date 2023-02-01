using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IDbContext
    {
        DbSet<Game> Games { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<Genre> Genres { get; set; }

        DbSet<PlatformType> PlatformTypes { get; set; }

        DbSet<Publisher> Publishers { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<OrderDetails> OrderDetails { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Role> Roles { get; set; }

        Database Database { get; }

        DbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();

        void Dispose();
    }
}