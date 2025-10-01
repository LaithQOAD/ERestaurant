using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Repository.BaseRepository;

namespace ERestaurant.Domain.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<Combo> Combo { get; }
        public IBaseRepository<Material> Material { get; }
        public IBaseRepository<ComboMaterial> ComboMaterial { get; }
        public IBaseRepository<Order> Order { get; }
        public IBaseRepository<OrderItem> OrderItem { get; }
        public IBaseRepository<AdditionalMaterial> AdditionalMaterial { get; }
        Task<int> SaveChangesAsync();

    }
}
