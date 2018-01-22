namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiaryNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiaryNote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        OpenOrder = c.Decimal(precision: 18, scale: 2),
                        CloseOrder = c.Decimal(precision: 18, scale: 2),
                        Open = c.DateTime(),
                        Close = c.DateTime(),
                        OpenPrice = c.Decimal(precision: 18, scale: 2),
                        ClosePrice = c.Decimal(precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DiaryNote");
        }
    }
}
