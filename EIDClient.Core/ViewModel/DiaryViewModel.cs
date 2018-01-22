using EIDClient.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class DiaryViewModel : ViewModelBase
    {
        private INavigationService _navigationService = null;

        public ObservableCollection<Core.Entities.DiaryNote> DiaryNoteItems { get; set; }

        public DiaryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            this.DiaryNoteItems = new ObservableCollection<Entities.DiaryNote>();

            Messenger.Default.Register<DiaryLoadedMessage>(this, (msg) =>
            {
                this.DiaryNoteItems.Clear();
                foreach (Entities.DiaryNote note in msg.DiaryNoteList)
                {
                    this.DiaryNoteItems.Add(note);
                }
            });
        }

        public void LoadData()
        {
            Messenger.Default.Send<LoadDiaryMessage>(new LoadDiaryMessage());

        }
    }
}
