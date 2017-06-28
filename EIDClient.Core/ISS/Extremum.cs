using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.ISS
{
    public class ExtItem
    {
        public string ext { get; set; }
        public decimal val { get; set; }
    }
    public class Extremum : List<ExtItem>
    {
        public Extremum(IList<decimal> data, int period, int period2)
        {

            List<ExtItem> temp = new List<ExtItem>();

            foreach (decimal v in data.Skip(data.Count - period2))
            {
                temp.Add(new ExtItem() { val = v });
            }

            List<ExtItem> newdata = new List<ExtItem>();

            int x = 0;

            while(!Validate(temp))
            {
                x++;

                int c = temp.Count() / period;

                for (int n = 0; n <= c; n++)
                {
                    var loc = temp.Skip(n * period).Take(period).ToList();

                    if (loc.Count() > 1)
                    {
                        ExtItem loc_min = loc.First(e => e.val == loc.Min(i => i.val));
                        ExtItem loc_max = loc.First(e => e.val == loc.Max(i => i.val));

                        int index_min = loc.IndexOf(loc_min);
                        int index_max = loc.IndexOf(loc_max);

                        if (index_max >= index_min)
                        {
                            newdata.Add(new ExtItem() { val = loc_min.val, ext = "L" });
                            newdata.Add(new ExtItem() { val = loc_max.val, ext = "H" });
                        }
                        else
                        {
                            newdata.Add(new ExtItem() { val = loc_max.val, ext = "H" });
                            newdata.Add(new ExtItem() { val = loc_min.val, ext = "L" });
                        }
                    }
                }

                temp.Clear();
                temp.AddRange(newdata);
                newdata.Clear();

                if (x == 5)
                {
                    Compress(temp);
                }
            }

            this.AddRange(temp);

        }

        public bool Validate(IList<ExtItem> data)
        {
            bool res = true;

            bool curr_h = data[0].ext == "H";

            for (int i = 1; i < data.Count(); i++)
            {
                if (curr_h && (data[i].val > data[i - 1].val))
                {
                    res = false;
                }

                if (!curr_h && (data[i].val < data[i - 1].val))
                {
                    res = false;
                }

                curr_h = !curr_h;
            }

            return res;
        }

        public void Compress(IList<ExtItem> data)
        {
            for (int i = 1; i < data.Count(); i++)
            {
                if (data[i].ext == data[i-1].ext)
                {
                    if (data[i].ext == "H")
                    {
                        data[i-1].val = Math.Max(data[i].val, data[i-1].val);
                    }

                    if (data[i].ext == "L")
                    {
                        data[i - 1].val = Math.Min(data[i].val, data[i - 1].val);
                    }

                    data.RemoveAt(i);

                }
            }
        }
    }
}
