namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterDeals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DealSet", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.DealSet", "Processed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DealSet", "Processed");
            DropColumn("dbo.DealSet", "DateTime");
        }
    }
}
