﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Entities
{
    public class MarketData
    {
        public string Code { get; set; }

        public decimal LCURRENTPRICE { get; set; }
        public decimal OPENPERIODPRICE { get; set; }
    }
}
