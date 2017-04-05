namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmitentSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Key = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FinancialSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        Revenue = c.Decimal(precision: 18, scale: 2),
                        OperatingExpenses = c.Decimal(precision: 18, scale: 2),
                        Expenses = c.Decimal(precision: 18, scale: 2),
                        OperatingIncome = c.Decimal(precision: 18, scale: 2),
                        NetIncome = c.Decimal(precision: 18, scale: 2),
                        CurrentAssets = c.Decimal(precision: 18, scale: 2),
                        FixedAssets = c.Decimal(precision: 18, scale: 2),
                        Equity = c.Decimal(precision: 18, scale: 2),
                        CurrentLiabilities = c.Decimal(precision: 18, scale: 2),
                        LongTermLiabilities = c.Decimal(precision: 18, scale: 2),
                        Key = c.String(maxLength: 50),
                        EmitentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmitentSet", t => t.EmitentId, cascadeDelete: true)
                .Index(t => t.EmitentId);
            
            CreateTable(
                "dbo.SecuritySet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        IssueSize = c.Int(nullable: false),
                        Key = c.String(maxLength: 50),
                        EmitentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmitentSet", t => t.EmitentId, cascadeDelete: true)
                .Index(t => t.EmitentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecuritySet", "EmitentId", "dbo.EmitentSet");
            DropForeignKey("dbo.FinancialSet", "EmitentId", "dbo.EmitentSet");
            DropIndex("dbo.SecuritySet", new[] { "EmitentId" });
            DropIndex("dbo.FinancialSet", new[] { "EmitentId" });
            DropTable("dbo.SecuritySet");
            DropTable("dbo.FinancialSet");
            DropTable("dbo.EmitentSet");
        }
    }
}
