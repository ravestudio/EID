using EID.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDService.Common.Entities
{
    public class DiaryNote : Entity<int>
    {
        public DiaryNote()
        {
            NotePositions = new Collection<NotePosition>();
        }

        public override string Key
        {
            get { return null; }
            protected set => base.Key = value;
        }

        public string Code { get; set; }

        public NoteType NoteType { get; set; }

        public DateTime? Open { get; set; }
        public DateTime? Close { get; set; }

        public decimal? OpenPrice { get; set; }
        public decimal? ClosePrice { get; set; }

        public decimal? OpenValue { get; set; }
        public decimal? CloseValue { get; set; }

        public int Count { get; set; }

        public virtual ICollection<NotePosition> NotePositions { get; set; }

    }
}
