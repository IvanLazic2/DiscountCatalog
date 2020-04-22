namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "UserImage_Id", "dbo.ImageEntities");
            DropForeignKey("dbo.ProductEntities", "ProductImage_Id", "dbo.ImageEntities");
            DropForeignKey("dbo.StoreEntities", "StoreImage_Id", "dbo.ImageEntities");
            DropIndex("dbo.AspNetUsers", new[] { "UserImage_Id" });
            DropIndex("dbo.StoreEntities", new[] { "StoreImage_Id" });
            DropIndex("dbo.ProductEntities", new[] { "ProductImage_Id" });
            AddColumn("dbo.AspNetUsers", "UserImage", c => c.Binary());
            AddColumn("dbo.StoreEntities", "StoreImage", c => c.Binary());
            AddColumn("dbo.ProductEntities", "ProductImage", c => c.Binary());
            DropColumn("dbo.AspNetUsers", "UserImage_Id");
            DropColumn("dbo.StoreEntities", "StoreImage_Id");
            DropColumn("dbo.ProductEntities", "ProductImage_Id");
            DropTable("dbo.ImageEntities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProductEntities", "ProductImage_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.StoreEntities", "StoreImage_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "UserImage_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.ProductEntities", "ProductImage");
            DropColumn("dbo.StoreEntities", "StoreImage");
            DropColumn("dbo.AspNetUsers", "UserImage");
            CreateIndex("dbo.ProductEntities", "ProductImage_Id");
            CreateIndex("dbo.StoreEntities", "StoreImage_Id");
            CreateIndex("dbo.AspNetUsers", "UserImage_Id");
            AddForeignKey("dbo.StoreEntities", "StoreImage_Id", "dbo.ImageEntities", "Id");
            AddForeignKey("dbo.ProductEntities", "ProductImage_Id", "dbo.ImageEntities", "Id");
            AddForeignKey("dbo.AspNetUsers", "UserImage_Id", "dbo.ImageEntities", "Id");
        }
    }
}
