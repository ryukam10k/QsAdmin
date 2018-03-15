namespace QsAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWebLink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.String(maxLength: 128),
                        Url = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AccountId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebLinks", "AccountId", "dbo.AspNetUsers");
            DropIndex("dbo.WebLinks", new[] { "AccountId" });
            DropTable("dbo.WebLinks");
        }
    }
}
