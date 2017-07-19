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
                Operation = _strategy.GetOperation(),
                Account = order.Account,
                OrderType = "Тэйк - профит и стоп - лимит",
                Count = order.Count,
                StopPrice = Math.Round(_strategy.GetStopPrice(price, profit),2),
                StopLimitPrice = Math.Round(_strategy.GetStopLimitPrice(price, limit),2),
                Price = Math.Round(_strategy.GetPrice(price, limit),2),

                Client = order.Client,
                Class = order.Class,
                State = "Активна"
            };

            if (security.MinStep == 1)
            {
                stop.StopPrice = Math.Round(stop.StopPrice);
                stop.StopLimitPrice = Math.Round(stop.StopLimitPrice);
                stop.Price = Math.Round(stop.Price);
            }

            if (settings.Mode == "Demo")
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

                model.CreateStopOrder(stop, stop_trn.Id);
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