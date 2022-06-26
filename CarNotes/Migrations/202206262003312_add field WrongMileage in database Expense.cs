namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldWrongMileageindatabaseExpense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "WrongMileage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "WrongMileage");
        }
    }
}
