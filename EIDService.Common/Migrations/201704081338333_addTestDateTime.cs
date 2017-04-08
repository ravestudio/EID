namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTestDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "TestDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "TestDateTime");
        }
    }
}
