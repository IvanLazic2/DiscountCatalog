namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsStore1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsStore", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsStore");
        }
    }
}
