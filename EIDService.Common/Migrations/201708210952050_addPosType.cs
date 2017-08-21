namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPosType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionSet", "PosType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PositionSet", "PosType");
        }
    }
}
