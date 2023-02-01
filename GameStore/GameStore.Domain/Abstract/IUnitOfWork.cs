using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Game> Games { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Genre> Genres { get; }

        IRepository<PlatformType> PlatformTypes { get; }

        IRepository<Publisher> Publishers { get; }

        IRepository<Order> Orders { get; }

        IRepository<OrderDetails> OrderDetails { get; }

        IUserRepository Users { get; }

        IRepository<Role> Roles { get; }

        void Commit();
    }
}