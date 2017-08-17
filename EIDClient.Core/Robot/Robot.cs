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

        private IList<StrategyDecision> _decisionlist = null;

        public Robot(IStrategy strategy, IList<Security> securitylist)
        {
            _securitylist = securitylist;
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();

            actions = new Dictionary<string, Action<string, StrategyDecision>>();

            _decisionlist = new List<StrategyDecision>();

            actions.Add("open long", (sec, dec) =>
                {
                    Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage()
                    {
                        Code = sec,
                        Count = dec.Count,
                        Price = dec.LongPrice,
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
                    Count = dec.Count,
                    Price = dec.ShortPrice,
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

                _decisionlist.Clear();

                foreach (string sec in msg.Сandles.Keys)
                {
                    Security securirty = _securitylist.Single(s => s.Code == sec);

                    StrategyDecision dec = _strategy.GetDecision(msg.Сandles[sec], sec, msg.Positions[sec], securirty, msg.DateTime);

                    dec.Count = securirty.DealSize;

                    _decisionlist.Add(dec);

                    analystData.Add(new AnalystData()
                    {
                        Sec = sec,
                        LastPrice = dec.LastPrice,
                        Advice = string.IsNullOrEmpty(dec.Decision) ? "Neutral" : dec.Decision
                    });

                    if (!string.IsNullOrEmpty(dec.Decision))
                    {
                        actions[dec.Decision].Invoke(sec, dec);
                    }
                }

                Messenger.Default.Send<ShowAnalystDataMessage>(new ShowAnalystDataMessage()
                {
                    AnalystDatalist = analystData
                });
            });

            Messenger.Default.Register<ClientMakeDealMessage>(this, (msg) =>
            {
                var dec = _decisionlist.SingleOrDefault(d => d.Code == msg.Sec);

                actions[msg.Operation].Invoke(msg.Sec, dec);
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
