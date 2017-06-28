namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ATRtoDeals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionSet", "Profit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransactionSet", "StopLoss", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionSet", "StopLoss");
            DropColumn("dbo.TransactionSet", "Profit");
        }
    }
}
