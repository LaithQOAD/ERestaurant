using ERestaurant.Domain.Entity;

namespace ERestaurant.Domain.Repository.BaseRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> FindByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        IQueryable<T> Query(bool asNoTracking = true);

    }
}
