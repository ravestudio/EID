namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotePosition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DealSet", "DiaryNote_Id", "dbo.DiaryNote");
            DropIndex("dbo.DealSet", new[] { "DiaryNote_Id" });
            CreateTable(
                "dbo.NotePositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 50),
                        Count = c.Int(nullable: false),
                        Deal_Id = c.Int(),
                        DiaryNote_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DealSet", t => t.Deal_Id)
                .ForeignKey("dbo.DiaryNote", t => t.DiaryNote_Id)
                .Index(t => t.Deal_Id)
                .Index(t => t.DiaryNote_Id);
            
            DropColumn("dbo.DealSet", "DiaryNote_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DealSet", "DiaryNote_Id", c => c.Int());
            DropForeignKey("dbo.NotePositions", "DiaryNote_Id", "dbo.DiaryNote");
            DropForeignKey("dbo.NotePositions", "Deal_Id", "dbo.DealSet");
            DropIndex("dbo.NotePositions", new[] { "DiaryNote_Id" });
            DropIndex("dbo.NotePositions", new[] { "Deal_Id" });
            DropTable("dbo.NotePositions");
            CreateIndex("dbo.DealSet", "DiaryNote_Id");
            AddForeignKey("dbo.DealSet", "DiaryNote_Id", "dbo.DiaryNote", "Id");
        }
    }
}
