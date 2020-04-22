namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntities", "Currency", c => c.String());
            AddColumn("dbo.ProductEntities", "MeasuringUnit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntities", "MeasuringUnit");
            DropColumn("dbo.ProductEntities", "Currency");
        }
    }
}
