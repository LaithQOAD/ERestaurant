using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERestaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalMaterial", x => x.Id);
                    table.UniqueConstraint("AK_AdditionalMaterial_Id_TenantId", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_AdditionalMaterial_TenantId_Id", x => new { x.TenantId, x.Id });
                    table.CheckConstraint("CK_AdditionalMaterial_PricePerUnit_GT0", "[PricePerUnit] > 0");
                    table.CheckConstraint("CK_AdditionalMaterial_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
                });

            migrationBuilder.CreateTable(
                name: "Combo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combo", x => x.Id);
                    table.UniqueConstraint("AK_Combo_Id_TenantId", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Combo_TenantId_Id", x => new { x.TenantId, x.Id });
                    table.CheckConstraint("CK_Combo_Price_GT0", "[Price] > 0");
                    table.CheckConstraint("CK_Combo_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                    table.UniqueConstraint("AK_Material_Id_TenantId", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Material_TenantId_Id", x => new { x.TenantId, x.Id });
                    table.CheckConstraint("CK_Material_PricePerUnit_GT0", "[PricePerUnit] > 0");
                    table.CheckConstraint("CK_Material_Tax_0_1", "[Tax] >= 0 AND [Tax] <= 1");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TotalPriceBeforeTax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TotalPriceAfterTax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.UniqueConstraint("AK_Order_Id_TenantId", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Order_TenantId_Id", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "ComboMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboMaterial", x => x.Id);
                    table.CheckConstraint("CK_ComboMaterial_Qty_GT0", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_ComboMaterial_Combo_ComboId_TenantId",
                        columns: x => new { x.ComboId, x.TenantId },
                        principalTable: "Combo",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboMaterial_Material_MaterialId_TenantId",
                        columns: x => new { x.MaterialId, x.TenantId },
                        principalTable: "Material",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceBeforeTax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    PriceAfterTax = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComboId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdditionalMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.CheckConstraint("CK_OrderItem_Qty_GT0", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_OrderItem_AdditionalMaterial_AdditionalMaterialId_TenantId",
                        columns: x => new { x.AdditionalMaterialId, x.TenantId },
                        principalTable: "AdditionalMaterial",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Combo_ComboId_TenantId",
                        columns: x => new { x.ComboId, x.TenantId },
                        principalTable: "Combo",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Material_MaterialId_TenantId",
                        columns: x => new { x.MaterialId, x.TenantId },
                        principalTable: "Material",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId_TenantId",
                        columns: x => new { x.OrderId, x.TenantId },
                        principalTable: "Order",
                        principalColumns: new[] { "Id", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdditionalMaterial",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "ImageUrl", "IsActive", "IsDeleted", "NameAr", "NameEn", "PricePerUnit", "Tax", "TenantId", "Unit", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0990f98e-5a05-27db-2d66-48418f559836"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/additional-material/bbq-sauce-cup", true, false, "كوب باربكيو", "BBQ Sauce Cup", 0.006m, 0.160m, 1, 2, null, null },
                    { new Guid("14b50fd0-72c5-89dd-7d54-c593ae93ceb1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/additional-material/extra-parmesan", true, false, "بارميزان إضافي", "Extra Parmesan", 0.020m, 0.160m, 2, 2, null, null },
                    { new Guid("159d7843-2143-562c-c89a-df5c21a2d3f2"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/additional-material/water-bottle", true, false, "ماء", "Water Bottle", 0.200m, 0.160m, 5, 1, null, null },
                    { new Guid("2612a2c4-94e9-093a-65ea-4c8339b3ab60"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/additional-material/chili-flakes-pack", true, false, "كيس فلفل مجروش", "Chili Flakes Pack", 0.070m, 0.160m, 2, 1, null, null },
                    { new Guid("2d31de49-5867-3101-ba7b-792e10492629"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/additional-material/pickles-side", true, false, "مخللات إضافية", "Pickles Side", 0.004m, 0.160m, 3, 2, null, null },
                    { new Guid("383f966b-1275-92d5-d63e-850bf939242c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/additional-material/extra-cheese", true, false, "جبنة إضافية", "Extra Cheese", 0.015m, 0.160m, 1, 2, null, null },
                    { new Guid("673fa6b8-a7cd-b730-1a00-565d8a2c4979"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/additional-material/cola-can", true, false, "مشروب غازي", "Cola Can", 0.500m, 0.160m, 1, 1, null, null },
                    { new Guid("6d905535-a7ad-b61a-ac9b-9712e3b440dd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/additional-material/chili-sauce-cup", true, false, "كوب صوص حار", "Chili Sauce Cup", 0.008m, 0.160m, 3, 2, null, null },
                    { new Guid("740804a5-4819-c426-fe3a-ba7e15c80de1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/additional-material/cookie", true, false, "بسكويت", "Cookie", 0.300m, 0.160m, 4, 1, null, null },
                    { new Guid("7495dc48-2430-21c3-d7b4-56fa2fe05e52"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/additional-material/caramel-syrup-pump", true, false, "ضغطة كراميل", "Caramel Syrup Pump", 0.015m, 0.160m, 4, 2, null, null },
                    { new Guid("7f4761f1-617d-1b64-3215-9744c2975aa2"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/additional-material/mayonnaise-cup", true, false, "كوب مايونيز", "Mayonnaise Cup", 0.005m, 0.160m, 1, 2, null, null },
                    { new Guid("860de8c8-5474-9e8b-4996-d674578fec15"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/additional-material/garlic-dip", true, false, "صلصة ثومية", "Garlic Dip", 0.008m, 0.160m, 2, 2, null, null },
                    { new Guid("a1a5ae43-d61b-72b7-7722-a9e4ebd8c0a0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/additional-material/tahini-cup", true, false, "كوب طحينة", "Tahini Cup", 0.008m, 0.160m, 3, 2, null, null },
                    { new Guid("a253b678-a717-5582-e7bb-ced83c2df33c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/additional-material/garlic-sauce-cup", true, false, "كوب صوص ثوم", "Garlic Sauce Cup", 0.006m, 0.160m, 3, 2, null, null },
                    { new Guid("aee669bd-f8eb-77d8-daac-1d566619aaa1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/additional-material/soft-drink", true, false, "مشروب غازي", "Soft Drink", 0.500m, 0.160m, 2, 1, null, null },
                    { new Guid("b0f6f398-4378-b53e-8d65-11232ad9aa69"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/additional-material/ketchup-sachet", true, false, "كيس كاتشب", "Ketchup Sachet", 0.050m, 0.160m, 1, 1, null, null },
                    { new Guid("b71d6a1e-d98f-4b77-5c5d-74db51069275"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/additional-material/chocolate-sauce", true, false, "صلصة شوكولاتة", "Chocolate Sauce", 0.012m, 0.160m, 5, 2, null, null },
                    { new Guid("b74272c9-a6a0-27d2-f440-5c49771f776d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/additional-material/vanilla-ice-cream", true, false, "بولة فانيلا", "Vanilla Ice Cream", 1.000m, 0.160m, 5, 1, null, null },
                    { new Guid("bd05a712-d837-9dd5-11cd-9a03a64f1644"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/additional-material/vanilla-syrup-pump", true, false, "ضغطة فانيلا", "Vanilla Syrup Pump", 0.015m, 0.160m, 4, 2, null, null },
                    { new Guid("c5668d60-4404-597e-49e5-a0579d8de503"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/additional-material/strawberry-sauce", true, false, "صلصة فراولة", "Strawberry Sauce", 0.012m, 0.160m, 5, 2, null, null },
                    { new Guid("d06ed5e7-d5a4-f2cd-852a-bd6ff3437e52"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/additional-material/extra-shot", true, false, "شوت إضافي", "Extra Shot", 0.030m, 0.160m, 4, 2, null, null },
                    { new Guid("d3dace19-f1f6-2326-ab9b-886022880409"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/additional-material/ranch-dip", true, false, "صلصة رانش", "Ranch Dip", 0.010m, 0.160m, 2, 2, null, null },
                    { new Guid("d5ddb405-bda3-312e-5487-c3ab449e43a5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/additional-material/whipped-cream-top", true, false, "طبقة كريمة", "Whipped Cream Top", 0.015m, 0.160m, 4, 2, null, null },
                    { new Guid("f4b49873-078d-4655-c85a-ba007d3727f8"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/additional-material/cola-can", true, false, "مشروب غازي", "Cola Can", 0.500m, 0.160m, 3, 1, null, null },
                    { new Guid("ff80e6a1-b675-674d-32ea-2384237dcf86"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/additional-material/pistachio-sprinkle", true, false, "رشة فستق", "Pistachio Sprinkle", 0.025m, 0.160m, 5, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Combo",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "ImageUrl", "IsActive", "IsDeleted", "NameAr", "NameEn", "Price", "Tax", "TenantId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/combo/bbq-burger-meal", true, false, "وجبة برجر باربكيو", "BBQ Burger Meal", 2.715m, 0.160m, 1, null, null },
                    { new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/combo/double-cheese-meal", true, false, "وجبة دبل تشيز", "Double Cheese Meal", 3.458m, 0.160m, 1, null, null },
                    { new Guid("23c24ec5-bfe2-bb93-a250-80a8b9c553f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/combo/beef-shawarma-plate", true, false, "صحن شاورما لحم", "Beef Shawarma Plate", 0.039m, 0.160m, 3, null, null },
                    { new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/combo/classic-burger-meal", true, false, "وجبة برجر كلاسيك", "Classic Burger Meal", 3.126m, 0.160m, 1, null, null },
                    { new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/combo/chocolate-cake-slice", true, false, "شريحة كيك شوكولاتة", "Chocolate Cake Slice", 0.316m, 0.160m, 5, null, null },
                    { new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/combo/chicken-shawarma-sandwich", true, false, "شاورما دجاج ساندويتش", "Chicken Shawarma Sandwich", 0.486m, 0.160m, 3, null, null },
                    { new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/combo/chicken-burger-meal", true, false, "وجبة برجر دجاج", "Chicken Burger Meal", 2.255m, 0.160m, 1, null, null },
                    { new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/combo/pepperoni-pizza", true, false, "بيتزا بيبروني", "Pepperoni Pizza", 1.078m, 0.160m, 2, null, null },
                    { new Guid("8aac1e89-a0b8-a9cb-a134-f22400f0666e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/combo/mix-shawarma-plate", true, false, "شاورما مكس", "Mix Shawarma Plate", 0.501m, 0.160m, 3, null, null },
                    { new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/combo/bbq-chicken-pizza", true, false, "بيتزا دجاج باربكيو", "BBQ Chicken Pizza", 1.072m, 0.160m, 2, null, null },
                    { new Guid("b40a1c27-0233-0129-656c-1b5881236ce5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/combo/cappuccino", true, false, "كابتشينو", "Cappuccino", 0.041m, 0.160m, 4, null, null },
                    { new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/combo/arabic-shawarma", true, false, "شاورما عربي", "Arabic Shawarma", 0.544m, 0.160m, 3, null, null },
                    { new Guid("b61394cf-d01d-a1cd-622b-59c77f493b15"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/combo/caffe-latte", true, false, "كافيه لاتيه", "Caffè Latte", 0.035m, 0.160m, 4, null, null },
                    { new Guid("cc47ce80-f259-eb2b-0284-e0479c03fad1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/combo/mocha", true, false, "موكا", "Mocha", 0.048m, 0.160m, 4, null, null },
                    { new Guid("ccdb7873-4647-94c9-7a0b-87b344a82f9c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/combo/tiramisu-cup", true, false, "تيراميسو كوب", "Tiramisu Cup", 0.066m, 0.160m, 5, null, null },
                    { new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/combo/fruit-tart", true, false, "تارت فواكه", "Fruit Tart", 0.059m, 0.160m, 5, null, null },
                    { new Guid("d4ce09b6-79af-697e-c72d-c77dadfc2b85"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/combo/flat-white", true, false, "فلات وايت", "Flat White", 0.035m, 0.160m, 4, null, null },
                    { new Guid("e3649000-060c-2ca2-8b35-ff540447740e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/combo/cheesecake", true, false, "تشيزكيك", "Cheesecake", 0.052m, 0.160m, 5, null, null },
                    { new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/combo/margherita-pizza", true, false, "بيتزا مارجريتا", "Margherita Pizza", 1.083m, 0.160m, 2, null, null },
                    { new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/combo/veggie-pizza", true, false, "بيتزا خضار", "Veggie Pizza", 1.093m, 0.160m, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Material",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "ImageUrl", "IsActive", "IsDeleted", "NameAr", "NameEn", "PricePerUnit", "Tax", "TenantId", "Unit", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("01b6018a-f77b-0ff0-36c5-99f9e2554aba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/hazelnut-syrup", true, false, "شراب بندق", "Hazelnut Syrup", 0.015m, 0.160m, 4, 2, null, null },
                    { new Guid("06d8fd70-254d-85ba-6d1f-2ba07310a1c2"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/pita-bread", true, false, "خبز عربي", "Pita Bread", 0.400m, 0.160m, 3, 1, null, null },
                    { new Guid("094e0e93-f114-2ce0-8393-c55343eb7314"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/strawberries", true, false, "فراولة", "Strawberries", 0.015m, 0.160m, 5, 2, null, null },
                    { new Guid("13c628de-04f2-47a1-3b6e-c36e10501e0f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/fries", true, false, "بطاطا مقلية", "Fries", 0.007m, 0.160m, 1, 2, null, null },
                    { new Guid("1556c112-5050-1683-1997-a7aa4c1af717"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/pickles", true, false, "مخلل", "Pickles", 0.004m, 0.160m, 1, 2, null, null },
                    { new Guid("1af9fe7b-7ae6-8665-eaf1-6d388fb8ee72"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/tomato", true, false, "طماطم", "Tomato", 0.006m, 0.160m, 1, 2, null, null },
                    { new Guid("23858376-a7d4-0f32-6701-fe9645c7a8f6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/chicken-fillet", true, false, "فيليه دجاج", "Chicken Fillet", 1.400m, 0.160m, 1, 1, null, null },
                    { new Guid("238f2777-73e8-da46-7099-7564dc8f9370"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/beef-patty", true, false, "لحم برجر بقري", "Beef Patty", 1.800m, 0.160m, 1, 1, null, null },
                    { new Guid("28e5ebc6-32a9-8e68-86bc-481df9ed6f13"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/bell-pepper", true, false, "فليفلة", "Bell Pepper", 0.009m, 0.160m, 2, 2, null, null },
                    { new Guid("2a276b43-acf3-ec8a-bafc-88ffdfc969f1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/cream-cheese", true, false, "جبنة كريمية", "Cream Cheese", 0.020m, 0.160m, 2, 2, null, null },
                    { new Guid("2c3e51d7-5d6f-36f1-b8a3-dd240f7c2635"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/arabic-bread", true, false, "خبز شراك", "Arabic Bread", 0.450m, 0.160m, 3, 1, null, null },
                    { new Guid("2e5a334d-7083-22dc-1d68-e3d07cf946e0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/oregano", true, false, "أوريغانو", "Oregano", 0.020m, 0.160m, 2, 2, null, null },
                    { new Guid("3adcc7a3-9d9c-f4af-c9fd-130338b556ea"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/arabica-beans", true, false, "قهوة عربية", "Arabica Beans", 0.018m, 0.160m, 4, 2, null, null },
                    { new Guid("3b571624-798f-22ff-07ce-61bf1470bfc5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/cucumber", true, false, "خيار", "Cucumber", 0.005m, 0.160m, 3, 2, null, null },
                    { new Guid("3cdb8db2-23f5-41c6-dba7-b63a173f53c3"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/cocoa-powder", true, false, "كاكاو بودرة", "Cocoa Powder", 0.012m, 0.160m, 5, 2, null, null },
                    { new Guid("425c4740-2192-0c65-c246-5f8dbcd6ab65"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/mushrooms", true, false, "فطر", "Mushrooms", 0.010m, 0.160m, 2, 2, null, null },
                    { new Guid("49b406d2-5b65-0e2c-5873-c9aa1206ce72"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/pickles", true, false, "مخللات", "Pickles", 0.004m, 0.160m, 3, 2, null, null },
                    { new Guid("4bcb4bee-2a19-363f-eb0c-a4a925a0e6a1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/chicken-strips", true, false, "شرائح دجاج", "Chicken Strips", 0.012m, 0.160m, 3, 2, null, null },
                    { new Guid("4e94443d-8098-62ea-5c52-3d06d568f540"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/beef-strips", true, false, "شرائح لحم", "Beef Strips", 0.018m, 0.160m, 3, 2, null, null },
                    { new Guid("501446b9-b700-4d14-5581-a52e2c6209b4"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/blueberries", true, false, "توت أزرق", "Blueberries", 0.020m, 0.160m, 5, 2, null, null },
                    { new Guid("52defe2d-7a18-98da-0893-af7f5561cc0e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/bread-chips", true, false, "خبز محمص", "Bread Chips", 0.007m, 0.160m, 3, 2, null, null },
                    { new Guid("53dd5631-c753-5642-ea1d-fe357d64f566"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/paper-cups", true, false, "أكواب ورقية", "Paper Cups", 0.050m, 0.160m, 4, 1, null, null },
                    { new Guid("571e5ccd-58c7-256a-4bc0-5ef412a51814"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/chocolate-chips", true, false, "رقائق شوكولاتة", "Chocolate Chips", 0.020m, 0.160m, 4, 2, null, null },
                    { new Guid("5a5f6bb5-2155-6127-a894-c7e5b38e0a75"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/cocoa-powder", true, false, "كاكاو بودرة", "Cocoa Powder", 0.012m, 0.160m, 4, 2, null, null },
                    { new Guid("5b24bb41-5b05-9029-8a27-876a7a75358b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/lids", true, false, "أغطية أكواب", "Lids", 0.030m, 0.160m, 4, 1, null, null },
                    { new Guid("5e5b631b-e1ee-bc88-40dd-813490645f40"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/flour", true, false, "طحين", "Flour", 0.001m, 0.160m, 5, 2, null, null },
                    { new Guid("610b225a-18e9-6a16-a2b9-5246cb691024"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/mascarpone", true, false, "ماسكربوني", "Mascarpone", 0.030m, 0.160m, 5, 2, null, null },
                    { new Guid("6114f55f-bfa7-eaf3-c927-a590f64fb7d8"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/cinnamon", true, false, "قرفة", "Cinnamon", 0.010m, 0.160m, 4, 2, null, null },
                    { new Guid("637a520b-9c6d-21ad-73b9-fda1ce4a33e1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/tomato", true, false, "طماطم", "Tomato", 0.006m, 0.160m, 3, 2, null, null },
                    { new Guid("64d061a5-e883-71fd-ae87-332cfea498da"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/sumac", true, false, "سماق", "Sumac", 0.020m, 0.160m, 3, 2, null, null },
                    { new Guid("69695b27-1fa1-1cc3-d357-b9c9557e11d4"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/milk", true, false, "حليب", "Milk", 0.005m, 0.160m, 5, 2, null, null },
                    { new Guid("6aa1f09c-29bd-5117-d132-e299622cdd02"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/honey", true, false, "عسل", "Honey", 0.015m, 0.160m, 5, 2, null, null },
                    { new Guid("714f237b-e822-1e2e-ec09-7b52721d1a0e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/bacon-bits", true, false, "لحم مقدد مفروم", "Bacon Bits", 0.025m, 0.160m, 1, 2, null, null },
                    { new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/tomato-sauce", true, false, "صلصة طماطم", "Tomato Sauce", 0.005m, 0.160m, 2, 2, null, null },
                    { new Guid("73540134-8f35-bff6-85c9-df6905a0eb51"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/eggs", true, false, "بيض", "Eggs", 0.250m, 0.160m, 5, 1, null, null },
                    { new Guid("73dbc344-86e4-4f0b-5a65-7e732af128f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/hummus", true, false, "حمص", "Hummus", 0.010m, 0.160m, 3, 2, null, null },
                    { new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/milk", true, false, "حليب", "Milk", 0.005m, 0.160m, 4, 2, null, null },
                    { new Guid("8008525a-2905-c01c-d91d-9abe1a2ca16a"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/jalapenos", true, false, "هالبينو", "Jalapenos", 0.010m, 0.160m, 1, 2, null, null },
                    { new Guid("82e0ef06-6f4f-398c-6044-63a7f7c6eb3a"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/olive-oil", true, false, "زيت زيتون", "Olive Oil", 0.015m, 0.160m, 2, 2, null, null },
                    { new Guid("8668a7a4-fb4e-e933-2679-b8e80cfc3136"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/sugar", true, false, "سكر", "Sugar", 0.002m, 0.160m, 5, 2, null, null },
                    { new Guid("88ae16a5-7b7c-ec9c-a747-ef9668399f8a"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/pizza-dough", true, false, "عجين بيتزا", "Pizza Dough", 0.900m, 0.160m, 2, 1, null, null },
                    { new Guid("910f5d24-c557-697d-1980-1702197bf19d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/ice", true, false, "ثلج", "Ice", 0.001m, 0.160m, 4, 2, null, null },
                    { new Guid("911b8e0e-c7e0-e819-7a79-5aabc7ed7320"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/lemon-juice", true, false, "عصير ليمون", "Lemon Juice", 0.015m, 0.160m, 3, 2, null, null },
                    { new Guid("94a8cbae-bd34-96b4-6085-4b9548375bc2"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/espresso", true, false, "إسبريسو", "Espresso", 0.025m, 0.160m, 4, 2, null, null },
                    { new Guid("95cddd82-63c9-68d4-14ea-034c7c780f8e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/green-tea", true, false, "شاي أخضر", "Green Tea", 0.006m, 0.160m, 4, 2, null, null },
                    { new Guid("b1205177-eaa7-8a32-e295-2216cbdf4cae"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/pepperoni", true, false, "بيبروني", "Pepperoni", 0.020m, 0.160m, 2, 2, null, null },
                    { new Guid("b22b0862-e2c5-8a00-0e08-60530a848d9f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/basil", true, false, "ريحان", "Basil", 0.025m, 0.160m, 2, 2, null, null },
                    { new Guid("ba355d1a-fa6a-90f3-d3cf-dbde681c839d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/onion", true, false, "بصل", "Onion", 0.004m, 0.160m, 1, 2, null, null },
                    { new Guid("c1e82f71-471f-3dd8-24d7-253f57a5c65f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/onion", true, false, "بصل", "Onion", 0.004m, 0.160m, 2, 2, null, null },
                    { new Guid("c1f5ace9-452a-c23a-0004-92023e3ce5ce"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/sesame-bun", true, false, "خبز سمسم", "Sesame Bun", 0.550m, 0.160m, 1, 1, null, null },
                    { new Guid("c48078e5-8d1d-eac8-7508-cc37d199d328"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/cabbage", true, false, "ملفوف", "Cabbage", 0.006m, 0.160m, 3, 2, null, null },
                    { new Guid("c4d405fc-61a1-cc50-59fd-2c514502228c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/butter", true, false, "زبدة", "Butter", 0.012m, 0.160m, 5, 2, null, null },
                    { new Guid("c7569fe6-37b7-af71-2424-112bd2f5b299"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/swiss-cheese", true, false, "جبنة سويسرية", "Swiss Cheese", 0.350m, 0.160m, 1, 1, null, null },
                    { new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/mozzarella", true, false, "موزاريلا", "Mozzarella", 0.012m, 0.160m, 2, 2, null, null },
                    { new Guid("c88feadc-eeb2-be58-5fbb-684f00e0c8f4"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/lettuce", true, false, "خس", "Lettuce", 0.005m, 0.160m, 1, 2, null, null },
                    { new Guid("cb841c99-138c-e2c2-bb34-dccd9da72144"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/biscuit-base", true, false, "قاعدة بسكويت", "Biscuit Base", 0.010m, 0.160m, 5, 2, null, null },
                    { new Guid("cbd2b759-27f4-7e0c-9ef2-3af07489608b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/sweet-corn", true, false, "ذرة", "Sweet Corn", 0.007m, 0.160m, 2, 2, null, null },
                    { new Guid("cfdde016-0fc0-ad61-ed71-03a819df623d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/cream-cheese", true, false, "جبنة كريمية", "Cream Cheese", 0.020m, 0.160m, 5, 2, null, null },
                    { new Guid("d3d115c3-43be-b89c-46f0-b083c8c5c8c9"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/chicken-topping", true, false, "دجاج للبيتزا", "Chicken Topping", 0.015m, 0.160m, 2, 2, null, null },
                    { new Guid("d4fe6114-8e8a-8dcb-f7a0-2d7e089786f4"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/onion", true, false, "بصل", "Onion", 0.004m, 0.160m, 3, 2, null, null },
                    { new Guid("dc1bdcdf-c464-507c-dea0-4e07e636eaf8"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/parsley", true, false, "بقدونس", "Parsley", 0.008m, 0.160m, 3, 2, null, null },
                    { new Guid("dc365f35-2b0c-4ea0-3dd5-44c6eddcfa7b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/mint", true, false, "نعناع", "Mint", 0.015m, 0.160m, 4, 2, null, null },
                    { new Guid("e0220e50-7ef4-78c0-c152-22d7fc6b9709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/whipping-cream", true, false, "كريمة خفق", "Whipping Cream", 0.015m, 0.160m, 5, 2, null, null },
                    { new Guid("e050e211-04e4-e3df-8eab-492b80d43096"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/pistachio", true, false, "فستق حلبي", "Pistachio", 0.030m, 0.160m, 5, 2, null, null },
                    { new Guid("e0a740b1-1e96-6048-541e-df4f10730fcc"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/milk-foam", true, false, "رغوة حليب", "Milk Foam", 0.006m, 0.160m, 4, 2, null, null },
                    { new Guid("e10fe6a6-a387-4a47-e12b-be81a53b19b1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/brioche-bun", true, false, "خبز بريوش", "Brioche Bun", 0.600m, 0.160m, 1, 1, null, null },
                    { new Guid("e1df13e4-4fc1-91d4-7517-c201a9b43ad8"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/brown-sugar", true, false, "سكر بني", "Brown Sugar", 0.003m, 0.160m, 4, 2, null, null },
                    { new Guid("e665d270-525b-7cbc-ed58-78f4b627e2b4"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/parmesan", true, false, "بارميزان", "Parmesan", 0.018m, 0.160m, 2, 2, null, null },
                    { new Guid("e666c887-9eff-6be4-be39-49f27509fea9"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/3/material/fries", true, false, "بطاطا مقلية", "Fries", 0.007m, 0.160m, 3, 2, null, null },
                    { new Guid("e7697cd5-d8fd-fe8f-1eba-37dd1c57f836"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/4/material/water", true, false, "ماء", "Water", 0.200m, 0.160m, 4, 1, null, null },
                    { new Guid("eb8fa1cf-a00e-e9c7-398e-ff0a663c745f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/5/material/baking-powder", true, false, "بيكنج باودر", "Baking Powder", 0.006m, 0.160m, 5, 2, null, null },
                    { new Guid("ee1b714b-229b-5e70-69c0-9388ae4fdab6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/2/material/black-olives", true, false, "زيتون أسود", "Black Olives", 0.010m, 0.160m, 2, 2, null, null },
                    { new Guid("f2b5deb5-f1b2-de9a-b562-29c8b3f9b584"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/coleslaw", true, false, "سلطة ملفوف", "Coleslaw", 0.008m, 0.160m, 1, 2, null, null },
                    { new Guid("fefa1d74-92a2-e5f1-7af7-5df004c6e73a"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/cheddar-slice", true, false, "شريحة شيدر", "Cheddar Slice", 0.300m, 0.160m, 1, 1, null, null },
                    { new Guid("ff344306-81ff-df1a-4121-d18dfe25b2cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, "https://www.ERestaurant.com/1/material/mushrooms", true, false, "فطر", "Mushrooms", 0.010m, 0.160m, 1, 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "CustomerName", "CustomerPhone", "DeletedBy", "DeletedDate", "IsActive", "IsDeleted", "OrderDate", "TenantId", "TotalPriceAfterTax", "TotalPriceBeforeTax", "TotalTax", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("23fb143c-909e-5b13-f97b-29e3715949de"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Ahmed Ali", "0790000001", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 2, 1.280m, 1.103m, 0.177m, null, null },
                    { new Guid("40a04b89-4927-de74-10b1-c1c3cb72d63d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Omar Nasser", "0790000003", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 2, 1.313m, 1.132m, 0.181m, null, null },
                    { new Guid("52599bb3-14ed-5971-e68e-1611728069eb"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Ahmed Ali", "0790000001", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 4, 0.070m, 0.060m, 0.010m, null, null },
                    { new Guid("66b67a75-5dc1-26fc-2e18-571408b61607"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Omar Nasser", "0790000003", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 4, 0.446m, 0.384m, 0.062m, null, null },
                    { new Guid("7514793f-8aed-2fa2-568f-9472dd282595"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Omar Nasser", "0790000003", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 3, 0.678m, 0.584m, 0.094m, null, null },
                    { new Guid("80f96d44-678a-abe9-de93-7879203b2d90"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Omar Nasser", "0790000003", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 5, 0.312m, 0.269m, 0.043m, null, null },
                    { new Guid("825abaf3-539a-2cc4-905a-b5e6645e800f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Omar Nasser", "0790000003", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 1, 6.444m, 5.555m, 0.889m, null, null },
                    { new Guid("966eb05c-0663-a48f-1c77-b551e688ff85"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Sara Hussein", "0790000002", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 3, 1.264m, 1.090m, 0.174m, null, null },
                    { new Guid("9c351c71-b5a0-d440-a58d-9826ca98b36e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Ahmed Ali", "0790000001", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 5, 1.532m, 1.320m, 0.212m, null, null },
                    { new Guid("a96c1d60-7f59-87c4-f8c8-a05dfc510ab5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Sara Hussein", "0790000002", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 1, 8.422m, 7.260m, 1.162m, null, null },
                    { new Guid("b8a6a30e-68c3-104f-3298-f32f10b52ec0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Ahmed Ali", "0790000001", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 1, 4.908m, 4.231m, 0.677m, null, null },
                    { new Guid("cf54e7b3-dc3c-df47-5817-d42960580d6f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Ahmed Ali", "0790000001", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 3, 1.617m, 1.394m, 0.223m, null, null },
                    { new Guid("d6cda97f-7137-abdb-487c-5dab9b73c5cb"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Sara Hussein", "0790000002", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 5, 0.469m, 0.404m, 0.065m, null, null },
                    { new Guid("def518c8-a3c0-3659-05bc-d1a0192b486d"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Sara Hussein", "0790000002", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 2, 3.675m, 3.168m, 0.507m, null, null },
                    { new Guid("ffe6e861-805e-6065-3ccb-626aa1b3d840"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), "Sara Hussein", "0790000002", null, null, true, false, new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), 4, 0.137m, 0.118m, 0.019m, null, null }
                });

            migrationBuilder.InsertData(
                table: "ComboMaterial",
                columns: new[] { "Id", "ComboId", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "MaterialId", "Quantity", "TenantId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0059d9cc-a74b-3b6e-6541-0430a7bc80d4"), new Guid("b61394cf-d01d-a1cd-622b-59c77f493b15"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), 1, 4, null, null },
                    { new Guid("0222e1d7-5a4f-0c55-59ea-b8a04039950c"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("425c4740-2192-0c65-c246-5f8dbcd6ab65"), 1, 2, null, null },
                    { new Guid("05c96aa4-95b4-6558-5fa2-85475a9e7b3b"), new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c4d405fc-61a1-cc50-59fd-2c514502228c"), 1, 5, null, null },
                    { new Guid("08bb7f2b-82fb-14be-6137-a03fdf8ec87c"), new Guid("cc47ce80-f259-eb2b-0284-e0479c03fad1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("94a8cbae-bd34-96b4-6085-4b9548375bc2"), 1, 4, null, null },
                    { new Guid("0aa67155-afbe-340d-c9fc-5293e1aac710"), new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), 1, 2, null, null },
                    { new Guid("0b2f8c3e-42db-b220-725c-7a28c066bfab"), new Guid("b40a1c27-0233-0129-656c-1b5881236ce5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), 1, 4, null, null },
                    { new Guid("0dfeb6f8-e4d0-23cf-24eb-8e72f3cb496a"), new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("d3d115c3-43be-b89c-46f0-b083c8c5c8c9"), 1, 2, null, null },
                    { new Guid("11657c2e-ea79-c4d3-ccee-66c624ccb0a7"), new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("238f2777-73e8-da46-7099-7564dc8f9370"), 1, 1, null, null },
                    { new Guid("1339cb16-bc25-6647-fcb1-b024460f7fa2"), new Guid("23c24ec5-bfe2-bb93-a250-80a8b9c553f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4e94443d-8098-62ea-5c52-3d06d568f540"), 1, 3, null, null },
                    { new Guid("158d05f8-4390-b6b8-e515-e1b3e5a2a4f2"), new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("238f2777-73e8-da46-7099-7564dc8f9370"), 1, 1, null, null },
                    { new Guid("214c2c08-54ad-bbe9-ea5b-18ebab09f715"), new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), 1, 2, null, null },
                    { new Guid("283d808e-3077-5e5c-e4ed-d056d4e317ef"), new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4bcb4bee-2a19-363f-eb0c-a4a925a0e6a1"), 1, 3, null, null },
                    { new Guid("29c401ca-0bb0-e43b-2115-dc32f97b7c03"), new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("49b406d2-5b65-0e2c-5873-c9aa1206ce72"), 1, 3, null, null },
                    { new Guid("2ec98587-e6f7-0f8b-e23f-ed7dee7e880c"), new Guid("e3649000-060c-2ca2-8b35-ff540447740e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("cb841c99-138c-e2c2-bb34-dccd9da72144"), 1, 5, null, null },
                    { new Guid("2f4ef6c0-4933-4ab9-2847-1a95bb298cf0"), new Guid("d4ce09b6-79af-697e-c72d-c77dadfc2b85"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), 1, 4, null, null },
                    { new Guid("30be877a-e950-1996-bb83-00cc39a22037"), new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c1f5ace9-452a-c23a-0004-92023e3ce5ce"), 1, 1, null, null },
                    { new Guid("30dbac5b-3006-f7a2-a009-a08d73be440b"), new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e666c887-9eff-6be4-be39-49f27509fea9"), 1, 3, null, null },
                    { new Guid("312ea682-29f0-1410-721b-4e9e6e8dd461"), new Guid("8aac1e89-a0b8-a9cb-a134-f22400f0666e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("637a520b-9c6d-21ad-73b9-fda1ce4a33e1"), 1, 3, null, null },
                    { new Guid("35b3ee80-c95e-1499-2980-cb64cf730a1f"), new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), 1, 2, null, null },
                    { new Guid("39eba4d4-197d-5a7f-0c73-312769f56c3f"), new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e666c887-9eff-6be4-be39-49f27509fea9"), 1, 3, null, null },
                    { new Guid("475eb5b1-dd6a-8cd7-42d1-59de6a40027f"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c88feadc-eeb2-be58-5fbb-684f00e0c8f4"), 1, 1, null, null },
                    { new Guid("47613f2d-6db0-c5e7-dff8-c1930d32df3e"), new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("3cdb8db2-23f5-41c6-dba7-b63a173f53c3"), 1, 5, null, null },
                    { new Guid("49358342-aedb-09df-58c3-2bc02d058e56"), new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c1f5ace9-452a-c23a-0004-92023e3ce5ce"), 1, 1, null, null },
                    { new Guid("4b0d865a-afbd-7817-dd0b-d47afe8b26a9"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e10fe6a6-a387-4a47-e12b-be81a53b19b1"), 1, 1, null, null },
                    { new Guid("4bf34310-43f9-f9d4-7137-62a6d9c73536"), new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("88ae16a5-7b7c-ec9c-a747-ef9668399f8a"), 1, 2, null, null },
                    { new Guid("4f070718-6bd6-c126-34b7-d6a75c686e90"), new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("5e5b631b-e1ee-bc88-40dd-813490645f40"), 1, 5, null, null },
                    { new Guid("50afb674-cecf-17cf-fbce-0e72256553fa"), new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("ba355d1a-fa6a-90f3-d3cf-dbde681c839d"), 1, 1, null, null },
                    { new Guid("50d0b801-77f5-34b4-44db-9f88cc5e1ee0"), new Guid("b40a1c27-0233-0129-656c-1b5881236ce5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("94a8cbae-bd34-96b4-6085-4b9548375bc2"), 1, 4, null, null },
                    { new Guid("5d1dcc5c-90ab-7f55-4302-aaa2dbb1fdd4"), new Guid("e3649000-060c-2ca2-8b35-ff540447740e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("094e0e93-f114-2ce0-8393-c55343eb7314"), 1, 5, null, null },
                    { new Guid("630da10e-3019-87aa-1bb1-44eb15a10b67"), new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("fefa1d74-92a2-e5f1-7af7-5df004c6e73a"), 1, 1, null, null },
                    { new Guid("6388bfac-930a-e18e-e3c9-31ce3b7c48f3"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("ee1b714b-229b-5e70-69c0-9388ae4fdab6"), 1, 2, null, null },
                    { new Guid("72c9175a-e036-c6f1-7f64-30f08dc98088"), new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("1556c112-5050-1683-1997-a7aa4c1af717"), 1, 1, null, null },
                    { new Guid("78e94b8a-6f6d-753a-8c46-62f637b2b535"), new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), 1, 2, null, null },
                    { new Guid("7d2fea36-96df-3345-d448-18e634e8a466"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("88ae16a5-7b7c-ec9c-a747-ef9668399f8a"), 1, 2, null, null },
                    { new Guid("7d7f5170-cbae-99c8-9e97-20117ff4ee51"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("238f2777-73e8-da46-7099-7564dc8f9370"), 1, 1, null, null },
                    { new Guid("7e9d5d4c-5b40-d0e4-f4ad-ac2601c8f04b"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("28e5ebc6-32a9-8e68-86bc-481df9ed6f13"), 1, 2, null, null },
                    { new Guid("81e3ca62-b8b5-8bce-d513-793307be8a34"), new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("88ae16a5-7b7c-ec9c-a747-ef9668399f8a"), 1, 2, null, null },
                    { new Guid("86e0b5b3-e734-967a-2238-e19bdc00477b"), new Guid("cc47ce80-f259-eb2b-0284-e0479c03fad1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), 1, 4, null, null },
                    { new Guid("8d5d11be-4d90-9eae-1125-7aac542aad34"), new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4bcb4bee-2a19-363f-eb0c-a4a925a0e6a1"), 1, 3, null, null },
                    { new Guid("8fbcecd8-5392-9e3f-47a0-ea5ed50c182f"), new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("b1205177-eaa7-8a32-e295-2216cbdf4cae"), 1, 2, null, null },
                    { new Guid("8fe039cc-2d59-2445-3932-e9fb1755d63a"), new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("06d8fd70-254d-85ba-6d1f-2ba07310a1c2"), 1, 3, null, null },
                    { new Guid("9aac9060-e2f5-71d9-4d34-22d76b0b6d4d"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("fefa1d74-92a2-e5f1-7af7-5df004c6e73a"), 1, 1, null, null },
                    { new Guid("9e812d9a-eae2-be60-7259-964310898348"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), 1, 2, null, null },
                    { new Guid("a1ca4e76-9bea-56b8-ea66-44b28ba95e96"), new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("13c628de-04f2-47a1-3b6e-c36e10501e0f"), 1, 1, null, null },
                    { new Guid("a2d4a73d-0193-28ce-4f01-12e506e2e301"), new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e0220e50-7ef4-78c0-c152-22d7fc6b9709"), 1, 5, null, null },
                    { new Guid("a2f3c3dd-3f74-a06b-c8a3-dc236cfae36b"), new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("2c3e51d7-5d6f-36f1-b8a3-dd240f7c2635"), 1, 3, null, null },
                    { new Guid("a45a4136-18c6-56cb-718e-0fd43508b5eb"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c1e82f71-471f-3dd8-24d7-253f57a5c65f"), 1, 2, null, null },
                    { new Guid("a9fd01f1-09a8-2d0d-258d-b9ff1d8f1075"), new Guid("8aac1e89-a0b8-a9cb-a134-f22400f0666e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("06d8fd70-254d-85ba-6d1f-2ba07310a1c2"), 1, 3, null, null },
                    { new Guid("b04a0c93-3906-8d24-d500-0f0bbddcdec5"), new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("49b406d2-5b65-0e2c-5873-c9aa1206ce72"), 1, 3, null, null },
                    { new Guid("b10ab4ea-c258-bd66-94ea-66d41082ebe2"), new Guid("ccdb7873-4647-94c9-7a0b-87b344a82f9c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("69695b27-1fa1-1cc3-d357-b9c9557e11d4"), 1, 5, null, null },
                    { new Guid("b3b23423-2696-a5a0-f8d2-7f660176822d"), new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("094e0e93-f114-2ce0-8393-c55343eb7314"), 1, 5, null, null },
                    { new Guid("b589fd1c-3cf7-4b1f-c0b1-5a56892663dd"), new Guid("ccdb7873-4647-94c9-7a0b-87b344a82f9c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("3cdb8db2-23f5-41c6-dba7-b63a173f53c3"), 1, 5, null, null },
                    { new Guid("b97ae74a-9c53-cc54-887b-dde0a28f1816"), new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), 1, 2, null, null },
                    { new Guid("bdc15d0c-21ce-018b-d95c-3bfe113acd11"), new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("b22b0862-e2c5-8a00-0e08-60530a848d9f"), 1, 2, null, null },
                    { new Guid("c39899a9-a880-0be9-c05a-06dc36a9a6d1"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("13c628de-04f2-47a1-3b6e-c36e10501e0f"), 1, 1, null, null },
                    { new Guid("c5a82cd5-0380-43ad-b7da-4cf841ce45da"), new Guid("8aac1e89-a0b8-a9cb-a134-f22400f0666e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4bcb4bee-2a19-363f-eb0c-a4a925a0e6a1"), 1, 3, null, null },
                    { new Guid("c628fb4e-5133-148d-0dde-7128e6857f4c"), new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c1f5ace9-452a-c23a-0004-92023e3ce5ce"), 1, 1, null, null },
                    { new Guid("c62fcf3e-5f22-b708-8d74-7a5da0c05a94"), new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("88ae16a5-7b7c-ec9c-a747-ef9668399f8a"), 1, 2, null, null },
                    { new Guid("cbb10d82-ba10-b366-58f5-3b9d2c60493b"), new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("501446b9-b700-4d14-5581-a52e2c6209b4"), 1, 5, null, null },
                    { new Guid("cc8c42fa-1c64-4e4b-2a5c-a5f6d4be734d"), new Guid("d4ce09b6-79af-697e-c72d-c77dadfc2b85"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("94a8cbae-bd34-96b4-6085-4b9548375bc2"), 1, 4, null, null },
                    { new Guid("d03f4be6-8684-fc8d-40bb-ead4f1f91c98"), new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("13c628de-04f2-47a1-3b6e-c36e10501e0f"), 1, 1, null, null },
                    { new Guid("d387eccd-52e0-222b-0c54-ece3bd9f1f29"), new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c7569fe6-37b7-af71-2424-112bd2f5b299"), 1, 1, null, null },
                    { new Guid("d6c633a4-e488-fad7-e8d4-806a8996834a"), new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("5e5b631b-e1ee-bc88-40dd-813490645f40"), 1, 5, null, null },
                    { new Guid("d884ee8e-1d7a-6020-3808-ccc16a81ecf6"), new Guid("ccdb7873-4647-94c9-7a0b-87b344a82f9c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("cb841c99-138c-e2c2-bb34-dccd9da72144"), 1, 5, null, null },
                    { new Guid("dcb0a179-09c4-9e99-fc7b-3719524fe6c5"), new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("73540134-8f35-bff6-85c9-df6905a0eb51"), 1, 5, null, null },
                    { new Guid("e1f6c099-e617-e32f-87a2-70e3c4080043"), new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("1af9fe7b-7ae6-8665-eaf1-6d388fb8ee72"), 1, 1, null, null },
                    { new Guid("e2acc9a4-e9ef-8dd9-dfc2-9c36f3d33c1a"), new Guid("fc613c0c-0c0f-626a-cf8b-25812f216709"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), 1, 2, null, null },
                    { new Guid("e7f7a308-99f0-9cad-f4bd-9c385abeda54"), new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), 1, 2, null, null },
                    { new Guid("e9c42e8e-baf7-889f-32a9-badd09d282b2"), new Guid("23c24ec5-bfe2-bb93-a250-80a8b9c553f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("73dbc344-86e4-4f0b-5a65-7e732af128f0"), 1, 3, null, null },
                    { new Guid("ec887898-0622-dd85-6ff1-867bee80e22b"), new Guid("b40a1c27-0233-0129-656c-1b5881236ce5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e0a740b1-1e96-6048-541e-df4f10730fcc"), 1, 4, null, null },
                    { new Guid("ee2a0b05-67f0-0e63-38b8-777619f6a928"), new Guid("8aac1e89-a0b8-a9cb-a134-f22400f0666e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4e94443d-8098-62ea-5c52-3d06d568f540"), 1, 3, null, null },
                    { new Guid("eec8e6bf-4ad9-7ad2-9966-6ee5fd5c3812"), new Guid("cc47ce80-f259-eb2b-0284-e0479c03fad1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("5a5f6bb5-2155-6127-a894-c7e5b38e0a75"), 1, 4, null, null },
                    { new Guid("f48b8c03-416f-53bb-c0d6-fa8194e9d388"), new Guid("ccdb7873-4647-94c9-7a0b-87b344a82f9c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("610b225a-18e9-6a16-a2b9-5246cb691024"), 1, 5, null, null },
                    { new Guid("fbb98808-fd6f-c2f5-9b1b-c686f70d7ec4"), new Guid("23c24ec5-bfe2-bb93-a250-80a8b9c553f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("637a520b-9c6d-21ad-73b9-fda1ce4a33e1"), 1, 3, null, null },
                    { new Guid("fc345088-fb54-5fd4-0fce-e6d35aba43e6"), new Guid("b61394cf-d01d-a1cd-622b-59c77f493b15"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("94a8cbae-bd34-96b4-6085-4b9548375bc2"), 1, 4, null, null },
                    { new Guid("fd116a92-beeb-7e9c-e468-6523fe91fa52"), new Guid("e3649000-060c-2ca2-8b35-ff540447740e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("cfdde016-0fc0-ad61-ed71-03a819df623d"), 1, 5, null, null },
                    { new Guid("fe7dc970-6cc9-4183-2b91-2006cd61749c"), new Guid("12754a97-d51b-8889-f948-1750ecf94d5c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("13c628de-04f2-47a1-3b6e-c36e10501e0f"), 1, 1, null, null },
                    { new Guid("ffa31d6c-c46f-150b-f33b-82dc81f6369a"), new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("23858376-a7d4-0f32-6701-fe9645c7a8f6"), 1, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderItem",
                columns: new[] { "Id", "AdditionalMaterialId", "ComboId", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "MaterialId", "OrderId", "PriceAfterTax", "PriceBeforeTax", "Quantity", "Tax", "TenantId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("01fe06eb-bef5-e08d-95ec-e527c7cba708"), new Guid("d5ddb405-bda3-312e-5487-c3ab449e43a5"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("ffe6e861-805e-6065-3ccb-626aa1b3d840"), 0.035m, 0.030m, 2, 0.005m, 4, null, null },
                    { new Guid("02a745e0-d8d8-108e-e3ff-de2fb53bf061"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4e94443d-8098-62ea-5c52-3d06d568f540"), new Guid("7514793f-8aed-2fa2-568f-9472dd282595"), 0.042m, 0.036m, 2, 0.006m, 3, null, null },
                    { new Guid("0901c94f-4481-5c93-19dc-b3c259a4de4f"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("71c203cb-75b9-61f0-5a2a-1d322e0b9910"), new Guid("23fb143c-909e-5b13-f97b-29e3715949de"), 0.012m, 0.010m, 2, 0.002m, 2, null, null },
                    { new Guid("095f9f5d-336a-0604-f17b-9e9530540ed2"), null, new Guid("713cfae5-57b6-3bca-c55b-f10f8eb7334c"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("9c351c71-b5a0-d440-a58d-9826ca98b36e"), 0.367m, 0.316m, 1, 0.051m, 5, null, null },
                    { new Guid("0f17f50c-9906-2541-3876-e74664c05a54"), null, new Guid("23c24ec5-bfe2-bb93-a250-80a8b9c553f0"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("966eb05c-0663-a48f-1c77-b551e688ff85"), 0.090m, 0.078m, 2, 0.012m, 3, null, null },
                    { new Guid("163b306e-8fad-8dad-be6d-a2d121fd9a42"), new Guid("7495dc48-2430-21c3-d7b4-56fa2fe05e52"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("52599bb3-14ed-5971-e68e-1611728069eb"), 0.017m, 0.015m, 1, 0.002m, 4, null, null },
                    { new Guid("26b678cd-aa70-53dd-899e-7fd13559b075"), new Guid("2d31de49-5867-3101-ba7b-792e10492629"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("7514793f-8aed-2fa2-568f-9472dd282595"), 0.005m, 0.004m, 1, 0.001m, 3, null, null },
                    { new Guid("2b438d00-83b5-e5aa-6d05-0397229ce165"), new Guid("14b50fd0-72c5-89dd-7d54-c593ae93ceb1"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("40a04b89-4927-de74-10b1-c1c3cb72d63d"), 0.023m, 0.020m, 1, 0.003m, 2, null, null },
                    { new Guid("33fa602a-5917-0809-9c54-45f0213de207"), new Guid("aee669bd-f8eb-77d8-daac-1d566619aaa1"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("def518c8-a3c0-3659-05bc-d1a0192b486d"), 1.160m, 1.000m, 2, 0.160m, 2, null, null },
                    { new Guid("3cfc93c0-fc73-7c11-d857-8844a9ca3264"), new Guid("740804a5-4819-c426-fe3a-ba7e15c80de1"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("66b67a75-5dc1-26fc-2e18-571408b61607"), 0.348m, 0.300m, 1, 0.048m, 4, null, null },
                    { new Guid("3f4076f9-78f8-f07d-32a1-2a2794b80e7f"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c83e5c9c-6791-c216-a4ce-d6e4aebc91d5"), new Guid("def518c8-a3c0-3659-05bc-d1a0192b486d"), 0.014m, 0.012m, 1, 0.002m, 2, null, null },
                    { new Guid("43590431-a0d3-7d16-77c3-df13067de1ac"), null, new Guid("83571b57-eac8-6e22-6570-6c421f5b7aaa"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("def518c8-a3c0-3659-05bc-d1a0192b486d"), 2.501m, 2.156m, 2, 0.345m, 2, null, null },
                    { new Guid("45695860-7436-3a5f-e159-10c355393a78"), null, new Guid("8e0512d0-fe84-6739-806f-e8943fbeeda6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("40a04b89-4927-de74-10b1-c1c3cb72d63d"), 1.244m, 1.072m, 1, 0.172m, 2, null, null },
                    { new Guid("47c1671d-3cd3-7a86-a319-4f340da47ca9"), new Guid("673fa6b8-a7cd-b730-1a00-565d8a2c4979"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("825abaf3-539a-2cc4-905a-b5e6645e800f"), 0.580m, 0.500m, 1, 0.080m, 1, null, null },
                    { new Guid("54a9f1d1-2d6a-cb5e-9bc8-af8468078d10"), new Guid("d3dace19-f1f6-2326-ab9b-886022880409"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("23fb143c-909e-5b13-f97b-29e3715949de"), 0.012m, 0.010m, 1, 0.002m, 2, null, null },
                    { new Guid("57e429b0-d909-3700-2966-0e3e9513a7e3"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("8668a7a4-fb4e-e933-2679-b8e80cfc3136"), new Guid("9c351c71-b5a0-d440-a58d-9826ca98b36e"), 0.005m, 0.004m, 2, 0.001m, 5, null, null },
                    { new Guid("682cc0bc-e756-699b-8981-9f756b47d643"), null, new Guid("0e6e7f41-23f3-8e05-f20c-5362f062e9db"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("a96c1d60-7f59-87c4-f8c8-a05dfc510ab5"), 6.299m, 5.430m, 2, 0.869m, 1, null, null },
                    { new Guid("6b80fb63-fcbc-1672-10f1-888f61015e24"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("2c3e51d7-5d6f-36f1-b8a3-dd240f7c2635"), new Guid("cf54e7b3-dc3c-df47-5817-d42960580d6f"), 1.044m, 0.900m, 2, 0.144m, 3, null, null },
                    { new Guid("6ba7bc10-24c7-5642-386c-f94fc56b952a"), new Guid("7f4761f1-617d-1b64-3215-9744c2975aa2"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("b8a6a30e-68c3-104f-3298-f32f10b52ec0"), 0.006m, 0.005m, 1, 0.001m, 1, null, null },
                    { new Guid("7053e4b9-4378-44bc-6c80-0231d2a37c15"), null, new Guid("416ec7ac-be5b-8926-d7c5-8358ae9e1681"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("b8a6a30e-68c3-104f-3298-f32f10b52ec0"), 3.626m, 3.126m, 1, 0.500m, 1, null, null },
                    { new Guid("74d2a99a-3856-f699-fba2-7fbca580d1bb"), new Guid("ff80e6a1-b675-674d-32ea-2384237dcf86"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("d6cda97f-7137-abdb-487c-5dab9b73c5cb"), 0.058m, 0.050m, 2, 0.008m, 5, null, null },
                    { new Guid("7c806d38-e9cf-0c59-7e01-a0a4c1b3e8ab"), null, new Guid("cc47ce80-f259-eb2b-0284-e0479c03fad1"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("66b67a75-5dc1-26fc-2e18-571408b61607"), 0.056m, 0.048m, 1, 0.008m, 4, null, null },
                    { new Guid("7d78f0bc-6f86-1e78-19d5-a2e7c9bc10ae"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("23858376-a7d4-0f32-6701-fe9645c7a8f6"), new Guid("825abaf3-539a-2cc4-905a-b5e6645e800f"), 3.248m, 2.800m, 2, 0.448m, 1, null, null },
                    { new Guid("93e46597-4d03-83bf-c75e-bfc3c5792969"), null, new Guid("b5190c99-3388-aa88-181f-c4fef602a70f"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("7514793f-8aed-2fa2-568f-9472dd282595"), 0.631m, 0.544m, 1, 0.087m, 3, null, null },
                    { new Guid("93ea6ef0-d5ad-d704-78b3-f910fd0533c8"), null, new Guid("e3649000-060c-2ca2-8b35-ff540447740e"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("d6cda97f-7137-abdb-487c-5dab9b73c5cb"), 0.121m, 0.104m, 2, 0.017m, 5, null, null },
                    { new Guid("942db4b6-ec70-d4fc-3a51-ed71c0fcff63"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("e0a740b1-1e96-6048-541e-df4f10730fcc"), new Guid("ffe6e861-805e-6065-3ccb-626aa1b3d840"), 0.007m, 0.006m, 1, 0.001m, 4, null, null },
                    { new Guid("a3212aa8-b9c2-1450-6687-73273e56e794"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("7accf885-f92c-5467-95e5-bc25fa34f0bd"), new Guid("52599bb3-14ed-5971-e68e-1611728069eb"), 0.012m, 0.010m, 2, 0.002m, 4, null, null },
                    { new Guid("a4756f13-d922-77fa-6a79-31e57c5d89aa"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("69695b27-1fa1-1cc3-d357-b9c9557e11d4"), new Guid("80f96d44-678a-abe9-de93-7879203b2d90"), 0.012m, 0.010m, 2, 0.002m, 5, null, null },
                    { new Guid("a5f06eb1-170f-9072-14c4-6b7f03edf824"), new Guid("b74272c9-a6a0-27d2-f440-5c49771f776d"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("9c351c71-b5a0-d440-a58d-9826ca98b36e"), 1.160m, 1.000m, 1, 0.160m, 5, null, null },
                    { new Guid("ba8fcda5-ea18-d5eb-115c-a450e90fa7ba"), null, new Guid("74ba2d5d-4877-4023-c2de-b3235dc0abd6"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("cf54e7b3-dc3c-df47-5817-d42960580d6f"), 0.564m, 0.486m, 1, 0.078m, 3, null, null },
                    { new Guid("be5658e0-c706-afb0-848d-1b4efee9e8f5"), new Guid("f4b49873-078d-4655-c85a-ba007d3727f8"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("966eb05c-0663-a48f-1c77-b551e688ff85"), 1.160m, 1.000m, 2, 0.160m, 3, null, null },
                    { new Guid("c02e5847-3db3-3a59-7853-c1323898074d"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("4bcb4bee-2a19-363f-eb0c-a4a925a0e6a1"), new Guid("966eb05c-0663-a48f-1c77-b551e688ff85"), 0.014m, 0.012m, 1, 0.002m, 3, null, null },
                    { new Guid("c0738971-8c1b-0a2d-1c1d-c6fda5c5d326"), new Guid("6d905535-a7ad-b61a-ac9b-9712e3b440dd"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("cf54e7b3-dc3c-df47-5817-d42960580d6f"), 0.009m, 0.008m, 1, 0.001m, 3, null, null },
                    { new Guid("c156a7b5-21a8-be32-477c-5618b826964c"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("238f2777-73e8-da46-7099-7564dc8f9370"), new Guid("a96c1d60-7f59-87c4-f8c8-a05dfc510ab5"), 2.088m, 1.800m, 1, 0.288m, 1, null, null },
                    { new Guid("c554ed75-2649-3523-70b4-871b823b84b6"), null, new Guid("e98601f3-752a-f086-f4d0-e6cbd09fcd1b"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("23fb143c-909e-5b13-f97b-29e3715949de"), 1.256m, 1.083m, 1, 0.173m, 2, null, null },
                    { new Guid("cfc9dbbe-8eb9-f256-0be4-b0cd303c7ee2"), null, new Guid("7f76dec4-ba09-2561-5639-064d282fc7cd"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("825abaf3-539a-2cc4-905a-b5e6645e800f"), 2.616m, 2.255m, 1, 0.361m, 1, null, null },
                    { new Guid("d49ea483-93eb-75a0-3b69-cd00fe61efd6"), null, new Guid("b61394cf-d01d-a1cd-622b-59c77f493b15"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("52599bb3-14ed-5971-e68e-1611728069eb"), 0.041m, 0.035m, 1, 0.006m, 4, null, null },
                    { new Guid("d5a45e2c-e44c-5abd-63c0-fc5d6dabecce"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("b1205177-eaa7-8a32-e295-2216cbdf4cae"), new Guid("40a04b89-4927-de74-10b1-c1c3cb72d63d"), 0.046m, 0.040m, 2, 0.006m, 2, null, null },
                    { new Guid("e98b509c-2168-7e7b-a896-411c4da7a0e6"), new Guid("383f966b-1275-92d5-d63e-850bf939242c"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("a96c1d60-7f59-87c4-f8c8-a05dfc510ab5"), 0.035m, 0.030m, 2, 0.005m, 1, null, null },
                    { new Guid("e9992a24-9ab6-4592-98a0-c1d26904b4ea"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("3adcc7a3-9d9c-f4af-c9fd-130338b556ea"), new Guid("66b67a75-5dc1-26fc-2e18-571408b61607"), 0.042m, 0.036m, 2, 0.006m, 4, null, null },
                    { new Guid("e9afbcd4-d476-fe6a-7146-9aac1ef45e2b"), null, new Guid("d08446f2-176d-538b-f604-abecae35c0ba"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("80f96d44-678a-abe9-de93-7879203b2d90"), 0.068m, 0.059m, 1, 0.009m, 5, null, null },
                    { new Guid("ebbb7cda-a857-ad03-182b-9d0bf99d7c9a"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("73540134-8f35-bff6-85c9-df6905a0eb51"), new Guid("d6cda97f-7137-abdb-487c-5dab9b73c5cb"), 0.290m, 0.250m, 1, 0.040m, 5, null, null },
                    { new Guid("f6dea1b5-f5ab-7aa7-840d-e231b5f91bcd"), null, null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, new Guid("c1f5ace9-452a-c23a-0004-92023e3ce5ce"), new Guid("b8a6a30e-68c3-104f-3298-f32f10b52ec0"), 1.276m, 1.100m, 2, 0.176m, 1, null, null },
                    { new Guid("f902e993-b2d2-e89a-d9be-c0b67ca61a4e"), new Guid("159d7843-2143-562c-c89a-df5c21a2d3f2"), null, "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("80f96d44-678a-abe9-de93-7879203b2d90"), 0.232m, 0.200m, 1, 0.032m, 5, null, null },
                    { new Guid("fa5db61c-cbbb-67d6-899b-bb9350ec1bf4"), null, new Guid("b40a1c27-0233-0129-656c-1b5881236ce5"), "System", new DateTimeOffset(new DateTime(2025, 10, 1, 10, 50, 21, 432, DateTimeKind.Unspecified).AddTicks(6867), new TimeSpan(0, 0, 0, 0, 0)), null, null, false, null, new Guid("ffe6e861-805e-6065-3ccb-626aa1b3d840"), 0.095m, 0.082m, 2, 0.013m, 4, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalMaterial_TenantId",
                table: "AdditionalMaterial",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Combo_TenantId",
                table: "Combo",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboMaterial_ComboId_TenantId",
                table: "ComboMaterial",
                columns: new[] { "ComboId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ComboMaterial_MaterialId_TenantId",
                table: "ComboMaterial",
                columns: new[] { "MaterialId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ComboMaterial_TenantId_ComboId",
                table: "ComboMaterial",
                columns: new[] { "TenantId", "ComboId" });

            migrationBuilder.CreateIndex(
                name: "IX_ComboMaterial_TenantId_MaterialId",
                table: "ComboMaterial",
                columns: new[] { "TenantId", "MaterialId" });

            migrationBuilder.CreateIndex(
                name: "IX_Material_TenantId",
                table: "Material",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TenantId",
                table: "Order",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_AdditionalMaterialId_TenantId",
                table: "OrderItem",
                columns: new[] { "AdditionalMaterialId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ComboId_TenantId",
                table: "OrderItem",
                columns: new[] { "ComboId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_MaterialId_TenantId",
                table: "OrderItem",
                columns: new[] { "MaterialId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId_TenantId",
                table: "OrderItem",
                columns: new[] { "OrderId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_TenantId_AdditionalMaterialId",
                table: "OrderItem",
                columns: new[] { "TenantId", "AdditionalMaterialId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_TenantId_ComboId",
                table: "OrderItem",
                columns: new[] { "TenantId", "ComboId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_TenantId_MaterialId",
                table: "OrderItem",
                columns: new[] { "TenantId", "MaterialId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_TenantId_OrderId",
                table: "OrderItem",
                columns: new[] { "TenantId", "OrderId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboMaterial");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "AdditionalMaterial");

            migrationBuilder.DropTable(
                name: "Combo");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
