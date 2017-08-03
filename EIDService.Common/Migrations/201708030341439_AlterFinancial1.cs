namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFinancial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FinancialSet", "Revenue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "OperatingIncome", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "NetIncome", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "CurrentAssets", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FixedAssets", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "CurrentLiabilities", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "LongTermLiabilities", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowOperatingActivities", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowInvestingActivities", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowFinancingActivities", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "StockIssuance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "DividendsPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "EarningsPerShare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FinancialSet", "EarningsPerShare", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "DividendsPaid", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "StockIssuance", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowFinancingActivities", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowInvestingActivities", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FlowOperatingActivities", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "LongTermLiabilities", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "CurrentLiabilities", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "FixedAssets", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "CurrentAssets", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "NetIncome", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "OperatingIncome", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.FinancialSet", "Revenue", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
