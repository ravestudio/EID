namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFCF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinancialSet", "Amortization", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinancialSet", "Capex", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinancialSet", "Capex");
            DropColumn("dbo.FinancialSet", "Amortization");
        }
    }
}
