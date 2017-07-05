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
        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public RelayCommand StartAnalyst { get; set; }

        private Robot.Analyst _analyst = null;

        private CoreDispatcher dispatcher;

        public ObservableCollection<Robot.AnalystData> AnalystDataList { get; set; }

        public AnalysisViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this.AnalystDataList = new ObservableCollection<Robot.AnalystData>();
            this._commandBar = commandBar;

            this.StartAnalyst = new RelayCommand(() =>
            {
                this._analyst = new Robot.Analyst(new Robot.AnalystStrategy());

                this._analyst.Run();
            });

            Messenger.Default.Register<ShowDataMessage>(this, (msg) =>
            {
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
