using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ViewModel
{
    public class EmitentListViewModel : ViewModelBase
    {
        public ObservableCollection<Core.Entities.Emitent> EmitentList { get; set; }

        public EmitentListViewModel()
        {
            this.EmitentList = new ObservableCollection<Entities.Emitent>();

            EIDClient.Core.Repository.EmitentRepository repo = new Core.Repository.EmitentRepository(new Core.WebApiClient());
            repo.GetAll().ContinueWith(t =>
            {
                foreach(var emitent in t.Result)
                {
                    this.EmitentList.Add(emitent);
                }
            });
        }
    }
}
