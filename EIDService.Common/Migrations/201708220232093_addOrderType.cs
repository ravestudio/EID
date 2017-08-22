namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderState",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderSet", "OrderState", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderSet", "OrderState");
            DropTable("dbo.OrderState");
        }
    }
}
