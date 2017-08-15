using EIDClient.Core.ISS;
using EIDClient.Core.Managers;
using EIDClient.Core.Messages;
using EIDClient.Core.Robot;
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
        public ObservableCollection<Robot.AnalystData> AnalystDataList { get; set; }

        public RelayCommand StartRobot { get; set; }

        public RelayCommand<Robot.AnalystData> BuyCmd { get; set; }

        private Robot.Robot _robot = null;

        private CoreDispatcher dispatcher;

        private IList<Entities.Security> SecurityList = null;

        

        public RobotControlViewModel(INavigationService navigationService, IMainCommandBar commandBar, IChart chart)
        {
            SecurityList = new List<Entities.Security>();

            this._navigationService = navigationService;
            this.DealList = new ObservableCollection<Entities.Deal>();
            this.AnalystDataList = new ObservableCollection<Robot.AnalystData>();

            this._commandBar = commandBar;
            this._chart = chart;

            Messenger.Default.Register<SecurityListLoadedMessage>(this, (msg) =>
            {
                this.SecurityList = msg.SecurityList.Where(s => s.AlgoTrade).ToList();
            });

            this.StartRobot = new RelayCommand(() =>
            {
                this._robot = new Robot.Robot(new Robot.CandleStrategy(), SecurityList);

                this._robot.Run();

            });


            this.BuyCmd = new RelayCommand<Robot.AnalystData>(async d =>
            {
                string msg = string.Format("Купить {0} за {1}?", d.Sec, d.LastPrice);
                var dlg = new Windows.UI.Popups.MessageDialog(msg);

                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Accept"));
                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Cancel"));

                var dialogResult = await dlg.ShowAsync();

                if (dialogResult.Label == "Accept")
                {
                    Messenger.Default.Send<ClientMakeDealMessage>(new ClientMakeDealMessage()
                    {
                        Sec = d.Sec,
                        Operation = "open long"
                    });
                }
            });

            Messenger.Default.Register<ShowAnalystDataMessage>(this, (msg) =>
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (AnalystDataList.Count == 0)
                    {
                        foreach (AnalystData data in msg.AnalystDatalist)
                        {
                            this.AnalystDataList.Add(data);
                        }
                    }

                    if (AnalystDataList.Count > 0)
                    {
                        foreach (AnalystData data in msg.AnalystDatalist)
                        {
                            var advice = this.AnalystDataList.Single(d => d.Sec == data.Sec);

                            advice.Advice = data.Advice;
                            advice.LastPrice = data.LastPrice;
                        }
                    }
                });

            });

            Messenger.Default.Register<ShowDataMessage>(this, (msg) =>
            {


                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //this.DealList.Clear();

                    var candlelist = msg.Сandles["AFLT"]["5"];

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
            Messenger.Default.Send<LoadSecurityListMessage>(new LoadSecurityListMessage());

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
