using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Repository.BaseRepository;
using ERestaurant.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ERestaurantDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ERestaurantDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool asNoTracking = true)
        {
            var q = _dbSet.AsQueryable();
            return asNoTracking ? q.AsNoTracking() : q;
        }


        public virtual async Task<T?> FindByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null) return;
            _dbSet.Remove(entity);
        }

    }
}
