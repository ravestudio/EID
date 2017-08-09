namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFinancial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinancialSet", "ChangesInAssets", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "FlowOperatingTaxesPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinancialSet", "FlowOperatingTaxesPaid");
            DropColumn("dbo.FinancialSet", "ChangesInAssets");
        }
    }
}
