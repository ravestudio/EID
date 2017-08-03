namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFinancial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinancialSet", "FlowOperatingActivities", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "FlowInvestingActivities", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "FlowFinancingActivities", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "StockIssuance", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "DividendsPaid", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "EarningsPerShare", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.FinancialSet", "OperatingExpenses");
            DropColumn("dbo.FinancialSet", "Expenses");
            DropColumn("dbo.FinancialSet", "Equity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FinancialSet", "Equity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "Expenses", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "OperatingExpenses", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.FinancialSet", "EarningsPerShare");
            DropColumn("dbo.FinancialSet", "DividendsPaid");
            DropColumn("dbo.FinancialSet", "StockIssuance");
            DropColumn("dbo.FinancialSet", "FlowFinancingActivities");
            DropColumn("dbo.FinancialSet", "FlowInvestingActivities");
            DropColumn("dbo.FinancialSet", "FlowOperatingActivities");
        }
    }
}
