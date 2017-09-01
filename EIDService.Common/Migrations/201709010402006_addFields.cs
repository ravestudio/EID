namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "OFFSET", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Settings", "SPREAD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TransactionSet", "CODE", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionSet", "CODE");
            DropColumn("dbo.Settings", "SPREAD");
            DropColumn("dbo.Settings", "OFFSET");
        }
    }
}
