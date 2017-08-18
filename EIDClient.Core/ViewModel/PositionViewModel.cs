using EIDClient.Core.Entities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class PositionViewModel : ViewModelBase
    {
        private Position _position = null;

        public string Code
        {
            get
            {
                return _position.Code;
            }
        }

        public int Available
        {
            get
            {
                return _position.Available;
            }
        }

        public int CurrentBalance
        {
            get
            {
                return _position.CurrentBalance;
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return _position.PurchasePrice;
            }
        }

        public PositionViewModel(Position pos)
        {
            _position = pos;
        }

        public void Update(Position pos)
        {
            _position = pos;

            this.RaisePropertyChanged("CurrentBalance");
            this.RaisePropertyChanged("Available");
            this.RaisePropertyChanged("PurchasePrice");
        }
    }
}
