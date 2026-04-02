using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostgresInitialFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "text", nullable: false),
                    ResourceId = table.Column<string>(type: "text", nullable: true),
                    ResourceName = table.Column<string>(type: "text", nullable: true),
                    OldValues = table.Column<string>(type: "text", nullable: false),
                    NewValues = table.Column<string>(type: "text", nullable: false),
                    IPAddress = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Group = table.Column<string>(type: "text", nullable: true),
                    AvgMaintenanceCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StandardWarrantyMonths = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DurationInDays = table.Column<int>(type: "integer", nullable: false),
                    DurationInMonths = table.Column<int>(type: "integer", nullable: false),
                    SessionLimit = table.Column<int>(type: "integer", nullable: true),
                    HasPT = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    SupplyType = table.Column<string>(type: "text", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "text", nullable: true),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Permissions = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EquipmentCode = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WarrantyExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    UsefulLifeMonths = table.Column<int>(type: "integer", nullable: false),
                    SalvageValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepreciationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MonthlyDepreciationAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AccumulatedDepreciation = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsFullyDepreciated = table.Column<bool>(type: "boolean", nullable: false),
                    MaintenanceIntervalDays = table.Column<int>(type: "integer", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "EquipmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipments_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CostPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    MinStockThreshold = table.Column<int>(type: "integer", nullable: false),
                    MaxStockThreshold = table.Column<int>(type: "integer", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrackInventory = table.Column<bool>(type: "boolean", nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: true),
                    SessionCount = table.Column<int>(type: "integer", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MemberCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    EmergencyContact = table.Column<string>(type: "text", nullable: true),
                    EmergencyPhone = table.Column<string>(type: "text", nullable: true),
                    FaceEncoding = table.Column<string>(type: "text", nullable: true),
                    QRCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrainerCode = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Specialization = table.Column<string>(type: "text", nullable: true),
                    ExperienceYears = table.Column<int>(type: "integer", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "text", nullable: true),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SessionRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Depreciations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PeriodMonth = table.Column<int>(type: "integer", nullable: false),
                    PeriodYear = table.Column<int>(type: "integer", nullable: false),
                    RemainingValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depreciations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depreciations_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentProviderHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OldProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    NewProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentProviderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Providers_NewProviderId",
                        column: x => x.NewProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentProviderHistories_Providers_OldProviderId",
                        column: x => x.OldProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    BeforeQuantity = table.Column<int>(type: "integer", nullable: false),
                    AfterQuantity = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    FromLocation = table.Column<string>(type: "text", nullable: true),
                    ToLocation = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentTransactions_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<string>(type: "text", nullable: false),
                    ReportedBy = table.Column<string>(type: "text", nullable: true),
                    ResolutionStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentLogs_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Technician = table.Column<string>(type: "text", nullable: true),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    BeforeQuantity = table.Column<int>(type: "integer", nullable: false),
                    AfterQuantity = table.Column<int>(type: "integer", nullable: false),
                    PerformedBy = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTransactions_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MemberSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RemainingSessions = table.Column<int>(type: "integer", nullable: true),
                    OriginalPackageName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    OriginalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FinalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberSubscriptions_MembershipPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "MembershipPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "text", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassName = table.Column<string>(type: "text", nullable: false),
                    ClassType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TrainerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleDay = table.Column<string>(type: "text", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    CurrentEnrollment = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerMemberAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerMemberAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerMemberAssignments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerMemberAssignments_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenanceLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceMaterials_MaintenanceLogs_MaintenanceLogId",
                        column: x => x.MaintenanceLogId,
                        principalTable: "MaintenanceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceMaterials_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemType = table.Column<string>(type: "text", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ItemName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckIns_MemberSubscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "MemberSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CheckIns_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_MemberSubscriptions_MemberSubscriptionId",
                        column: x => x.MemberSubscriptionId,
                        principalTable: "MemberSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrolledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAttended = table.Column<bool>(type: "boolean", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EquipmentCategories",
                columns: new[] { "Id", "AvgMaintenanceCost", "Code", "CreatedAt", "Description", "Group", "IsDeleted", "Name", "StandardWarrantyMonths", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), 200000m, "CARDIO", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Máy chạy bộ, xe đạp, trượt tuyết", "Máy móc", false, "Máy Cardio", null, null },
                    { new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"), 150000m, "STRENGTH", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Máy tập cơ ngực, xô, vai, chân", "Máy móc", false, "Máy Tập Khối (Strength)", null, null },
                    { new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"), 50000m, "FREEWEIGHT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tạ tay, tạ đòn, bóng tập", "Dụng cụ", false, "Tạ & Phụ kiện", null, null },
                    { new Guid("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4"), 300000m, "ELECTRONIC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Màn hình, hệ thống âm thanh, cổng từ", "Nội thất", false, "Thiết bị Điện tử", null, null }
                });

            migrationBuilder.InsertData(
                table: "MembershipPackages",
                columns: new[] { "Id", "CreatedAt", "Description", "DiscountPrice", "DurationInDays", "DurationInMonths", "HasPT", "IsActive", "IsDeleted", "Name", "Price", "SessionLimit", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Toàn bộ dịch vụ + PT cá nhân trong 365 ngày", 4000000m, 365, 12, false, true, false, "Gói VIP 1 Năm", 4500000m, null, null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Truy cập phòng gym không giới hạn trong 30 ngày", null, 30, 1, false, true, false, "Gói Cơ Bản 1 Tháng", 500000m, null, null },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gym + Tất cả lớp học trong 30 ngày", 750000m, 30, 1, false, true, false, "Gói Premium 1 Tháng", 800000m, null, null },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10 buổi tập, hạn sử dụng 60 ngày", null, 60, 2, false, true, false, "Gói 10 Buổi Tập", 450000m, 10, null },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Truy cập không giới hạn 90 ngày", 1100000m, 90, 3, false, true, false, "Gói 3 Tháng", 1200000m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "Address", "BankAccountNumber", "BankName", "Code", "ContactPerson", "CreatedAt", "Email", "IsActive", "IsDeleted", "Name", "Note", "PhoneNumber", "SupplyType", "TaxCode", "UpdatedAt" },
                values: new object[] { new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), "123 Đường số 7, TP.HCM", null, null, null, "Nguyễn Văn Cung", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "contact@gymglobal.com", true, false, "Công ty Thiết bị Gym Toàn Cầu", null, "02838445566", null, "0314567890", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Permissions", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "👑 Admin/Manager (Quản lý chủ gym) - TOÀN QUYỀN", "\"*\"", "Admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tiểu quản lý vận hành", "[\"*\"]", "Manager" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "🏋️ Trainer - Chuyên môn", "[\"member.read\",\"class.manage\",\"equipment.read\",\"equipment.report\",\"inventory.consume\",\"product.read\",\"trainer.read\"]", "Trainer" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "👩💼 Receptionist - Vận hành hàng ngày", "[\"member.read\",\"member.create\",\"member.update\",\"checkin.create\",\"checkin.read\",\"package.read\",\"inventory.read\",\"inventory.consume\",\"report.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"payment.read\",\"payment.create\",\"billing.read\",\"billing.create\",\"product.read\",\"class.read\",\"trainer.read\"]", "Receptionist" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "👤 Member - Tự phục vụ", "[\"member.read\",\"checkin.self\",\"class.read\",\"subscription.read\"]", "Member" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "💰 Accountant - Quản lý tài chính", "[\"billing.read\",\"billing.create\",\"payment.read\",\"payment.create\",\"report.read\",\"package.read\",\"member.read\",\"subscription.read\",\"subscription.create\",\"subscription.update\",\"checkin.read\",\"product.read\",\"class.read\",\"trainer.read\"]", "Accountant" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gym.com", "System Administrator", true, null, "AQAAAAIAAYagAAAAEDXUlTVihV03KzVTcOE75jLHq/3GVsywEp6snnNj0AjxgsUfMUCTW6dZSfK+Wzaq5Q==", "0901234567", null, "admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@gym.com", "Nguyễn Văn Manager", true, null, "AQAAAAIAAYagAAAAEKp2RDrdX3JLSQ0YcEyoPW1yIk8K3vovzusIL2uN5KPh8weWFLBBHIYRnihNLoKS5w==", "0902345678", null, "manager" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer1@gym.com", "Trần Thị Hương", true, null, "AQAAAAIAAYagAAAAEC34EurbKKIBzE4x2hVY6Kpi7qn8yL48Of6qJ1q3jYKgNgkG3EVC1zwqy84qXOCKJw==", "0903456789", null, "trainer1" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer2@gym.com", "Lê Văn Nam", true, null, "AQAAAAIAAYagAAAAELBj+IpAh+a6wqj/wm+GIjXsZpqLlLyQNxgKz+2ZPhMKkuxcsr9i2geTOzeaUMXiCQ==", "0904567890", null, "trainer2" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "receptionist@gym.com", "Phạm Thị Lan", true, null, "AQAAAAIAAYagAAAAEHPfri56OuJMrYRxxzQLvHtvFcUue8a0B65DyNvVuDdNUnLI6mh0MgWOI1L+nVtbKw==", "0905678901", null, "receptionist" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nguyenvana@gmail.com", "Nguyễn Văn A", true, null, "AQAAAAIAAYagAAAAELgLs+Bqc44G5aIcOyB9QrnzADOhVjQL9P5zbEjGn44l7OjR1+ECfd9ZBqNirB0DGw==", "0906789012", null, "member001" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tranthib@gmail.com", "Trần Thị B", true, null, "AQAAAAIAAYagAAAAEHTKkTJkhNx83n1SxqnTMbHPs7zpvM1207+kbXiRVYcB5djB9IxnKm3vWV+uowKIhw==", "0907890123", null, "member002" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "levanc_member@gmail.com", "Lê Văn C (Member)", true, null, "AQAAAAIAAYagAAAAEC+qq67gJyVCj4c8Jvn/Gk1MyqvnhEaU+kr4bQD3k/xWP2TU7Qgr/iqf5h3nYF/5CA==", "0908901234", null, "member003" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "levanc@gym.com", "Lê Văn C", true, null, "AQAAAAIAAYagAAAAECcvc3Gw+J87zhQDrAFdWBJTL+EAa2qtL4U6E109W7rtgiAbPIH8uLJGywOS/KX99A==", "0909999999", null, "levanc" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "Location", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho nhập hàng lớn và lưu trữ chính", true, false, "Tầng hầm", "Kho Tổng (Main)", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kho bán lẻ & vật tư tại quầy", true, false, "Sảnh chính", "Kho Quầy", null }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "AccumulatedDepreciation", "CategoryId", "CreatedAt", "DepreciationStartDate", "Description", "EquipmentCode", "IsDeleted", "IsFullyDepreciated", "LastMaintenanceDate", "Location", "MaintenanceIntervalDays", "MonthlyDepreciationAmount", "Name", "NextMaintenanceDate", "Priority", "ProviderId", "PurchaseDate", "PurchasePrice", "Quantity", "RemainingValue", "SalvageValue", "SerialNumber", "Status", "UpdatedAt", "UsefulLifeMonths", "WarrantyExpiryDate", "Weight" },
                values: new object[] { new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), 0m, new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "EQP-RUN-001", false, false, null, "Khu Cardio Tầng 1", 90, 333333m, "Máy chạy bộ Matrix T7", null, 3, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25000000m, 5, 25000000m, 5000000m, "MTX7-2024-X1", 1, null, 60, null, null });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Address", "CreatedAt", "DateOfBirth", "Email", "EmergencyContact", "EmergencyPhone", "FaceEncoding", "FullName", "Gender", "IsDeleted", "JoinedDate", "MemberCode", "Note", "PhoneNumber", "QRCode", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("20202020-2020-2020-2020-202020202020"), null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nguyenvana@gmail.com", null, null, "MOCK_FACE_VECTOR_GYM2024001", "Nguyễn Văn A", null, false, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GYM2024001", null, "0906789012", null, 1, null, new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("30303030-3030-3030-3030-303030303030"), null, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1992, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "tranthib@gmail.com", null, null, "MOCK_FACE_VECTOR_GYM2024002", "Trần Thị B", null, false, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "GYM2024002", null, "0907890123", null, 1, null, new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("40404040-4040-4040-4040-404040404040"), null, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1998, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "levanc@gmail.com", null, null, "MOCK_FACE_VECTOR_GYM2024003", "Lê Văn C", null, false, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "GYM2024003", null, "0908901234", null, 1, null, new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "Category", "CostPrice", "CreatedAt", "Description", "DurationDays", "ExpirationDate", "ImageUrl", "IsActive", "IsDeleted", "MaxStockThreshold", "MinStockThreshold", "Name", "Price", "ProviderId", "SKU", "SessionCount", "StockQuantity", "TrackInventory", "Type", "Unit", "UpdatedAt" },
                values: new object[] { new Guid("91919191-9191-9191-9191-919191919191"), null, "Đồ uống", 5000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nước khoáng thiên nhiên", null, null, null, true, false, 200, 20, "Nước suối Lavie 500ml", 10000m, new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), "WTR-001", null, 100, true, 3, "Chai", null });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "Bio", "CreatedAt", "Email", "ExperienceYears", "FullName", "HireDate", "IsActive", "IsDeleted", "PhoneNumber", "ProfilePhoto", "Salary", "SessionRate", "Specialization", "TrainerCode", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "HLV Yoga với 8 năm kinh nghiệm, chứng chỉ quốc tế RYT-500", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer1@gym.com", 0, "Trần Thị Hương", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "0903456789", null, 0m, 0m, "Yoga, Pilates, Meditation", "", null, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Võ sư Boxing chuyên nghiệp, từng thi đấu SEA Games", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "trainer2@gym.com", 0, "Lê Văn Nam", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "0904567890", null, 0m, 0m, "Boxing, Muay Thai, CrossFit", "", null, new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "AssignedAt" },
                values: new object[,]
                {
                    { 1, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6304) },
                    { 2, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6311) },
                    { 3, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6313) },
                    { 3, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6316) },
                    { 4, new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6317) },
                    { 5, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6322) },
                    { 5, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6324) },
                    { 5, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6325) },
                    { 3, new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6320) },
                    { 4, new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 2, 5, 7, 32, 505, DateTimeKind.Utc).AddTicks(6318) }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName", "ClassType", "CreatedAt", "CurrentEnrollment", "Description", "EndTime", "IsActive", "IsDeleted", "MaxCapacity", "ScheduleDay", "StartTime", "TrainerId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50505050-5050-5050-5050-505050505050"), "Yoga Buổi Sáng", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Yoga cơ bản cho người mới bắt đầu", new TimeSpan(0, 7, 30, 0, 0), true, false, 20, "Monday", new TimeSpan(0, 6, 0, 0, 0), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null },
                    { new Guid("60606060-6060-6060-6060-606060606060"), "Boxing Tối", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Boxing căn bản và nâng cao", new TimeSpan(0, 19, 30, 0, 0), true, false, 15, "Wednesday", new TimeSpan(0, 18, 0, 0, 0), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null },
                    { new Guid("70707070-7070-7070-7070-707070707070"), "Pilates Chiều", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "Pilates cho sức khỏe và vóc dáng", new TimeSpan(0, 17, 0, 0, 0), true, false, 12, "Friday", new TimeSpan(0, 16, 0, 0, 0), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null }
                });

            migrationBuilder.InsertData(
                table: "MemberSubscriptions",
                columns: new[] { "Id", "CreatedAt", "DiscountApplied", "EndDate", "FinalPrice", "IsDeleted", "MemberId", "OriginalPackageName", "OriginalPrice", "PackageId", "RemainingSessions", "StartDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("80808080-8080-8080-8080-808080808080"), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0m, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, false, new Guid("20202020-2020-2020-2020-202020202020"), "", 0m, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { new Guid("90909090-9090-9090-9090-909090909090"), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 0m, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, false, new Guid("30303030-3030-3030-3030-303030303030"), "", 0m, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), 10, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "CreatedAt", "IsDeleted", "MemberSubscriptionId", "Method", "Note", "PaymentDate", "Status", "TransactionId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), 750000m, new DateTime(2024, 2, 1, 10, 30, 0, 0, DateTimeKind.Utc), false, new Guid("80808080-8080-8080-8080-808080808080"), 2, "Thanh toán gói Premium 1 tháng", new DateTime(2024, 2, 1, 10, 30, 0, 0, DateTimeKind.Utc), 2, "TXN20240201001", null },
                    { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 450000m, new DateTime(2024, 2, 5, 14, 15, 0, 0, DateTimeKind.Utc), false, new Guid("90909090-9090-9090-9090-909090909090"), 1, "Thanh toán gói 10 buổi tập", new DateTime(2024, 2, 5, 14, 15, 0, 0, DateTimeKind.Utc), 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_CheckInTime",
                table: "CheckIns",
                column: "CheckInTime");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberId",
                table: "CheckIns",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberId_CheckInTime",
                table: "CheckIns",
                columns: new[] { "MemberId", "CheckInTime" });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_SubscriptionId",
                table: "CheckIns",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassId",
                table: "ClassEnrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassId_MemberId",
                table: "ClassEnrollments",
                columns: new[] { "ClassId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_EnrolledDate",
                table: "ClassEnrollments",
                column: "EnrolledDate");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_MemberId",
                table: "ClassEnrollments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleDay",
                table: "Classes",
                column: "ScheduleDay",
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleDay_StartTime",
                table: "Classes",
                columns: new[] { "ScheduleDay", "StartTime" },
                filter: "IsActive = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TrainerId",
                table: "Classes",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Depreciations_EquipmentId",
                table: "Depreciations",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_EquipmentId",
                table: "EquipmentProviderHistories",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_NewProviderId",
                table: "EquipmentProviderHistories",
                column: "NewProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProviderHistories_OldProviderId",
                table: "EquipmentProviderHistories",
                column: "OldProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_CategoryId",
                table: "Equipments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ProviderId",
                table: "Equipments",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTransactions_EquipmentId",
                table: "EquipmentTransactions",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentLogs_EquipmentId",
                table: "IncidentLogs",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_WarehouseId",
                table: "Inventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_MemberId",
                table: "Invoices",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_EquipmentId",
                table: "MaintenanceLogs",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_ProviderId",
                table: "MaintenanceLogs",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMaterials_MaintenanceLogId",
                table: "MaintenanceMaterials",
                column: "MaintenanceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMaterials_ProductId",
                table: "MaintenanceMaterials",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Members_FullName",
                table: "Members",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberCode",
                table: "Members",
                column: "MemberCode",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PhoneNumber",
                table: "Members",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Status",
                table: "Members",
                column: "Status",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPackages_IsActive",
                table: "MembershipPackages",
                column: "IsActive",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_EndDate",
                table: "MemberSubscriptions",
                column: "EndDate",
                filter: "Status = 2");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_MemberId",
                table: "MemberSubscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_MemberId_Status",
                table: "MemberSubscriptions",
                columns: new[] { "MemberId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_PackageId",
                table: "MemberSubscriptions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSubscriptions_Status",
                table: "MemberSubscriptions",
                column: "Status",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MemberId",
                table: "Orders",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MemberSubscriptionId",
                table: "Payments",
                column: "MemberSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                filter: "TransactionId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProviderId",
                table: "Products",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_FromWarehouseId",
                table: "StockTransactions",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProductId",
                table: "StockTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProviderId",
                table: "StockTransactions",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ToWarehouseId",
                table: "StockTransactions",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerMemberAssignments_MemberId",
                table: "TrainerMemberAssignments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerMemberAssignments_TrainerId",
                table: "TrainerMemberAssignments",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                filter: "Email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_IsActive",
                table: "Trainers",
                column: "IsActive",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                table: "Trainers",
                column: "UserId",
                unique: true,
                filter: "UserId IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "ClassEnrollments");

            migrationBuilder.DropTable(
                name: "Depreciations");

            migrationBuilder.DropTable(
                name: "EquipmentProviderHistories");

            migrationBuilder.DropTable(
                name: "EquipmentTransactions");

            migrationBuilder.DropTable(
                name: "IncidentLogs");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "MaintenanceMaterials");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "TrainerMemberAssignments");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MaintenanceLogs");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MemberSubscriptions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MembershipPackages");

            migrationBuilder.DropTable(
                name: "EquipmentCategories");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
