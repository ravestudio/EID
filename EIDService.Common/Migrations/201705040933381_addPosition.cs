namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPosition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PositionSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Firm = c.String(nullable: false, maxLength: 50, unicode: false),
                        SecurityName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Account = c.String(nullable: false, maxLength: 50, unicode: false),
                        Client = c.String(nullable: false, maxLength: 20, unicode: false),
                        Type = c.String(nullable: false, maxLength: 10, unicode: false),
                        IncomingBalance = c.Int(nullable: false),
                        IncomingLimit = c.Int(nullable: false),
                        CurrentBalance = c.Int(nullable: false),
                        CurrentLimit = c.Int(nullable: false),
                        Blocked = c.Int(nullable: false),
                        BlockedForPurchase = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Available = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PositionSet");
        }
    }
}
