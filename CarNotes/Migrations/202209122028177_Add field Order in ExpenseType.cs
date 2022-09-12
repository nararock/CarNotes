namespace CarNotes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddfieldOrderinExpenseType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseTypes", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExpenseTypes", "Order");
        }
    }
}
