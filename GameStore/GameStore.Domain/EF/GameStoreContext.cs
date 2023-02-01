using System.Data.Entity;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.Domain.EF
{
    public class GameStoreContext : DbContext, IDbContext
    {
        static GameStoreContext()
        {
            Database.SetInitializer<GameStoreContext>(new GameStoreDbInitializer());
        }

        public GameStoreContext(string connection) : base(connection)
        {        
        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<PlatformType> PlatformTypes { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}