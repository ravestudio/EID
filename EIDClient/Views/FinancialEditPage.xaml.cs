using EIDClient.Core.Entities;
using EIDClient.Core.ViewModel;
using EIDClient.Views.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace EIDClient.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FinancialEditPage : Page
    {
        private Dictionary<FinancialItemGroup, Action<FinancialLineItem>> addItemActions = null;

        public FinancialEditPage()
        {
            this.InitializeComponent();

            this.Loaded += FinancialEditPage_Loaded;

            addItemActions = new Dictionary<FinancialItemGroup, Action<FinancialLineItem>>();

            addItemActions.Add(FinancialItemGroup.IncomeStatement, (item) =>
            {
                this.IncomeItems.Children.Add(item);
            });

            addItemActions.Add(FinancialItemGroup.BalanceItems, (item) =>
            {
                this.BalanceItems.Children.Add(item);
            });

            addItemActions.Add(FinancialItemGroup.CashFlowItems, (item) =>
            {
                this.CashFlowItems.Children.Add(item);
            });

        }

        private void FinancialEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((FinancialEditViewModel)this.DataContext).LoadData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FinancialEditViewModel vm = (FinancialEditViewModel)DataContext;

            Financial financial = null;

            if (e.Parameter != null)
            {
                financial = (Financial)e.Parameter;        
            }

            if (financial == null)
            {
                financial = new Financial();
            }

            vm.Set(financial);

            foreach (FinancialItem item in vm.FinancialItems)
            {
                addItem(item);
            }
        }

        void addItem(FinancialItem item)
        {
            Controls.FinancialLineItem line_item = new Controls.FinancialLineItem();
            line_item.AddItem(item);

            addItemActions[item.Group].Invoke(line_item);
            //this.FinancialItems.Children.Add(line_item);

        }
    }
}
