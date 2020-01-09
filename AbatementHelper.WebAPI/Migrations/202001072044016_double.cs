namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductEntities", "ProductOldPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProductEntities", "ProductNewPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProductEntities", "DiscountPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductEntities", "DiscountPercentage", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductEntities", "ProductNewPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.ProductEntities", "ProductOldPrice", c => c.Double(nullable: false));
        }
    }
}
