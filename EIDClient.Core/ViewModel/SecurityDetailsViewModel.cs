using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EIDClient.Core.Managers;
using EIDClient.Core.Messages;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace EIDClient.Core.ViewModel
{
    public class SecurityDetailsViewModel
    {
        private INavigationService _navigationService = null;
        private Security _security = null;

        public SecurityDetailsViewModel(INavigationService navigationService, IChart chart)
        {
            this._navigationService = navigationService;

            Messenger.Default.Register<SecurityLoadedMessage>(this, (msg) =>
            {
                this._security = msg.Security;

                MicexISSClient client = new MicexISSClient(new WebApiClient());
                IList<Candle> candlelist = client.GetCandles(_security.Code, new DateTime(2017, 3, 10), 60).Result;

                chart.ShowCandles(candlelist);

                SimpleMovingAverage sma = new SimpleMovingAverage(candlelist, 26);
                chart.ShowMA(sma, Colors.Yellow);

                SimpleMovingAverage sma2 = new SimpleMovingAverage(candlelist, 12);
                chart.ShowMA(sma2, Colors.LemonChiffon);

                //ExponentialMovingAverage ema = new ExponentialMovingAverage(candlelist, 9);
                //chart.ShowMA(ema, Colors.Blue);

                MACD macd = new MACD(candlelist, 12, 26, 9);

                chart.ShowMACD(macd, Colors.Blue);
            });
        }

        public void LoadData(int SecurityId)
        {
            Messenger.Default.Send<LoadSecurityMessage>(new LoadSecurityMessage() { Id = SecurityId });

        }
    }
}
