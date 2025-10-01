using ERestaurant.Domain.Entity;
using ERestaurant.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ERestaurant.Infrastructure.HelperClass.SeedData
{
    internal record Localized(string En, string Ar);

    internal sealed class SeedData
    {
        private const decimal TaxRate = 0.160m;
        private const string SeedCreatedBy = "System";
        private static readonly DateTimeOffset SeedCreatedAt = DateTimeOffset.UtcNow;

        private const string MockBaseUrl = "https://www.ERestaurant.com";

        private static readonly (int TenantId, string Theme)[] Tenants =
        {
            (1, "Burgers"),
            (2, "Pizza"),
            (3, "Shawarma"),
            (4, "Coffee&Tea"),
            (5, "Desserts"),
        };

        public static void Apply(ModelBuilder mb)
        {
            var allMaterials = new List<Material>();
            var allAdditional = new List<AdditionalMaterial>();
            var allCombos = new List<Combo>();
            var allComboLinks = new List<ComboMaterial>();
            var allOrders = new List<Order>();
            var allOrderItems = new List<OrderItem>();

            foreach (var (tenantId, theme) in Tenants)
            {
                // 1) Materials (exactly 15)
                var materials = BuildMaterialsForTenant(tenantId, theme);
                allMaterials.AddRange(materials);

                // 2) Additional Materials (exactly 5)
                var additions = BuildAdditionalMaterialsForTenant(tenantId, theme);
                allAdditional.AddRange(additions);

                // 3) Combos (>= 4) + Links
                var (combos, links) = BuildCombosForTenant(tenantId, theme, materials);
                allCombos.AddRange(combos);
                allComboLinks.AddRange(links);

                // 4) Orders (exactly 3) + Items (each: at least 1 combo + 1 material + 1 additional)
                var (orders, items) = BuildOrdersForTenant(tenantId, materials, additions, combos);
                allOrders.AddRange(orders);
                allOrderItems.AddRange(items);
            }

            mb.Entity<Material>().HasData(allMaterials);
            mb.Entity<AdditionalMaterial>().HasData(allAdditional);
            mb.Entity<Combo>().HasData(allCombos);
            mb.Entity<ComboMaterial>().HasData(allComboLinks);
            mb.Entity<Order>().HasData(allOrders);
            mb.Entity<OrderItem>().HasData(allOrderItems);
        }

        // ================== Helpers: URL & Slug ==================
        private static string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Normalize to decompose, remove diacritics
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(capacity: normalized.Length);

            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc == UnicodeCategory.NonSpacingMark) continue;

                char c = ch;
                if (char.IsWhiteSpace(c) || c == '_' || c == '/' || c == '&')
                {
                    sb.Append('-');
                }
                else if (char.IsLetterOrDigit(c) || c == '-')
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    // replace other punctuation with hyphen
                    sb.Append('-');
                }
            }

            // collapse multiple hyphens and trim
            var slug = sb.ToString();
            while (slug.Contains("--")) slug = slug.Replace("--", "-");
            slug = slug.Trim('-');

            return slug;
        }

        private static string BuildMockUrl(int tenantId, string entitySegment, string nameEn)
            => $"{MockBaseUrl}/{tenantId}/{entitySegment}/{Slugify(nameEn)}";

        private static decimal R3(decimal v) => decimal.Round(v, 3, MidpointRounding.AwayFromZero);

        // ================== Materials (15 per tenant) ==================
        private static List<Material> BuildMaterialsForTenant(int tenantId, string theme)
        {
            var templates = theme.ToLowerInvariant() switch
            {
                "burgers" => BurgerMaterials(),
                "pizza" => PizzaMaterials(),
                "shawarma" => ShawarmaMaterials(),
                "coffee&tea" => CoffeeTeaMaterials(),
                _ => DessertMaterials()
            };

            var picked = templates.Take(15).ToList();

            return picked.Select((m, idx) => new Material
            {
                Id = DG(tenantId, "Material", idx),
                TenantId = tenantId,
                CreatedBy = SeedCreatedBy,
                CreatedDate = SeedCreatedAt,
                IsDeleted = false,

                NameEn = m.Name.En,
                NameAr = m.Name.Ar,
                Unit = m.Unit,
                PricePerUnit = R3(m.Price),
                Tax = TaxRate,
                ImageUrl = BuildMockUrl(tenantId, "material", m.Name.En),
                IsActive = true
            }).ToList();
        }

        private static List<(Localized Name, MaterialUnit Unit, decimal Price)> BurgerMaterials() => new()
        {
            (new("Brioche Bun","خبز بريوش"),           MaterialUnit.Piece, 0.600m),
            (new("Sesame Bun","خبز سمسم"),             MaterialUnit.Piece, 0.550m),
            (new("Beef Patty","لحم برجر بقري"),        MaterialUnit.Piece, 1.800m),
            (new("Chicken Fillet","فيليه دجاج"),       MaterialUnit.Piece, 1.400m),
            (new("Cheddar Slice","شريحة شيدر"),        MaterialUnit.Piece, 0.300m),
            (new("Swiss Cheese","جبنة سويسرية"),       MaterialUnit.Piece, 0.350m),
            (new("Lettuce","خس"),                       MaterialUnit.Gram,  0.005m),
            (new("Tomato","طماطم"),                     MaterialUnit.Gram,  0.006m),
            (new("Onion","بصل"),                        MaterialUnit.Gram,  0.004m),
            (new("Pickles","مخلل"),                     MaterialUnit.Gram,  0.004m),
            (new("Fries","بطاطا مقلية"),               MaterialUnit.Gram,  0.007m),
            (new("Coleslaw","سلطة ملفوف"),             MaterialUnit.Gram,  0.008m),
            (new("Jalapenos","هالبينو"),                MaterialUnit.Gram,  0.010m),
            (new("Mushrooms","فطر"),                    MaterialUnit.Gram,  0.010m),
            (new("Bacon Bits","لحم مقدد مفروم"),       MaterialUnit.Gram,  0.025m),
        };

        private static List<(Localized Name, MaterialUnit Unit, decimal Price)> PizzaMaterials() => new()
        {
            (new("Pizza Dough","عجين بيتزا"),          MaterialUnit.Piece, 0.900m),
            (new("Tomato Sauce","صلصة طماطم"),         MaterialUnit.Gram,  0.005m),
            (new("Mozzarella","موزاريلا"),             MaterialUnit.Gram,  0.012m),
            (new("Pepperoni","بيبروني"),               MaterialUnit.Gram,  0.020m),
            (new("Mushrooms","فطر"),                    MaterialUnit.Gram,  0.010m),
            (new("Bell Pepper","فليفلة"),               MaterialUnit.Gram,  0.009m),
            (new("Black Olives","زيتون أسود"),          MaterialUnit.Gram,  0.010m),
            (new("Onion","بصل"),                        MaterialUnit.Gram,  0.004m),
            (new("Basil","ريحان"),                      MaterialUnit.Gram,  0.025m),
            (new("Parmesan","بارميزان"),                MaterialUnit.Gram,  0.018m),
            (new("Chicken Topping","دجاج للبيتزا"),     MaterialUnit.Gram,  0.015m),
            (new("Cream Cheese","جبنة كريمية"),         MaterialUnit.Gram,  0.020m),
            (new("Oregano","أوريغانو"),                 MaterialUnit.Gram,  0.020m),
            (new("Olive Oil","زيت زيتون"),              MaterialUnit.Gram,  0.015m),
            (new("Sweet Corn","ذرة"),                   MaterialUnit.Gram,  0.007m),
        };

        private static List<(Localized Name, MaterialUnit Unit, decimal Price)> ShawarmaMaterials() => new()
        {
            (new("Pita Bread","خبز عربي"),             MaterialUnit.Piece, 0.400m),
            (new("Arabic Bread","خبز شراك"),           MaterialUnit.Piece, 0.450m),
            (new("Chicken Strips","شرائح دجاج"),       MaterialUnit.Gram,  0.012m),
            (new("Beef Strips","شرائح لحم"),           MaterialUnit.Gram,  0.018m),
            (new("Pickles","مخللات"),                  MaterialUnit.Gram,  0.004m),
            (new("Tomato","طماطم"),                    MaterialUnit.Gram,  0.006m),
            (new("Onion","بصل"),                       MaterialUnit.Gram,  0.004m),
            (new("Parsley","بقدونس"),                  MaterialUnit.Gram,  0.008m),
            (new("Fries","بطاطا مقلية"),               MaterialUnit.Gram,  0.007m),
            (new("Hummus","حمص"),                      MaterialUnit.Gram,  0.010m),
            (new("Sumac","سماق"),                      MaterialUnit.Gram,  0.020m),
            (new("Cabbage","ملفوف"),                   MaterialUnit.Gram,  0.006m),
            (new("Cucumber","خيار"),                   MaterialUnit.Gram,  0.005m),
            (new("Bread Chips","خبز محمص"),            MaterialUnit.Gram,  0.007m),
            (new("Lemon Juice","عصير ليمون"),          MaterialUnit.Gram,  0.015m),
        };

        private static List<(Localized Name, MaterialUnit Unit, decimal Price)> CoffeeTeaMaterials() => new()
        {
            (new("Espresso","إسبريسو"),                MaterialUnit.Gram,  0.025m),
            (new("Milk","حليب"),                       MaterialUnit.Gram,  0.005m),
            (new("Milk Foam","رغوة حليب"),             MaterialUnit.Gram,  0.006m),
            (new("Arabica Beans","قهوة عربية"),        MaterialUnit.Gram,  0.018m),
            (new("Green Tea","شاي أخضر"),              MaterialUnit.Gram,  0.006m),
            (new("Mint","نعناع"),                      MaterialUnit.Gram,  0.015m),
            (new("Ice","ثلج"),                         MaterialUnit.Gram,  0.001m),
            (new("Cocoa Powder","كاكاو بودرة"),        MaterialUnit.Gram,  0.012m),
            (new("Paper Cups","أكواب ورقية"),          MaterialUnit.Piece, 0.050m),
            (new("Lids","أغطية أكواب"),                MaterialUnit.Piece, 0.030m),
            (new("Water","ماء"),                        MaterialUnit.Piece, 0.200m),
            (new("Brown Sugar","سكر بني"),             MaterialUnit.Gram,  0.003m),
            (new("Cinnamon","قرفة"),                   MaterialUnit.Gram,  0.010m),
            (new("Hazelnut Syrup","شراب بندق"),        MaterialUnit.Gram,  0.015m),
            (new("Chocolate Chips","رقائق شوكولاتة"),  MaterialUnit.Gram,  0.020m),
        };

        private static List<(Localized Name, MaterialUnit Unit, decimal Price)> DessertMaterials() => new()
        {
            (new("Flour","طحين"),                      MaterialUnit.Gram,  0.001m),
            (new("Sugar","سكر"),                       MaterialUnit.Gram,  0.002m),
            (new("Eggs","بيض"),                        MaterialUnit.Piece, 0.250m),
            (new("Milk","حليب"),                       MaterialUnit.Gram,  0.005m),
            (new("Butter","زبدة"),                     MaterialUnit.Gram,  0.012m),
            (new("Cocoa Powder","كاكاو بودرة"),        MaterialUnit.Gram,  0.012m),
            (new("Baking Powder","بيكنج باودر"),       MaterialUnit.Gram,  0.006m),
            (new("Cream Cheese","جبنة كريمية"),        MaterialUnit.Gram,  0.020m),
            (new("Strawberries","فراولة"),             MaterialUnit.Gram,  0.015m),
            (new("Blueberries","توت أزرق"),            MaterialUnit.Gram,  0.020m),
            (new("Whipping Cream","كريمة خفق"),        MaterialUnit.Gram,  0.015m),
            (new("Biscuit Base","قاعدة بسكويت"),       MaterialUnit.Gram,  0.010m),
            (new("Honey","عسل"),                       MaterialUnit.Gram,  0.015m),
            (new("Pistachio","فستق حلبي"),             MaterialUnit.Gram,  0.030m),
            (new("Mascarpone","ماسكربوني"),            MaterialUnit.Gram,  0.030m),
        };

        // ================== Additional Materials (5 per tenant) ==================
        private static List<AdditionalMaterial> BuildAdditionalMaterialsForTenant(int tenantId, string theme)
        {
            var templates = theme.ToLowerInvariant() switch
            {
                "burgers" => new[]
                {
                    (new Localized("BBQ Sauce Cup","كوب باربكيو"),        MaterialUnit.Gram,  0.006m),
                    (new Localized("Ketchup Sachet","كيس كاتشب"),          MaterialUnit.Piece, 0.050m),
                    (new Localized("Mayonnaise Cup","كوب مايونيز"),        MaterialUnit.Gram,  0.005m),
                    (new Localized("Extra Cheese","جبنة إضافية"),          MaterialUnit.Gram,  0.015m),
                    (new Localized("Cola Can","مشروب غازي"),               MaterialUnit.Piece, 0.500m),
                },
                "pizza" => new[]
                {
                    (new Localized("Garlic Dip","صلصة ثومية"),              MaterialUnit.Gram,  0.008m),
                    (new Localized("Chili Flakes Pack","كيس فلفل مجروش"),  MaterialUnit.Piece, 0.070m),
                    (new Localized("Ranch Dip","صلصة رانش"),                MaterialUnit.Gram,  0.010m),
                    (new Localized("Soft Drink","مشروب غازي"),              MaterialUnit.Piece, 0.500m),
                    (new Localized("Extra Parmesan","بارميزان إضافي"),      MaterialUnit.Gram,  0.020m),
                },
                "shawarma" => new[]
                {
                    (new Localized("Garlic Sauce Cup","كوب صوص ثوم"),      MaterialUnit.Gram,  0.006m),
                    (new Localized("Tahini Cup","كوب طحينة"),              MaterialUnit.Gram,  0.008m),
                    (new Localized("Chili Sauce Cup","كوب صوص حار"),       MaterialUnit.Gram,  0.008m),
                    (new Localized("Cola Can","مشروب غازي"),               MaterialUnit.Piece, 0.500m),
                    (new Localized("Pickles Side","مخللات إضافية"),         MaterialUnit.Gram,  0.004m),
                },
                "coffee&tea" => new[]
                {
                    (new Localized("Extra Shot","شوت إضافي"),               MaterialUnit.Gram,  0.030m),
                    (new Localized("Vanilla Syrup Pump","ضغطة فانيلا"),     MaterialUnit.Gram,  0.015m),
                    (new Localized("Caramel Syrup Pump","ضغطة كراميل"),     MaterialUnit.Gram,  0.015m),
                    (new Localized("Whipped Cream Top","طبقة كريمة"),       MaterialUnit.Gram,  0.015m),
                    (new Localized("Cookie","بسكويت"),                      MaterialUnit.Piece, 0.300m),
                },
                _ => new[]
                {
                    (new Localized("Chocolate Sauce","صلصة شوكولاتة"),      MaterialUnit.Gram,  0.012m),
                    (new Localized("Strawberry Sauce","صلصة فراولة"),       MaterialUnit.Gram,  0.012m),
                    (new Localized("Vanilla Ice Cream","بولة فانيلا"),      MaterialUnit.Piece, 1.000m),
                    (new Localized("Pistachio Sprinkle","رشة فستق"),        MaterialUnit.Gram,  0.025m),
                    (new Localized("Water Bottle","ماء"),                   MaterialUnit.Piece, 0.200m),
                }
            };

            return templates.Select((t, idx) => new AdditionalMaterial
            {
                Id = DG(tenantId, "AdditionalMaterial", idx),
                TenantId = tenantId,
                CreatedBy = SeedCreatedBy,
                CreatedDate = SeedCreatedAt,
                IsDeleted = false,

                NameEn = t.Item1.En,
                NameAr = t.Item1.Ar,
                Unit = t.Item2,
                PricePerUnit = R3(t.Item3),
                Tax = TaxRate,
                ImageUrl = BuildMockUrl(tenantId, "additional-material", t.Item1.En),
                IsActive = true
            }).ToList();
        }

        // ================== Combos (>= 4) ==================
        private static (List<Combo> combos, List<ComboMaterial> links) BuildCombosForTenant(int tenantId, string theme, List<Material> materials)
        {
            Material Pick(string name) =>
                materials.First(m => m.NameEn.Equals(name, StringComparison.OrdinalIgnoreCase));

            var defs = theme.ToLowerInvariant() switch
            {
                "burgers" => new[]
                {
                    (En:"Classic Burger Meal", Ar:"وجبة برجر كلاسيك", Parts:new[]{ "Brioche Bun","Beef Patty","Cheddar Slice","Lettuce","Tomato","Fries" }),
                    (En:"BBQ Burger Meal",     Ar:"وجبة برجر باربكيو", Parts:new[]{ "Sesame Bun","Beef Patty","Onion","Fries" }),
                    (En:"Chicken Burger Meal", Ar:"وجبة برجر دجاج",    Parts:new[]{ "Sesame Bun","Chicken Fillet","Pickles","Fries" }),
                    (En:"Double Cheese Meal",  Ar:"وجبة دبل تشيز",     Parts:new[]{ "Sesame Bun","Beef Patty","Swiss Cheese","Cheddar Slice","Fries" }),
                },
                "pizza" => new[]
                {
                    (En:"Margherita Pizza",  Ar:"بيتزا مارجريتا", Parts:new[]{ "Pizza Dough","Tomato Sauce","Mozzarella","Basil" }),
                    (En:"Pepperoni Pizza",   Ar:"بيتزا بيبروني",   Parts:new[]{ "Pizza Dough","Tomato Sauce","Mozzarella","Pepperoni" }),
                    (En:"BBQ Chicken Pizza", Ar:"بيتزا دجاج باربكيو", Parts:new[]{ "Pizza Dough","Tomato Sauce","Mozzarella","Chicken Topping" }),
                    (En:"Veggie Pizza",      Ar:"بيتزا خضار",     Parts:new[]{ "Pizza Dough","Tomato Sauce","Mozzarella","Mushrooms","Bell Pepper","Black Olives","Onion" }),
                },
                "shawarma" => new[]
                {
                    (En:"Chicken Shawarma Sandwich", Ar:"شاورما دجاج ساندويتش", Parts:new[]{ "Pita Bread","Chicken Strips","Pickles","Fries" }),
                    (En:"Beef Shawarma Plate",       Ar:"صحن شاورما لحم",        Parts:new[]{ "Beef Strips","Hummus","Tomato" }),
                    (En:"Arabic Shawarma",           Ar:"شاورما عربي",           Parts:new[]{ "Arabic Bread","Chicken Strips","Pickles","Fries" }),
                    (En:"Mix Shawarma Plate",        Ar:"شاورما مكس",            Parts:new[]{ "Pita Bread","Chicken Strips","Beef Strips","Tomato" }),
                },
                "coffee&tea" => new[]
                {
                    (En:"Caffè Latte",        Ar:"كافيه لاتيه",       Parts:new[]{ "Espresso","Milk" }),
                    (En:"Cappuccino",         Ar:"كابتشينو",          Parts:new[]{ "Espresso","Milk","Milk Foam" }),
                    (En:"Mocha",              Ar:"موكا",              Parts:new[]{ "Espresso","Milk","Cocoa Powder" }),
                    (En:"Flat White",         Ar:"فلات وايت",         Parts:new[]{ "Espresso","Milk" }),
                },
                _ => new[]
                {
                    (En:"Chocolate Cake Slice", Ar:"شريحة كيك شوكولاتة", Parts:new[]{ "Flour","Cocoa Powder","Butter","Eggs" }),
                    (En:"Cheesecake",           Ar:"تشيزكيك",             Parts:new[]{ "Cream Cheese","Biscuit Base","Strawberries" }),
                    (En:"Fruit Tart",           Ar:"تارت فواكه",           Parts:new[]{ "Strawberries","Blueberries","Whipping Cream","Flour" }),
                    (En:"Tiramisu Cup",         Ar:"تيراميسو كوب",         Parts:new[]{ "Mascarpone","Cocoa Powder","Milk","Biscuit Base" }),
                }
            };

            var combos = new List<Combo>();
            var links = new List<ComboMaterial>();

            for (int i = 0; i < defs.Length; i++)
            {
                var def = defs[i];
                var parts = def.Parts.Select(Pick).ToArray();

                var baseSum = parts.Sum(p => p.PricePerUnit);
                var price = R3(baseSum * 1.150m); // منطق بيع فوق التكلفة

                var comboId = DG(tenantId, "Combo", i);

                combos.Add(new Combo
                {
                    Id = comboId,
                    TenantId = tenantId,
                    CreatedBy = SeedCreatedBy,
                    CreatedDate = SeedCreatedAt,
                    IsDeleted = false,

                    NameEn = def.En,
                    NameAr = def.Ar,
                    Price = price,
                    ImageUrl = BuildMockUrl(tenantId, "combo", def.En),
                    IsActive = true,
                    Tax = TaxRate
                });

                for (int j = 0; j < parts.Length; j++)
                {
                    links.Add(new ComboMaterial
                    {
                        Id = DG(tenantId, "ComboMaterial", i * 10 + j),
                        TenantId = tenantId,
                        CreatedBy = SeedCreatedBy,
                        CreatedDate = SeedCreatedAt,
                        IsDeleted = false,

                        ComboId = comboId,
                        MaterialId = parts[j].Id,
                        Quantity = 1
                    });
                }
            }

            return (combos, links);
        }

        // ================== Orders (3 per tenant) ==================
        private static (List<Order> orders, List<OrderItem> items) BuildOrdersForTenant(
            int tenantId,
            List<Material> materials,
            List<AdditionalMaterial> additions,
            List<Combo> combos)
        {
            var orders = new List<Order>();
            var items = new List<OrderItem>();

            var customers = new (string Name, string Phone)[]
            {
                ("Ahmed Ali",    "0790000001"),
                ("Sara Hussein", "0790000002"),
                ("Omar Nasser",  "0790000003"),
            };

            Material PickMat(int i) => materials[i % materials.Count];
            AdditionalMaterial PickAdd(int i) => additions[i % additions.Count];
            Combo PickCombo(int i) => combos[i % combos.Count];

            for (int i = 0; i < customers.Length; i++)
            {
                var combo = PickCombo(i);
                var mat = PickMat(i + 1);
                var add = PickAdd(i + 2);

                // كميات منطقية (1 أو 2)
                var qtyCombo = 1 + (i % 2);
                var qtyMat = 1 + ((i + 1) % 2);
                var qtyAdd = 1 + ((i + 2) % 2);

                // أسعار العناصر (قبل/ضريبة/بعد)
                var comboBefore = combo.Price * qtyCombo;
                var comboTax = R3(comboBefore * combo.Tax);
                var comboAfter = comboBefore + comboTax;

                var matBefore = mat.PricePerUnit * qtyMat;
                var matTax = R3(matBefore * mat.Tax);
                var matAfter = matBefore + matTax;

                var addBefore = add.PricePerUnit * qtyAdd;
                var addTax = R3(addBefore * add.Tax);
                var addAfter = addBefore + addTax;

                var totalBefore = comboBefore + matBefore + addBefore;
                var totalTax = comboTax + matTax + addTax;
                var totalAfter = totalBefore + totalTax;

                var orderId = DG(tenantId, "Order", i);

                orders.Add(new Order
                {
                    Id = orderId,
                    TenantId = tenantId,
                    CreatedBy = SeedCreatedBy,
                    CreatedDate = SeedCreatedAt,
                    IsDeleted = false,

                    CustomerName = customers[i].Name,
                    CustomerPhone = customers[i].Phone,
                    IsActive = true,
                    TotalPriceBeforeTax = R3(totalBefore),
                    TotalTax = R3(totalTax),
                    TotalPriceAfterTax = R3(totalAfter),
                    OrderDate = SeedCreatedAt
                });

                // Item: Combo
                items.Add(new OrderItem
                {
                    Id = DG(tenantId, "OrderItem", i * 100 + 1),
                    TenantId = tenantId,
                    CreatedBy = SeedCreatedBy,
                    CreatedDate = SeedCreatedAt,
                    IsDeleted = false,

                    OrderId = orderId,
                    Quantity = qtyCombo,
                    PriceBeforeTax = R3(comboBefore),
                    Tax = R3(comboTax),
                    PriceAfterTax = R3(comboAfter),

                    ComboId = combo.Id,
                    MaterialId = null,
                    AdditionalMaterialId = null
                });

                // Item: Material
                items.Add(new OrderItem
                {
                    Id = DG(tenantId, "OrderItem", i * 100 + 2),
                    TenantId = tenantId,
                    CreatedBy = SeedCreatedBy,
                    CreatedDate = SeedCreatedAt,
                    IsDeleted = false,

                    OrderId = orderId,
                    Quantity = qtyMat,
                    PriceBeforeTax = R3(matBefore),
                    Tax = R3(matTax),
                    PriceAfterTax = R3(matAfter),

                    ComboId = null,
                    MaterialId = mat.Id,
                    AdditionalMaterialId = null
                });

                // Item: Additional Material
                items.Add(new OrderItem
                {
                    Id = DG(tenantId, "OrderItem", i * 100 + 3),
                    TenantId = tenantId,
                    CreatedBy = SeedCreatedBy,
                    CreatedDate = SeedCreatedAt,
                    IsDeleted = false,

                    OrderId = orderId,
                    Quantity = qtyAdd,
                    PriceBeforeTax = R3(addBefore),
                    Tax = R3(addTax),
                    PriceAfterTax = R3(addAfter),

                    ComboId = null,
                    MaterialId = null,
                    AdditionalMaterialId = add.Id
                });
            }

            return (orders, items);
        }

        // ================== Deterministic GUID ==================
        private static Guid DG(int tenantId, string entity, int index)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes($"{tenantId}:{entity}:{index}"));
            return new Guid(bytes);
        }
    }
}
