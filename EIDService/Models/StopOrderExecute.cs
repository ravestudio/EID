using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EID.Library;

namespace EIDService.Models
{
    public class StopOrderExecute
    {
        IExecuteStrategy _strategy;
        Models.TradeModel tradeModel;

        public StopOrderExecute(IExecuteStrategy strategy)
        {
            _strategy = strategy;

            tradeModel = new Models.TradeModel();
        }

        public void ApplyStop(UnitOfWork unit, StopOrder order, Settings settings)
        {
            bool apply = false;
            decimal applyPrice = 0;

            decimal price = tradeModel.GetPrice(unit, order.Code, settings.TestDateTime);

            var transaction = unit.TransactionRepository.Query<Common.Entities.Transaction>(t => t.OrderNumber == order.Number).Single();

            _strategy.CalcProfit(transaction, order, price);

            decimal? lossProfit = _strategy.GetLossProfit(transaction, price);


            //отступ от max 0,3%
            if (lossProfit.HasValue && (lossProfit.Value / price)*100 > 0.5m)
            {
                apply = true;
            }

            if (_strategy.StopLimit(order, price))
            {
                apply = true;
            }

            if (apply)
            {
                Random rnd = new Random();
                order.OrderState = OrderStateEnum.Executed;

                Common.Entities.Order new_order = new Common.Entities.Order()
                {
                    Number = rnd.Next(7000, 900000),
                    Code = order.Code,
                    Time = settings.TestDateTime.ToString("HH:mm"),
                    OrderOperation = order.OrderOperation,
                    Account = order.Account,
                    Price = applyPrice,
                    Count = order.Count,
                    OrderState = OrderStateEnum.IsActive,
                    Class = order.Class,
                    Client = order.Client,
                    Comment = order.Client
                };

                order.OrderNumber = new_order.Number;
                unit.OrderRepository.Create(new_order);
            }
        }
    }
}