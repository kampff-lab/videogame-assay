using Ode.Net;
using Ode.Net.Collision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class SetBoxLengths : Sink<Box>
    {
        [TypeConverter(typeof(NumericAggregateConverter))]
        [Description("The lengths of the X, Y, Z sides of the box.")]
        public Vector3 Lengths { get; set; }

        public override IObservable<Box> Process(IObservable<Box> source)
        {
            return source.Do(input =>
            {
                input.Lengths = Lengths;
            });
        }

        public IObservable<Geom> Process(IObservable<Geom> source)
        {
            return source.Do(input =>
            {
                ((Box)input).Lengths = Lengths;
            });
        }
    }
}
