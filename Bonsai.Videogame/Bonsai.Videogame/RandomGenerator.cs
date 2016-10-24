using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class RandomGenerator : Source<Random>
    {
        public int? Seed { get; set; }

        public override IObservable<Random> Generate()
        {
            return Observable.Defer(() =>
            {
                var seed = Seed;
                var random = seed.HasValue ? new Random(seed.Value) : new Random();
                return Observable.Return(random);
            });
        }
    }
}
