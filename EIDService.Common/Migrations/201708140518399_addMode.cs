namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModeSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firm = c.String(nullable: false),
                        Account = c.String(nullable: false),
                        Class = c.String(nullable: false),
                        Client = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ModeSet");
        }
    }
}
