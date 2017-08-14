namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPositionSet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PositionSet", "Code", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PositionSet", "Code", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
