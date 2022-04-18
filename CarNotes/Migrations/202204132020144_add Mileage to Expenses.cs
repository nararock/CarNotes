namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMileagetoExpenses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Mileage", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "Mileage");
        }
    }
}
