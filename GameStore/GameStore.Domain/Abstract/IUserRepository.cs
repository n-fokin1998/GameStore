using System.Linq;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Entities;

namespace GameStore.Domain.Abstract
{
    public interface IUserRepository
    {
        User Login(string name, string password);

        User GetUser(string name);

        void Register(User user);

        User GetById(int id);

        IQueryable<User> GetList();

        void Update(User item);

        void Delete(User item);
    }
}