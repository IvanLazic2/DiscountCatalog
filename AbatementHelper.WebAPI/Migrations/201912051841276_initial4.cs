namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StoreEntities", "StoreName", c => c.String(maxLength: 200));
            CreateIndex("dbo.StoreEntities", "StoreName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.StoreEntities", new[] { "StoreName" });
            AlterColumn("dbo.StoreEntities", "StoreName", c => c.String());
        }
    }
}
