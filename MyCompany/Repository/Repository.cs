using Microsoft.EntityFrameworkCore;
using MyCompany.Models;
using MyCompany.Repository.IRepository;
using System.Linq.Expressions;

namespace MyCompany.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyCompanyContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(MyCompanyContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteListAsync(List<int> ids)
        {
            if (ids.Count == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            for (int i = 0; i < ids.Count; i++)
            {
                var entity = await _dbSet.FindAsync(ids[i]);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
