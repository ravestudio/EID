namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMoneylimit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoneyLimitSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firm = c.String(nullable: false, maxLength: 50, unicode: false),
                        Currency = c.String(nullable: false, maxLength: 10, unicode: false),
                        MoneyGroup = c.String(nullable: false, maxLength: 10, unicode: false),
                        Client = c.String(nullable: false, maxLength: 20, unicode: false),
                        Type = c.String(nullable: false, maxLength: 10, unicode: false),
                        Available = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MoneyLimitSet");
        }
    }
}
