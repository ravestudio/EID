using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EID.Library
{
    public class MathFunctions
    {
        public decimal Optimize(decimal value, decimal step)
        {
            decimal mod = value % step;
            decimal ret = value - mod;


            if (value - ret + mod > step)
            {

                ret = ret + step;
            }

            return ret;
        }

        public decimal GetDiff(decimal v1, decimal v2)
        {
            if (v1 == 0m) { v1 = 0.01m; }

            decimal diff = v2 - v1;
            decimal prc = Math.Abs(v1) / 100;
            decimal d = diff / prc;
            return d;
        }
    }
}
