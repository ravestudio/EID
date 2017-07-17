using EIDClient.Core.ISS;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class RobotControlViewModel : ViewModelBase
    {
        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;
        private IChart _chart = null;

        public ObservableCollection<Core.Entities.Deal> DealList { get; set; }

        public RelayCommand StartRobot { get; set; }

        private Robot.Robot _robot = null;

        private CoreDispatcher dispatcher;

        public RobotControlViewModel(INavigationService navigationService, IMainCommandBar commandBar, IChart chart)
        {
            this._navigationService = navigationService;
            this.DealList = new ObservableCollection<Entities.Deal>();
            this._commandBar = commandBar;
            this._chart = chart;

            this.StartRobot = new RelayCommand(() =>
            {
                this._robot = new Robot.Robot(new Robot.DemoStrategy());

                this._robot.Run();

            });

            Messenger.Default.Register<ShowDataMessage>(this, (msg) =>
            {


                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //this.DealList.Clear();

                    var candlelist = msg.Сandles["GMKN"][5];

                    this._chart.ShowCandles(candlelist);

                    //SimpleMovingAverage smaLong = new SimpleMovingAverage(candlelist, 20);
                    //this._chart.ShowMA(smaLong, Colors.LemonChiffon);

                    SimpleMovingAverage sma = new SimpleMovingAverage(candlelist, 9);
                    this._chart.ShowMA(sma, Colors.Yellow);

                    MACD macd = new MACD(candlelist, 12, 26, 9);
                    this._chart.ShowMACD(macd, Colors.Blue);

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
