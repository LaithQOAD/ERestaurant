using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Domain.Repository.BaseRepository;
using ERestaurant.Infrastructure.DatabaseContext;

namespace ERestaurant.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ERestaurantDbContext _context;

        public IBaseRepository<AdditionalMaterial> AdditionalMaterial { get; private set; }

        public IBaseRepository<Combo> Combo { get; private set; }

        public IBaseRepository<Material> Material { get; private set; }

        public IBaseRepository<ComboMaterial> ComboMaterial { get; private set; }

        public IBaseRepository<Order> Order { get; private set; }

        public IBaseRepository<OrderItem> OrderItem { get; private set; }

        public UnitOfWork(
            ERestaurantDbContext context,
            IBaseRepository<Combo> combo,
            IBaseRepository<Material> material,
            IBaseRepository<ComboMaterial> comboMaterial,
            IBaseRepository<Order> order,
            IBaseRepository<OrderItem> orderItem,
            IBaseRepository<AdditionalMaterial> additionalMaterial)
        {
            _context = context;
            Combo = combo;
            Material = material;
            ComboMaterial = comboMaterial;
            Order = order;
            OrderItem = orderItem;
            AdditionalMaterial = additionalMaterial;
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

    }
}
