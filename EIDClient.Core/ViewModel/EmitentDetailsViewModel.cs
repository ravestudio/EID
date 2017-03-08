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
        public RelayCommand EditFinancialCmd { get; set; }
        public RelayCommand<object> SelectFinancialCmd { get; set; }

        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        private Financial _selectedFinancial = null;

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

            this.SelectFinancialCmd = new RelayCommand<object>((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                this._selectedFinancial = (Financial)e.ClickedItem;
            });

            this.AddFinancialCmd = new RelayCommand(() =>
            {
                this._navigationService.NavigateTo("FinancialEdit");
            });

            this.EditFinancialCmd = new RelayCommand(() =>
            {
                this._navigationService.NavigateTo("FinancialEdit", this._selectedFinancial);
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

            _commandBar.AddCommandButton(new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Edit),
                Command = this.EditFinancialCmd
            });

        }
    }
}
