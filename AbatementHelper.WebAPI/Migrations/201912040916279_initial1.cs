namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreEntities", "StoreAdmin_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.StoreEntities", "StoreAdmin_Id");
            AddForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StoreEntities", new[] { "StoreAdmin_Id" });
            DropColumn("dbo.StoreEntities", "StoreAdmin_Id");
        }
    }
}
