namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingrelationbetweenRepairEventandCarPart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CarParts", "RepairEvent_Id", "dbo.RepairEvents");
            DropIndex("dbo.CarParts", new[] { "RepairEvent_Id" });
            AlterColumn("dbo.CarParts", "RepairEvent_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.CarParts", "RepairEvent_Id");
            AddForeignKey("dbo.CarParts", "RepairEvent_Id", "dbo.RepairEvents", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "RepairEvent_Id", "dbo.RepairEvents");
            DropIndex("dbo.CarParts", new[] { "RepairEvent_Id" });
            AlterColumn("dbo.CarParts", "RepairEvent_Id", c => c.Int());
            CreateIndex("dbo.CarParts", "RepairEvent_Id");
            AddForeignKey("dbo.CarParts", "RepairEvent_Id", "dbo.RepairEvents", "Id");
        }
    }
}
