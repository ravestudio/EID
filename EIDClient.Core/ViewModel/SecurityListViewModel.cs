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
using Windows.UI.Xaml;

namespace EIDClient.Core.ViewModel
{
    public class SecurityListViewModel : ViewModelBase
    {
        public ObservableCollection<Core.Entities.Security> SecurityList { get; set; }
        private INavigationService _navigationService = null;

        private DispatcherTimer timer = null;

        public SecurityListViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.SecurityList = new ObservableCollection<Entities.Security>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;

            Messenger.Default.Register<SecurityListLoadedMessage>(this, (msg) =>
            {
                timer.Stop();

                this.SecurityList.Clear();
                foreach (Entities.Security security in msg.SecurityList)
                {
                    this.SecurityList.Add(security);
                }

                timer.Start();
            });


        }

        private void Timer_Tick(object sender, object e)
        {
            Messenger.Default.Send<IISGetSecurityInfo>(new IISGetSecurityInfo() {
                SecurityList = new List<Entities.Security>(this.SecurityList)
            });
        }

        public void LoadSecurities()
        {
            Messenger.Default.Send<LoadSecurityListMessage>(new LoadSecurityListMessage());

        }

    }
}
