using EIDClient.Core.Managers;
using EIDClient.Core.Repository;
using EIDClient.Core.ViewModel;
using EID.Library;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIDClient.Core.DataModel;
using EIDClient.Core.Entities;

namespace EIDMobile.ViewModel
{
    class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<WebApiClient>();

            SimpleIoc.Default.Register<CandleRepository>();
            SimpleIoc.Default.Register<TradeSessionRepository>();
            SimpleIoc.Default.Register<OrderRepository>();
            SimpleIoc.Default.Register<PositionRepository>();
            SimpleIoc.Default.Register<DealRepository>();



            SimpleIoc.Default.Register<IMenu, Menu>();
            SimpleIoc.Default.Register<IChart, Chart>();
            SimpleIoc.Default.Register<IMainCommandBar, MainCommandBar>();
            SimpleIoc.Default.Register<INavigationService>(GetNavigationService);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AnalysisViewModel>();
            SimpleIoc.Default.Register<AnalysisDetailsViewModel>();
            SimpleIoc.Default.Register<ITradeMode, AnalystMode>();

            SimpleIoc.Default.Register<TradeModel>(()=>
            {
                Mode modeProperties = ServiceLocator.Current.GetInstance<ModeRepository>().GetSingle().Result;

                return new TradeModel(
                    ServiceLocator.Current.GetInstance<TradeSessionRepository>(),
                    ServiceLocator.Current.GetInstance<CandleRepository>(),
                    ServiceLocator.Current.GetInstance<OrderRepository>(),
                    ServiceLocator.Current.GetInstance<PositionRepository>(),
                    ServiceLocator.Current.GetInstance<DealRepository>(),
                    ServiceLocator.Current.GetInstance<WebApiClient>(),
                    ServiceLocator.Current.GetInstance<ITradeMode>(),
                    modeProperties,
                    TokenModel.Instance.AnalystToken()
                    );
            },true);
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AnalysisViewModel AnalysisViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalysisViewModel>();
            }
        }

        public AnalysisDetailsViewModel AnalysisDetailsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalysisDetailsViewModel>();
            }
        }

        private static INavigationService GetNavigationService()
        {

            var navigationService = new EIDClient.Core.ViewModel.NavigationService();

            navigationService.Configure("AnalysisDetails", typeof(Views.AnalysisDetailsPage));
            navigationService.Configure("Analysis", typeof(Views.AnalysisPage));
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
