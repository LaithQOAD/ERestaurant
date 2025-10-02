using AutoMapper;
using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialDTOs;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Application.DTOs.OrderDTOs;
using ERestaurant.Application.Materials.DTOs;
using ERestaurant.Application.OrderItems.DTOs;
using ERestaurant.Application.Orders.DTOs;
using ERestaurant.Application.Orders.OrderServices;
using ERestaurant.Domain.Entity;
using ERestaurant.Domain.IUnitOfWork;
using ERestaurant.Domain.Repository.BaseRepository;
using ERestaurant.Infrastructure.DatabaseContext;
using ERestaurant.Testing.SharedHelperClass;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Testing
{
    public sealed class OrderServicesTests
    {
        private static (OrderService svc, TestUow uow) CreateSystem()
        {
            var dbOptions = new DbContextOptionsBuilder<ERestaurantDbContext>()
                .UseInMemoryDatabase($"orders-db-{Guid.NewGuid()}")
                .Options;

            var reqTenant = new MockRequestTenant { TenantId = 1 };
            var stamper = new TestAuditStamper("Tester");

            var db = new ERestaurantDbContext(dbOptions, reqTenant, stamper);
            var uow = new TestUow(db);
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Material, MaterialDTO>();
                c.CreateMap<Combo, ComboDTO>();
                c.CreateMap<AdditionalMaterial, AdditionalMaterialDTO>();
                c.CreateMap<OrderItem, OrderItemDTO>();
                c.CreateMap<Order, OrderDTO>();
            });
            var mapper = cfg.CreateMapper();

            var lang = new MockRequestLanguage();

            var svc = new OrderService(uow, mapper, lang);
            return (svc, uow);
        }
        private static (Material m, Combo c, AdditionalMaterial a) SeedCatalog(TestUow uow)
        {
            var now = DateTimeOffset.UtcNow;

            var m = new Material
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = now,
                NameEn = "Flour",
                NameAr = "طحين",
                Unit = ERestaurant.Domain.Enums.MaterialUnit.Gram,
                PricePerUnit = 2.000m,
                Tax = 0.160m,
                IsActive = true,
                IsDeleted = false
            };
            var c = new Combo
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = now,
                NameEn = "Classic Combo",
                NameAr = "كومبو كلاسيك",
                Price = 5.000m,
                Tax = 0.160m,
                IsActive = true,
                IsDeleted = false
            };
            var a = new AdditionalMaterial
            {
                Id = Guid.NewGuid(),
                TenantId = 1,
                CreatedBy = "seed",
                CreatedDate = now,
                NameEn = "BBQ Sauce",
                NameAr = "صوص باربكيو",
                Unit = ERestaurant.Domain.Enums.MaterialUnit.Gram,
                PricePerUnit = 1.500m,
                Tax = 0.160m,
                IsActive = true,
                IsDeleted = false
            };

            uow.Material.AddAsync(m).GetAwaiter().GetResult();
            uow.Combo.AddAsync(c).GetAwaiter().GetResult();
            uow.AdditionalMaterial.AddAsync(a).GetAwaiter().GetResult();
            uow.SaveChangesAsync().GetAwaiter().GetResult();

            return (m, c, a);
        }


        [Fact]
        public async Task CreateShouldThrowValidationWhenNoItems()
        {
            var (svc, _) = CreateSystem();

            var dto = new CreateOrderDTO
            {
                CustomerName = "Ahmed",
                CustomerPhone = "079",
                OrderItem = new List<CreateOrderItemDTO>()
            };

            await Assert.ThrowsAsync<ValidationException>(() => svc.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateShouldThrowValidationWhenOneOfRuleBroken()
        {
            var (svc, uow) = CreateSystem();
            var (m, c, a) = SeedCatalog(uow);

            var dto = new CreateOrderDTO
            {
                CustomerName = "Sara",
                CustomerPhone = "078",
                OrderItem = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO
                    {
                        MaterialId = m.Id,
                        ComboId = c.Id,
                        Quantity = 1
                    }
                }
            };

            await Assert.ThrowsAsync<ValidationException>(() => svc.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateShouldThrowValidationWhenQuantityNotPositive()
        {
            var (svc, uow) = CreateSystem();
            var (m, _, _) = SeedCatalog(uow);

            var dto = new CreateOrderDTO
            {
                CustomerName = "Ali",
                CustomerPhone = "077",
                OrderItem = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO
                    {
                        MaterialId = m.Id,
                        Quantity = 0 // غلط
                    }
                }
            };

            await Assert.ThrowsAsync<ValidationException>(() => svc.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateRecalculatesTotalsWithRounding3dp()
        {
            var (svc, uow) = CreateSystem();
            var (m, c, _) = SeedCatalog(uow);

            var dto = new CreateOrderDTO
            {
                CustomerName = "Nour",
                CustomerPhone = "070",
                OrderItem = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 2 },
                    new CreateOrderItemDTO { ComboId = c.Id, Quantity = 1 }
                }
            };

            var result = await svc.CreateAsync(dto);

            Assert.Equal(9.000m, result.TotalPriceBeforeTax);
            Assert.Equal(1.440m, result.TotalTax);
            Assert.Equal(10.440m, result.TotalPriceAfterTax);
        }

        [Fact]
        public async Task UpdateRecalculatesTotalsAfterItemsChange()
        {
            var (svc, uow) = CreateSystem();
            var (m, _, a) = SeedCatalog(uow);

            var created = await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "Omar",
                CustomerPhone = "071",
                OrderItem = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 }
                }
            });

            uow.ClearTracking();

            var updated = await svc.UpdateAsync(new UpdateOrderDTO
            {
                Id = created.Id,
                OrderItem = new List<UpdateOrderItemDTO>
                {
                    new UpdateOrderItemDTO { AdditionalMaterialId = a.Id, Quantity = 3 }
                }
            });

            Assert.Equal(4.500m, updated.TotalPriceBeforeTax);
            Assert.Equal(0.720m, updated.TotalTax);
            Assert.Equal(5.220m, updated.TotalPriceAfterTax);
        }

        [Fact]
        public async Task FindByIdReturnsNullWhenNotFound()
        {
            var (svc, _) = CreateSystem();
            var fetched = await svc.FindByIdAsync(Guid.NewGuid());
            Assert.Null(fetched);
        }

        [Fact]
        public async Task DeleteRemovesOrder()
        {
            var (svc, uow) = CreateSystem();
            var (m, _, _) = SeedCatalog(uow);

            var created = await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "Lama",
                CustomerPhone = "073",
                OrderItem = new List<CreateOrderItemDTO>
                {
                    new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 },
                }
            });

            Assert.NotNull(await svc.FindByIdAsync(created.Id));

            await svc.DeleteAsync(created.Id);

            Assert.Null(await svc.FindByIdAsync(created.Id));
        }

        [Fact]
        public async Task SearchIsCaseInsensitiveOnCustomerName()
        {
            var (svc, uow) = CreateSystem();
            var (m, _, _) = SeedCatalog(uow);

            await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "ahmed ali",
                CustomerPhone = "0",
                OrderItem = new List<CreateOrderItemDTO> { new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 } }
            });
            await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "Sara",
                CustomerPhone = "1",
                OrderItem = new List<CreateOrderItemDTO> { new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 } }
            });

            var list = await svc.FindAllAsync(searchQuery: "ahm");
            Assert.Single(list);
            Assert.Equal("ahmed ali", list[0].CustomerName);
        }


        [Fact]
        public async Task OrderByDefaultsToOrderDateDESCWhenInvalid()
        {
            var (svc, uow) = CreateSystem();
            var (m, _, _) = SeedCatalog(uow);

            var o1 = await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "X",
                CustomerPhone = "1",
                OrderItem = new List<CreateOrderItemDTO> { new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 } }
            });
            await Task.Delay(5);
            var o2 = await svc.CreateAsync(new CreateOrderDTO
            {
                CustomerName = "Y",
                CustomerPhone = "2",
                OrderItem = new List<CreateOrderItemDTO> { new CreateOrderItemDTO { MaterialId = m.Id, Quantity = 1 } }
            });

            var list = await svc.FindAllAsync(orderBy: "INVALID_COL", orderByDirection: "DESC");
            Assert.Equal(new[] { o2.Id, o1.Id }, list.Select(x => x.Id).ToArray());
        }

        internal sealed class TestUow : IUnitOfWork
        {
            private readonly ERestaurantDbContext _db;

            public TestUow(ERestaurantDbContext db)
            {
                _db = db;
                Material = new EfRepo<Material>(_db);
                Combo = new EfRepo<Combo>(_db);
                AdditionalMaterial = new EfRepo<AdditionalMaterial>(_db);
                Order = new EfRepo<Order>(_db);
                ComboMaterial = new EfRepo<ComboMaterial>(_db);
                OrderItem = new EfRepo<OrderItem>(_db);
            }

            public IBaseRepository<Material> Material { get; }
            public IBaseRepository<Combo> Combo { get; }
            public IBaseRepository<AdditionalMaterial> AdditionalMaterial { get; }
            public IBaseRepository<Order> Order { get; }
            public IBaseRepository<ComboMaterial> ComboMaterial { get; }
            public IBaseRepository<OrderItem> OrderItem { get; }
            public void ClearTracking() => _db.ChangeTracker.Clear();

            public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
            public void Dispose() => _db.Dispose();
        }

        internal sealed class EfRepo<T> : IBaseRepository<T> where T : BaseEntity
        {
            private readonly ERestaurantDbContext _db;
            private readonly DbSet<T> _set;

            public EfRepo(ERestaurantDbContext db)
            {
                _db = db;
                _set = db.Set<T>();
            }

            public IQueryable<T> Query(bool asNoTracking = false)
                => asNoTracking ? _set.AsNoTracking() : _set;

            public async Task<T?> FindByIdAsync(Guid id)
                => await _set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            public async Task<T> AddAsync(T entity)
            {
                if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
                await _set.AddAsync(entity);
                return entity;
            }

            public Task UpdateAsync(T entity)
            {
                _set.Update(entity);
                return Task.CompletedTask;
            }

            public async Task DeleteAsync(Guid id)
            {
                var entity = await _set.FirstOrDefaultAsync(e => e.Id == id);
                if (entity != null) _set.Remove(entity);
            }

            public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
        }
    }
}