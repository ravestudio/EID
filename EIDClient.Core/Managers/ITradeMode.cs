using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Managers
{
    public interface ITradeMode
    {
        DateTime GetDate();

        void SetAction(string name, Action action);

        void Start();
    }
}
