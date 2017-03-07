using Bonsai.Vision;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class ColorLedTracking : Transform<IplImage, Tuple<Point2f, Point2f>>
    {
        [Category("Threshold")]
        [Description("Threshold used to segment the red LED.")]
        public int ThresholdRed { get; set; }

        [Category("Threshold")]
        [Description("Threshold used to segment the blue LED.")]
        public int ThresholdBlue { get; set; }



        public override IObservable<Tuple<Point2f, Point2f>> Process(IObservable<IplImage> source)
        {
            return Observable.Defer(() =>
            {
                var red = default(IplImage);
                var blue = default(IplImage);
                return source.Select(input =>
                {
                    if (red == null) red = new IplImage(input.Size, input.Depth, 1);
                    if (blue == null) blue = new IplImage(input.Size, input.Depth, 1);
                    CV.Split(input, red, null, blue, null);
                    CV.Threshold(red, red, ThresholdRed, 255, ThresholdTypes.Binary);
                    CV.Threshold(blue, blue, ThresholdBlue, 255, ThresholdTypes.Binary);
                    var redled = ConnectedComponent.FromImage(red).Centroid;
                    var blueled = ConnectedComponent.FromImage(blue).Centroid;
                    return Tuple.Create(redled, blueled);
                });
            });
        }
    }
}
