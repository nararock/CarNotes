namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCarSystemandCarSubsystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarSubsystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarsystemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarSystems", t => t.CarsystemId, cascadeDelete: true)
                .Index(t => t.CarsystemId);
            
            CreateTable(
                "dbo.CarSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarSubsystems", "CarsystemId", "dbo.CarSystems");
            DropIndex("dbo.CarSubsystems", new[] { "CarsystemId" });
            DropTable("dbo.CarSystems");
            DropTable("dbo.CarSubsystems");
        }
    }
}
