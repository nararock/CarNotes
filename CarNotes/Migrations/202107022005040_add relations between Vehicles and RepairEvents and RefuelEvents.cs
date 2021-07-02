namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationsbetweenVehiclesandRepairEventsandRefuelEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefuelEvents", "VehicleId", c => c.Int(nullable: false));
            AddColumn("dbo.RepairEvents", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.RefuelEvents", "VehicleId");
            CreateIndex("dbo.RepairEvents", "VehicleId");
            AddForeignKey("dbo.RepairEvents", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RefuelEvents", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefuelEvents", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.RepairEvents", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.RepairEvents", new[] { "VehicleId" });
            DropIndex("dbo.RefuelEvents", new[] { "VehicleId" });
            DropColumn("dbo.RepairEvents", "VehicleId");
            DropColumn("dbo.RefuelEvents", "VehicleId");
        }
    }
}
