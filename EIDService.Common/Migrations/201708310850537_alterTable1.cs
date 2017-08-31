namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderSet", "Operation", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.OrderSet", "State", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.StopOrderSet", "Operation", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.StopOrderSet", "State", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StopOrderSet", "State", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.StopOrderSet", "Operation", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OrderSet", "State", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OrderSet", "Operation", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
