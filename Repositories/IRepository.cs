using System.Linq.Expressions;

namespace back_end.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
        IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter);
        IQueryable<T> GetDeletedByFilter(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void HardDelete(T entity);
        void HardDeleteRange(IEnumerable<T> entities);
        void SaveChanges();
    }
}
