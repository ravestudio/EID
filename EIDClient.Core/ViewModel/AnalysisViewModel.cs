using EID.Library;
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class AnalysisViewModel : ViewModelBase
    {
        public RelayCommand<object> SelectSecurityCmd { get; set; }

        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public RelayCommand StartAnalyst { get; set; }

        private Robot.Analyst _analyst = null;

        private CoreDispatcher dispatcher;

        public ObservableCollection<Robot.AnalystData> AnalystDataList { get; set; }
        private IDictionary<string, IDictionary<int, IList<ICandle>>> _candles { get; set; }

        private IList<Entities.Security> SecurityList = null;

        public AnalysisViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this.AnalystDataList = new ObservableCollection<Robot.AnalystData>();
            this._commandBar = commandBar;

            Messenger.Default.Register<SecurityListLoadedMessage>(this, (msg) =>
            {
                this.SecurityList = msg.SecurityList.Where(s => s.AlgoTrade).ToList();
            });

            this.StartAnalyst = new RelayCommand(() =>
            {
                this._analyst = new Robot.Analyst(new Robot.AnalystStrategy(), SecurityList);

                this._analyst.Run();
            });

            SelectSecurityCmd = new RelayCommand<object>((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                Robot.AnalystData selected = (Robot.AnalystData)e.ClickedItem;

                this._navigationService.NavigateTo("AnalysisDetails", _candles[selected.Sec]);
            });

            Messenger.Default.Register<ShowDataMessage>(this, (msg) =>
            {
                _candles = msg.Сandles;

                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AnalystDataList.Clear();

                    foreach (AnalystData data in msg.AnalystDatalist)
                    {
                        this.AnalystDataList.Add(data);
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
                Command = this.StartAnalyst
            });

        }
    }
}
