using EIDClient.Core.Entities;
using EIDClient.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EIDClient.Core.ViewModel
{
    public class EmitentListViewModel : ViewModelBase
    {
        public RelayCommand<object> SelectEmitentCmd { get; set; }

        public ObservableCollection<Core.Entities.Emitent> EmitentList { get; set; }

        private INavigationService _navigationService = null;
        private IMainCommandBar _commandBar = null;

        public EmitentListViewModel(INavigationService navigationService, IMainCommandBar commandBar)
        {
            this._navigationService = navigationService;
            this._commandBar = commandBar;

            this.EmitentList = new ObservableCollection<Entities.Emitent>();

            Messenger.Default.Register<EmitentListLoadedMessage>(this, (msg) =>
            {
                this.EmitentList.Clear();
                foreach (Entities.Emitent emitent in msg.EmitentList)
                {
                    this.EmitentList.Add(emitent);
                }
            });

            SelectEmitentCmd = new RelayCommand<object>((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                Emitent SelectedEmitent = (Emitent)e.ClickedItem;

                this._navigationService.NavigateTo("EmitentDetails", SelectedEmitent);
            });

        }

        public void LoadEmitents()
        {
            Messenger.Default.Send<LoadEmitentListMessage>(new LoadEmitentListMessage());

        }
    }
}
