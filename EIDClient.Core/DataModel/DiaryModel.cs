using EIDClient.Core.Entities;
using EIDClient.Core.Messages;
using EIDClient.Core.Repository;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.DataModel
{
    public class DiaryModel
    {
        DiaryNoteRepository _diaryNoteRepository = null;

        public DiaryModel(DiaryNoteRepository diaryNoteRepository)
        {
            _diaryNoteRepository = diaryNoteRepository;

            Messenger.Default.Register<LoadDiaryMessage>(this, async (msg) =>
            {
                IEnumerable<DiaryNote> diaryNoteList = await this._diaryNoteRepository.GetAll();

                Messenger.Default.Send<DiaryLoadedMessage>(new DiaryLoadedMessage()
                {
                    DiaryNoteList = diaryNoteList
                });
            });
        }
    }
}
