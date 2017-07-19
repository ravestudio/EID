using EIDClient.Core.DataModel;
using EIDClient.Core.Entities;
using EIDClient.Core.Messages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public class Analyst
    {
        private IDictionary<int, IList<decimal>> _frames = null;

        private IStrategy _strategy = null;
        private IList<Security> _securitylist = null;

        public Analyst(IStrategy strategy, IList<Security> securitylist)
        {
            _securitylist = securitylist;
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();

        }

        public void Run()
        {
            //IList<string> securities = new List<string>(new string[] { "GMKN", "LKOH", "GAZP", "MOEX", "SBER", "NVTK", "AFLT", "CHMF", "MFON", "ALRS", "MAGN", "MTLR", "MVID" });
            IList<string> securities = _securitylist.Select(s => s.Code).ToList();
            //IList<string> securities = new List<string>(new string[] { "GAZP" });

            Messenger.Default.Send<InitTradeModelMessage>(new InitTradeModelMessage()
            {
                securities = securities,
                frames = _strategy.Need()
            }, TokenModel.Instance.AnalystToken());

            Messenger.Default.Register<GetCandlesResponseMessage>(this, TokenModel.Instance.AnalystToken(), (msg) =>
            {
                IList<AnalystData> analystData = new List<AnalystData>();

                foreach (string sec in msg.Сandles.Keys)
                {
                    Security securirty = _securitylist.Single(s => s.Code == sec);

                    StrategyDecision dec = _strategy.GetDecision(msg.Сandles[sec], sec, null, securirty, msg.DateTime);
                    analystData.Add(new AnalystData()
                    {
                        Sec = sec,
                        Advice = string.IsNullOrEmpty(dec.Decision) ? "Neutral" : dec.Decision
                    });
                };

                Messenger.Default.Send<ShowDataMessage>(new ShowDataMessage()
                {
                    AnalystDatalist = analystData,
                    Сandles = msg.Сandles
                });
            });
        }
    }
}
