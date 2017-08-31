namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTriggers : DbMigration
    {
        public override void Up()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ALTER TRIGGER [dbo].[order_insert] ON [dbo].[OrderSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update o set o.OrderState = ISNULL(s.id, o.OrderState), o.OrderOperation = ISNULL(oper.id, o.OrderOperation)");
            sb.AppendLine("from OrderSet o");
            sb.AppendLine("join inserted ins on ins.Id = o.Id");
            sb.AppendLine("left join OrderState s on s.Name = o.State");
            sb.AppendLine("left join OrderOperation oper on oper.Name = o.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());

            sb = new System.Text.StringBuilder();
            sb.AppendLine("ALTER TRIGGER [dbo].[stopOrder_insert] ON [dbo].[StopOrderSet] AFTER INSERT, UPDATE AS BEGIN");
            sb.AppendLine("update o set o.OrderState = ISNULL(s.id, o.OrderState), o.OrderOperation = ISNULL(oper.id, o.OrderOperation)");
            sb.AppendLine("from StopOrderSet o");
            sb.AppendLine("join inserted ins on ins.Id = o.Id");
            sb.AppendLine("left join OrderState s on s.Name = o.State");
            sb.AppendLine("left join OrderOperation oper on oper.Name = o.Operation");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());
        }
        
        public override void Down()
        {
        }
    }
}
