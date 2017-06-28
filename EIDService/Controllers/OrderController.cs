﻿using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EIDService.Controllers
{
    public class OrderController : ApiController
    {
        // POST api/order
        public void Post([FromBody]Order order)
        {
            IDictionary<ModeType, Action> actions = new Dictionary<ModeType, Action>();

            Settings settings = null;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Settings>(null).Single();
            }

            actions.Add(ModeType.Test, () =>
            {
                Random rnd = new Random();

                order.Number = rnd.Next(100, 5000);
                order.StateType = OrderStateType.IsActive;
                order.Time = settings.TestDateTime.ToString("HH:mm");

                Transaction trn = new Transaction()
                {
                    OrderNumber = order.Number,
                    Name = "Ввод заявки",
                    Status = 3,
                    Profit = order.Profit,
                    StopLoss = order.StopLoss,
                    Processed = false
                };

                using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
                {
                    unit.OrderRepository.Create(order);
                    unit.TransactionRepository.Create(trn);
                    unit.Commit();
                }
            });

            actions.Add(ModeType.Work, () =>
            {

            });

            actions[settings.ModeType].Invoke();
        }
    }
}
