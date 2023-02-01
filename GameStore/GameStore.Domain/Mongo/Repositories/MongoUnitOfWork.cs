using System;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;

namespace GameStore.Domain.Mongo.Repositories
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly Lazy<ILogRepository> _logRepository;
        private readonly Lazy<IMongoRepository<Order>> _orderRepository;
        private readonly Lazy<IMongoRepository<Shipper>> _shipperRepository;

        public MongoUnitOfWork(
            Lazy<ILogRepository> logRepository,
            Lazy<IMongoRepository<Order>> orderRepository,
            Lazy<IMongoRepository<Shipper>> shipperRepository)
        {
            _logRepository = logRepository;
            _orderRepository = orderRepository;
            _shipperRepository = shipperRepository;
        }

        public ILogRepository Logs => _logRepository.Value;

        public IMongoRepository<Order> Orders => _orderRepository.Value;

        public IMongoRepository<Shipper> Shippers => _shipperRepository.Value;
    }
}