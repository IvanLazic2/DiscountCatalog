namespace AbatementHelper.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workinghours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreEntities", "WorkingHoursWeekBegin", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursWeekEnd", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursWeekendsBegin", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursWeekendsEnd", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursHolidaysBegin", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursHolidaysEnd", c => c.String());
            DropColumn("dbo.StoreEntities", "WorkingHoursWeek");
            DropColumn("dbo.StoreEntities", "WorkingHoursWeekends");
            DropColumn("dbo.StoreEntities", "WorkingHoursHolidays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoreEntities", "WorkingHoursHolidays", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursWeekends", c => c.String());
            AddColumn("dbo.StoreEntities", "WorkingHoursWeek", c => c.String());
            DropColumn("dbo.StoreEntities", "WorkingHoursHolidaysEnd");
            DropColumn("dbo.StoreEntities", "WorkingHoursHolidaysBegin");
            DropColumn("dbo.StoreEntities", "WorkingHoursWeekendsEnd");
            DropColumn("dbo.StoreEntities", "WorkingHoursWeekendsBegin");
            DropColumn("dbo.StoreEntities", "WorkingHoursWeekEnd");
            DropColumn("dbo.StoreEntities", "WorkingHoursWeekBegin");
        }
    }
}
