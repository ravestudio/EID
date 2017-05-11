namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        OrderNumber = c.Decimal(precision: 18, scale: 2),
                        Processed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionSet");
        }
    }
}
