using back_end.Data;
using back_end.Models;
using back_end.Interfaces;
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

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public void HardDelete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void HardDelete(int id)
        {
            _context.Set<T>().Where(x => x.Id == id).ExecuteDelete();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
