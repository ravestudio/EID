﻿using EID.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Robot
{
    public interface IStrategy
    {
        IList<int> Need();

        StrategyDecision GetDecision(IDictionary<int, IList<ICandle>> data, string name, string currentPos, DateTime CurrentDt);
    }
}