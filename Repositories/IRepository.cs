using System.Linq.Expressions;

namespace back_end.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
        IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void HardDelete(T entity);
        void SaveChanges();
    }
}
