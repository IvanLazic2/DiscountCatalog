namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _in : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagerEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Administrator_Id = c.String(nullable: false, maxLength: 128),
                        Identity_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreAdminEntities", t => t.Administrator_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Identity_Id)
                .Index(t => t.Administrator_Id)
                .Index(t => t.Identity_Id);
            
            CreateTable(
                "dbo.StoreAdminEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Identity_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Identity_Id)
                .Index(t => t.Identity_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Street = c.String(),
                        Approved = c.Boolean(),
                        Deleted = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        UserImage_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageEntities", t => t.UserImage_Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserImage_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ImageEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StoreName = c.String(nullable: false, maxLength: 200),
                        WorkingHoursWeekBegin = c.String(),
                        WorkingHoursWeekEnd = c.String(),
                        WorkingHoursWeekendsBegin = c.String(),
                        WorkingHoursWeekendsEnd = c.String(),
                        WorkingHoursHolidaysBegin = c.String(),
                        WorkingHoursHolidaysEnd = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Street = c.String(),
                        Approved = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Administrator_Id = c.String(nullable: false, maxLength: 128),
                        StoreImage_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreAdminEntities", t => t.Administrator_Id)
                .ForeignKey("dbo.ImageEntities", t => t.StoreImage_Id)
                .Index(t => t.StoreName)
                .Index(t => t.Administrator_Id)
                .Index(t => t.StoreImage_Id);
            
            CreateTable(
                "dbo.ProductEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(nullable: false),
                        CompanyName = c.String(),
                        OldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(),
                        DiscountPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountDateBegin = c.String(nullable: false),
                        DiscountDateEnd = c.String(nullable: false),
                        Quantity = c.String(),
                        MeasuringUnit = c.String(),
                        Description = c.String(),
                        Note = c.String(),
                        Expired = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ProductImage_Id = c.String(maxLength: 128),
                        Store_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageEntities", t => t.ProductImage_Id)
                .ForeignKey("dbo.StoreEntities", t => t.Store_Id)
                .Index(t => t.ProductImage_Id)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StoreEntityManagerEntities",
                c => new
                    {
                        StoreEntity_Id = c.String(nullable: false, maxLength: 128),
                        ManagerEntity_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.StoreEntity_Id, t.ManagerEntity_Id })
                .ForeignKey("dbo.StoreEntities", t => t.StoreEntity_Id)
                .ForeignKey("dbo.ManagerEntities", t => t.ManagerEntity_Id)
                .Index(t => t.StoreEntity_Id)
                .Index(t => t.ManagerEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ManagerEntities", "Identity_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManagerEntities", "Administrator_Id", "dbo.StoreAdminEntities");
            DropForeignKey("dbo.StoreEntities", "StoreImage_Id", "dbo.ImageEntities");
            DropForeignKey("dbo.ProductEntities", "Store_Id", "dbo.StoreEntities");
            DropForeignKey("dbo.ProductEntities", "ProductImage_Id", "dbo.ImageEntities");
            DropForeignKey("dbo.StoreEntityManagerEntities", "ManagerEntity_Id", "dbo.ManagerEntities");
            DropForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities");
            DropForeignKey("dbo.StoreEntities", "Administrator_Id", "dbo.StoreAdminEntities");
            DropForeignKey("dbo.StoreAdminEntities", "Identity_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "UserImage_Id", "dbo.ImageEntities");
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "ManagerEntity_Id" });
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductEntities", new[] { "Store_Id" });
            DropIndex("dbo.ProductEntities", new[] { "ProductImage_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreImage_Id" });
            DropIndex("dbo.StoreEntities", new[] { "Administrator_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreName" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserImage_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Email" });
            DropIndex("dbo.StoreAdminEntities", new[] { "Identity_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "Identity_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "Administrator_Id" });
            DropTable("dbo.StoreEntityManagerEntities");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductEntities");
            DropTable("dbo.StoreEntities");
            DropTable("dbo.ImageEntities");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StoreAdminEntities");
            DropTable("dbo.ManagerEntities");
        }
    }
}
