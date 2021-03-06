﻿using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EID.Library;

namespace EIDService.Models
{
    public class TradeModel
    {
        public Position ApplyOrder(UnitOfWork unit, Order order, Settings settings)
        {
            order.OrderState = OrderStateEnum.Executed;

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
                Common.Entities.Mode mode = unit.ModeRepository.Query<Common.Entities.Mode>(m => m.Active).Single();

                pos = new Common.Entities.Position()
                {
                    Firm = mode.Firm,
                    SecurityName = sec,
                    Code = sec,
                    Account = account,
                    Client = client,
                    Type = "T2"
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
                Operation = order.OrderOperation.ToString(),
                Account = order.Account,
                Price = price,
                Count = order.Count,
                Volume = price * order.Count,
                Class = order.Class
            };

            unit.DealRepository.Create(deal);

            MoneyLimit money = unit.MoneyLimitRepository.Query<MoneyLimit>(m => m.Type == "T2").Single();

            if (order.OrderOperation == OrderOperationEnum.Buy)
            {
                money.Available -= order.Count * price;
            }

            if (order.OrderOperation ==  OrderOperationEnum.Sell)
            {
                money.Available += order.Count * price;
            }

            return deal;
        }

    }
}