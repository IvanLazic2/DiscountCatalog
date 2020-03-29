namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntities", "OldPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProductEntities", "NewPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ProductEntities", "ProductOldPrice");
            DropColumn("dbo.ProductEntities", "ProductNewPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductEntities", "ProductNewPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProductEntities", "ProductOldPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ProductEntities", "NewPrice");
            DropColumn("dbo.ProductEntities", "OldPrice");
        }
    }
}
