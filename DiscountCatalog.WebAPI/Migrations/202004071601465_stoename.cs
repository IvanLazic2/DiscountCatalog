namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stoename : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StoreEntities", new[] { "StoreName" });
            CreateIndex("dbo.StoreEntities", "StoreName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StoreEntities", new[] { "StoreName" });
            CreateIndex("dbo.StoreEntities", "StoreName", unique: true);
        }
    }
}
