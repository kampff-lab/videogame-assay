using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class ShuffleNames : Transform<Random, IEnumerable<string>>
    {
        readonly Collection<Weighted<string>> names = new Collection<Weighted<string>>();

        public int TrialCount { get; set; }

        [TypeConverter(typeof(ExpandableCollectionConverter))]
        public Collection<Weighted<string>> Names
        {
            get { return names; }
        }

        public override IObservable<IEnumerable<string>> Process(IObservable<Random> source)
        {
            return source.Select(input =>
            {
                var trialCount = TrialCount;
                var trials = new List<string>(trialCount);
                foreach (var name in names)
                {
                    var count = name.Weight * trialCount;
                    for (int i = 0; i < count; i++)
                    {
                        trials.Add(name.Value);
                    }
                }

                for (int i = 0; i < trials.Count; i++)
                {
                    var index = input.Next(i, trials.Count);
                    var temp = trials[i];
                    trials[i] = trials[index];
                    trials[index] = temp;
                }

                return trials;
            });
        }


    }
}
