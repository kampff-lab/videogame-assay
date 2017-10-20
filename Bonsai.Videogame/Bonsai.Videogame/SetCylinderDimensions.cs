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
    public class SetCylinderDimensions : Sink<Cylinder>
    {
        [Description("The Radius of the Cylinder.")]
        public float Radius { get; set; }

        [Description("The Length of the Cylinder.")]
        public float Length { get; set; }

        public override IObservable<Cylinder> Process(IObservable<Cylinder> source)
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
                ((Cylinder)input).Radius = Radius;
                ((Cylinder)input).Length = Length;
            });
        }
    }
}

