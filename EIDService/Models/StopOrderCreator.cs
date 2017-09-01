using EID.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class StopOrderCreator
    {
        private ICreateStrategy _strategy;

        public StopOrderCreator(ICreateStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Create(UnitOfWork unit, Order order, Settings settings, Transaction trn)
        {
            decimal profit = trn.Profit;
            decimal limit = trn.StopLoss;

            var deals = unit.DealRepository.Query<Common.Entities.Deal>(d => d.OrderNumber == order.Number).ToList();

            var security = unit.SecurityRepository.Query<Common.Entities.Security>(s => s.Code == order.Code).Single();

            decimal price = _strategy.GetDealPrice(deals);

            Random rnd = new Random();

            Common.Entities.StopOrder stop = new Common.Entities.StopOrder()
            {
                Number = rnd.Next(7000, 900000),
                Code = order.Code,
                Time = settings.TestDateTime.ToString("HH:mm:00"),
                OrderOperation = _strategy.GetOperation(),
                Account = order.Account,
                OrderType = "Тэйк - профит и стоп - лимит",
                Count = order.Count,
                StopPrice = _strategy.GetStopPrice(price, profit),
                StopLimitPrice = _strategy.GetStopLimitPrice(price, limit),
                Price = Math.Round(_strategy.GetPrice(price, limit),2),

                Client = order.Client,
                Class = order.Class,
                OrderState = OrderStateEnum.IsActive
            };

            MathFunctions func = new MathFunctions();

            stop.Price = func.Optimize(stop.Price, security.MinStep);


            if (settings.Mode == "Demo" || settings.Mode == "Work")
            {
                Common.Entities.Transaction stop_trn = new Common.Entities.Transaction()
                {
                    Name = "Ввод лимитной заявки",
                    Status = 0,
                    Processed = false
                };
                unit.TransactionRepository.Create(stop_trn);
                unit.Commit();

                Models.TransactionModel model = new TransactionModel();

                model.CreateStopOrder(stop, stop_trn.Id, settings);
            }

            if (settings.Mode == "Test")
            {
                Common.Entities.Transaction stop_trn = new Common.Entities.Transaction()
                {
                    OrderNumber = stop.Number,
                    Name = "Ввод лимитной заявки",
                    Status = 3,
                    Processed = false
                };

                unit.TransactionRepository.Create(stop_trn);
                unit.StopOrderRepository.Create(stop);
            }

            trn.Processed = true;
        }
    }
}