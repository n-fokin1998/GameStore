using System.Linq;

namespace GameStore.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetList();

        T GetItem(int id);

        void Add(T item);

        void Update(T item, int id);

        void Delete(T item);
    }
}