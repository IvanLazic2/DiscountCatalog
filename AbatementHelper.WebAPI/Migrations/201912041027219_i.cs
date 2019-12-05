namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class i : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities");
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id" });
            DropPrimaryKey("dbo.StoreEntities");
            DropPrimaryKey("dbo.StoreEntityManagerEntities");
            AddColumn("dbo.StoreEntities", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.StoreEntities", "Deleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.StoreEntities", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StoreEntityManagerEntities", "StoreEntity_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.StoreEntities", "Id");
            AddPrimaryKey("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id", "ManagerEntity_Id" });
            CreateIndex("dbo.StoreEntityManagerEntities", "StoreEntity_Id");
            AddForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities");
            DropIndex("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id" });
            DropPrimaryKey("dbo.StoreEntityManagerEntities");
            DropPrimaryKey("dbo.StoreEntities");
            AlterColumn("dbo.StoreEntityManagerEntities", "StoreEntity_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.StoreEntities", "Id", c => c.Guid(nullable: false));
            DropColumn("dbo.StoreEntities", "Deleted");
            DropColumn("dbo.StoreEntities", "Approved");
            AddPrimaryKey("dbo.StoreEntityManagerEntities", new[] { "StoreEntity_Id", "ManagerEntity_Id" });
            AddPrimaryKey("dbo.StoreEntities", "Id");
            CreateIndex("dbo.StoreEntityManagerEntities", "StoreEntity_Id");
            AddForeignKey("dbo.StoreEntityManagerEntities", "StoreEntity_Id", "dbo.StoreEntities", "Id", cascadeDelete: true);
        }
    }
}
