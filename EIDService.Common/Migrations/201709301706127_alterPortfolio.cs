namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPortfolio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortfolioItemSet", "Account", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PortfolioItemSet", "Account");
        }
    }
}
