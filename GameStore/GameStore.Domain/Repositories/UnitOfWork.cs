using System;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private readonly Lazy<IRepository<Game>> _gameRepository;
        private readonly Lazy<IRepository<Comment>> _commentRepository;
        private readonly Lazy<IRepository<Genre>> _genreRepository;
        private readonly Lazy<IRepository<PlatformType>> _platformTypeRepository;
        private readonly Lazy<IRepository<Publisher>> _publisherRepository;
        private readonly Lazy<IRepository<Order>> _orderRepository;
        private readonly Lazy<IRepository<OrderDetails>> _orderDetailsRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IRepository<Role>> _roleRepository;

        public UnitOfWork(
            IDbContext context,
            Lazy<IRepository<Game>> gameRepository,
            Lazy<IRepository<Comment>> commentRepository,
            Lazy<IRepository<Genre>> genreRepository,
            Lazy<IRepository<PlatformType>> platformTypeRepository,
            Lazy<IRepository<Publisher>> publisherRepository,
            Lazy<IRepository<Order>> orderRepository,
            Lazy<IRepository<OrderDetails>> orderDetailsRepository,
            Lazy<IRepository<Role>> roleRepository,
            Lazy<IUserRepository> userRepository)
        {
            _context = context;
            _gameRepository = gameRepository;
            _commentRepository = commentRepository;
            _genreRepository = genreRepository;
            _platformTypeRepository = platformTypeRepository;
            _publisherRepository = publisherRepository;
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public IRepository<Game> Games => _gameRepository.Value;

        public IRepository<Comment> Comments => _commentRepository.Value;

        public IRepository<Genre> Genres => _genreRepository.Value;

        public IRepository<PlatformType> PlatformTypes => _platformTypeRepository.Value;

        public IRepository<Publisher> Publishers => _publisherRepository.Value;

        public IRepository<Order> Orders => _orderRepository.Value;

        public IRepository<OrderDetails> OrderDetails => _orderDetailsRepository.Value;

        public IUserRepository Users => _userRepository.Value;

        public IRepository<Role> Roles => _roleRepository.Value;

        public void Commit()
        {
            _context?.SaveChanges();
        }
    }
}