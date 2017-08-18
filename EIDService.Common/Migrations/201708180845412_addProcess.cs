namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProcess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EIDProcessSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Data = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EIDProcessSet");
        }
    }
}
