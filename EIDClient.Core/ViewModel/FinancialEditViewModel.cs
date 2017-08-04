using EIDClient.Core.Entities;
using EIDClient.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class FinancialEditViewModel : ViewModelBase
    {
        public Financial _financial = null;

        public IList<FinancialItem> FinancialItems { get; set; }

        public int Year { get; set; }

        public IList<KeyValuePair<int, string>> PeriodTypes { get; set; }

        public RelayCommand SaveCmd { get; set; }

        private KeyValuePair<int, string> _selectedPeriod;
        public KeyValuePair<int, string> SelectedPeriod { get
            {
                return _selectedPeriod;
            }
            set
            {
                _selectedPeriod = value;
            }
        }

        private IMainCommandBar _commandBar = null;

        public FinancialEditViewModel(IMainCommandBar commandBar)
        {
            this._commandBar = commandBar;
            this.FinancialItems = new List<FinancialItem>();

            this.PeriodTypes = new List<KeyValuePair<int, string>>();
            this.PeriodTypes.Add(new KeyValuePair<int, string>(1, "Q1"));
            this.PeriodTypes.Add(new KeyValuePair<int, string>(2, "Q2"));
            this.PeriodTypes.Add(new KeyValuePair<int, string>(3, "Q3"));
            this.PeriodTypes.Add(new KeyValuePair<int, string>(4, "Q4"));
            this.PeriodTypes.Add(new KeyValuePair<int, string>(7, "Y"));

            this.SaveCmd = new RelayCommand(() =>
            {
                _financial.Year = this.Year;
                _financial.Period = this.SelectedPeriod.Key;
                _financial.Revenue = decimal.Parse(this.FinancialItems.Single(i => i.Name == "Revenue").Value);
                _financial.OperatingIncome = decimal.Parse(this.FinancialItems.Single(i => i.Name == "OperatingIncome").Value);
                _financial.NetIncome = decimal.Parse(this.FinancialItems.Single(i => i.Name == "NetIncome").Value);
                _financial.CurrentAssets = decimal.Parse(this.FinancialItems.Single(i => i.Name == "CurrentAssets").Value);
                _financial.FixedAssets = decimal.Parse(this.FinancialItems.Single(i => i.Name == "FixedAssets").Value);
                _financial.CurrentLiabilities = decimal.Parse(this.FinancialItems.Single(i => i.Name == "CurrentLiabilities").Value);
                _financial.LongTermLiabilities = decimal.Parse(this.FinancialItems.Single(i => i.Name == "LongTermLiabilities").Value);

                _financial.FlowOperatingActivities = decimal.Parse(this.FinancialItems.Single(i => i.Name == "FlowOperatingActivities").Value);
                _financial.FlowInvestingActivities = decimal.Parse(this.FinancialItems.Single(i => i.Name == "FlowInvestingActivities").Value);
                _financial.FlowFinancingActivities = decimal.Parse(this.FinancialItems.Single(i => i.Name == "FlowFinancingActivities").Value);

                _financial.StockIssuance = decimal.Parse(this.FinancialItems.Single(i => i.Name == "StockIssuance").Value);
                _financial.DividendsPaid = decimal.Parse(this.FinancialItems.Single(i => i.Name == "DividendsPaid").Value);
                _financial.EarningsPerShare = decimal.Parse(this.FinancialItems.Single(i => i.Name == "EarningsPerShare").Value);
                _financial.Price = decimal.Parse(this.FinancialItems.Single(i => i.Name == "Price").Value);

                Messenger.Default.Send<SaveFinancialMessage>(new SaveFinancialMessage() { Financial= _financial });
            });

            
        }

        public void Set(Financial financial)
        {
            this._financial = financial;

            if (financial.Period != 0)
            {
                this.SelectedPeriod = this.PeriodTypes.Single(p => p.Key == financial.Period);
            }

            this.Year = financial.Year;

            AddItem("Revenue", _financial.Revenue);
            AddItem("OperatingIncome", _financial.OperatingIncome);
            AddItem("NetIncome", _financial.NetIncome);

            AddItem("CurrentAssets", _financial.CurrentAssets);
            AddItem("FixedAssets", _financial.FixedAssets);

            AddItem("CurrentLiabilities", _financial.CurrentLiabilities);
            AddItem("LongTermLiabilities", _financial.LongTermLiabilities);

            AddItem("FlowOperatingActivities", _financial.FlowOperatingActivities);
            AddItem("FlowInvestingActivities", _financial.FlowInvestingActivities);
            AddItem("FlowFinancingActivities", _financial.FlowFinancingActivities);
            

            AddItem("StockIssuance", _financial.StockIssuance);
            AddItem("DividendsPaid", _financial.DividendsPaid);
            AddItem("EarningsPerShare", _financial.EarningsPerShare);

            AddItem("Price", _financial.Price);

            this._commandBar.Clear();
            this._commandBar.AddCommandButton(new Windows.UI.Xaml.Controls.AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Save),
                Command = SaveCmd 

            });
        }

        private void AddItem(string name, decimal value)
        {
            this.FinancialItems.Add(new Core.ViewModel.FinancialItem() { Name = name, Value = value.ToString() });

        }
    }
}
