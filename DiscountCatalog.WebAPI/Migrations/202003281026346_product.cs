namespace DiscountCatalog.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntities", "DateUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntities", "DateUpdated");
        }
    }
}
