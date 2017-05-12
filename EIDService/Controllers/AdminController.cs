﻿using EIDClient.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.ISS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIDService.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PrepareData(string sec, DateTime from)
        {
            MicexISSClient client = new MicexISSClient(new WebApiClient());
            var candles = client.GetCandles(sec, from, null, 1).Result;


            //using (DataContext context = new DataContext())
            //{
            //    context.Database.
            //}

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                int key = 0;

                if (unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).Count() > 0)
                {
                    key = unit.CandleRepository.All<EIDService.Common.Entities.Candle>(null).Max(c => c.Id);
                }

                foreach (Candle candle in candles)
                {
                    key++;
                    EIDService.Common.Entities.Candle entity = new Common.Entities.Candle();
                    entity.Id = key;
                    entity.CandleDate = candle.begin.ToString("yyyyMMdd");
                    entity.CandleTime = candle.begin.ToString("HHmm00");
                    entity.MaxPrice = candle.high;
                    entity.OpenPrice = candle.open;
                    entity.ClosePrice = candle.close;
                    entity.Code = sec;

                    unit.CandleRepository.Create(entity);
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ProcessData()
        {
            Common.Entities.Settings settings = null;

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>>();

            actions.Add((o) => { return o.Price == 0 && o.Operation == "Купля"; },
                (unit, order) =>
                {
                    order.StateType = Common.Entities.OrderStateType.Executed;
                    Common.Entities.Position pos = unit.PositionRepository.Query<Common.Entities.Position>(p => p.Code == order.Code).SingleOrDefault();

                    var tempdata = unit.CandleRepository.Query<Common.Entities.Candle>(c => c.Code == order.Code).ToList();
                    var candles = tempdata.Select(c => new EIDService.Common.ISS.Candle(c)).ToList();

                    decimal price = candles.Last(c => c.begin < settings.TestDateTime).close;

                    if (pos == null)
                    {
                        pos = new Common.Entities.Position()
                        {
                            Firm = "NC0011100000",
                            SecurityName = order.Code,
                            Code = order.Code,
                            Account = order.Account,
                            Client = order.Comment,
                            Type = "Т0"
                        };
                        unit.PositionRepository.Create(pos);
                    }

                    Random rnd = new Random();

                    Common.Entities.Deal deal = new Common.Entities.Deal()
                    {
                        Number = rnd.Next(7000, 900000),
                        OrderNumber = order.Number,
                        Code = order.Code,
                        Time = settings.TestDateTime.ToString("HH:mm"),
                        Date = settings.TestDateTime.ToString("dd:MM:yyyy"),
                        Operation = order.Operation,
                        Account = order.Account,
                        Price = price,
                        Count = order.Count,
                        Volume = order.Price * order.Count,
                        Class = order.Class
                    };

                    unit.DealRepository.Create(deal);

                    pos.CurrentBalance += order.Count;
                    pos.PurchasePrice = price;
                });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Common.Entities.Settings>(null).Single();

                var orders_all = unit.OrderRepository.All<Common.Entities.Order>().ToList();

                var activeOrders = orders_all.Where(o => o.StateType == Common.Entities.OrderStateType.IsActive).ToList();

                foreach (Common.Entities.Order order in activeOrders)
                {
                    actions.Single(a => a.Key(order)).Value.Invoke(unit, order);
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateStopOrders()
        {
            decimal profit = 0.5m;
            decimal limit = 0.3m;

            Common.Entities.Settings settings = null;

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>>();

            actions.Add((o) => { return o != null && o.StateType == Common.Entities.OrderStateType.Executed && o.Operation == "Купля"; },
                (unit, order, trn) =>
                {
                    var deals = unit.DealRepository.Query<Common.Entities.Deal>(d => d.OrderNumber == order.Number).ToList();

                    decimal price = deals.Select(d => d.Price).Max();

                    Random rnd = new Random();

                    Common.Entities.StopOrder stop = new Common.Entities.StopOrder()
                    {
                        Number = rnd.Next(7000, 900000),
                        Code = order.Code,
                        Time = settings.TestDateTime.ToString("HH:mm:00"),
                        Operation = "Продажа",
                        Account = order.Account,
                        OrderType = "Тэйк - профит и стоп - лимит",
                        Count = order.Count,
                        StopPrice = price + profit * price / 100m,
                        StopLimitPrice = price - limit * price / 100m,
                        Price = price - (limit + 0.1m) * price / 100m,

                        Client = order.Client,
                        Class = order.Class,
                        State = "Активна"
                    };

                    unit.StopOrderRepository.Create(stop);

                    trn.Processed = true;

                });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Common.Entities.Settings>(null).Single();

                var transactions = unit.TransactionRepository.Query<Common.Entities.Transaction>(t => !t.Processed && t.Status == 3 && t.Name == "Ввод заявки", null).ToList();

                foreach(Common.Entities.Transaction transaction in transactions)
                {
                    var order = unit.OrderRepository.Query<Common.Entities.Order>(o => o.Number == transaction.OrderNumber).SingleOrDefault();

                    actions.Single(a => a.Key(order)).Value.Invoke(unit, order, transaction);
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}