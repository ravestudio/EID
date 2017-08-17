namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDealSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecuritySet", "DealSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecuritySet", "DealSize");
        }
    }
}
