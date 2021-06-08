namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRepairEventandCarPart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarManufacturer = c.String(),
                        Article = c.String(),
                        Price = c.Double(nullable: false),
                        RepairEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RepairEvents", t => t.RepairEvent_Id)
                .Index(t => t.RepairEvent_Id);
            
            CreateTable(
                "dbo.RepairEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Mileage = c.Double(nullable: false),
                        Repair = c.String(),
                        CarService = c.String(),
                        RepairCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "RepairEvent_Id", "dbo.RepairEvents");
            DropIndex("dbo.CarParts", new[] { "RepairEvent_Id" });
            DropTable("dbo.RepairEvents");
            DropTable("dbo.CarParts");
        }
    }
}
