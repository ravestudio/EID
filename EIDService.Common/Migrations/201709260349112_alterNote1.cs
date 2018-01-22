namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterNote1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiaryNote", "OpenValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DiaryNote", "CloseValue", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiaryNote", "CloseValue");
            DropColumn("dbo.DiaryNote", "OpenValue");
        }
    }
}
