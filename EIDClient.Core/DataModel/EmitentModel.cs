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
    public class EmitentModel
    {
        private EmitentRepository _emitentRepository = null;
        private FinancialRepository _financialRepository = null;

        public EmitentModel(EmitentRepository emitentRepository, FinancialRepository financialRepository)
        {
            this._emitentRepository = emitentRepository;
            this._financialRepository = financialRepository;

            Messenger.Default.Register<LoadEmitentListMessage>(this, async (msg) =>
            {
                IEnumerable<Emitent> emitentList = await this._emitentRepository.GetAll();

                Messenger.Default.Send<EmitentListLoadedMessage>(new EmitentListLoadedMessage()
                {
                    EmitentList = emitentList
                });
            });


            Messenger.Default.Register<LoadFinancialListMessage>(this, async (msg) =>
            {
                IEnumerable<Financial> financialList = await this._financialRepository.GetByEmitentId(msg.EmitentId);

                Messenger.Default.Send<FinancialListLoadedMessage>(new FinancialListLoadedMessage()
                {
                    FinancialList = financialList
                });
            });

            Messenger.Default.Register<SaveEmitentMessage>(this, (msg) =>
            {
                Task<string> task = null;

                if (msg.Emitent.Id != 0)
                {
                    task = this._emitentRepository.Update(msg.Emitent);
                }

            });

            Messenger.Default.Register<SaveFinancialMessage>(this, (msg) =>
            {
                Task<string> task = null;

                if (msg.Financial.Id != 0)
                {
                    task = this._financialRepository.Update(msg.Financial);
                }

                if (msg.Financial.Id == 0)
                {
                    task = this._financialRepository.Create(msg.Financial);
                }

                task.ContinueWith(t =>
                {
                    Messenger.Default.Send<SaveFinancialResultMeassage>(new SaveFinancialResultMeassage()
                    {
                        Result = t.Result
                    });
                });
            });
        }
    }
}
