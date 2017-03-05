using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class NavigationService : INavigationService
    {
        private Dictionary<string, Type> pageList;

        public NavigationService()
        {
            this.pageList = new Dictionary<string, Type>();
        }

        public string CurrentPageKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Configure(string key, Type type)
        {
            this.pageList.Add(key, type);
        }

        public void GoBack()
        {
            GetFrame().GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        private Frame GetFrame()
        {
            Frame frame = null;

            SplitView split = EIDClient.Core.Managers.ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");

            if (split != null)
            {
                frame = split.Content as Frame;
            }
            return frame;
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            Frame navigationFrame = null;

            navigationFrame = GetFrame();

            navigationFrame.Navigate(this.pageList[pageKey], parameter);
        }
    }
}
