using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot.Filter
{
    public interface IFilter
    {
        FilterResult Exec();
    }
}
