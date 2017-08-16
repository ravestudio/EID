using EIDService.Common.DataAccess;
using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EIDService.Models
{
    public class TransactionModel
    {
        string tri_filePath = @"C:\transactions\transaction.tri";
        string tro_filePath = @"C:\transactions\transaction.tro";

        public string CreateOrder(Order order, int TransId)
        {
            string mask = "ACCOUNT={0}; CLIENT_CODE={1}; TYPE={2}; TRANS_ID={3}; CLASSCODE = {4}; SECCODE = {5}; ACTION = {6}; OPERATION = {7}; PRICE = {8}; QUANTITY = {9}; ";
            string transaction = string.Format(mask, order.Account, order.Client, "L", TransId, order.Class, order.Code, "NEW_ORDER", order.Operation == "Купля"? "B": "S", order.Price, order.Count);

            using (var stream = System.IO.File.AppendText(tri_filePath))
            {
                stream.WriteLine(transaction);
            }

            return "ok";
        }

        public string CreateStopOrder(StopOrder order, int TransId)
        {
            StringBuilder tmlBulder = new StringBuilder();
            tmlBulder.Append("ACTION=NEW_STOP_ORDER; TRANS_ID={0}; CLASSCODE= {1};");
            tmlBulder.Append("SECCODE = {2}; ACCOUNT = {3}; CLIENT_CODE = {4}; OPERATION = {5};");
            tmlBulder.Append("QUANTITY = {6}; PRICE = {7}; STOPPRICE = {8};");
            tmlBulder.Append("STOP_ORDER_KIND = TAKE_PROFIT_AND_STOP_LIMIT_ORDER; OFFSET = {9};");
            tmlBulder.Append("OFFSET_UNITS = PERCENTS; SPREAD = {10}; SPREAD_UNITS = PERCENTS;");
            tmlBulder.Append("MARKET_TAKE_PROFIT = NO; STOPPRICE2 = {11}; IS_ACTIVE_IN_TIME = YES;");
            tmlBulder.Append("ACTIVE_FROM_TIME = 100001; ACTIVE_TO_TIME = 194545;");
            tmlBulder.Append("MARKET_STOP_LIMIT = NO");

            string transaction = string.Format(tmlBulder.ToString(), TransId, order.Class, order.Code, order.Account, order.Client,
                order.Operation == "Купля" ? "B" : "S", order.Count, order.Price, order.StopPrice, "1.0", "0.03", order.StopLimitPrice );

            using (var stream = System.IO.File.AppendText(tri_filePath))
            {
                stream.WriteLine(transaction);
            }

            return "ok";
        }

        public void ReadResults()
        {
            IList<string> lines = System.IO.File.ReadAllLines(tro_filePath).ToList();

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                int count = unit.TransactionResultRepository.All<TransactionResult>().Count();

                for(int i = count; i < lines.Count; i++)
                {
                    TransactionResult trnRes = new TransactionResult()
                    {
                        Record = lines[i]
                    };

                    unit.TransactionResultRepository.Create(trnRes);
                }

                unit.Commit();
            }

        }

        public void PreocessResults()
        {
            string mask = @"TRANS_ID=(?<trans>\d+);STATUS=(?<status>\d);.*DESCRIPTION="".*"";( ORDER_NUMBER=(?<order>\d+);)?$";

            Regex rgx = new Regex(mask);

            using (UnitOfWork unit = new UnitOfWork((DbContext)new DataContext()))
            {
                IList<TransactionResult> list = unit.TransactionResultRepository.Query<TransactionResult>(t => t.Processed == false).ToList();

                foreach(TransactionResult trnRes in list)
                {
                    Match match = rgx.Match(trnRes.Record);

                    int transId = 0;
                    int status = 0;
                    decimal order = 0m;

                    if (match.Success)
                    {
                        transId = int.Parse(match.Groups["trans"].Value);
                        status = int.Parse(match.Groups["status"].Value);

                        Transaction trn = unit.TransactionRepository.Query<Transaction>(t => t.Id == transId).Single();
                        trn.Status = status;

                        if (match.Groups["order"].Success)
                        {
                            order = decimal.Parse(match.Groups["order"].Value);
                            trn.OrderNumber = order;
                        }
                    }

                    trnRes.Processed = true;

                }

                unit.Commit();
            }
        }
    }
}