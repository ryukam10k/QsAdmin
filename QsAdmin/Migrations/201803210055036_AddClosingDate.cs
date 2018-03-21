namespace QsAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClosingDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "ClosingDate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "ClosingDate");
        }
    }
}
