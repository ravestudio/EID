﻿using EIDClient.Core.Entities;
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
using EID.Library;
using EIDClient.Library;

namespace EIDClient.Core.DataModel
{
    public class TradeModel
    {
        private TradeSessionRepository _tradeSessionRepository = null;
        private CandleRepository _candleRepository = null;
        private OrderRepository _orderRepository = null;

        private ITradeMode _mode = null;

        private IList<TradeSession> _sessions = null;
        private IDictionary<string, IDictionary<int, IList<ICandle>>> _candles = null;

        public TradeModel(TradeSessionRepository TradeSessionRepository, CandleRepository CandleRepository, OrderRepository OrderRepository, ITradeMode mode)
        {
            _tradeSessionRepository = TradeSessionRepository;
            _candleRepository = CandleRepository;
            _orderRepository = OrderRepository;

            _mode = mode;

            _candles = new Dictionary<string, IDictionary<int, IList<ICandle>>>();

            Messenger.Default.Register<InitTradeModelMessage>(this, async (msg) =>
            {
                var temp_sessions = await _tradeSessionRepository.GetAll();
                _sessions = temp_sessions.ToList();

                _mode.SetAction("init", () =>
                {
                    foreach (string sec in msg.securities)
                    {
                        _candles[sec] = new Dictionary<int, IList<ICandle>>();
                        foreach (int frame in msg.frames)
                        {
                            string res = InitCandles(sec, frame).Result;
                        }
                    }
                });

                _mode.SetAction("update", () =>
                {
                    IEnumerable<Candle> candles = _candleRepository.GetAll().Result;

                    foreach (string sec in msg.securities)
                    {
                        var tempData = candles.Where(c => c.Code == sec);
                        UpdateCadles(sec, tempData, msg.frames);
                    }

                });

                _mode.SetAction("sendToRobo", () =>
                {
                    Messenger.Default.Send<GetCandlesResponseMessage>(new GetCandlesResponseMessage()
                    {
                        Сandles = _candles
                    });
                });


                mode.Start();
            });

            Messenger.Default.Register<CreateOrderMessage>(this, async msg =>
            {
                Order order = new Order()
                {
                    Code = msg.Code,
                    Operation = msg.Operation,
                    Account = msg.Account,
                    Price = msg.Price,
                    Count = msg.Count,
                    Class = msg.Class,
                    Comment = msg.Comment
                };

               string result = await _orderRepository.Create(order);
            });

            //Messenger.Default.Register<GetCandlesMessage>(this, (msg) =>
            //{
            //    Messenger.Default.Send<GetCandlesResponseMessage>(new GetCandlesResponseMessage()
            //    {
            //        Сandles = _candles
            //    });
            //});
        }

        private void updateStoredData(IEnumerable<ICandle> candles, string code, int frame)
        {
            DateTime dt = candles.First().begin;

            ICandle candle = _candles[code][frame].First(c => c.begin >= dt);
            int index = _candles[code][frame].IndexOf(candle);

            foreach (ICandle item in candles)
            {
                _candles[code][frame][index] = item;
                index++;
            }
        }

        private void UpdateCadles(string code, IEnumerable<Candle> candles, IList<int> frames)
        {
            IDictionary<int, Action> actions = new Dictionary<int, Action>();

            actions.Add(5, () => {

                updateStoredData(candles, code, 5);
            });

            actions.Add(60, () =>
            {
                CandlesConverter converter = new CandlesConverter(() => { return new Candle(); });
                var work_data = converter.Convert(_candles[code][5], 5, 60);

                updateStoredData(work_data, code, 60);
            });

            foreach(int f in frames)
            {
                actions[f].Invoke();
            }
        }

        private Task<string> InitCandles(string sec, int timeframe)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>(); 

            IDictionary<Func<int, bool>, Action> actions = new Dictionary<Func<int, bool>, Action>();

            actions.Add((frame) => { return frame == 5; }, () =>
            {
                _candleRepository.GetHistory(sec, GetStartDate(timeframe, _sessions, _mode.GetDate()), 1).ContinueWith(t =>
                {
                    CandlesConverter converter = new CandlesConverter(() => { return new Candle(); });

                    var work_data = converter.Convert(t.Result.ToList(), 1, timeframe);
                    _candles[sec][timeframe] = work_data;

                    TCS.SetResult("ok");
                });
            });

            actions.Add((frame) => { return frame == 60; }, () =>
            {
                _candleRepository.GetHistory(sec, GetStartDate(timeframe, _sessions, _mode.GetDate()), timeframe).ContinueWith(t =>
                {
                    _candles[sec][timeframe] = t.Result.ToList();

                    TCS.SetResult("ok");
                });
            });

            actions.Single(a => a.Key.Invoke(timeframe)).Value.Invoke();

            return TCS.Task;
        }

        private DateTime GetStartDate(int timeframe, IList<TradeSession> sessions, DateTime currentDate)
        {
            //количество фреймов
            int count = 20;

            //текущая сессия
            TradeSession curentSession = sessions.Single(s => currentDate >= s.Date.AddHours(10) && currentDate < s.Date.AddHours(19));

            int curIndex = sessions.IndexOf(curentSession);

            DateTime curentSessionDt = curentSession.Date.AddHours(10);

            int currPart = (currentDate.Hour * 60 + currentDate.Minute) - (curentSessionDt.Hour * 60 + curentSessionDt.Minute);

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
                res = sessions[curIndex - fullSession - 1].Date.AddHours(19).AddMinutes(currPart - partSession);
            }
            else
            {
                res = sessions[curIndex - fullSession].Date.AddHours(10).AddMinutes(currPart - partSession);
            }

            return res;
        }
    }
}
