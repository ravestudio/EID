using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public class Strategy
    {
        
        public Strategy()
        {

        }

        public IList<int> Need()
        {
            IList<int> res = new List<int>();

            res.Add(5);
            res.Add(60);

            return res;
        }

        public string GetDecision(IDictionary<int, IList<decimal>> data, string name, string currentPos)
        {
            string dec = null;
            decimal h = data[60].Last();
            decimal m = data[5].Last();

            if ((m > h) && currentPos == "free")
            {
                dec = "open long";
            }

            return dec;
        }
    }
}
