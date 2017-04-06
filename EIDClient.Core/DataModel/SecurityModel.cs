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


        public SecurityModel(SecurityRepository securityRepository)
        {
            this._securityRepository = securityRepository;

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
                    ISSResponse resp = await _issClient.GetSecurityInfo(security.Code);

                    security.SecurityInfo = resp.SecurityInfo.Single(i => i.Code == security.Code);
                    security.MarketData = resp.MarketData.Single(m => m.Code == security.Code);
                }
            });
        }
    }
}
