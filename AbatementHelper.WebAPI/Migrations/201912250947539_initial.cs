namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagerEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StoreAdmin_Id = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.StoreAdmin_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.StoreAdmin_Id)
                .Index(t => t.User_Id);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StoreEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        StoreName = c.String(nullable: false, maxLength: 200),
                        WorkingHoursWeek = c.String(),
                        WorkingHoursWeekends = c.String(),
                        WorkingHoursHolidays = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Street = c.String(),
                        Approved = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        StoreAdmin_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.StoreAdmin_Id, cascadeDelete: true)
                .Index(t => t.StoreName, unique: true)
                .Index(t => t.StoreAdmin_Id);
            
            CreateTable(
                "dbo.ProductEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(nullable: false),
                        CompanyName = c.String(),
                        ProductOldPrice = c.Double(nullable: false),
                        ProductNewPrice = c.Double(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        DiscountDateBegin = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DiscountDateEnd = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Quantity = c.String(),
                        Description = c.String(),
                        Note = c.String(),
                        Expired = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Store_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreEntities", t => t.Store_Id, cascadeDelete: true)
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
                .ForeignKey("dbo.StoreEntities", t => t.StoreEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.ManagerEntities", t => t.ManagerEntity_Id, cascadeDelete: true)
                .Index(t => t.StoreEntity_Id)
                .Index(t => t.ManagerEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ManagerEntities", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductEntities", "Store_Id", "dbo.StoreEntities");
            DropForeignKey("dbo.StoreEntityManagerEntities", "ManagerEntity_Id", "dbo.ManagerEntities");
            DropForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities");
            DropForeignKey("dbo.ManagerEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "ManagerEntity_Id" });
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductEntities", new[] { "Store_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreAdmin_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreName" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Email" });
            DropIndex("dbo.ManagerEntities", new[] { "User_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "StoreAdmin_Id" });
            DropTable("dbo.StoreEntityManagerEntities");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductEntities");
            DropTable("dbo.StoreEntities");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ManagerEntities");
        }
    }
}
