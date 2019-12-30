namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreEntities", "StoreImage", c => c.Binary());
            AddColumn("dbo.ProductEntities", "ProductImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntities", "ProductImage");
            DropColumn("dbo.StoreEntities", "StoreImage");
        }
    }
}
