namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionSet", "CODE", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionSet", "CODE", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
