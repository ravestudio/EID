using EIDClient.Core;
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


        public Robot(IStrategy strategy)
        {
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();

            actions = new Dictionary<string, Action<string, StrategyDecision>>();

            actions.Add("open long", (sec, dec) =>
                {
                    Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage()
                    {
                        Account = "NL0011100043",
                        Code = sec,
                        Count = 10,
                        Price = 0,
                        Operation = "Купля",
                        Class = "QJSIM",
                        Client = "11272",
                        Comment = "11272",
                        Profit = dec.Profit,
                        StopLoss = dec.StopLoss
                    });
                });

            actions.Add("open short", (sec, dec) =>
            {
                Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage()
                {
                    Account = "NL0011100043",
                    Code = sec,
                    Count = 10,
                    Price = 0,
                    Operation = "Продажа",
                    Class = "QJSIM",
                    Client = "11272",
                    Comment = "11272",
                    Profit = dec.Profit,
                    StopLoss = dec.StopLoss
                });
            });

        }

        public void Run()
        {

            IList<string> securities = new List<string>(new string[]{ "GMKN" });

            Messenger.Default.Send<InitTradeModelMessage>(new InitTradeModelMessage()
            {
                securities = securities,
                frames = _strategy.Need()
            });

            Messenger.Default.Register<GetCandlesResponseMessage>(this, (msg) =>
            {
                foreach(string sec in msg.Сandles.Keys)
                {
                    StrategyDecision dec = _strategy.GetDecision(msg.Сandles[sec], sec, msg.Positions[sec], msg.DateTime);

                    if (!string.IsNullOrEmpty(dec.Decision))
                    {
                        actions[dec.Decision].Invoke(sec, dec);
                    }
                }
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
