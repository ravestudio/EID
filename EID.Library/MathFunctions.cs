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
    }
}
