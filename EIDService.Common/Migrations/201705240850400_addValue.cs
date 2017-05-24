namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CandleSet", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CandleSet", "Value");
        }
    }
}
