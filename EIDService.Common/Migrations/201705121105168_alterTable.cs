namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionSet", "MaxProfit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionSet", "MaxProfit");
        }
    }
}
