using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public class Robot
    {
        private IDictionary<int, IList<decimal>> _frames = null;
        private IDictionary<int, Stack<decimal>> _next = null;
        private Strategy _strategy = null;
        private System.Threading.CancellationTokenSource _stop = null;

        private IDictionary<string, Action<string>> actions = null;
        private IDictionary<string, string> positions = null;
        
        public Robot(Strategy strategy)
        {
            _strategy = strategy;

            _frames = new Dictionary<int, IList<decimal>>();
            _next = new Dictionary<int, Stack<decimal>>();

            _next.Add(5, new Stack<decimal>(new decimal[] {118.2m, 118.5m, 119m, 120, 110, 100 }));

            actions = new Dictionary<string, Action<string>>();
            positions = new Dictionary<string, string>();


            IList<decimal> data = new List<decimal>();

            data.Add(100);
            data.Add(105);
            data.Add(103);
            data.Add(106);
            data.Add(112);
            data.Add(115);
            data.Add(117);

            _frames.Add(60, data);

            data = new List<decimal>();
            data.Add(117.1m);
            data.Add(117.5m);
            data.Add(117.7m);
            data.Add(118);

            _frames.Add(5, data);

            actions.Add("open long", (sec) =>
                {
                    positions[sec] = "long";
                });


            positions["qwe"] = "free";

        }

        public void Run()
        {

            var temp = _frames.Where(f => _strategy.Need().Contains(f.Key));

            IDictionary<int, IList<decimal>> prepared = new Dictionary<int, IList<decimal>>();
            foreach (var pair in temp)
            {
                prepared.Add(pair.Key, pair.Value);
            }


            _stop = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = _stop.Token;

            Task t = Task.Run(() =>
            {

                while (true)
                {
                    string sec = "qwe";

                    string d = _strategy.GetDecision(prepared, sec, positions[sec]);

                    if (!string.IsNullOrEmpty(d))
                    {
                        actions[d].Invoke(sec);
                    }

                    Console.WriteLine("Action: {0}", d);

                    Task.Delay(1000).Wait();

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }, token);

            Task upd = Task.Run(() =>
                {
                    while (true)
                    {
                        Task.Delay(500).Wait();

                        foreach (int d in prepared.Keys)
                        {
                            prepared[d].Add(119);
                        }

                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }, token);

        }

        public void Stop()
        {
            _stop.Cancel();
        }


    }
}
