using EIDService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class TransactionModel
    {
        string filePath = @"C:\transactions\transaction.tri";

        public string CreateOrder(Order order, int TransId)
        {
            string mask = "ACCOUNT={0}; CLIENT_CODE={1}; TYPE={2}; TRANS_ID={3}; CLASSCODE = {4}; SECCODE = {5}; ACTION = {6}; OPERATION = {7}; PRICE = {8}; QUANTITY = {9}; ";
            string transaction = string.Format(mask, order.Account, order.Client, "L", TransId, order.Class, order.Code, "NEW_ORDER", order.Operation == "Купля"? "B": "S", order.Price, order.Count);

            using (var stream = System.IO.File.AppendText(filePath))
            {
                stream.WriteLine(transaction);
            }

            return "ok";
        }
    }
}