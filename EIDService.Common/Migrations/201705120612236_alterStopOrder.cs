namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterStopOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StopOrderSet", "Result", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StopOrderSet", "Result", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
    }
}
