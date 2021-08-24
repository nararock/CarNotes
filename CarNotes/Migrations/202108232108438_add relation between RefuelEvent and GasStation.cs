namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationbetweenRefuelEventandGasStation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RefuelEvents", "Station_ID", "dbo.GasStations");
            DropIndex("dbo.RefuelEvents", new[] { "Station_ID" });
            AlterColumn("dbo.RefuelEvents", "Station_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.RefuelEvents", "Station_ID");
            AddForeignKey("dbo.RefuelEvents", "Station_ID", "dbo.GasStations", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefuelEvents", "Station_ID", "dbo.GasStations");
            DropIndex("dbo.RefuelEvents", new[] { "Station_ID" });
            AlterColumn("dbo.RefuelEvents", "Station_ID", c => c.Int());
            CreateIndex("dbo.RefuelEvents", "Station_ID");
            AddForeignKey("dbo.RefuelEvents", "Station_ID", "dbo.GasStations", "ID");
        }
    }
}
