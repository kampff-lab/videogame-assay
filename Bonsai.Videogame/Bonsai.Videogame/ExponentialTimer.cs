using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Bonsai.Videogame
{
    public class ExponentialTimer : Source<DateTimeOffset>
    {
        public double Mean { get; set; }

        public double Offset { get; set; }

        static double Exponential(Random generator, double lambda)
        {
            var u = generator.NextDouble();
            return Math.Log(1 - u) * -lambda;
        }

        TimeSpan NextInterval(Random generator)
        {
            return TimeSpan.FromMilliseconds(Exponential(generator, Mean) + Offset);
        }

        public override IObservable<DateTimeOffset> Generate()
        {
            return Observable.Create<DateTimeOffset>(observer =>
            {
                var random = new Random();
                return HighResolutionScheduler.Default.Schedule(NextInterval(random), self =>
                {
                    observer.OnNext(HighResolutionScheduler.Now);
                    self(NextInterval(random));
                });
            });
        }
    }
}