using Ode.Net.Collision;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class TouchDisplacement : Transform<Tuple<Geom, Geom>, Geom>
    {
        public float Magnitude { get; set; }

        public override IObservable<Geom> Process(IObservable<Tuple<Geom, Geom>> source)
        {
            return source.Select(input =>
            {
                var geom = input.Item1;
                var displacement = input.Item2.Position - geom.Position;
                var length = Math.Sqrt(displacement.X * displacement.X + displacement.Y * displacement.Y);
                displacement /= length;
                geom.Position += Magnitude * displacement;
                return geom;
            });
        }
    }
}
