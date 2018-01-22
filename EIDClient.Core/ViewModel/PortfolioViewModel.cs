using EIDClient.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class PortfolioViewModel : ViewModelBase
    {
        private INavigationService _navigationService = null;

        public ObservableCollection<Core.Entities.PortfolioItem> PortfolioItems { get; set; }

        public decimal PortfolioPrice { get; set; }
        public decimal IncomeTotal { get; set; }

        public PortfolioViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.PortfolioItems = new ObservableCollection<Entities.PortfolioItem>();

            Messenger.Default.Register<PortfolioLoadedMessage>(this, (msg) =>
            {
                this.IncomeTotal = msg.IncomeTotal;
                this.PortfolioPrice = msg.PortfolioPrice;

                this.RaisePropertyChanged(() => IncomeTotal);
                this.RaisePropertyChanged(() => PortfolioPrice);

                this.PortfolioItems.Clear();
                foreach (Entities.PortfolioItem item in msg.PortfolioItems)
                {
                    this.PortfolioItems.Add(item);
                }
            });
        }

        public void LoadData()
        {
            Messenger.Default.Send<LoadPortfolioMessage>(new LoadPortfolioMessage());
        }
    }
}
