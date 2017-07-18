namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransactionResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionResultSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Record = c.String(nullable: false),
                        Processed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionResultSet");
        }
    }
}
