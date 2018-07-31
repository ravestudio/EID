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
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Time = c.String(nullable: false, maxLength: 10, unicode: false),
                        Date = c.String(nullable: false, maxLength: 10, unicode: false),
                        DateTime = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        Operation = c.String(nullable: false, maxLength: 50, unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Volume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderOperation = c.Byte(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DealGlobalRAWSet",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(nullable: false, maxLength: 10, unicode: false),
                        Time = c.String(nullable: false, maxLength: 10, unicode: false),
                        Date = c.String(nullable: false, maxLength: 10, unicode: false),
                        Operation = c.String(nullable: false, maxLength: 50, unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        Volume = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[dealGlobal_insert] ON [dbo].[DealGlobalSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update dg set dg.[DateTime] = PARSE(dg.[Date] + ' ' + dg.[Time] AS DATETIME USING 'en-gb'), dg.OrderOperation = oper.id");
            sb.AppendLine("from DealGlobalSet dg");
            sb.AppendLine("join inserted ins on ins.Id = dg.Id");
            sb.AppendLine("join OrderOperation oper on oper.Name = dg.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());

            sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[dealGlobalRAW_insert] ON [dbo].[DealGlobalRAWSet] AFTER INSERT AS BEGIN");
            sb.AppendLine("INSERT DealGlobalSet(Id, Code, [Time], [Date], Operation, Price, [Count], Volume)");
            sb.AppendLine("SELECT Id, Code, [Time], [Date], Operation, Price, [Count], Volume");
            sb.AppendLine("FROM inserted");
            sb.AppendLine("WHERE NOT EXISTS (SELECT Id FROM DealGlobalSet dg WHERE dg.Id = inserted.Id);");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());

        }
        
        public override void Down()
        {
            DropTable("dbo.DealGlobalRAWSet");
            DropTable("dbo.DealGlobalSet");
        }
    }
}
