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

namespace Robot
{
    public class Robot
    {
        private IDictionary<int, IList<decimal>> _frames = null;

        private Strategy _strategy = null;
        private System.Threading.CancellationTokenSource _stop = null;

        private IDictionary<string, Action<string>> actions = null;


        public Robot(Strategy strategy)
        {
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();

            actions = new Dictionary<string, Action<string>>();

            actions.Add("open long", (sec) =>
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
                        Comment = "11272"
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
                    string d = _strategy.GetDecision(msg.Сandles[sec], sec, msg.Positions[sec]);

                    if (!string.IsNullOrEmpty(d))
                    {
                        actions[d].Invoke(sec);
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
