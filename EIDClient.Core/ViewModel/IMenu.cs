using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public interface IMenu
    {
        void Open();
        IList<IMenuItem> GetMenuItems();
    }

    public interface IMenuItem
    {
        string Key { get; set; }
        string Text { get; set; }
    }
}
