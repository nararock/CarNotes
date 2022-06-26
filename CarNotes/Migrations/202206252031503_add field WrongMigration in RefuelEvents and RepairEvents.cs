namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldWrongMigrationinRefuelEventsandRepairEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefuelEvents", "WrongMileage", c => c.Boolean(nullable: false));
            AddColumn("dbo.RepairEvents", "WrongMileage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RepairEvents", "WrongMileage");
            DropColumn("dbo.RefuelEvents", "WrongMileage");
        }
    }
}
