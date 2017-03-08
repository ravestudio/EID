using EIDClient.Core.Entities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class FinancialEditViewModel : ViewModelBase
    {
        public Financial _financial = null;

        public IList<FinancialItem> FinancialItems { get; set; }

        public decimal Revenue { get; set; }

        public decimal OperatingExpenses { get; set; }

        public decimal Expenses { get; set; }

        public FinancialEditViewModel()
        {
            this.FinancialItems = new List<FinancialItem>();
        }

        public void Set(Financial financial)
        {
            this._financial = financial;

            AddItem("Revenue", _financial.Revenue);
            AddItem("OperatingExpenses", _financial.OperatingExpenses);
            AddItem("Expenses", _financial.Expenses);
            AddItem("OperatingIncome", _financial.OperatingIncome);
            AddItem("NetIncome", _financial.NetIncome);

            AddItem("CurrentAssets", _financial.CurrentAssets);
            AddItem("FixedAssets", _financial.FixedAssets);
            AddItem("Equity", _financial.Equity);
            AddItem("CurrentLiabilities", _financial.CurrentLiabilities);
            AddItem("LongTermLiabilities", _financial.LongTermLiabilities);
        }

        private void AddItem(string name, decimal value)
        {
            this.FinancialItems.Add(new Core.ViewModel.FinancialItem() { Name = name, Value = value });

        }
    }
}
