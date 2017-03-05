using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EIDClient.Core;
using EIDClient.Core.Repository;
using EIDClient.Core.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace EIDClient.ViewModel
{
    class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<EIDClient.Core.WebApiClient>();
            SimpleIoc.Default.Register<EmitentRepository>();
            SimpleIoc.Default.Register<FinancialRepository>();
            SimpleIoc.Default.Register<IMenu, Menu>();
            SimpleIoc.Default.Register<INavigationService>(GetNavigationService);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EmitentListViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public EmitentListViewModel EmitentListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmitentListViewModel>();
            }
        }

        private static INavigationService GetNavigationService()
        {

            var navigationService = new EIDClient.Core.ViewModel.NavigationService();

            navigationService.Configure("EmitentList", typeof(Views.EmitentListPage));
            navigationService.Configure("Main", typeof(Views.MainPage));

            return navigationService;
        }

        public ViewModelLocator()
        {

        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {

        }
    }
}
