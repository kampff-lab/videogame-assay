using Ode.Net.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class SetRadius : Sink<Sphere>
    {
        public float Radius { get; set; }

        public override IObservable<Sphere> Process(IObservable<Sphere> source)
        {
            return source.Do(input => input.Radius = Radius);
        }

        public IObservable<Geom> Process(IObservable<Geom> source)
        {
            return source.Do(input => ((Sphere)input).Radius = Radius);
        }
    }
}
