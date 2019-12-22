namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _int : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ManagerEntities", new[] { "StoreAdmin_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "User_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreAdmin_Id" });
            AlterColumn("dbo.ManagerEntities", "StoreAdmin_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ManagerEntities", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StoreEntities", "StoreAdmin_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ProductEntities", "ProductName", c => c.String(nullable: false));
            AlterColumn("dbo.ProductEntities", "ProductOldPrice", c => c.String(nullable: false));
            AlterColumn("dbo.ProductEntities", "ProductNewPrice", c => c.String(nullable: false));
            CreateIndex("dbo.ManagerEntities", "StoreAdmin_Id");
            CreateIndex("dbo.ManagerEntities", "User_Id");
            CreateIndex("dbo.StoreEntities", "StoreAdmin_Id");
            AddForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StoreEntities", new[] { "StoreAdmin_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "User_Id" });
            DropIndex("dbo.ManagerEntities", new[] { "StoreAdmin_Id" });
            AlterColumn("dbo.ProductEntities", "ProductNewPrice", c => c.String());
            AlterColumn("dbo.ProductEntities", "ProductOldPrice", c => c.String());
            AlterColumn("dbo.ProductEntities", "ProductName", c => c.String());
            AlterColumn("dbo.StoreEntities", "StoreAdmin_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.ManagerEntities", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.ManagerEntities", "StoreAdmin_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.StoreEntities", "StoreAdmin_Id");
            CreateIndex("dbo.ManagerEntities", "User_Id");
            CreateIndex("dbo.ManagerEntities", "StoreAdmin_Id");
            AddForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
