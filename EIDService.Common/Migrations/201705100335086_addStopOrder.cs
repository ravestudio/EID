namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStopOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StopOrderSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseOrder = c.Decimal(precision: 18, scale: 2),
                        OrderNumber = c.Decimal(precision: 18, scale: 2),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Time = c.String(nullable: false, maxLength: 10, unicode: false),
                        Operation = c.String(nullable: false, maxLength: 50, unicode: false),
                        Account = c.String(nullable: false, maxLength: 50, unicode: false),
                        OrderType = c.String(nullable: false, maxLength: 50, unicode: false),
                        Count = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StopPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StopLimitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Client = c.String(nullable: false, maxLength: 50, unicode: false),
                        Class = c.String(nullable: false, maxLength: 50, unicode: false),
                        State = c.String(nullable: false, maxLength: 50, unicode: false),
                        Result = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StopOrderSet");
        }
    }
}
