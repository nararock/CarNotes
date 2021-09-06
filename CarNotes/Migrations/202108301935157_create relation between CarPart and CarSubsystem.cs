namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createrelationbetweenCarPartandCarSubsystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarParts", "CarSubsystemId", c => c.Int(nullable: false));
            CreateIndex("dbo.CarParts", "CarSubsystemId");
            AddForeignKey("dbo.CarParts", "CarSubsystemId", "dbo.CarSubsystems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "CarSubsystemId", "dbo.CarSubsystems");
            DropIndex("dbo.CarParts", new[] { "CarSubsystemId" });
            DropColumn("dbo.CarParts", "CarSubsystemId");
        }
    }
}
