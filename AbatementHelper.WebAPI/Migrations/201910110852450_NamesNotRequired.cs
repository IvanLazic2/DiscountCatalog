namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamesNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.Boolean(nullable: true));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.Boolean(nullable: false));
        }
    }
}
