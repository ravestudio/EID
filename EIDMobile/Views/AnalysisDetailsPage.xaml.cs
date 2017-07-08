using EID.Library;
using EIDClient.Core.ViewModel;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace EIDMobile.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AnalysisDetailsPage : Page
    {
        private object _prs = null;

        public AnalysisDetailsPage()
        {
            this.InitializeComponent();

            this.Loaded += AnalysisDetailsPage_Loaded;
        }

        private void AnalysisDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((AnalysisDetailsViewModel)this.DataContext).LoadData((IDictionary<int, IList<ICandle>>)_prs);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _prs = e.Parameter;
        }
    }
}
