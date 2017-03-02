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
            var megafon = new Emitent() { Name = "Публичное акционерное общество \"МегаФон\"" };

            var gmkn = new Security() { Code = "GMKN", IssueSize = 158245476, Name = "ГМК \"Нор.Никель\" ПАО ао" };
            var lkoh = new Security() { Code = "LKOH", IssueSize = 850563255, Name = "НК ЛУКОЙЛ (ПАО) - ао" };
            var mfon = new Security() { Code = "MFON", IssueSize = 620000000, Name = "МегаФон ПАО ао" };

            var megafon_Q3 = new Financial()
            {
                Year = 2016,
                Period = (int)PeriodType.Q3,
                Revenue = 81115,
                OperatingExpenses = 64429,
                Expenses = 24745,
                OperatingIncome = 16686,
                NetIncome = 6545,
                CurrentAssets = 88796,
                FixedAssets = 376612,
                Equity = 135894,
                CurrentLiabilities = 69198,
                LongTermLiabilities = 260316,
                Emitent = megafon
            };



            gmkn.Emitent = nikel;
            lkoh.Emitent = lukoil;
            mfon.Emitent = megafon;

            context.Securities.Add(gmkn);
            context.Securities.Add(lkoh);
            context.Securities.Add(mfon);

            context.Financials.Add(megafon_Q3);



        }
    }
}
