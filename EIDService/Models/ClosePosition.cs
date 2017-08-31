using EID.Library;
using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class ClosePosition
    {
        private ICloseStrategy _closeStrategy = null;

        public ClosePosition(ICloseStrategy closeStrategy)
        {
            _closeStrategy = closeStrategy;
        }

        public Order CreateOrder(UnitOfWork unit, Position pos)
        {
            decimal price = GetPrice(unit, pos.Code);

            var security = unit.SecurityRepository.Query<Common.Entities.Security>(s => s.Code == pos.Code).Single();
            var mode = unit.ModeRepository.Query<Mode>(m => m.Active).Single();

            Order order = new Order()
            {
                Account = mode.Account,
                Client = mode.Client,
                Class = mode.Class,

                Code = pos.Code,
                OrderOperation = _closeStrategy.GetOperation(),
                Price = Math.Round(_closeStrategy.GetPrice(price), 2),
                Count = Math.Abs(pos.Total)
            };

            MathFunctions func = new MathFunctions();
            order.Price = func.Optimize(order.Price, security.MinStep);

            return order;
        }

        public decimal GetPrice(UnitOfWork unit, string sec)
        {
            var candle = unit.CandleRepository.Query<Common.Entities.Candle>(c => c.Code == sec).First();

            return candle.ClosePrice;
        }
    }
}