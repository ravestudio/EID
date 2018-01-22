using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class NotePosition : Entity<int>
    {

        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public int Count { get; set; }

        public virtual DiaryNote DiaryNote { get; set; }

        public virtual Deal Deal { get; set; }
    }
}
