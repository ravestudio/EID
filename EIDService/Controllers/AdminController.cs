﻿using EID.Library;
using EID.Library.ISS;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
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

                context.MoneyLimit.First().Available = 1000000m;

                context.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PrepareData(string sec, DateTime from)
        {
            MicexISSClient client = new MicexISSClient(new WebApiClient());
            var part1 = client.GetCandles(sec, from, from.AddHours(14), "1").Result;
            var part2 = client.GetCandles(sec, from.AddHours(14).AddMinutes(1), from.AddHours(19), "1").Result;

            List<ICandle> candles = new List<ICandle>();
            candles.AddRange(part1);
            candles.AddRange(part2);


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

                foreach (ICandle candle in candles)
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
        public JsonResult EmulateData()
        {
            Common.Entities.Settings settings = null;

            Models.TradeModel tradeModel = new Models.TradeModel();

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order>>();
            IDictionary<Func<Common.Entities.StopOrder, bool>, Action<UnitOfWork, Common.Entities.StopOrder>> stop_actions = new Dictionary<Func<Common.Entities.StopOrder, bool>, Action<UnitOfWork, Common.Entities.StopOrder>>();

            actions.Add((o) => { return o.OrderOperation == OrderOperationEnum.Sell; },
                (unit, order) =>
                {
                    Common.Entities.Position pos = tradeModel.ApplyOrder(unit, order, settings);
                    pos.CurrentBalance -= order.Count;
                });

            actions.Add((o) => { return o.OrderOperation ==  OrderOperationEnum.Buy; },
                (unit, order) =>
                {

                    Common.Entities.Position pos = tradeModel.ApplyOrder(unit, order, settings);
                    pos.CurrentBalance += order.Count;
                });

            stop_actions.Add((o) => { return o.OrderOperation == OrderOperationEnum.Sell; }, (unit, order) =>
            {
                Models.StopOrderExecute execute = new Models.StopOrderExecute(new Models.SellStrategy());
                execute.ApplyStop(unit, order, settings);
            });

            stop_actions.Add((o) => { return o.OrderOperation == OrderOperationEnum.Buy; }, (unit, order) =>
            {
                Models.StopOrderExecute execute = new Models.StopOrderExecute(new Models.BuyStrategy());
                execute.ApplyStop(unit, order, settings);
            });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Common.Entities.Settings>(null).Single();

                var stop_orders = unit.StopOrderRepository.Query<Common.Entities.StopOrder>(o => o.OrderState == OrderStateEnum.IsActive).ToList();
                foreach (Common.Entities.StopOrder order in stop_orders)
                {
                    stop_actions.Single(a => a.Key(order)).Value.Invoke(unit, order);
                }
                unit.Commit();

                var orders_all = unit.OrderRepository.All<Common.Entities.Order>().ToList();
                var activeOrders = orders_all.Where(o => o.OrderState == OrderStateEnum.IsActive).ToList();
                foreach (Common.Entities.Order order in activeOrders)
                {
                    actions.Single(a => a.Key(order)).Value.Invoke(unit, order);
                }

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InvokeProcess()
        {
            IDictionary<Func<Common.Entities.EIDProcess, bool>, Action<UnitOfWork, Common.Entities.EIDProcess>> process_actions = new Dictionary<Func<Common.Entities.EIDProcess, bool>, Action<UnitOfWork, Common.Entities.EIDProcess>>();

            //создан процесс на закрытие позиции
            process_actions.Add((p) => { return p.Type == EIDProcessType.ClosePosition && p.Status == EIDProcessStatus.Created; }, (unit, proc) =>
            {
                ClosePositionProcess closeModel = new ClosePositionProcess();
                closeModel.CancelStopOrders(unit, proc);

            });

            //процесс переведен на снятие стоп заявок
            process_actions.Add((p) => { return p.Type == EIDProcessType.ClosePosition && p.Status == EIDProcessStatus.KillStop; }, (unit, proc) =>
            {
                ClosePositionProcess closeModel = new ClosePositionProcess();

                var trn_id = closeModel.GetTransactionId(unit, proc);

                if (closeModel.CheckTransaction(unit, trn_id) == 0 && closeModel.CheckStop(unit, proc) == 0)
                {
                    proc.Data = string.Format("CODE:{0};", closeModel.GetCode(proc));
                    proc.Status = EIDProcessStatus.KillStopCompleted;

                    unit.Commit();
                }
            });

            //закрытие позиций
            process_actions.Add((p) =>{ return p.Type == EIDProcessType.ClosePosition && p.Status == EIDProcessStatus.KillStopCompleted; }, (unit, proc) =>
            {
                ClosePositionProcess closeModel = new ClosePositionProcess();

                var pos = closeModel.GetPosition(unit, proc);

                if (closeModel.CheckPosition(pos))
                {
                    closeModel.ClosePosition(unit, proc, pos);
                }
            });

            //проверка закрылась ли позиция
            process_actions.Add((p) =>{ return p.Type == EIDProcessType.ClosePosition && p.Status == EIDProcessStatus.ClosePosition; }, (unit, proc) =>
            {
                ClosePositionProcess closeModel = new ClosePositionProcess();

                var trn_id = closeModel.GetTransactionId(unit, proc);
                var pos = closeModel.GetPosition(unit, proc);

                if (closeModel.CheckOrder(unit, trn_id.Single()) && pos.Total == 0)
                {
                    proc.Status = EIDProcessStatus.Completed;
                    unit.Commit();
                }

            });

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                var processes = unit.EIDProcessRepository.Query<Common.Entities.EIDProcess>(p => p.Status != EIDProcessStatus.Completed).ToList();

                foreach (Common.Entities.EIDProcess proc in processes)
                {
                    process_actions.Single(p => p.Key(proc)).Value.Invoke(unit, proc);
                }
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateStopOrders()
        {
            Common.Entities.Settings settings = null;

            IDictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>> actions = new Dictionary<Func<Common.Entities.Order, bool>, Action<UnitOfWork, Common.Entities.Order, Common.Entities.Transaction>>();

            actions.Add((o) => { return o.OrderOperation == OrderOperationEnum.Buy; },
                (unit, order, trn) =>
                {
                    Models.StopOrderCreator creator = new Models.StopOrderCreator(new Models.CreateSellStrategy());
                    creator.Create(unit, order, settings, trn);
                });

            actions.Add((o) => { return o.OrderOperation == OrderOperationEnum.Sell; },
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
                    var order = unit.OrderRepository.Query<Common.Entities.Order>(o => o.Number == transaction.OrderNumber && o.OrderState == OrderStateEnum.Executed).SingleOrDefault();

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
            model.ProcessResults();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClosePosition(string sec)
        {
            EIDProcess process = new EIDProcess();

            process.Type =  EIDProcessType.ClosePosition;
            process.Status = EIDProcessStatus.Created;

            process.Data = string.Format("CODE:{0};",sec);

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                unit.EIDProcessRepository.Create(process);

                unit.Commit();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}