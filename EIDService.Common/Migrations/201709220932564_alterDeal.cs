namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterDeal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DealSet", "OrderOperation", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DealSet", "OrderOperation");
        }
    }
}
