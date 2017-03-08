using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public interface IMainCommandBar
    {
        void Clear();
        void AddCommandButton(AppBarButton button);

    }
}
