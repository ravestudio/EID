namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropBaseOrder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StopOrderSet", "BaseOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StopOrderSet", "BaseOrder", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
