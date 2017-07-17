using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class Settings: Entity<int>
    {
        public string Mode { get; set; }
        public DateTime TestDateTime { get; set; }

        private IDictionary<string, ModeType> _modeTypedic = new Dictionary<string, ModeType>();

        public Settings()
        {
            _modeTypedic.Add("Test", ModeType.Test);
            _modeTypedic.Add("Work", ModeType.Work);
            _modeTypedic.Add("Demo", ModeType.Demo);
        }

        public ModeType ModeType
        {
            get
            {
                return _modeTypedic[Mode];
            }

            set
            {
                this.Mode = _modeTypedic.Single(m => m.Value == value).Key;
            }
        }
    }
}
