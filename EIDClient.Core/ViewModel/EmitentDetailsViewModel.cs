using EIDClient.Core.Entities;
using EIDClient.Core.Managers;
using EIDClient.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace EIDClient.Core.ViewModel
{
    public class EmitentDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Core.Entities.Financial> FinancialList { get; set; }

        public RelayCommand AddFinancialCmd { get; set; }
        public RelayCommand<object> EditFinancialCmd { get; set; }

        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public EmitentDetailsViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this._commandBar = commandBar;
            this.FinancialList = new ObservableCollection<Entities.Financial>();

            Messenger.Default.Register<FinancialListLoadedMessage>(this, (msg) =>
            {
                this.FinancialList.Clear();
                foreach (Entities.Financial financial in msg.FinancialList) 
                {
                    this.FinancialList.Add(financial);
                }
            });

            this.AddFinancialCmd = new RelayCommand(() =>
            {
                this._navigationService.NavigateTo("FinancialEdit");
            });

            this.EditFinancialCmd = new RelayCommand<object>((param) =>
            {
                Financial financial = (Financial)param;

                this._navigationService.NavigateTo("FinancialEdit", financial);

            });
        }

        public void LoadData(int EmitentId)
        {
            Messenger.Default.Send<LoadFinancialListMessage>(new LoadFinancialListMessage() { EmitentId = EmitentId });

            _commandBar.Clear();
            _commandBar.AddCommandButton(new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Add),
                Command = this.AddFinancialCmd
            });

            Binding bind = new Binding();
            bind.Source = ChildFinder.FindChild<ListView>(Window.Current.Content, "financial_list");
            bind.Path = new Windows.UI.Xaml.PropertyPath("SelectedItem");

            AppBarButton btn = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Edit),
                Command = this.EditFinancialCmd
            };

            btn.SetBinding(AppBarButton.CommandParameterProperty, bind);
            _commandBar.AddCommandButton(btn);

        }
    }
}
