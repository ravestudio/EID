﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EID.Library
{
    public enum OrderStateEnum : byte
    {
        IsActive = 1,
        Executed = 2,
        Canceled = 3
    }
}
