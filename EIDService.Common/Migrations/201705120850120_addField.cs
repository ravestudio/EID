namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderSet", "Client", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderSet", "Client");
        }
    }
}
