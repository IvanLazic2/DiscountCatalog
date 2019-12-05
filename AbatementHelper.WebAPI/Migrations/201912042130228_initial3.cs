namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Products", newName: "ProductEntities");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProductEntities", newName: "Products");
        }
    }
}
