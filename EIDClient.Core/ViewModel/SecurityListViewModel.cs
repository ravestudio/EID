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
    public class SecurityListViewModel : ViewModelBase
    {
        public ObservableCollection<Core.Entities.Security> SecurityList { get; set; }
        private INavigationService _navigationService = null;

        public SecurityListViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.SecurityList = new ObservableCollection<Entities.Security>();

            Messenger.Default.Register<SecurityListLoadedMessage>(this, (msg) =>
            {
                this.SecurityList.Clear();
                foreach (Entities.Security security in msg.SecurityList)
                {
                    this.SecurityList.Add(security);
                }
            });


        }

        public void LoadSecurities()
        {
            Messenger.Default.Send<LoadSecurityListMessage>(new LoadSecurityListMessage());

        }

    }
}
