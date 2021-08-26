namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomStationfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefuelEvents", "CustomStation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RefuelEvents", "CustomStation");
        }
    }
}
