namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DealSet", "DiaryNote_Id", c => c.Int());
            AddColumn("dbo.DiaryNote", "NoteType", c => c.Int(nullable: false));
            CreateIndex("dbo.DealSet", "DiaryNote_Id");
            AddForeignKey("dbo.DealSet", "DiaryNote_Id", "dbo.DiaryNote", "Id");
            DropColumn("dbo.DiaryNote", "OpenOrder");
            DropColumn("dbo.DiaryNote", "CloseOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DiaryNote", "CloseOrder", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DiaryNote", "OpenOrder", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.DealSet", "DiaryNote_Id", "dbo.DiaryNote");
            DropIndex("dbo.DealSet", new[] { "DiaryNote_Id" });
            DropColumn("dbo.DiaryNote", "NoteType");
            DropColumn("dbo.DealSet", "DiaryNote_Id");
        }
    }
}
