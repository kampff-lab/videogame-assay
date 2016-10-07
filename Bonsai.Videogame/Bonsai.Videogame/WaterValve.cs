using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bonsai.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.Videogame
{
    [TypeVisualizer("Bonsai.BehaviorControl.Design.WaterValveVisualizer, Bonsai.BehaviorControl.Design")]
    public class WaterValve : Combinator<bool>
    {
        public double MillilitersPerSecond { get; set; }

        public double MicrolitersPerDelivery { get; set; }

        public override IObservable<bool> Process<TSource>(IObservable<TSource> source)
        {
            return source.SelectMany(
                Observable.Repeat(true, 1)
                          .Merge(Observable.Repeat(false, 1)
                          .Delay(TimeSpan.FromMilliseconds(MicrolitersPerDelivery / MillilitersPerSecond), HighResolutionScheduler.Default)));
        }
    }
}