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
    public class SetCapsuleDimensions : Sink<Capsule>
    {
        [Description("The Radius of the Capsule.")]
        public float Radius { get; set; }

        [Description("The Length of the straight part of the Capsule (for total Length add this to the Radius).")]
        public float Length { get; set; }

        public override IObservable<Capsule> Process(IObservable<Capsule> source)
        {
            return source.Do(input =>
            {
                input.Radius = Radius;
                input.Length = Length;
            });
        }

        public IObservable<Geom> Process(IObservable<Geom> source)
        {
            return source.Do(input =>
            {
                ((Capsule)input).Radius = Radius;
                ((Capsule)input).Length = Length;
            });
        }
    }
}
