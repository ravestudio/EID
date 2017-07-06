using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class Crossover
    {
        private IList<decimal> a = null;
        private IList<decimal> b = null;

        public Crossover(IList<decimal> A, IList<decimal> B)
        {
            a = A;
            b = B;
        }

        public bool GetResult()
        {
            bool res = false;

            //res = a.Last() > b.Last() && a[a.Count-3] < b[b.Count - 3];
            res = a.Last() > b.Last();

            return res;
        }
    }
}
