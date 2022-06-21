namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewfieldOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GasStations", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GasStations", "Order");
        }
    }
}
