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
using EIDClient.Core.DataModel;
using EIDClient.Core.Managers;
using EID.Library;
using EIDClient.Core.Entities;

namespace EIDClient.ViewModel
{
    class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<WebApiClient>();

            SimpleIoc.Default.Register<EmitentRepository>();
            SimpleIoc.Default.Register<FinancialRepository>();
            SimpleIoc.Default.Register<SecurityRepository>();
            SimpleIoc.Default.Register<TradeSessionRepository>();
            SimpleIoc.Default.Register<CandleRepository>();
            SimpleIoc.Default.Register<SecurityDataRepository>();
            SimpleIoc.Default.Register<SettingsRepository>();
            SimpleIoc.Default.Register<ModeRepository>();
            SimpleIoc.Default.Register<OrderRepository>();
            SimpleIoc.Default.Register<PositionRepository>();
            SimpleIoc.Default.Register<DealRepository>();

            SimpleIoc.Default.Register<IMenu, Menu>();
            SimpleIoc.Default.Register<IChart, Chart>();
            SimpleIoc.Default.Register<IMainCommandBar, MainCommandBar>();
            SimpleIoc.Default.Register<INavigationService>(GetNavigationService);
            //SimpleIoc.Default.Register<ITradeMode, WorkMode>();
            SimpleIoc.Default.Register<ITradeMode>(GetTradeMode);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EmitentListViewModel>();
            SimpleIoc.Default.Register<SecurityListViewModel>();
            SimpleIoc.Default.Register<EmitentDetailsViewModel>();
            SimpleIoc.Default.Register<SecurityDetailsViewModel>();
            SimpleIoc.Default.Register<FinancialEditViewModel>();
            SimpleIoc.Default.Register<RobotControlViewModel>();

            SimpleIoc.Default.Register<EmitentModel>(true);
            SimpleIoc.Default.Register<SecurityModel>(true);

            SimpleIoc.Default.Register<TradeModel>(() =>
            {
                Mode modeProperties = ServiceLocator.Current.GetInstance<ModeRepository>().GetSingle().Result;

                return new TradeModel(
                    ServiceLocator.Current.GetInstance<TradeSessionRepository>(),
                    ServiceLocator.Current.GetInstance<CandleRepository>(),
                    ServiceLocator.Current.GetInstance<OrderRepository>(),
                    ServiceLocator.Current.GetInstance<PositionRepository>(),
                    ServiceLocator.Current.GetInstance<DealRepository>(),
                    ServiceLocator.Current.GetInstance<ITradeMode>(),
                    modeProperties,
                    TokenModel.Instance.RobotToken()
                    );
            }, true);
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

        public SecurityListViewModel SecurityListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SecurityListViewModel>();
            }
        }

        public EmitentDetailsViewModel EmitentDetailsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmitentDetailsViewModel>();
            }
        }

        public SecurityDetailsViewModel SecurityDetailsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SecurityDetailsViewModel>();
            }
        }

        public FinancialEditViewModel FinancialEditViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FinancialEditViewModel>();
            }
        }

        public RobotControlViewModel RobotControlViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RobotControlViewModel>();
            }
        }

        private static ITradeMode GetTradeMode()
        {
            ITradeMode result = null;

            SettingsRepository repo = ServiceLocator.Current.GetInstance<SettingsRepository>();
            WebApiClient apiClient = ServiceLocator.Current.GetInstance<WebApiClient>();

            var list = repo.GetAll().Result;

            Settings settings = list.First();

            if (settings.Mode == "Test")
            {
                result = new TestMode(apiClient, repo);
            }

            if (settings.Mode == "Work")
            {
                result = new WorkMode();
            }

            if (settings.Mode == "Demo")
            {
                result = new DemoMode(apiClient);
            }

            return result;
        }

        private static INavigationService GetNavigationService()
        {

            var navigationService = new EIDClient.Core.ViewModel.NavigationService();

            navigationService.Configure("EmitentList", typeof(Views.EmitentListPage));
            navigationService.Configure("SecurityList", typeof(Views.SecurityListPage));
            navigationService.Configure("EmitentDetails", typeof(Views.EmitentDetailsPage));
            navigationService.Configure("SecurityDetails", typeof(Views.SecurityDetailsPage));
            navigationService.Configure("FinancialEdit", typeof(Views.FinancialEditPage));
            navigationService.Configure("Robot", typeof(Views.RobotControlPage));
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
