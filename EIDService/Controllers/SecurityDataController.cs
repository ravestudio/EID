using EID.Library;
using EID.Library.ISS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EIDService.Controllers
{
    public class SecurityDataController : ApiController
    {
        // GET api/SecurityData
        public SecurityData Get(string Id)
        {
            SecurityData data = new SecurityData();

            MicexISSClient client = new MicexISSClient(new WebApiClient());

            ISSResponse resp = client.GetSecurityInfo(Id).Result;

            data.SecurityInfo = resp.SecurityInfo.Single(i => i.Code == Id);
            data.MarketData = resp.MarketData.Single(m => m.Code == Id);

            return data;
        }
    }
}
