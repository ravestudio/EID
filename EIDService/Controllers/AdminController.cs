using EID.Library;
using EID.Library.ISS;
using EIDService.Common.DataAccess;
using EIDService.Models;
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
        public JsonResult Reset(DateTime testTime)
        {
            using (DataContext context = new DataContext())
            {
                context.Database.ExecuteSqlCommand("delete from DealSet");
                context.Database.ExecuteSqlCommand("delete from OrderSet");
                context.Database.ExecuteSqlCommand("delete from PositionSet");
                context.Database.ExecuteSqlCommand("delete from StopOrderSet");
                context.Database.ExecuteSqlCommand("delete from TransactionSet");

                context.Settings.First().TestDateTime = testTime;

                context.MoneyLimit.First().Available = 100000m;

                context.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
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
                    entity.MinPrice = candle.low; 
                    entity.OpenPrice = candle.open;
                    entity.ClosePrice = candle.close;
                    entity.Volume = candle.volume;
                    entity.Value = candle.value;
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

            Models.TradeModel tradeModel = new Models.TradeModel();

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>>();
            IDictionary<Func<Common.Entities.StopOrder, bool>, Action<UnitOfWork, Common.Entities.StopOrder>> stop_actions = new Dictionary<Func<Common.Entities.StopOrder, bool>, Action<UnitOfWork, Common.Entities.StopOrder>>();

            actions.Add((o) => { return o.Price == 0 && o.Operation == "Продажа"; },
                (unit, order) =>
                {
                    Common.Entities.Position pos = tradeModel.ApplyOrder(unit, order, settings);
                    pos.CurrentBalance -= order.Count;
                });

            actions.Add((o) => { return o.Price == 0 && o.Operation == "Купля"; },
                (unit, order) =>
                {

                    Common.Entities.Position pos = tradeModel.ApplyOrder(unit, order, settings);
                    pos.CurrentBalance += order.Count;
                });

            stop_actions.Add((o) => { return o.Operation == "Продажа"; }, (unit, order) =>
            {
                Models.StopOrderExecute execute = new Models.StopOrderExecute(new Models.SellStrategy());
                execute.ApplyStop(unit, order, settings);
            });

            stop_actions.Add((o) => { return o.Operation == "Купля"; }, (unit, order) =>
            {
                Models.StopOrderExecute execute = new Models.StopOrderExecute(new Models.BuyStrategy());
                execute.ApplyStop(unit, order, settings);
            });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Common.Entities.Settings>(null).Single();

                var stop_orders = unit.StopOrderRepository.Query<Common.Entities.StopOrder>(o => o.State == "Активна").ToList();
                foreach (Common.Entities.StopOrder order in stop_orders)
                {
                    stop_actions.Single(a => a.Key(order)).Value.Invoke(unit, order);
                }
                unit.Commit();

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
            Common.Entities.Settings settings = null;

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>>();

            actions.Add((o) => { return o.Operation == "Купля"; },
                (unit, order, trn) =>
                {
                    Models.StopOrderCreator creator = new Models.StopOrderCreator(new Models.CreateSellStrategy());
                    creator.Create(unit, order, settings, trn);
                });

            actions.Add((o) => { return o.Operation == "Продажа"; },
                (unit, order, trn) =>
                {
                    Models.StopOrderCreator creator = new Models.StopOrderCreator(new Models.CreateBuyStrategy());
                    creator.Create(unit, order, settings, trn);
                });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Common.Entities.Settings>(null).Single();

                var transactions = unit.TransactionRepository.Query<Common.Entities.Transaction>(t => !t.Processed && t.Status == 3 && t.Name == "Ввод заявки", null).ToList();

                foreach(Common.Entities.Transaction transaction in transactions)
                {
                    var order = unit.OrderRepository.Query<Common.Entities.Order>(o => o.Number == transaction.OrderNumber && o.State == "Исполнена").SingleOrDefault();

                    if (order != null)
                    {
                        actions.Single(a => a.Key(order)).Value.Invoke(unit, order, transaction);
                    }
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReadTransactionResult()
        {
            TransactionModel model = new TransactionModel();

            model.ReadResults();
            model.PreocessResults();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}