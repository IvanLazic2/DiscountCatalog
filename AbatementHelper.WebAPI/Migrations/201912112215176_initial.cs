namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManagerEntities", "StoreAdmin_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ManagerEntities", "StoreAdmin_Id");
            AddForeignKey("dbo.ManagerEntities", "StoreAdmin_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManagerEntities", "StoreAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ManagerEntities", new[] { "StoreAdmin_Id" });
            DropColumn("dbo.ManagerEntities", "StoreAdmin_Id");
        }
    }
}
