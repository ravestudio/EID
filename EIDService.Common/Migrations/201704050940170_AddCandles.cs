namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCandles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CandleSet",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        CandleDate = c.String(nullable: false, maxLength: 30, unicode: false),
                        CandleTime = c.String(nullable: false, maxLength: 30, unicode: false),
                        OpenPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClosePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Volume = c.Decimal(nullable: false, precision: 18, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CandleSet", new[] { "Code" });
            DropTable("dbo.CandleSet");
        }
    }
}
