using EIDClient.Core;
using EIDClient.Core.DataModel;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EIDClient.Core.Messages;
using EIDClient.Core.Repository;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public class Robot
    {
        private IDictionary<int, IList<decimal>> _frames = null;

        private IStrategy _strategy = null;
        private System.Threading.CancellationTokenSource _stop = null;

        private IDictionary<string, Action<string, StrategyDecision>> actions = null;
        private IList<Security> _securitylist = null;

        public Robot(IStrategy strategy, IList<Security> securitylist)
        {
            _securitylist = securitylist;
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();

            actions = new Dictionary<string, Action<string, StrategyDecision>>();

            actions.Add("open long", (sec, dec) =>
                {
                    Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage()
                    {
                        Code = sec,
                        Count = 10,
                        Price = dec.Price,
                        Operation = "Купля",
                        Profit = dec.Profit,
                        StopLoss = dec.StopLoss
                    });
                });

            actions.Add("open short", (sec, dec) =>
            {
                Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage()
                {
                    Code = sec,
                    Count = 10,
                    Price = dec.Price,
                    Operation = "Продажа",
                    Profit = dec.Profit,
                    StopLoss = dec.StopLoss
                });
            });

        }

        public void Run()
        {

            IList<string> securities = _securitylist.Select(s => s.Code).ToList();

            Messenger.Default.Send<InitTradeModelMessage>(new InitTradeModelMessage()
            {
                securities = securities,
                frames = _strategy.Need()
            }, TokenModel.Instance.RobotToken());

            Messenger.Default.Register<GetCandlesResponseMessage>(this, TokenModel.Instance.RobotToken(), (msg) =>
            {

                IList<AnalystData> analystData = new List<AnalystData>();

                foreach (string sec in msg.Сandles.Keys)
                {
                    Security securirty = _securitylist.Single(s => s.Code == sec);

                    StrategyDecision dec = _strategy.GetDecision(msg.Сandles[sec], sec, msg.Positions[sec], securirty, msg.DateTime);

                    analystData.Add(new AnalystData()
                    {
                        Sec = sec,
                        Advice = string.IsNullOrEmpty(dec.Decision) ? "Neutral" : dec.Decision
                    });

                    //if (!string.IsNullOrEmpty(dec.Decision))
                    //{
                    //    actions[dec.Decision].Invoke(sec, dec);
                    //}
                }

                Messenger.Default.Send<ShowAnalystDataMessage>(new ShowAnalystDataMessage()
                {
                    AnalystDatalist = analystData
                });
            });


            _stop = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = _stop.Token;

            Task t = Task.Run(() =>
            {

                while (true)
                {
                    Task.Delay(5000).Wait();

                    //Messenger.Default.Send<GetCandlesMessage>(new GetCandlesMessage());

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }, token);

        }

        public void Stop()
        {
            _stop.Cancel();
        }


    }
}
