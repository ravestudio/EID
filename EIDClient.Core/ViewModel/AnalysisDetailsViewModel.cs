using EIDClient.Core.Managers;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class AnalysisDetailsViewModel
    {
        private INavigationService _navigationService = null;
        private IChart _chart = null;

        public AnalysisDetailsViewModel(INavigationService navigationService, IChart chart)
        {
            this._navigationService = navigationService;
            this._chart = chart;


        }
    }
}
