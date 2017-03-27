using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class TimestampEvent : Combinator<TimestampedEvent>, INamedElement
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public override IObservable<TimestampedEvent> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(input => new TimestampedEvent(HighResolutionScheduler.Now, Name, Value));
        }

        public IObservable<TimestampedEvent> Process<TSource>(IObservable<Timestamped<TSource>> source)
        {
            return source.Select(input => new TimestampedEvent(input.Timestamp, Name, Value));
        }
    }
}
