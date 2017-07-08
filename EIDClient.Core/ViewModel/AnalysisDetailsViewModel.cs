using EID.Library;
using EIDClient.Core.ISS;
using EIDClient.Core.Managers;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace EIDClient.Core.ViewModel
{
    public class AnalysisDetailsViewModel
    {
        private INavigationService _navigationService = null;
        private IChart _chart = null;

        private IDictionary<int, IList<ICandle>> _candles = null;
        private CoreDispatcher dispatcher;

        public AnalysisDetailsViewModel(INavigationService navigationService, IChart chart)
        {
            this._navigationService = navigationService;
            this._chart = chart;


        }

        public void LoadData(IDictionary<int, IList<ICandle>> candles)
        {
            dispatcher = Window.Current.Content.Dispatcher;

            _candles = candles;

            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //this.DealList.Clear();

                var candlelist = _candles[60];

                this._chart.ShowCandles(candlelist);

                //SimpleMovingAverage smaLong = new SimpleMovingAverage(candlelist, 20);
                //this._chart.ShowMA(smaLong, Colors.LemonChiffon);

                SimpleMovingAverage sma = new SimpleMovingAverage(candlelist, 9);
                this._chart.ShowMA(sma, Colors.Green);

                SimpleMovingAverage long_sma = new SimpleMovingAverage(candlelist, 20);
                this._chart.ShowMA(long_sma, Colors.Yellow);

            });
        }
    }
}
