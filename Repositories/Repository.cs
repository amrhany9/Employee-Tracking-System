using back_end.Data;
using back_end.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace back_end.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted);
        }

        public IQueryable<T> GetById(int id)
        {
            return _context.Set<T>().Where(x => id == x.Id && !x.IsDeleted);
        }

        public IQueryable<T> GetByFilter(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().Where(x => !x.IsDeleted).Where(filter);
        }

        public IQueryable<T> GetDeletedByFilter(Expression<Func<T, bool>> filter)
        {
            return _context.Set<T>().Where(x => x.IsDeleted).Where(filter);
        }

        public void Add(T entity)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            _context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            entity.IsDeleted = true;
            Update(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            foreach (var entity in entities) 
            {
                entity.IsDeleted = true;
            }
            UpdateRange(entities);
        }

        public void HardDelete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void HardDeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
