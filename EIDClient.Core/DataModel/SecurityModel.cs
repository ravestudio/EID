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
    public class SecurityModel
    {
        private SecurityRepository _securityRepository = null;

        public SecurityModel(SecurityRepository securityRepository)
        {
            this._securityRepository = securityRepository;

            Messenger.Default.Register<LoadSecurityListMessage>(this, async (msg) =>
            {
                IEnumerable<Security> securityList = await this._securityRepository.GetAll();

                Messenger.Default.Send<SecurityListLoadedMessage>(new SecurityListLoadedMessage()
                {
                    SecurityList = securityList
                });

            });
        }
    }
}
