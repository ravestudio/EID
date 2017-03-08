using EIDClient.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class MainCommandBar : IMainCommandBar
    {
        private CommandBar _commandBar = null;

        public void AddCommandButton(AppBarButton button)
        {
            GetBar().PrimaryCommands.Add(button);
        }

        public void Clear()
        {
            GetBar().PrimaryCommands.Clear();
        }

        private CommandBar GetBar()
        {
            return this._commandBar??
                ChildFinder.FindChild<CommandBar>(Window.Current.Content, "mainCommandBar");
        }
    }
}
