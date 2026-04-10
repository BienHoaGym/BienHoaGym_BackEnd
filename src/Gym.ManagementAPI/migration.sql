BEGIN TRANSACTION;

DROP INDEX "IX_Trainers_Email";

DROP INDEX "IX_Trainers_IsActive";

DROP INDEX "IX_Trainers_UserId";

DROP INDEX "IX_Products_SKU";

DROP INDEX "IX_Payments_TransactionId";

DROP INDEX "IX_MemberSubscriptions_EndDate";

DROP INDEX "IX_MemberSubscriptions_Status";

DROP INDEX "IX_MembershipPackages_IsActive";

DROP INDEX "IX_Members_MemberCode";

DROP INDEX "IX_Members_Status";

DROP INDEX "IX_Invoices_InvoiceNumber";

DROP INDEX "IX_Classes_ScheduleDay";

DROP INDEX "IX_Classes_ScheduleDay_StartTime";

ALTER TABLE "MemberSubscriptions" ADD "AutoPauseExtensionDays" INTEGER NULL;

ALTER TABLE "MemberSubscriptions" ADD "LastPausedAt" TEXT NULL;

ALTER TABLE "Invoices" ADD "CreatedByUserId" TEXT NULL;

ALTER TABLE "Invoices" ADD "CreatedByUserName" TEXT NULL;

ALTER TABLE "InvoiceDetails" ADD "SubscriptionId" TEXT NULL;

UPDATE "MemberSubscriptions" SET "AutoPauseExtensionDays" = NULL, "LastPausedAt" = NULL
WHERE "Id" = '80808080-8080-8080-8080-808080808080';
SELECT changes();


UPDATE "MemberSubscriptions" SET "AutoPauseExtensionDays" = NULL, "LastPausedAt" = NULL
WHERE "Id" = '90909090-9090-9090-9090-909090909090';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964451'
WHERE "RoleId" = 1 AND "UserId" = '11111111-1111-1111-1111-111111111111';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964461'
WHERE "RoleId" = 2 AND "UserId" = '22222222-2222-2222-2222-222222222222';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964463'
WHERE "RoleId" = 3 AND "UserId" = '33333333-3333-3333-3333-333333333333';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964465'
WHERE "RoleId" = 3 AND "UserId" = '44444444-4444-4444-4444-444444444444';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964468'
WHERE "RoleId" = 4 AND "UserId" = '55555555-5555-5555-5555-555555555555';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964474'
WHERE "RoleId" = 5 AND "UserId" = '66666666-6666-6666-6666-666666666666';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964477'
WHERE "RoleId" = 5 AND "UserId" = '77777777-7777-7777-7777-777777777777';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964478'
WHERE "RoleId" = 5 AND "UserId" = '88888888-8888-8888-8888-888888888888';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.8964471'
WHERE "RoleId" = 3 AND "UserId" = '99999999-9999-9999-9999-999999999999';
SELECT changes();


UPDATE "UserRoles" SET "AssignedAt" = '2026-04-09 08:02:15.896447'
WHERE "RoleId" = 4 AND "UserId" = '99999999-9999-9999-9999-999999999999';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEItj91k9msN+CgfR42bBN7O7aqCQl1nk0SbXl74Lb71xJc6u8jsPDo18o+bXg5zaJg=='
WHERE "Id" = '11111111-1111-1111-1111-111111111111';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAELDJkoJ3AAxPTyYLNnFOnCXR865ulj3OsTG5zjTBZjJ8ZVsz3BHkZQ3BLbY1ebVT7w=='
WHERE "Id" = '22222222-2222-2222-2222-222222222222';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEJYqaOsfokXGcr9szI3AMFf7nDE8iZunA904LnlVH5KgXojcPF7wHoJ2V+NhlLQ0fw=='
WHERE "Id" = '33333333-3333-3333-3333-333333333333';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEAiNmnhU220iLVvRVrR2N63t1r6WYAu+uPydTg+3nZ/1l3Q+jGCCcY1LSWMMqQ97sQ=='
WHERE "Id" = '44444444-4444-4444-4444-444444444444';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAELRswg5kmRB5XEydffDKaqI0WapsICPDIT4CjJrX1GG8SpQiWnwJtoRw7N7cqR3l2Q=='
WHERE "Id" = '55555555-5555-5555-5555-555555555555';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEJMSPZn+flMFA4L0IqyviEJhdhPP2K76T2wAfXZUZTNOzpUwCygjo1qasS/e+ZBLCA=='
WHERE "Id" = '66666666-6666-6666-6666-666666666666';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEMQI8WINAFebYP27dfBJ5i09WUy7PTFM/nMgjVkaQ+mj6uqW3ON56OfBeiBzVa8aEg=='
WHERE "Id" = '77777777-7777-7777-7777-777777777777';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEBhVPLqACcsJAVDA/LKiL/lHYD4cAoI8DF2uNfBaBv0B7S2hPSFKNvLxbyeYJFIR4Q=='
WHERE "Id" = '88888888-8888-8888-8888-888888888888';
SELECT changes();


UPDATE "Users" SET "PasswordHash" = 'AQAAAAIAAYagAAAAEMlD5Fov9GwuPpbg4i0TR8rVV9aRSaP5hoWEbIyywqPNukxOtQoXdHKrFhgYpaKjeA=='
WHERE "Id" = '99999999-9999-9999-9999-999999999999';
SELECT changes();


CREATE INDEX "IX_Trainers_Email" ON "Trainers" ("Email") WHERE Email IS NOT NULL;

CREATE INDEX "IX_Trainers_IsActive" ON "Trainers" ("IsActive") WHERE IsDeleted = 0;

CREATE UNIQUE INDEX "IX_Trainers_UserId" ON "Trainers" ("UserId") WHERE UserId IS NOT NULL;

CREATE UNIQUE INDEX "IX_Products_SKU" ON "Products" ("SKU") WHERE IsDeleted = 0;

CREATE INDEX "IX_Payments_TransactionId" ON "Payments" ("TransactionId") WHERE TransactionId IS NOT NULL;

CREATE INDEX "IX_MemberSubscriptions_EndDate" ON "MemberSubscriptions" ("EndDate") WHERE Status = 2;

CREATE INDEX "IX_MemberSubscriptions_Status" ON "MemberSubscriptions" ("Status") WHERE IsDeleted = 0;

CREATE INDEX "IX_MembershipPackages_IsActive" ON "MembershipPackages" ("IsActive") WHERE IsDeleted = 0;

CREATE UNIQUE INDEX "IX_Members_MemberCode" ON "Members" ("MemberCode") WHERE IsDeleted = 0;

CREATE INDEX "IX_Members_Status" ON "Members" ("Status") WHERE IsDeleted = 0;

CREATE UNIQUE INDEX "IX_Invoices_InvoiceNumber" ON "Invoices" ("InvoiceNumber") WHERE IsDeleted = 0;

CREATE INDEX "IX_Classes_ScheduleDay" ON "Classes" ("ScheduleDay") WHERE IsActive = 1;

CREATE INDEX "IX_Classes_ScheduleDay_StartTime" ON "Classes" ("ScheduleDay", "StartTime") WHERE IsActive = 1;

CREATE TABLE "ef_temp_Warehouses" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Warehouses" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "Description" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Location" TEXT NULL,
    "Name" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL
);

INSERT INTO "ef_temp_Warehouses" ("Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "Location", "Name", "UpdatedAt")
SELECT "Id", "CreatedAt", "Description", "IsActive", "IsDeleted", "Location", "Name", "UpdatedAt"
FROM "Warehouses";

CREATE TABLE "ef_temp_Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "FullName" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "LastLoginAt" TEXT NULL,
    "PasswordHash" TEXT NOT NULL,
    "PhoneNumber" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    "Username" TEXT NOT NULL
);

INSERT INTO "ef_temp_Users" ("Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "UpdatedAt", "Username")
SELECT "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "UpdatedAt", "Username"
FROM "Users";

CREATE TABLE "ef_temp_UserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" INTEGER NOT NULL,
    "AssignedAt" TEXT NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_UserRoles" ("UserId", "RoleId", "AssignedAt")
SELECT "UserId", "RoleId", "AssignedAt"
FROM "UserRoles";

CREATE TABLE "ef_temp_Trainers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Trainers" PRIMARY KEY,
    "Bio" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Email" TEXT NULL,
    "ExperienceYears" INTEGER NOT NULL,
    "FullName" TEXT NOT NULL,
    "HireDate" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "PhoneNumber" TEXT NULL,
    "ProfilePhoto" TEXT NULL,
    "Salary" TEXT NOT NULL,
    "SessionRate" TEXT NOT NULL,
    "Specialization" TEXT NULL,
    "TrainerCode" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UserId" TEXT NULL,
    CONSTRAINT "FK_Trainers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE SET NULL
);

INSERT INTO "ef_temp_Trainers" ("Id", "Bio", "CreatedAt", "Email", "ExperienceYears", "FullName", "HireDate", "IsActive", "IsDeleted", "PhoneNumber", "ProfilePhoto", "Salary", "SessionRate", "Specialization", "TrainerCode", "UpdatedAt", "UserId")
SELECT "Id", "Bio", "CreatedAt", "Email", "ExperienceYears", "FullName", "HireDate", "IsActive", "IsDeleted", "PhoneNumber", "ProfilePhoto", "Salary", "SessionRate", "Specialization", "TrainerCode", "UpdatedAt", "UserId"
FROM "Trainers";

CREATE TABLE "ef_temp_TrainerMemberAssignments" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_TrainerMemberAssignments" PRIMARY KEY,
    "AssignedDate" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberId" TEXT NOT NULL,
    "Notes" TEXT NULL,
    "TrainerId" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_TrainerMemberAssignments_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_TrainerMemberAssignments_Trainers_TrainerId" FOREIGN KEY ("TrainerId") REFERENCES "Trainers" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_TrainerMemberAssignments" ("Id", "AssignedDate", "CreatedAt", "IsActive", "IsDeleted", "MemberId", "Notes", "TrainerId", "UpdatedAt")
SELECT "Id", "AssignedDate", "CreatedAt", "IsActive", "IsDeleted", "MemberId", "Notes", "TrainerId", "UpdatedAt"
FROM "TrainerMemberAssignments";

CREATE TABLE "ef_temp_StockTransactions" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_StockTransactions" PRIMARY KEY,
    "AfterQuantity" INTEGER NOT NULL,
    "BeforeQuantity" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Date" TEXT NOT NULL,
    "FromWarehouseId" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Note" TEXT NULL,
    "PerformedBy" TEXT NULL,
    "ProductId" TEXT NOT NULL,
    "ProviderId" TEXT NULL,
    "Quantity" INTEGER NOT NULL,
    "ReferenceNumber" TEXT NULL,
    "ToWarehouseId" TEXT NULL,
    "Type" INTEGER NOT NULL,
    "UnitPrice" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_StockTransactions_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_StockTransactions_Providers_ProviderId" FOREIGN KEY ("ProviderId") REFERENCES "Providers" ("Id"),
    CONSTRAINT "FK_StockTransactions_Warehouses_FromWarehouseId" FOREIGN KEY ("FromWarehouseId") REFERENCES "Warehouses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StockTransactions_Warehouses_ToWarehouseId" FOREIGN KEY ("ToWarehouseId") REFERENCES "Warehouses" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_StockTransactions" ("Id", "AfterQuantity", "BeforeQuantity", "CreatedAt", "Date", "FromWarehouseId", "IsDeleted", "Note", "PerformedBy", "ProductId", "ProviderId", "Quantity", "ReferenceNumber", "ToWarehouseId", "Type", "UnitPrice", "UpdatedAt")
SELECT "Id", "AfterQuantity", "BeforeQuantity", "CreatedAt", "Date", "FromWarehouseId", "IsDeleted", "Note", "PerformedBy", "ProductId", "ProviderId", "Quantity", "ReferenceNumber", "ToWarehouseId", "Type", "UnitPrice", "UpdatedAt"
FROM "StockTransactions";

CREATE TABLE "ef_temp_Roles" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Roles" PRIMARY KEY AUTOINCREMENT,
    "CreatedAt" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Permissions" TEXT NULL,
    "RoleName" TEXT NOT NULL
);

INSERT INTO "ef_temp_Roles" ("Id", "CreatedAt", "Description", "Permissions", "RoleName")
SELECT "Id", "CreatedAt", "Description", "Permissions", "RoleName"
FROM "Roles";

CREATE TABLE "ef_temp_Providers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Providers" PRIMARY KEY,
    "Address" TEXT NULL,
    "BankAccountNumber" TEXT NULL,
    "BankName" TEXT NULL,
    "Code" TEXT NULL,
    "ContactPerson" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Email" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Note" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "SupplyType" TEXT NULL,
    "TaxCode" TEXT NULL,
    "UpdatedAt" TEXT NULL
);

INSERT INTO "ef_temp_Providers" ("Id", "Address", "BankAccountNumber", "BankName", "Code", "ContactPerson", "CreatedAt", "Email", "IsActive", "IsDeleted", "Name", "Note", "PhoneNumber", "SupplyType", "TaxCode", "UpdatedAt")
SELECT "Id", "Address", "BankAccountNumber", "BankName", "Code", "ContactPerson", "CreatedAt", "Email", "IsActive", "IsDeleted", "Name", "Note", "PhoneNumber", "SupplyType", "TaxCode", "UpdatedAt"
FROM "Providers";

CREATE TABLE "ef_temp_Products" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY,
    "Barcode" TEXT NULL,
    "Category" TEXT NULL,
    "CostPrice" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Description" TEXT NULL,
    "DurationDays" INTEGER NULL,
    "ExpirationDate" TEXT NULL,
    "ImageUrl" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MaxStockThreshold" INTEGER NOT NULL,
    "MinStockThreshold" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "ProviderId" TEXT NULL,
    "SKU" TEXT NOT NULL,
    "SessionCount" INTEGER NULL,
    "StockQuantity" INTEGER NOT NULL,
    "TrackInventory" INTEGER NOT NULL,
    "Type" INTEGER NOT NULL,
    "Unit" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Products_Providers_ProviderId" FOREIGN KEY ("ProviderId") REFERENCES "Providers" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_Products" ("Id", "Barcode", "Category", "CostPrice", "CreatedAt", "Description", "DurationDays", "ExpirationDate", "ImageUrl", "IsActive", "IsDeleted", "MaxStockThreshold", "MinStockThreshold", "Name", "Price", "ProviderId", "SKU", "SessionCount", "StockQuantity", "TrackInventory", "Type", "Unit", "UpdatedAt")
SELECT "Id", "Barcode", "Category", "CostPrice", "CreatedAt", "Description", "DurationDays", "ExpirationDate", "ImageUrl", "IsActive", "IsDeleted", "MaxStockThreshold", "MinStockThreshold", "Name", "Price", "ProviderId", "SKU", "SessionCount", "StockQuantity", "TrackInventory", "Type", "Unit", "UpdatedAt"
FROM "Products";

CREATE TABLE "ef_temp_Payments" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Payments" PRIMARY KEY,
    "Amount" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberSubscriptionId" TEXT NOT NULL,
    "Method" INTEGER NOT NULL,
    "Note" TEXT NULL,
    "PaymentDate" TEXT NOT NULL,
    "Status" INTEGER NOT NULL,
    "TransactionId" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Payments_MemberSubscriptions_MemberSubscriptionId" FOREIGN KEY ("MemberSubscriptionId") REFERENCES "MemberSubscriptions" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_Payments" ("Id", "Amount", "CreatedAt", "IsDeleted", "MemberSubscriptionId", "Method", "Note", "PaymentDate", "Status", "TransactionId", "UpdatedAt")
SELECT "Id", "Amount", "CreatedAt", "IsDeleted", "MemberSubscriptionId", "Method", "Note", "PaymentDate", "Status", "TransactionId", "UpdatedAt"
FROM "Payments";

CREATE TABLE "ef_temp_Orders" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "CreatedDate" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberId" TEXT NULL,
    "OrderNumber" TEXT NOT NULL,
    "Status" TEXT NULL,
    "TotalAmount" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Orders_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id")
);

INSERT INTO "ef_temp_Orders" ("Id", "CreatedAt", "CreatedDate", "IsDeleted", "MemberId", "OrderNumber", "Status", "TotalAmount", "UpdatedAt")
SELECT "Id", "CreatedAt", "CreatedDate", "IsDeleted", "MemberId", "OrderNumber", "Status", "TotalAmount", "UpdatedAt"
FROM "Orders";

CREATE TABLE "ef_temp_OrderDetails" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_OrderDetails" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "OrderId" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "ProductId" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_OrderDetails_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderDetails_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_OrderDetails" ("Id", "CreatedAt", "IsDeleted", "OrderId", "Price", "ProductId", "Quantity", "UpdatedAt")
SELECT "Id", "CreatedAt", "IsDeleted", "OrderId", "Price", "ProductId", "Quantity", "UpdatedAt"
FROM "OrderDetails";

CREATE TABLE "ef_temp_MemberSubscriptions" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MemberSubscriptions" PRIMARY KEY,
    "AutoPauseExtensionDays" INTEGER NULL,
    "CreatedAt" TEXT NOT NULL,
    "DiscountApplied" TEXT NOT NULL,
    "EndDate" TEXT NOT NULL,
    "FinalPrice" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "LastPausedAt" TEXT NULL,
    "MemberId" TEXT NOT NULL,
    "OriginalPackageName" TEXT NOT NULL DEFAULT '',
    "OriginalPrice" TEXT NOT NULL,
    "PackageId" TEXT NOT NULL,
    "RemainingSessions" INTEGER NULL,
    "StartDate" TEXT NOT NULL,
    "Status" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_MemberSubscriptions_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_MemberSubscriptions_MembershipPackages_PackageId" FOREIGN KEY ("PackageId") REFERENCES "MembershipPackages" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_MemberSubscriptions" ("Id", "AutoPauseExtensionDays", "CreatedAt", "DiscountApplied", "EndDate", "FinalPrice", "IsDeleted", "LastPausedAt", "MemberId", "OriginalPackageName", "OriginalPrice", "PackageId", "RemainingSessions", "StartDate", "Status", "UpdatedAt")
SELECT "Id", "AutoPauseExtensionDays", "CreatedAt", "DiscountApplied", "EndDate", "FinalPrice", "IsDeleted", "LastPausedAt", "MemberId", "OriginalPackageName", "OriginalPrice", "PackageId", "RemainingSessions", "StartDate", "Status", "UpdatedAt"
FROM "MemberSubscriptions";

CREATE TABLE "ef_temp_MembershipPackages" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MembershipPackages" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "DiscountPrice" TEXT NULL,
    "DurationDays" INTEGER NOT NULL,
    "DurationInMonths" INTEGER NOT NULL,
    "HasPT" INTEGER NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "SessionLimit" INTEGER NULL,
    "UpdatedAt" TEXT NULL
);

INSERT INTO "ef_temp_MembershipPackages" ("Id", "CreatedAt", "Description", "DiscountPrice", "DurationDays", "DurationInMonths", "HasPT", "IsActive", "IsDeleted", "Name", "Price", "SessionLimit", "UpdatedAt")
SELECT "Id", "CreatedAt", "Description", "DiscountPrice", "DurationDays", "DurationInMonths", "HasPT", "IsActive", "IsDeleted", "Name", "Price", "SessionLimit", "UpdatedAt"
FROM "MembershipPackages";

CREATE TABLE "ef_temp_Members" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Members" PRIMARY KEY,
    "Address" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "DateOfBirth" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "EmergencyContact" TEXT NULL,
    "EmergencyPhone" TEXT NULL,
    "FaceEncoding" TEXT NULL,
    "FullName" TEXT NOT NULL,
    "Gender" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "JoinedDate" TEXT NOT NULL,
    "MemberCode" TEXT NOT NULL,
    "Note" TEXT NULL,
    "PhoneNumber" TEXT NOT NULL,
    "QRCode" TEXT NULL,
    "Status" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UserId" TEXT NULL,
    CONSTRAINT "FK_Members_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Members" ("Id", "Address", "CreatedAt", "DateOfBirth", "Email", "EmergencyContact", "EmergencyPhone", "FaceEncoding", "FullName", "Gender", "IsDeleted", "JoinedDate", "MemberCode", "Note", "PhoneNumber", "QRCode", "Status", "UpdatedAt", "UserId")
SELECT "Id", "Address", "CreatedAt", "DateOfBirth", "Email", "EmergencyContact", "EmergencyPhone", "FaceEncoding", "FullName", "Gender", "IsDeleted", "JoinedDate", "MemberCode", "Note", "PhoneNumber", "QRCode", "Status", "UpdatedAt", "UserId"
FROM "Members";

CREATE TABLE "ef_temp_MaintenanceMaterials" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MaintenanceMaterials" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MaintenanceLogId" TEXT NOT NULL,
    "ProductId" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_MaintenanceMaterials_MaintenanceLogs_MaintenanceLogId" FOREIGN KEY ("MaintenanceLogId") REFERENCES "MaintenanceLogs" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MaintenanceMaterials_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_MaintenanceMaterials" ("Id", "CreatedAt", "IsDeleted", "MaintenanceLogId", "ProductId", "Quantity", "UnitPrice", "UpdatedAt")
SELECT "Id", "CreatedAt", "IsDeleted", "MaintenanceLogId", "ProductId", "Quantity", "UnitPrice", "UpdatedAt"
FROM "MaintenanceMaterials";

CREATE TABLE "ef_temp_MaintenanceLogs" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_MaintenanceLogs" PRIMARY KEY,
    "Cost" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Date" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "EquipmentId" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "ProviderId" TEXT NULL,
    "ScheduledDate" TEXT NULL,
    "Status" INTEGER NOT NULL,
    "Technician" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_MaintenanceLogs_Equipments_EquipmentId" FOREIGN KEY ("EquipmentId") REFERENCES "Equipments" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MaintenanceLogs_Providers_ProviderId" FOREIGN KEY ("ProviderId") REFERENCES "Providers" ("Id")
);

INSERT INTO "ef_temp_MaintenanceLogs" ("Id", "Cost", "CreatedAt", "Date", "Description", "EquipmentId", "IsDeleted", "ProviderId", "ScheduledDate", "Status", "Technician", "UpdatedAt")
SELECT "Id", "Cost", "CreatedAt", "Date", "Description", "EquipmentId", "IsDeleted", "ProviderId", "ScheduledDate", "Status", "Technician", "UpdatedAt"
FROM "MaintenanceLogs";

CREATE TABLE "ef_temp_Invoices" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Invoices" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "CreatedByUserId" TEXT NULL,
    "CreatedByUserName" TEXT NULL,
    "DiscountAmount" TEXT NOT NULL,
    "InvoiceNumber" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberId" TEXT NULL,
    "Note" TEXT NULL,
    "PaymentMethod" INTEGER NOT NULL,
    "Status" INTEGER NOT NULL,
    "TotalAmount" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Invoices_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE SET NULL
);

INSERT INTO "ef_temp_Invoices" ("Id", "CreatedAt", "CreatedByUserId", "CreatedByUserName", "DiscountAmount", "InvoiceNumber", "IsDeleted", "MemberId", "Note", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt")
SELECT "Id", "CreatedAt", "CreatedByUserId", "CreatedByUserName", "DiscountAmount", "InvoiceNumber", "IsDeleted", "MemberId", "Note", "PaymentMethod", "Status", "TotalAmount", "UpdatedAt"
FROM "Invoices";

CREATE TABLE "ef_temp_InvoiceDetails" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_InvoiceDetails" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "InvoiceId" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "ItemName" TEXT NOT NULL,
    "ItemType" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "ReferenceId" TEXT NULL,
    "SubscriptionId" TEXT NULL,
    "UnitPrice" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_InvoiceDetails_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_InvoiceDetails" ("Id", "CreatedAt", "InvoiceId", "IsDeleted", "ItemName", "ItemType", "Quantity", "ReferenceId", "SubscriptionId", "UnitPrice", "UpdatedAt")
SELECT "Id", "CreatedAt", "InvoiceId", "IsDeleted", "ItemName", "ItemType", "Quantity", "ReferenceId", "SubscriptionId", "UnitPrice", "UpdatedAt"
FROM "InvoiceDetails";

CREATE TABLE "ef_temp_Inventories" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Inventories" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "LastUpdated" TEXT NOT NULL,
    "ProductId" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    "WarehouseId" TEXT NOT NULL,
    CONSTRAINT "FK_Inventories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Inventories_Warehouses_WarehouseId" FOREIGN KEY ("WarehouseId") REFERENCES "Warehouses" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Inventories" ("Id", "CreatedAt", "IsDeleted", "LastUpdated", "ProductId", "Quantity", "UpdatedAt", "WarehouseId")
SELECT "Id", "CreatedAt", "IsDeleted", "LastUpdated", "ProductId", "Quantity", "UpdatedAt", "WarehouseId"
FROM "Inventories";

CREATE TABLE "ef_temp_IncidentLogs" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_IncidentLogs" PRIMARY KEY,
    "CreatedAt" TEXT NOT NULL,
    "Date" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "EquipmentId" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "ReportedBy" TEXT NULL,
    "ResolutionStatus" TEXT NOT NULL,
    "Severity" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_IncidentLogs_Equipments_EquipmentId" FOREIGN KEY ("EquipmentId") REFERENCES "Equipments" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_IncidentLogs" ("Id", "CreatedAt", "Date", "Description", "EquipmentId", "IsDeleted", "ReportedBy", "ResolutionStatus", "Severity", "UpdatedAt")
SELECT "Id", "CreatedAt", "Date", "Description", "EquipmentId", "IsDeleted", "ReportedBy", "ResolutionStatus", "Severity", "UpdatedAt"
FROM "IncidentLogs";

CREATE TABLE "ef_temp_EquipmentTransactions" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_EquipmentTransactions" PRIMARY KEY,
    "AfterQuantity" INTEGER NOT NULL,
    "BeforeQuantity" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CreatedBy" TEXT NULL,
    "Date" TEXT NOT NULL,
    "EquipmentId" TEXT NOT NULL,
    "FromLocation" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Note" TEXT NULL,
    "Quantity" INTEGER NOT NULL,
    "ToLocation" TEXT NULL,
    "Type" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_EquipmentTransactions_Equipments_EquipmentId" FOREIGN KEY ("EquipmentId") REFERENCES "Equipments" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_EquipmentTransactions" ("Id", "AfterQuantity", "BeforeQuantity", "CreatedAt", "CreatedBy", "Date", "EquipmentId", "FromLocation", "IsDeleted", "Note", "Quantity", "ToLocation", "Type", "UpdatedAt")
SELECT "Id", "AfterQuantity", "BeforeQuantity", "CreatedAt", "CreatedBy", "Date", "EquipmentId", "FromLocation", "IsDeleted", "Note", "Quantity", "ToLocation", "Type", "UpdatedAt"
FROM "EquipmentTransactions";

CREATE TABLE "ef_temp_Equipments" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Equipments" PRIMARY KEY,
    "AccumulatedDepreciation" TEXT NOT NULL,
    "CategoryId" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "DepreciationStartDate" TEXT NULL,
    "Description" TEXT NULL,
    "EquipmentCode" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "IsFullyDepreciated" INTEGER NOT NULL,
    "LastMaintenanceDate" TEXT NULL,
    "Location" TEXT NULL,
    "MaintenanceIntervalDays" INTEGER NOT NULL,
    "MonthlyDepreciationAmount" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "NextMaintenanceDate" TEXT NULL,
    "Priority" INTEGER NOT NULL,
    "ProviderId" TEXT NULL,
    "PurchaseDate" TEXT NOT NULL,
    "PurchasePrice" TEXT NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "RemainingValue" TEXT NOT NULL,
    "SalvageValue" TEXT NOT NULL,
    "SerialNumber" TEXT NULL,
    "Status" INTEGER NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UsefulLifeMonths" INTEGER NOT NULL,
    "WarrantyExpiryDate" TEXT NULL,
    "Weight" REAL NULL,
    CONSTRAINT "FK_Equipments_EquipmentCategories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "EquipmentCategories" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Equipments_Providers_ProviderId" FOREIGN KEY ("ProviderId") REFERENCES "Providers" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_Equipments" ("Id", "AccumulatedDepreciation", "CategoryId", "CreatedAt", "DepreciationStartDate", "Description", "EquipmentCode", "IsDeleted", "IsFullyDepreciated", "LastMaintenanceDate", "Location", "MaintenanceIntervalDays", "MonthlyDepreciationAmount", "Name", "NextMaintenanceDate", "Priority", "ProviderId", "PurchaseDate", "PurchasePrice", "Quantity", "RemainingValue", "SalvageValue", "SerialNumber", "Status", "UpdatedAt", "UsefulLifeMonths", "WarrantyExpiryDate", "Weight")
SELECT "Id", "AccumulatedDepreciation", "CategoryId", "CreatedAt", "DepreciationStartDate", "Description", "EquipmentCode", "IsDeleted", "IsFullyDepreciated", "LastMaintenanceDate", "Location", "MaintenanceIntervalDays", "MonthlyDepreciationAmount", "Name", "NextMaintenanceDate", "Priority", "ProviderId", "PurchaseDate", "PurchasePrice", "Quantity", "RemainingValue", "SalvageValue", "SerialNumber", "Status", "UpdatedAt", "UsefulLifeMonths", "WarrantyExpiryDate", "Weight"
FROM "Equipments";

CREATE TABLE "ef_temp_EquipmentProviderHistories" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_EquipmentProviderHistories" PRIMARY KEY,
    "ChangeDate" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "EquipmentId" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "NewProviderId" TEXT NULL,
    "OldProviderId" TEXT NULL,
    "Reason" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_EquipmentProviderHistories_Equipments_EquipmentId" FOREIGN KEY ("EquipmentId") REFERENCES "Equipments" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EquipmentProviderHistories_Providers_NewProviderId" FOREIGN KEY ("NewProviderId") REFERENCES "Providers" ("Id"),
    CONSTRAINT "FK_EquipmentProviderHistories_Providers_OldProviderId" FOREIGN KEY ("OldProviderId") REFERENCES "Providers" ("Id")
);

INSERT INTO "ef_temp_EquipmentProviderHistories" ("Id", "ChangeDate", "CreatedAt", "EquipmentId", "IsDeleted", "NewProviderId", "OldProviderId", "Reason", "UpdatedAt")
SELECT "Id", "ChangeDate", "CreatedAt", "EquipmentId", "IsDeleted", "NewProviderId", "OldProviderId", "Reason", "UpdatedAt"
FROM "EquipmentProviderHistories";

CREATE TABLE "ef_temp_EquipmentCategories" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_EquipmentCategories" PRIMARY KEY,
    "AvgMaintenanceCost" TEXT NULL,
    "Code" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Group" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "StandardWarrantyMonths" INTEGER NULL,
    "UpdatedAt" TEXT NULL
);

INSERT INTO "ef_temp_EquipmentCategories" ("Id", "AvgMaintenanceCost", "Code", "CreatedAt", "Description", "Group", "IsDeleted", "Name", "StandardWarrantyMonths", "UpdatedAt")
SELECT "Id", "AvgMaintenanceCost", "Code", "CreatedAt", "Description", "Group", "IsDeleted", "Name", "StandardWarrantyMonths", "UpdatedAt"
FROM "EquipmentCategories";

CREATE TABLE "ef_temp_Depreciations" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Depreciations" PRIMARY KEY,
    "Amount" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "Date" TEXT NOT NULL,
    "EquipmentId" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "Note" TEXT NULL,
    "PeriodMonth" INTEGER NOT NULL,
    "PeriodYear" INTEGER NOT NULL,
    "RemainingValue" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Depreciations_Equipments_EquipmentId" FOREIGN KEY ("EquipmentId") REFERENCES "Equipments" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Depreciations" ("Id", "Amount", "CreatedAt", "Date", "EquipmentId", "IsDeleted", "Note", "PeriodMonth", "PeriodYear", "RemainingValue", "UpdatedAt")
SELECT "Id", "Amount", "CreatedAt", "Date", "EquipmentId", "IsDeleted", "Note", "PeriodMonth", "PeriodYear", "RemainingValue", "UpdatedAt"
FROM "Depreciations";

CREATE TABLE "ef_temp_Classes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Classes" PRIMARY KEY,
    "ClassName" TEXT NOT NULL,
    "ClassType" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "CurrentEnrollment" INTEGER NOT NULL,
    "Description" TEXT NULL,
    "EndTime" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MaxCapacity" INTEGER NOT NULL,
    "ScheduleDay" TEXT NULL,
    "StartTime" TEXT NOT NULL,
    "TrainerId" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Classes_Trainers_TrainerId" FOREIGN KEY ("TrainerId") REFERENCES "Trainers" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_Classes" ("Id", "ClassName", "ClassType", "CreatedAt", "CurrentEnrollment", "Description", "EndTime", "IsActive", "IsDeleted", "MaxCapacity", "ScheduleDay", "StartTime", "TrainerId", "UpdatedAt")
SELECT "Id", "ClassName", "ClassType", "CreatedAt", "CurrentEnrollment", "Description", "EndTime", "IsActive", "IsDeleted", "MaxCapacity", "ScheduleDay", "StartTime", "TrainerId", "UpdatedAt"
FROM "Classes";

CREATE TABLE "ef_temp_ClassEnrollments" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ClassEnrollments" PRIMARY KEY,
    "AttendanceDate" TEXT NULL,
    "ClassId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "EnrolledDate" TEXT NOT NULL,
    "IsAttended" INTEGER NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberId" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_ClassEnrollments_Classes_ClassId" FOREIGN KEY ("ClassId") REFERENCES "Classes" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ClassEnrollments_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_ClassEnrollments" ("Id", "AttendanceDate", "ClassId", "CreatedAt", "EnrolledDate", "IsAttended", "IsDeleted", "MemberId", "UpdatedAt")
SELECT "Id", "AttendanceDate", "ClassId", "CreatedAt", "EnrolledDate", "IsAttended", "IsDeleted", "MemberId", "UpdatedAt"
FROM "ClassEnrollments";

CREATE TABLE "ef_temp_CheckIns" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_CheckIns" PRIMARY KEY,
    "CheckInTime" TEXT NOT NULL,
    "CheckOutTime" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "MemberId" TEXT NOT NULL,
    "Note" TEXT NULL,
    "SubscriptionId" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_CheckIns_MemberSubscriptions_SubscriptionId" FOREIGN KEY ("SubscriptionId") REFERENCES "MemberSubscriptions" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_CheckIns_Members_MemberId" FOREIGN KEY ("MemberId") REFERENCES "Members" ("Id") ON DELETE RESTRICT
);

INSERT INTO "ef_temp_CheckIns" ("Id", "CheckInTime", "CheckOutTime", "CreatedAt", "IsDeleted", "MemberId", "Note", "SubscriptionId", "UpdatedAt")
SELECT "Id", "CheckInTime", "CheckOutTime", "CreatedAt", "IsDeleted", "MemberId", "Note", "SubscriptionId", "UpdatedAt"
FROM "CheckIns";

CREATE TABLE "ef_temp_AuditLogs" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AuditLogs" PRIMARY KEY,
    "Action" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "EntityName" TEXT NOT NULL,
    "IPAddress" TEXT NULL,
    "IsDeleted" INTEGER NOT NULL,
    "NewValues" TEXT NOT NULL,
    "OldValues" TEXT NOT NULL,
    "Reason" TEXT NULL,
    "ResourceId" TEXT NULL,
    "ResourceName" TEXT NULL,
    "Severity" INTEGER NOT NULL,
    "Timestamp" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "UserAgent" TEXT NULL,
    "UserId" TEXT NOT NULL,
    "UserName" TEXT NOT NULL,
    "UserRole" TEXT NOT NULL
);

INSERT INTO "ef_temp_AuditLogs" ("Id", "Action", "CreatedAt", "EntityName", "IPAddress", "IsDeleted", "NewValues", "OldValues", "Reason", "ResourceId", "ResourceName", "Severity", "Timestamp", "UpdatedAt", "UserAgent", "UserId", "UserName", "UserRole")
SELECT "Id", "Action", "CreatedAt", "EntityName", "IPAddress", "IsDeleted", "NewValues", "OldValues", "Reason", "ResourceId", "ResourceName", "Severity", "Timestamp", "UpdatedAt", "UserAgent", "UserId", "UserName", "UserRole"
FROM "AuditLogs";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Warehouses";

ALTER TABLE "ef_temp_Warehouses" RENAME TO "Warehouses";

DROP TABLE "Users";

ALTER TABLE "ef_temp_Users" RENAME TO "Users";

DROP TABLE "UserRoles";

ALTER TABLE "ef_temp_UserRoles" RENAME TO "UserRoles";

DROP TABLE "Trainers";

ALTER TABLE "ef_temp_Trainers" RENAME TO "Trainers";

DROP TABLE "TrainerMemberAssignments";

ALTER TABLE "ef_temp_TrainerMemberAssignments" RENAME TO "TrainerMemberAssignments";

DROP TABLE "StockTransactions";

ALTER TABLE "ef_temp_StockTransactions" RENAME TO "StockTransactions";

DROP TABLE "Roles";

ALTER TABLE "ef_temp_Roles" RENAME TO "Roles";

DROP TABLE "Providers";

ALTER TABLE "ef_temp_Providers" RENAME TO "Providers";

DROP TABLE "Products";

ALTER TABLE "ef_temp_Products" RENAME TO "Products";

DROP TABLE "Payments";

ALTER TABLE "ef_temp_Payments" RENAME TO "Payments";

DROP TABLE "Orders";

ALTER TABLE "ef_temp_Orders" RENAME TO "Orders";

DROP TABLE "OrderDetails";

ALTER TABLE "ef_temp_OrderDetails" RENAME TO "OrderDetails";

DROP TABLE "MemberSubscriptions";

ALTER TABLE "ef_temp_MemberSubscriptions" RENAME TO "MemberSubscriptions";

DROP TABLE "MembershipPackages";

ALTER TABLE "ef_temp_MembershipPackages" RENAME TO "MembershipPackages";

DROP TABLE "Members";

ALTER TABLE "ef_temp_Members" RENAME TO "Members";

DROP TABLE "MaintenanceMaterials";

ALTER TABLE "ef_temp_MaintenanceMaterials" RENAME TO "MaintenanceMaterials";

DROP TABLE "MaintenanceLogs";

ALTER TABLE "ef_temp_MaintenanceLogs" RENAME TO "MaintenanceLogs";

DROP TABLE "Invoices";

ALTER TABLE "ef_temp_Invoices" RENAME TO "Invoices";

DROP TABLE "InvoiceDetails";

ALTER TABLE "ef_temp_InvoiceDetails" RENAME TO "InvoiceDetails";

DROP TABLE "Inventories";

ALTER TABLE "ef_temp_Inventories" RENAME TO "Inventories";

DROP TABLE "IncidentLogs";

ALTER TABLE "ef_temp_IncidentLogs" RENAME TO "IncidentLogs";

DROP TABLE "EquipmentTransactions";

ALTER TABLE "ef_temp_EquipmentTransactions" RENAME TO "EquipmentTransactions";

DROP TABLE "Equipments";

ALTER TABLE "ef_temp_Equipments" RENAME TO "Equipments";

DROP TABLE "EquipmentProviderHistories";

ALTER TABLE "ef_temp_EquipmentProviderHistories" RENAME TO "EquipmentProviderHistories";

DROP TABLE "EquipmentCategories";

ALTER TABLE "ef_temp_EquipmentCategories" RENAME TO "EquipmentCategories";

DROP TABLE "Depreciations";

ALTER TABLE "ef_temp_Depreciations" RENAME TO "Depreciations";

DROP TABLE "Classes";

ALTER TABLE "ef_temp_Classes" RENAME TO "Classes";

DROP TABLE "ClassEnrollments";

ALTER TABLE "ef_temp_ClassEnrollments" RENAME TO "ClassEnrollments";

DROP TABLE "CheckIns";

ALTER TABLE "ef_temp_CheckIns" RENAME TO "CheckIns";

DROP TABLE "AuditLogs";

ALTER TABLE "ef_temp_AuditLogs" RENAME TO "AuditLogs";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");

CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");

CREATE INDEX "IX_Trainers_Email" ON "Trainers" ("Email") WHERE Email IS NOT NULL;

CREATE INDEX "IX_Trainers_IsActive" ON "Trainers" ("IsActive") WHERE IsDeleted = 0;

CREATE UNIQUE INDEX "IX_Trainers_UserId" ON "Trainers" ("UserId") WHERE UserId IS NOT NULL;

CREATE INDEX "IX_TrainerMemberAssignments_MemberId" ON "TrainerMemberAssignments" ("MemberId");

CREATE INDEX "IX_TrainerMemberAssignments_TrainerId" ON "TrainerMemberAssignments" ("TrainerId");

CREATE INDEX "IX_StockTransactions_FromWarehouseId" ON "StockTransactions" ("FromWarehouseId");

CREATE INDEX "IX_StockTransactions_ProductId" ON "StockTransactions" ("ProductId");

CREATE INDEX "IX_StockTransactions_ProviderId" ON "StockTransactions" ("ProviderId");

CREATE INDEX "IX_StockTransactions_ToWarehouseId" ON "StockTransactions" ("ToWarehouseId");

CREATE INDEX "IX_Products_ProviderId" ON "Products" ("ProviderId");

CREATE UNIQUE INDEX "IX_Products_SKU" ON "Products" ("SKU") WHERE IsDeleted = 0;

CREATE INDEX "IX_Payments_MemberSubscriptionId" ON "Payments" ("MemberSubscriptionId");

CREATE INDEX "IX_Payments_PaymentDate" ON "Payments" ("PaymentDate");

CREATE INDEX "IX_Payments_Status" ON "Payments" ("Status");

CREATE INDEX "IX_Payments_TransactionId" ON "Payments" ("TransactionId") WHERE TransactionId IS NOT NULL;

CREATE INDEX "IX_Orders_MemberId" ON "Orders" ("MemberId");

CREATE INDEX "IX_OrderDetails_OrderId" ON "OrderDetails" ("OrderId");

CREATE INDEX "IX_OrderDetails_ProductId" ON "OrderDetails" ("ProductId");

CREATE INDEX "IX_MemberSubscriptions_EndDate" ON "MemberSubscriptions" ("EndDate") WHERE Status = 2;

CREATE INDEX "IX_MemberSubscriptions_MemberId" ON "MemberSubscriptions" ("MemberId");

CREATE INDEX "IX_MemberSubscriptions_MemberId_Status" ON "MemberSubscriptions" ("MemberId", "Status");

CREATE INDEX "IX_MemberSubscriptions_PackageId" ON "MemberSubscriptions" ("PackageId");

CREATE INDEX "IX_MemberSubscriptions_Status" ON "MemberSubscriptions" ("Status") WHERE IsDeleted = 0;

CREATE INDEX "IX_MembershipPackages_IsActive" ON "MembershipPackages" ("IsActive") WHERE IsDeleted = 0;

CREATE INDEX "IX_Members_Email" ON "Members" ("Email");

CREATE INDEX "IX_Members_FullName" ON "Members" ("FullName");

CREATE UNIQUE INDEX "IX_Members_MemberCode" ON "Members" ("MemberCode") WHERE IsDeleted = 0;

CREATE INDEX "IX_Members_PhoneNumber" ON "Members" ("PhoneNumber");

CREATE INDEX "IX_Members_Status" ON "Members" ("Status") WHERE IsDeleted = 0;

CREATE UNIQUE INDEX "IX_Members_UserId" ON "Members" ("UserId");

CREATE INDEX "IX_MaintenanceMaterials_MaintenanceLogId" ON "MaintenanceMaterials" ("MaintenanceLogId");

CREATE INDEX "IX_MaintenanceMaterials_ProductId" ON "MaintenanceMaterials" ("ProductId");

CREATE INDEX "IX_MaintenanceLogs_EquipmentId" ON "MaintenanceLogs" ("EquipmentId");

CREATE INDEX "IX_MaintenanceLogs_ProviderId" ON "MaintenanceLogs" ("ProviderId");

CREATE UNIQUE INDEX "IX_Invoices_InvoiceNumber" ON "Invoices" ("InvoiceNumber") WHERE IsDeleted = 0;

CREATE INDEX "IX_Invoices_MemberId" ON "Invoices" ("MemberId");

CREATE INDEX "IX_InvoiceDetails_InvoiceId" ON "InvoiceDetails" ("InvoiceId");

CREATE INDEX "IX_Inventories_ProductId" ON "Inventories" ("ProductId");

CREATE INDEX "IX_Inventories_WarehouseId" ON "Inventories" ("WarehouseId");

CREATE INDEX "IX_IncidentLogs_EquipmentId" ON "IncidentLogs" ("EquipmentId");

CREATE INDEX "IX_EquipmentTransactions_EquipmentId" ON "EquipmentTransactions" ("EquipmentId");

CREATE INDEX "IX_Equipments_CategoryId" ON "Equipments" ("CategoryId");

CREATE INDEX "IX_Equipments_ProviderId" ON "Equipments" ("ProviderId");

CREATE INDEX "IX_EquipmentProviderHistories_EquipmentId" ON "EquipmentProviderHistories" ("EquipmentId");

CREATE INDEX "IX_EquipmentProviderHistories_NewProviderId" ON "EquipmentProviderHistories" ("NewProviderId");

CREATE INDEX "IX_EquipmentProviderHistories_OldProviderId" ON "EquipmentProviderHistories" ("OldProviderId");

CREATE INDEX "IX_Depreciations_EquipmentId" ON "Depreciations" ("EquipmentId");

CREATE INDEX "IX_Classes_ScheduleDay" ON "Classes" ("ScheduleDay") WHERE IsActive = 1;

CREATE INDEX "IX_Classes_ScheduleDay_StartTime" ON "Classes" ("ScheduleDay", "StartTime") WHERE IsActive = 1;

CREATE INDEX "IX_Classes_TrainerId" ON "Classes" ("TrainerId");

CREATE INDEX "IX_ClassEnrollments_ClassId" ON "ClassEnrollments" ("ClassId");

CREATE UNIQUE INDEX "IX_ClassEnrollments_ClassId_MemberId" ON "ClassEnrollments" ("ClassId", "MemberId");

CREATE INDEX "IX_ClassEnrollments_EnrolledDate" ON "ClassEnrollments" ("EnrolledDate");

CREATE INDEX "IX_ClassEnrollments_MemberId" ON "ClassEnrollments" ("MemberId");

CREATE INDEX "IX_CheckIns_CheckInTime" ON "CheckIns" ("CheckInTime");

CREATE INDEX "IX_CheckIns_MemberId" ON "CheckIns" ("MemberId");

CREATE INDEX "IX_CheckIns_MemberId_CheckInTime" ON "CheckIns" ("MemberId", "CheckInTime");

CREATE INDEX "IX_CheckIns_SubscriptionId" ON "CheckIns" ("SubscriptionId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260409080220_AddStaffToInvoice', '8.0.0');

COMMIT;

