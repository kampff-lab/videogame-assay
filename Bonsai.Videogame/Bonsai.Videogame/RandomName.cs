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
    public class RandomName : Transform<Random, string>
    {
        readonly Collection<string> names = new Collection<string>();

        [TypeConverter(typeof(ExpandableCollectionConverter))]
        public Collection<string> Names
        {
            get { return names; }
        }

        public override IObservable<string> Process(IObservable<Random> source)
        {
            return source.Select(input => names[input.Next(names.Count)]);
        }
    }
}
