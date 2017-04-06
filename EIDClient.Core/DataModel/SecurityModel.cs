using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
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
        private SecurityDataRepository _securityDataRepository = null;

        public SecurityModel(SecurityRepository securityRepository, SecurityDataRepository securityDataRepository)
        {
            this._securityRepository = securityRepository;
            this._securityDataRepository = securityDataRepository;

            Messenger.Default.Register<LoadSecurityMessage>(this, async (msg) =>
            {
                Security security = await this._securityRepository.GetById(msg.Id);

                Messenger.Default.Send<SecurityLoadedMessage>(new SecurityLoadedMessage()
                {
                    Security = security
                });
            });

            Messenger.Default.Register<LoadSecurityListMessage>(this, async (msg) =>
            {
                IEnumerable<Security> securityList = await this._securityRepository.GetAll();

                Messenger.Default.Send<SecurityListLoadedMessage>(new SecurityListLoadedMessage()
                {
                    SecurityList = securityList
                });

            });

            Messenger.Default.Register<IISGetSecurityInfo>(this, async (msg) =>
            {
                foreach (Security security in msg.SecurityList)
                {
                    SecurityData data = await _securityDataRepository.GetById(security.Code);

                    security.SecurityInfo = data.SecurityInfo;
                    security.MarketData = data.MarketData;
                }
            });
        }
    }
}
