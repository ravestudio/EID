using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.DataAccess
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var nikel = new Emitent() { Name = "Горно-металлургическая компания \"Норильский никель\"" };
            var lukoil = new Emitent() { Name = "Нефтяная компания \"ЛУКОЙЛ\"" };

            var gmkn = new Security() { Code = "GMKN", IssueSize = 158245476, Name = "ГМК \"Нор.Никель\" ПАО ао" };
            var lkoh = new Security() { Code = "LKOH", IssueSize = 850563255, Name = "НК ЛУКОЙЛ (ПАО) - ао" };



            gmkn.Emitent = nikel;
            lkoh.Emitent = lukoil;

            context.Securities.Add(gmkn);
            context.Securities.Add(lkoh);

        }
    }
}
