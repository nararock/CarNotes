namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRefuelEventandGasStation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GasStations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RefuelEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Mileage = c.Double(nullable: false),
                        Fuel = c.Int(nullable: false),
                        Volume = c.Double(nullable: false),
                        PricePerOneLiter = c.Double(nullable: false),
                        FullTank = c.Boolean(nullable: false),
                        ForgotRecordPreviousGasStation = c.Boolean(nullable: false),
                        Station_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GasStations", t => t.Station_ID)
                .Index(t => t.Station_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefuelEvents", "Station_ID", "dbo.GasStations");
            DropIndex("dbo.RefuelEvents", new[] { "Station_ID" });
            DropTable("dbo.RefuelEvents");
            DropTable("dbo.GasStations");
        }
    }
}
