using Ode.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class RandomPosition : Combinator<Random, Vector3>
    {
        [TypeConverter(typeof(NumericAggregateConverter))]
        public Vector3? Minimum { get; set; }

        [TypeConverter(typeof(NumericAggregateConverter))]
        public Vector3? Maximum { get; set; }

        public override IObservable<Vector3> Process(IObservable<Random> source)
        {
            return source.Select(random =>
            {
                var minimum = Minimum.GetValueOrDefault(Vector3.Zero);
                var maximum = Maximum.GetValueOrDefault(Vector3.One);
                return new Vector3(
                    minimum.X == maximum.X ? minimum.X : random.NextDouble() * (maximum.X - minimum.X) + minimum.X,
                    minimum.Y == maximum.Y ? minimum.Y : random.NextDouble() * (maximum.Y - minimum.Y) + minimum.Y,
                    minimum.Z == maximum.Z ? minimum.Z : random.NextDouble() * (maximum.Z - minimum.Z) + minimum.Z);
            });
        }
    }
}
