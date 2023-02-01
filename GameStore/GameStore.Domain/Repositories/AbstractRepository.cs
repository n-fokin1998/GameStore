using System.Linq;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Repositories
{
    public class AbstractRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbContext Context;

        public AbstractRepository(IDbContext context)
        {
            Context = context;
        }

        public virtual void Add(T item)
        {
            Context.Set<T>().Add(item);
        }

        public T GetItem(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public IQueryable<T> GetList()
        {
            return Context.Set<T>();
        }

        public virtual void Update(T item, int id)
        {
            var old = Context.Set<T>().Find(id);
            Context.Entry(old).CurrentValues.SetValues(item);
        }

        public virtual void Delete(T item)
        {
            Context.Set<T>().Remove(item);
        }
    }
}