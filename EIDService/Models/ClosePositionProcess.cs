using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace EIDService.Models
{
    public class ClosePositionProcess
    {
        public void CancelStopOrders(UnitOfWork unit, EIDProcess proc)
        {

            string sec = GetCode(proc);

            var orders = unit.StopOrderRepository.Query<StopOrder>(p => p.Code == sec && p.OrderState == OrderStateEnum.IsActive, null).ToList();

            if (orders.Count > 0)
            {
                CancelStop(orders, proc, sec, unit);
            }

            if (orders.Count == 0)
            {
                proc.Status = EIDProcessStatus.KillStopCompleted;
            }

            unit.Commit();
        }

        /// <summary>
        /// Получить позицию
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        public Position GetPosition(UnitOfWork unit, EIDProcess proc)
        {
            string sec = GetCode(proc);

            var pos = unit.PositionRepository.Query<Position>(p => p.Code == sec && p.PosType == PosTypeEnum.T2, null).SingleOrDefault();

            return pos;
        }

        /// <summary>
        /// проверить что позиция открыта, и ничего не заблокировано
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool CheckPosition(Position pos)
        {
            return pos.Total != 0 && pos.Total == pos.Available;
        }

        /// <summary>
        /// Проверить что ордер исполнился
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="trn_id"></param>
        /// <returns></returns>
        public bool CheckOrder(UnitOfWork unit, int trn_id)
        {
            var trn = unit.TransactionRepository.Query<Transaction>(t => t.Id == trn_id).Single();

            var order = unit.OrderRepository.Query<Order>(o => o.Number == trn.OrderNumber).SingleOrDefault();

            return order != null && order.OrderState == OrderStateEnum.Executed;
        }

        public void ClosePosition(UnitOfWork unit, EIDProcess proc, Position pos)
        {

            IDictionary<Func<Position, bool>, Func<UnitOfWork, Position, Order>> actionDic = new Dictionary<Func<Position, bool>, Func<UnitOfWork, Position, Order>>();

            //Если позиция лонг
            actionDic.Add((p) => { return p.Total > 0; }, (u, p) =>
            {
                ClosePosition cp = new Models.ClosePosition(new CloseLongStrategy());

                return cp.CreateOrder(u, p);
            });

            Transaction trn = new Transaction()
            {
                Name = "Ввод заявки",
                Status = 0,
                Processed = true
            };

            unit.TransactionRepository.Create(trn);

            unit.Commit();

            Order order = actionDic.Single(d => d.Key.Invoke(pos)).Value.Invoke(unit, pos);

            TransactionModel trsModel = new TransactionModel();
            trsModel.CreateOrder(order, trn.Id);

            proc.Data = string.Format("CODE:{0};TRN({1});", pos.Code, trn.Id);

            proc.Status = EIDProcessStatus.ClosePosition;

            unit.Commit();
        }

        public IList<int> GetTransactionId(UnitOfWork unit, EIDProcess proc)
        {
            string mask = @"TRN\((?<trn>.+)\);";

            Regex rgx = new Regex(mask);
            Match match = rgx.Match(proc.Data);

            string trn = null;

            if (match.Success)
            {
                trn = match.Groups["trn"].Value;
            }

            string[] trnArr = trn.Split(',');

            IList<int> id_arr = new List<int>();

            foreach (string id in trnArr)
            {
                id_arr.Add(int.Parse(id));
            }

            return id_arr;
        }

        public int CheckTransaction(UnitOfWork unit, IList<int> id_arr)
        {
            int trn_count = 0;

            trn_count = unit.TransactionRepository.Query<Transaction>(t => id_arr.Contains(t.Id) && t.Status != 3, null).Count();

            return trn_count;
        }

        public int CheckStop(UnitOfWork unit, EIDProcess proc)
        {
            int order_count = 0;

            string sec = GetCode(proc);

            order_count = unit.StopOrderRepository.Query<StopOrder>(s => s.Code == sec && s.OrderState == OrderStateEnum.IsActive, null).Count();

            return order_count;
        }

        private void CancelStop(List<StopOrder> orders, EIDProcess proc, string sec, UnitOfWork unit)
        {
            IList<Transaction> trnList = new List<Transaction>();

            foreach (var order in orders)
            {
                Transaction trn = CreateKillTransaction(unit, order);

                trnList.Add(trn);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Transaction trn in trnList)
            {
                sb.Append(trn.Id.ToString());
                sb.Append(",");
            }
            string trnline = sb.ToString().Remove(sb.Length - 1);


            proc.Data = string.Format("CODE:{0};TRN({1});", sec, trnline);
            proc.Status = EIDProcessStatus.KillStop;

        }

        private Transaction CreateKillTransaction(UnitOfWork unit, StopOrder order)
        {
            Transaction trn = new Transaction()
            {
                Name = "Отмена заявки",
                Status = 0,
                Processed = false
            };

            unit.TransactionRepository.Create(trn);

            unit.Commit();

            TransactionModel trsModel = new TransactionModel();
            trsModel.KillStopOrder(order, trn.Id);

            return trn;
        }

        public string GetCode(EIDProcess proc)
        {
            string mask = @"CODE:(?<code>\w+);";

            Regex rgx = new Regex(mask);
            Match match = rgx.Match(proc.Data);

            string sec = null;

            if (match.Success)
            {
                sec = match.Groups["code"].Value;
            }

            return sec;
        }
    }
}