namespace EIDService.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPosTrigger : DbMigration
    {
        public override void Up()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("CREATE TRIGGER [dbo].[positions_insert] ON  [dbo].[PositionSet] AFTER INSERT AS BEGIN");
            sb.AppendLine("update ps set ps.PosType = pt.id");
            sb.AppendLine("from PositionSet ps");
            sb.AppendLine("join inserted ins on ins.Id = ps.Id");
            sb.AppendLine("join PosType pt on pt.Name = ps.Type");
            sb.AppendLine("END");
            sb.AppendLine("GO");

            Sql(sb.ToString());
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[positions_insert]");
        }
    }
}
