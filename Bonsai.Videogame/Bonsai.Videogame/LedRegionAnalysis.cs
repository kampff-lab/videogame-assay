using Bonsai.Vision;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class LedRegionAnalysis : Transform<Tuple<Point2f, Point2f>, ConnectedComponent>
    {
        public override IObservable<ConnectedComponent> Process(IObservable<Tuple<Point2f, Point2f>> source)
        {
            return source.Select(input =>
            {
                var result = new ConnectedComponent();
                var redled = input.Item1;
                var blueled = input.Item2;
                var direction = blueled - redled;
                var r2 = direction.X * direction.X + direction.Y * direction.Y;
                var r = Math.Sqrt(r2);
                result.Centroid = redled;
                result.Orientation = Math.Atan2(direction.Y, direction.X);
                result.MajorAxisLength = result.MinorAxisLength = 2 * r;
                result.Area = 2 * Math.PI * r2;
                return result;
            });
        }
    }
}
