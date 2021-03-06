﻿using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using EIDService.Models;
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

            //количество транзакций по инструменту
            int trn_count = 0;

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                settings = unit.SettingsRepository.All<Settings>(null).Single();

                trn_count = unit.TransactionRepository.Query<Transaction>(t => t.CODE == order.Code && t.Name == "Ввод заявки").Count();
            }

            actions.Add(ModeType.Test, () =>
            {
                Random rnd = new Random();

                order.Number = rnd.Next(100, 5000);
                order.OrderState = EID.Library.OrderStateEnum.IsActive;
                order.Time = settings.TestDateTime.ToString("HH:mm");

                Transaction trn = new Transaction()
                {
                    CODE = order.Code,
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

            Action RealTransaction = () =>
            {
                Transaction trn = new Transaction()
                {
                    CODE = order.Code,
                    Name = "Ввод заявки",
                    Status = 0,
                    Profit = order.Profit,
                    StopLoss = order.StopLoss,
                    Processed = false
                };

                using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
                {
                    unit.TransactionRepository.Create(trn);
                    unit.Commit();
                }

                TransactionModel trsModel = new TransactionModel();
                trsModel.CreateOrder(order, trn.Id);
            };

            actions.Add(ModeType.Work, RealTransaction);

            actions.Add(ModeType.Demo, RealTransaction);

            //если транзакци нет, выполняем
            if (trn_count == 0)
            {
                actions[settings.ModeType].Invoke();
            }
        }
    }
}
