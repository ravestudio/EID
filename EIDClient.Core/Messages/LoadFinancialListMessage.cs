using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.Messages
{
    /// <summary>
    /// Загрузить список отчетов
    /// </summary>
    public class LoadFinancialListMessage
    {
        public int EmitentId { get; set; }
    }
}
