using System;
using System.Data.Entity;
using System.Linq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContext _context;
        private readonly ILogRepository _logRepository;

        public UserRepository(IDbContext context, ILogRepository logRepository)
        {
            _context = context;
            _logRepository = logRepository;
        }

        public User Login(string email, string password)
        {
            var result = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            return result?.Email == email && result?.PasswordHash == password ? result : null;
        }

        public User GetUser(string email)
        {
            var result = _context.Users.FirstOrDefault(u => u.Email == email);
            return result?.Email == email ? result : null;
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _logRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Add",
                EntityType = typeof(User).ToString(),
                NewEntityVersion = user.ToBsonDocument()
            });
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public IQueryable<User> GetList()
        {
            return _context.Users;
        }

        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _logRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Update",
                EntityType = typeof(User).ToString(),
                NewEntityVersion = item.ToBsonDocument()
            });
        }

        public void Delete(User item)
        {
            _context.Users.Remove(item);
            _logRepository.Add(new Log
            {
                Date = DateTime.UtcNow,
                Action = "Delete",
                EntityType = typeof(User).ToString(),
                OldEntityVersion = item.ToBsonDocument()
            });
        }
    }
}