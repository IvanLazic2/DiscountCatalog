namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _int : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductEntities", "DiscountDateBegin", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ProductEntities", "DiscountDateEnd", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductEntities", "DiscountDateEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProductEntities", "DiscountDateBegin", c => c.DateTime(nullable: false));
        }
    }
}
