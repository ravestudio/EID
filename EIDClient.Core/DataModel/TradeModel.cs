using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EIDClient.Core.Managers;
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
        private CandleRepository _candleRepository = null;
        private ITradeMode _mode = null;

        private IList<TradeSession> _sessions = null;
        private IDictionary<string, IDictionary<int, IList<Candle>>> _candles = null;

        public TradeModel(TradeSessionRepository TradeSessionRepository, CandleRepository CandleRepository, ITradeMode mode)
        {
            _tradeSessionRepository = TradeSessionRepository;
            _candleRepository = CandleRepository;
            _mode = mode;

            _candles = new Dictionary<string, IDictionary<int, IList<Candle>>>();

            Messenger.Default.Register<InitTradeModelMessage>(this, async (msg) =>
            {
                var temp_sessions = await _tradeSessionRepository.GetAll();
                _sessions = temp_sessions.ToList();

                Do(msg.securities, msg.frames);

                mode.Start();
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
            _mode.SetAction("updater", () =>
            {
                foreach (string sec in securities)
                {
                    _candles[sec] = new Dictionary<int, IList<Candle>>();
                    foreach (int frame in frames)
                    {
                        string res = updateCandles(sec, frame).Result;
                    }
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

                _candleRepository.GetHistory(sec, GetStartDate(timeframe, _sessions, last), 1).ContinueWith(t =>
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

                _candleRepository.GetHistory(sec, GetStartDate(timeframe, _sessions, last), timeframe).ContinueWith(t =>
                {
                    _candles[sec][timeframe] = t.Result.ToList();

                    TCS.SetResult("ok");
                });
            });

            actions.Single(a => a.Key.Invoke(timeframe)).Value.Invoke();

            return TCS.Task;
        }

        private DateTime GetStartDate(int timeframe, IList<TradeSession> sessions, DateTime? lastDate, DateTime currentDate)
        {
            //количество фреймов
            int count = lastDate.HasValue ? 10 : 30;

            //текущая сессия
            DateTime curentSession = sessions.Single(s => currentDate >= s.AddHours(10) && currentDate < s.AddHours(19));

            int curIndex = sessions.IndexOf(curentSession);

            curentSession = curentSession.AddHours(10);

            int currPart = (currentDate.Hour * 60 + currentDate.Minute) - (curentSession.Hour * 60 + curentSession.Minute);

            //длина сессии
            int sessionlength = 9 * 60;
            //длина смещения
            int sessiondiv = timeframe * count;

            //полных сессий
            int fullSession = sessiondiv / sessionlength;
            //часть сессии
            int partSession = sessiondiv % sessionlength;

            DateTime res;

            if (currPart < partSession)
            {
                res = sessions[curIndex - fullSession - 1].AddHours(19).AddMinutes(currPart - partSession);
            }
            else
            {
                res = sessions[curIndex - fullSession].AddHours(10).AddMinutes(currPart - partSession);
            }

            return res;
        }
    }
}
