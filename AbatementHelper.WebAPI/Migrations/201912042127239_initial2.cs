namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(),
                        Store_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreEntities", t => t.Store_Id)
                .Index(t => t.Store_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Store_Id", "dbo.StoreEntities");
            DropIndex("dbo.Products", new[] { "Store_Id" });
            DropTable("dbo.Products");
        }
    }
}
