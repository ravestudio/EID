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

namespace EIDClient.Core.DataModel
{
    public class TradeModel
    {
        private TradeSessionRepository _tradeSessionRepository = null;
        private MicexISSClient _client = null;

        private IList<TradeSession> _sessions = null;
        private IDictionary<string, IDictionary<int, IList<Candle>>> _candles = null;

        public TradeModel(TradeSessionRepository TradeSessionRepository, MicexISSClient client)
        {
            _tradeSessionRepository = TradeSessionRepository;
            _client = client;

            _candles = new Dictionary<string, IDictionary<int, IList<Candle>>>();

            Messenger.Default.Register<InitTradeModelMessage>(this, async (msg) =>
            {
                var temp_sessions = await _tradeSessionRepository.GetAll();
                _sessions = temp_sessions.ToList();

                Do(msg.securities, msg.frames);
            });

            Messenger.Default.Register<GetCandlesMessage>(this, (msg) =>
            {
                Messenger.Default.Send<GetCandlesResponseMessage>(new GetCandlesResponseMessage()
                {
                    Сandles = _candles
                });
            });
        }

        private void Do(IList<string> securities, IList<int> frames)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    foreach (string sec in securities)
                    {
                        _candles[sec] = new Dictionary<int, IList<Candle>>();
                        foreach (int frame in frames)
                        {
                            string res = updateCandles(sec, frame).Result;
                        }
                    }

                    Task.Delay(5000).Wait();
                }
            });
        }

        private Task<string> updateCandles(string sec, int timeframe)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>(); 

            IDictionary<Func<int, bool>, Action> actions = new Dictionary<Func<int, bool>, Action>();

            actions.Add((frame) => { return frame == 5; }, () =>
            {
                DateTime? last = null;
                if (_candles[sec].ContainsKey(timeframe))
                {
                    last = _candles[sec][timeframe].Last().begin;
                }

                _client.GetCandles(sec, GetStartDate(timeframe, _sessions, last), 1).ContinueWith(t =>
                {
                    CandlesConverter converter = new CandlesConverter();

                    var work_data = converter.Convert(t.Result, 1, timeframe);
                    _candles[sec][timeframe] = work_data;

                    TCS.SetResult("ok");
                });
            });

            actions.Add((frame) => { return frame == 60; }, () =>
            {
                DateTime? last = null;
                if (_candles[sec].ContainsKey(timeframe))
                {
                    last = _candles[sec][timeframe].Last().begin;
                }

                _client.GetCandles(sec, GetStartDate(timeframe, _sessions, last), timeframe).ContinueWith(t =>
                {
                    _candles[sec][timeframe] = t.Result;

                    TCS.SetResult("ok");
                });
            });

            actions.Single(a => a.Key.Invoke(timeframe)).Value.Invoke();

            return TCS.Task;
        }

        private DateTime GetStartDate(int timeframe, IList<TradeSession> sessions, DateTime? lastDate)
        {
            int count = lastDate.HasValue ? 10 : 40;

            int sessionlength = 9 * 60;
            int sessiondiv = timeframe * count;

            int fullSession = sessiondiv / sessionlength;
            int partSession = sessiondiv % sessionlength;

            DateTime res = sessions[sessions.Count - fullSession - 1].Date.AddHours(10).AddMinutes(partSession);

            return res;
        }
    }
}
