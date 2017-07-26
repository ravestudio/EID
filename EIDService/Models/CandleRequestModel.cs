using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIDService.Models
{
    public class CandleRequestModel
    {
        public string security { get; set; }
        public DateTime? from { get; set; }
        public DateTime? till { get; set; }
        public string interval { get; set; }
    }
}