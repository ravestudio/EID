namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderOperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderOperation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderSet", "OrderOperation", c => c.Byte(nullable: false));
            AddColumn("dbo.StopOrderSet", "OrderState", c => c.Byte(nullable: false));
            AddColumn("dbo.StopOrderSet", "OrderOperation", c => c.Byte(nullable: false));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[order_insert] ON [dbo].[OrderSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update o set o.OrderState = s.id, o.OrderOperation = oper.id");
            sb.AppendLine("from OrderSet o");
            sb.AppendLine("join inserted ins on ins.Id = o.Id");
            sb.AppendLine("join OrderState s on s.Name = o.State");
            sb.AppendLine("join OrderOperation oper on oper.Name = o.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());

            sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[stopOrder_insert] ON [dbo].[StopOrderSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update o set o.OrderState = s.id, o.OrderOperation = oper.id");
            sb.AppendLine("from StopOrderSet o");
            sb.AppendLine("join inserted ins on ins.Id = o.Id");
            sb.AppendLine("join OrderState s on s.Name = o.State");
            sb.AppendLine("join OrderOperation oper on oper.Name = o.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StopOrderSet", "OrderOperation");
            DropColumn("dbo.StopOrderSet", "OrderState");
            DropColumn("dbo.OrderSet", "OrderOperation");
            DropTable("dbo.OrderOperation");

            Sql("DROP TRIGGER [dbo].[order_insert]");
            Sql("DROP TRIGGER [dbo].[stopOrder_insert]");
        }
    }
}
