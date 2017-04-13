namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderSet",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Time = c.String(nullable: false, maxLength: 10, unicode: false),
                        Operation = c.String(nullable: false, maxLength: 50, unicode: false),
                        Account = c.String(nullable: false, maxLength: 50, unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Volume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.String(nullable: false, maxLength: 50, unicode: false),
                        Class = c.String(nullable: false, maxLength: 50, unicode: false),
                        Comment = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderSet");
        }
    }
}
