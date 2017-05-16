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
            decimal profit = 0.2m;
            decimal limit = 0.15m;

            var deals = unit.DealRepository.Query<Common.Entities.Deal>(d => d.OrderNumber == order.Number).ToList();

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
                StopPrice = _strategy.GetStopPrice(price, profit),
                StopLimitPrice = _strategy.GetStopLimitPrice(price, limit),
                Price = _strategy.GetPrice(price, limit),

                Client = order.Client,
                Class = order.Class,
                State = "Активна"
            };

            Common.Entities.Transaction stop_trn = new Common.Entities.Transaction()
            {
                OrderNumber = stop.Number,
                Name = "Ввод лимитной заявки",
                Status = 3,
                Processed = false
            };

            unit.TransactionRepository.Create(stop_trn);
            unit.StopOrderRepository.Create(stop);

            trn.Processed = true;
        }
    }
}