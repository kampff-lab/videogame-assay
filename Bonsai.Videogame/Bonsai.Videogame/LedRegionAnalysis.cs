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
        public bool KeepLatest { get; set; }

        public override IObservable<ConnectedComponent> Process(IObservable<Tuple<Point2f, Point2f>> source)
        {
            return Observable.Defer(() =>
            {
                var latest = default(ConnectedComponent);
                return source.Select(input =>
                {
                    var redled = input.Item1;
                    var blueled = input.Item2;
                    if ((float.IsNaN(redled.X) || float.IsNaN(redled.Y)) && latest != null)
                    {
                        return latest;
                    }

                    var result = new ConnectedComponent();
                    var direction = blueled - redled;
                    var r2 = direction.X * direction.X + direction.Y * direction.Y;
                    var r = Math.Sqrt(r2);
                    result.Centroid = redled;
                    result.Orientation = Math.Atan2(direction.Y, direction.X);
                    result.MajorAxisLength = result.MinorAxisLength = 2 * r;
                    result.Area = 2 * Math.PI * r2;
                    if (KeepLatest) latest = result;
                    else latest = null;
                    return result;
                });
            });
        }
    }
}
