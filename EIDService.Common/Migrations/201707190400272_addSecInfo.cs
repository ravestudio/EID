namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSecInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecuritySet", "MinStep", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SecuritySet", "LotSize", c => c.Int(nullable: false));
            AddColumn("dbo.SecuritySet", "AlgoTrade", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecuritySet", "AlgoTrade");
            DropColumn("dbo.SecuritySet", "LotSize");
            DropColumn("dbo.SecuritySet", "MinStep");
        }
    }
}
