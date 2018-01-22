namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPortfolio1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PortfolioItemSet", "Account", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PortfolioItemSet", "Account", c => c.String());
        }
    }
}
