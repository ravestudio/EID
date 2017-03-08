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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EIDClient.Views.Controls
{
    public sealed partial class FinancialLineItem : UserControl
    {
        public FinancialItem Item { get; set; }

        public FinancialLineItem()
        {
            this.InitializeComponent();
        }

        public void AddItem(FinancialItem item)
        {
            this.Item = item;
        }
    }
}
