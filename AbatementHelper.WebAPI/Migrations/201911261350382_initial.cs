namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AdminInfoes", newName: "WebApiAdminInfoes");
            RenameTable(name: "dbo.StoreAdminInfoes", newName: "WebApiStoreAdminInfoes");
            RenameTable(name: "dbo.StoreInfoes", newName: "WebApiStoreInfoes");
            RenameTable(name: "dbo.UserInfoes", newName: "WebApiUserInfoes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.WebApiUserInfoes", newName: "UserInfoes");
            RenameTable(name: "dbo.WebApiStoreInfoes", newName: "StoreInfoes");
            RenameTable(name: "dbo.WebApiStoreAdminInfoes", newName: "StoreAdminInfoes");
            RenameTable(name: "dbo.WebApiAdminInfoes", newName: "AdminInfoes");
        }
    }
}
