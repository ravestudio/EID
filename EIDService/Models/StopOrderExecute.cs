using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            if (lossProfit.HasValue && lossProfit.Value > 5m)
            {
                apply = true;
            }

            if (apply)
            {
                Random rnd = new Random();
                order.State = "Исполнена";

                Common.Entities.Order new_order = new Common.Entities.Order()
                {
                    Number = rnd.Next(7000, 900000),
                    Code = order.Code,
                    Time = settings.TestDateTime.ToString("HH:mm"),
                    Operation = order.Operation,
                    Account = order.Account,
                    Price = applyPrice,
                    Count = order.Count,
                    StateType = Common.Entities.OrderStateType.IsActive,
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