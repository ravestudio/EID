namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmitentModification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmitentSet", "Description", c => c.String());
            AddColumn("dbo.EmitentSet", "WebSite", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmitentSet", "WebSite");
            DropColumn("dbo.EmitentSet", "Description");
        }
    }
}
