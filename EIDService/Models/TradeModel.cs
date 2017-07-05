using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class TradeModel
    {
        public Position ApplyOrder(UnitOfWork unit, Order order, Settings settings)
        {
            order.StateType = Common.Entities.OrderStateType.Executed;

            Position pos = this.GetPosition(unit, order.Code, order.Account, order.Client);

            decimal price = this.GetPrice(unit, order.Code, settings.TestDateTime);

            Common.Entities.Deal deal = this.MakeDeal(unit, order, settings.TestDateTime, price);

            pos.PurchasePrice = price;

            return pos;
        }

        public Common.Entities.Position GetPosition(UnitOfWork unit, string sec, string account, string client)
        {
            Common.Entities.Position pos = unit.PositionRepository.Query<Common.Entities.Position>(p => p.Code == sec).SingleOrDefault();

            if (pos == null)
            {
                pos = new Common.Entities.Position()
                {
                    Firm = "NC0011100000",
                    SecurityName = sec,
                    Code = sec,
                    Account = account,
                    Client = client,
                    Type = "Т0"
                };
                unit.PositionRepository.Create(pos);
            }

            return pos;
        }

        public decimal GetPrice(UnitOfWork unit, string sec, DateTime dateTime)
        {
            EIDService.Common.CandleToISS candleToISS = new Common.CandleToISS();
            var tempdata = unit.CandleRepository.Query<Common.Entities.Candle>(c => c.Code == sec).ToList();
            var candles = tempdata.Select(c => candleToISS.Convert(c)).ToList();

            decimal price = candles.Last(c => c.begin < dateTime).close;

            return price;
        }

        public Common.Entities.Deal MakeDeal(UnitOfWork unit, Common.Entities.Order order, DateTime dateTime, decimal price)
        {
            Random rnd = new Random();

            Common.Entities.Deal deal = new Common.Entities.Deal()
            {
                Number = rnd.Next(7000, 900000),
                OrderNumber = order.Number,
                Code = order.Code,
                Time = dateTime.ToString("HH:mm"),
                Date = dateTime.ToString("dd:MM:yyyy"),
                Operation = order.Operation,
                Account = order.Account,
                Price = price,
                Count = order.Count,
                Volume = price * order.Count,
                Class = order.Class
            };

            unit.DealRepository.Create(deal);

            MoneyLimit money = unit.MoneyLimitRepository.Query<MoneyLimit>(m => m.Type == "T0").Single();

            if (order.Operation == "Купля")
            {
                money.Available -= order.Count * price;
            }

            if (order.Operation == "Продажа")
            {
                money.Available += order.Count * price;
            }

            return deal;
        }

    }
}