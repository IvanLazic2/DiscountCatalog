namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductEntities", "DiscountDateBegin", c => c.String(nullable: false));
            AlterColumn("dbo.ProductEntities", "DiscountDateEnd", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductEntities", "DiscountDateEnd", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ProductEntities", "DiscountDateBegin", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
