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
using EID.Library;
using EIDClient.Library;

namespace EIDClient.Core.DataModel
{
    public class TradeModel
    {
        private TradeSessionRepository _tradeSessionRepository = null;
        private CandleRepository _candleRepository = null;
        private OrderRepository _orderRepository = null;
        private PositionRepository _positionRepository = null;
        private DealRepository _dealRepository = null;

        private ITradeMode _mode = null;
        private Mode _modeProperties = null;

        private IList<TradeSession> _sessions = null;
        private IDictionary<string, IDictionary<string, IList<ICandle>>> _candles = null;
        private IDictionary<string, string> _positions = null;
        private IEnumerable<Position> _positionlist = null;

        private object _token = null;

        private string ServerURL = "http://localhost:99/";
        private WebApiClient _apiClient = null;

        public TradeModel(TradeSessionRepository TradeSessionRepository,
            CandleRepository CandleRepository,
            OrderRepository OrderRepository,
            PositionRepository PositionRepository,
            DealRepository DealRepository,
            WebApiClient client,
            ITradeMode mode, Mode modeProperties, object token)
        {
            _token = token;
            _tradeSessionRepository = TradeSessionRepository;
            _candleRepository = CandleRepository;
            _orderRepository = OrderRepository;
            _positionRepository = PositionRepository;
            _dealRepository = DealRepository;
            _apiClient = client;

            _mode = mode;
            _modeProperties = modeProperties;

            _candles = new Dictionary<string, IDictionary<string, IList<ICandle>>>();

            Messenger.Default.Register<InitTradeModelMessage>(this, _token, async (msg) =>
            {
                var temp_sessions = await _tradeSessionRepository.GetAll();
                _sessions = temp_sessions.OrderBy(t => t.Date).ToList();

                _mode.SetAction("init", () =>
                {
                    foreach (string sec in msg.securities)
                    {
                        _candles[sec] = new Dictionary<string, IList<ICandle>>();
                        foreach (string frame in msg.frames)
                        {
                            string res = InitCandles(sec, frame).Result;
                        }
                    }
                });

                _mode.SetAction("initEmpty", () =>
                {
                    foreach (string sec in msg.securities)
                    {
                        _candles[sec] = new Dictionary<string, IList<ICandle>>();
                        foreach (string frame in msg.frames)
                        {
                            _candles[sec][frame] = new List<ICandle>();
                        }
                    }
                });

                _mode.SetAction("update", () =>
                {
                    IEnumerable<Candle> candles = _candleRepository.GetAll().Result;

                    _positionlist = _positionRepository.GetAll().Result;

                    foreach (string sec in msg.securities)
                    {
                        var tempData = candles.Where(c => c.Code == sec).OrderBy(c => c.begin);
                        UpdateCadles(sec, tempData, msg.frames);
                    }

                    this._positions = GetPositions(_positionlist, msg.securities);

                });

                _mode.SetAction("sendToRobo", () =>
                {
                    Messenger.Default.Send<GetCandlesResponseMessage>(new GetCandlesResponseMessage()
                    {
                        DateTime = mode.GetDate(),
                        Сandles = _candles,
                        Positions = _positions
                    }, _token);
                });

                _mode.SetAction("showData", () =>
                {
                    _dealRepository.GetAll().ContinueWith(t =>
                    {
                        Messenger.Default.Send<ShowDataMessage>(new ShowDataMessage()
                        {
                            Сandles = CopyCandles(_candles),
                            Deals = t.Result,
                            Positions = _positionlist
                        });
                    });
                });

                mode.Start();
            });

            Messenger.Default.Register<CreateOrderMessage>(this, msg =>
            {
                Order order = new Order()
                {
                    Code = msg.Code,
                    Operation = msg.Operation,
                    Account = _modeProperties.Account,
                    Price = msg.Price,
                    Count = msg.Count,
                    Class = _modeProperties.Class,
                    Client = _modeProperties.Client,
                    Comment = _modeProperties.Client,
                    Profit = msg.Profit,
                    StopLoss = msg.StopLoss
                };

               string result = _orderRepository.Create(order).Result;
            });

            //Messenger.Default.Register<GetCandlesMessage>(this, (msg) =>
            //{
            //    Messenger.Default.Send<GetCandlesResponseMessage>(new GetCandlesResponseMessage()
            //    {
            //        Сandles = _candles
            //    });
            //});

            Messenger.Default.Register<ClosePositionMessage>(this, msg =>
            {
                string result = _apiClient.GetData(string.Format("{0}admin/ClosePosition?sec={1}", this.ServerURL, msg.Code)).Result;
            });
        }

        private IDictionary<string, IDictionary<string, IList<ICandle>>> CopyCandles(IDictionary<string, IDictionary<string, IList<ICandle>>> candles)
        {
            IDictionary<string, IDictionary<string, IList<ICandle>>> newdata = new Dictionary<string, IDictionary<string, IList<ICandle>>>();

            foreach(string sec in candles.Keys)
            {
                newdata.Add(sec, new Dictionary<string, IList<ICandle>>());

                foreach(string f in candles[sec].Keys)
                {
                    newdata[sec].Add(f, new List<ICandle>());

                    foreach(ICandle candle in candles[sec][f])
                    {
                        newdata[sec][f].Add(new Candle()
                        {
                            Code = candle.Code,
                            begin = candle.begin,
                            open = candle.open,
                            close = candle.close,
                            high = candle.high
                        });
                    }
                }
            }

            return newdata;
        }

        private IDictionary<string, string> GetPositions(IEnumerable<Position> positions, IList<string> securities)
        {
            IDictionary<string, string> res = new Dictionary<string, string>();

            IDictionary<Func<Position, bool>, string> actions = new Dictionary<Func<Position, bool>, string>();

            actions.Add((p) => { return p.CurrentBalance == 0 && p.BlockedForPurchase == 0; }, "free");
            actions.Add((p) => { return p.CurrentBalance > 0; }, "long");
            actions.Add((p) => { return p.CurrentBalance < 0; }, "short");

            foreach (string sec in securities)
            {
                res.Add(sec, "free");

                Position pos = positions.SingleOrDefault(p => p.Code == sec && p.Type == "T2");

                if (pos != null)
                {
                    res[sec] = actions.Single(a => a.Key(pos)).Value;
                }
            }

            return res;
        }

        private void updateStoredData(IEnumerable<ICandle> candles, string code, string frame)
        {
            DateTime dt = candles.First().begin;

            int index = -1;

            ICandle candle = _candles[code][frame].FirstOrDefault(c => c.begin >= dt);

            if (candle != null)
            {
                index = _candles[code][frame].IndexOf(candle);
            }

            foreach (ICandle item in candles)
            {
                if (index == -1)
                {
                    
                    _candles[code][frame].Add(item);
                }

                if (index >= 0 && index >= _candles[code][frame].Count)
                {
                    _candles[code][frame].Add(item);
                    index++;
                }

                if (index >= 0 && index < _candles[code][frame].Count)
                {
                    _candles[code][frame][index] = item;
                    index++;
                }

            }
        }

        private void UpdateCadles(string code, IEnumerable<Candle> candles, IList<string> frames)
        {
            IDictionary<string, Action> actions = new Dictionary<string, Action>();

            actions.Add("5", () => {

                updateStoredData(candles, code, "5");
            });

            actions.Add("60", () =>
            {
                CandlesConverter converter = new CandlesConverter(() => { return new Candle(); });
                var work_data = converter.Convert(_candles[code]["5"], 5, 60);

                updateStoredData(work_data, code, "60");
            });

            foreach(string f in frames)
            {
                actions[f].Invoke();
            }
        }

        private Task<string> InitCandles(string sec, string timeframe)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>(); 

            IDictionary<Func<string, bool>, Action> actions = new Dictionary<Func<string, bool>, Action>();

            actions.Add((frame) => { return frame == "5"; }, () =>
            {
                _candleRepository.GetHistory(sec, GetStartDate(timeframe, _sessions, _mode.GetDate()), "1").ContinueWith(t =>
                {
                    CandlesConverter converter = new CandlesConverter(() => { return new Candle(); });

                    var work_data = converter.Convert(t.Result.ToList(), 1, 5);
                    _candles[sec][timeframe] = work_data;

                    TCS.SetResult("ok");
                });
            });

            actions.Add((frame) => { return frame == "60"; }, () =>
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

        private DateTime GetStartDate(string timeframe, IList<TradeSession> sessions, DateTime currentDate)
        {
            IDictionary<string, int> _frameDic = new Dictionary<string, int>();

            _frameDic.Add("5", 5);
            _frameDic.Add("60", 60);

            TradeSession lastSession = sessions.Last();

            //if (lastSession.Date.AddHours(19) < currentDate)
            //{
            //    currentDate = lastSession.Date.AddHours(19).AddMinutes(-1);
            //}

            //if (lastSession.Date.AddHours(10) > currentDate)
            //{
            //    currentDate = sessions[sessions.Count -2].Date.AddHours(19).AddMinutes(-1);
            //}

            //количество фреймов
            int count = 80;

            //текущая сессия

            var ss = sessions.Where(s => currentDate >= s.Date.AddHours(10) && currentDate < s.Date.AddHours(19)).ToList();
            TradeSession curentSession = sessions.Single(s => currentDate >= s.Date.AddHours(10) && currentDate < s.Date.AddHours(19));

            int curIndex = sessions.IndexOf(curentSession);

            DateTime curentSessionDt = curentSession.Date.AddHours(10);

            int currPart = (currentDate.Hour * 60 + currentDate.Minute) - (curentSessionDt.Hour * 60 + curentSessionDt.Minute);

            //длина сессии
            int sessionlength = 9 * 60;
            //длина смещения
            int sessiondiv = _frameDic[timeframe] * count;

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
