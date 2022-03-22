namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrelationsbetweenEspenseandVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Expenses", "VehicleId");
            AddForeignKey("dbo.Expenses", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.Expenses", new[] { "VehicleId" });
            DropColumn("dbo.Expenses", "VehicleId");
        }
    }
}
