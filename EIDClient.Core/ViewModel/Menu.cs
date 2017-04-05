using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using EIDClient.Core.Managers;
using Windows.UI.Xaml;

namespace EIDClient.Core.ViewModel
{
    public class Menu : IMenu
    {
        private SplitView _splitmenu;

        private IList<IMenuItem> _itemList = null;

        public Menu()
        {
            this._itemList = new List<IMenuItem>();

            _itemList.Add(new MenuItem() { Key = "home", Text = "Home" });
            _itemList.Add(new MenuItem() { Key = "emitents", Text = "Emitents" });
            _itemList.Add(new MenuItem() { Key = "securities", Text = "Securities" });
            _itemList.Add(new MenuItem() { Key = "robot", Text = "Robot" });
        }

        public IList<IMenuItem> GetMenuItems()
        {
            return this._itemList;
        }

        public void Open()
        {
            if (this._splitmenu == null)
            {
                this._splitmenu = ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");
            }

            this._splitmenu.IsPaneOpen = !this._splitmenu.IsPaneOpen;
        }
    }

    public class MenuItem : EIDClient.Core.ViewModel.IMenuItem
    {
        public string Key { get; set; }

        public string Text { get; set; }
    }
}
