using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand ClickMenuCmd { get; set; }

        public RelayCommand<object> MenuSelectionChanged { get; set; }

        private IMenu _menu = null;
        private INavigationService _navigationServie = null;

        public IList<IMenuItem> MenuItems { get { return this._menu.GetMenuItems(); } }
        private IDictionary<string, Action> _menuActions = null;

        public MainViewModel(IMenu menu, INavigationService navigationService)
        {
            this._menu = menu;
            this._navigationServie = navigationService;

            _menuActions = new Dictionary<string, Action>();

            _menuActions.Add("emitents", () =>
            {
                this._navigationServie.NavigateTo("EmitentList");
            });

            _menuActions.Add("securities", () =>
            {
                this._navigationServie.NavigateTo("SecurityList");
            });

            _menuActions.Add("robot", () =>
            {
                this._navigationServie.NavigateTo("Robot");
            });

            _menuActions.Add("analysis", () =>
            {
                this._navigationServie.NavigateTo("Analysis");
            });

            this.ClickMenuCmd = new RelayCommand(() =>
            {
                this._menu.Open();
            });

            this.MenuSelectionChanged = new RelayCommand<object>((obj) =>
            {
                var menu_item = (IMenuItem)obj;

                _menuActions[menu_item.Key].Invoke();
            });
        }
    }
}
