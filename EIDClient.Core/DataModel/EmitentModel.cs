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
        }
    }
}
