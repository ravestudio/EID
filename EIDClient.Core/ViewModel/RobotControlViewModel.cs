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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class RobotControlViewModel : ViewModelBase
    {
        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public ObservableCollection<Core.Entities.Deal> DealList { get; set; }

        public RelayCommand StartRobot { get; set; }

        private Robot.Robot _robot = null;

        private CoreDispatcher dispatcher;

        public RobotControlViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this.DealList = new ObservableCollection<Entities.Deal>();
            this._commandBar = commandBar;

            this.StartRobot = new RelayCommand(() =>
            {
                this._robot = new Robot.Robot(new Robot.Strategy());

                this._robot.Run();

            });

            Messenger.Default.Register<ShowDataMessage>(this, (msg) =>
            {


                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //this.DealList.Clear();

                    var newdeals = msg.Deals.Where(d => !this.DealList.Select(old => old.Id).Contains(d.Id));

                    foreach (Entities.Deal deal in newdeals)
                    {
                        this.DealList.Add(deal);
                    }
                });

            });

        }

        public void LoadData()
        {
            dispatcher = Window.Current.Content.Dispatcher;

            _commandBar.Clear();
            _commandBar.AddCommandButton(new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Play),
                Command = this.StartRobot
            });

        }
    }
}
