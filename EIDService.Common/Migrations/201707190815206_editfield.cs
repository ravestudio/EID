namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SecuritySet", "IssueSize", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SecuritySet", "IssueSize", c => c.Int(nullable: false));
        }
    }
}
