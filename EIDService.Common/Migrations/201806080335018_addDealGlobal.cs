namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDealGlobal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DealGlobalSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Time = c.String(nullable: false, maxLength: 10, unicode: false),
                        Date = c.String(nullable: false, maxLength: 10, unicode: false),
                        Operation = c.String(nullable: false, maxLength: 50, unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Volume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderOperation = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[dealGlobal_insert] ON [dbo].[DealGlobalSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update dg set dg.OrderOperation = oper.id");
            sb.AppendLine("from DealGlobalSet dg");
            sb.AppendLine("join inserted ins on ins.Id = dg.Id");
            sb.AppendLine("join OrderOperation oper on oper.Name = dg.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());

        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[dealGlobal_insert]");

            DropTable("dbo.DealGlobalSet");
        }
    }
}
