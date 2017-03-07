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

        [Category("Undistort")]
        [TypeConverter(typeof(UnidimensionalArrayConverter))]
        [Description("Linear coefficients used to undistort the X position.")]
        public double[] UndistortX { get; set; }

        [Category("Undistort")]
        [TypeConverter(typeof(UnidimensionalArrayConverter))]
        [Description("Linear coefficients used to undistort the Y position.")]
        public double[] UndistortY { get; set; }

        Point2f UndistortPoint(Point2f point, double[] xw, double[] yw)
        {
            var x = xw[0] + xw[1] * point.X + xw[2] * point.Y + xw[3] * point.X * point.X + xw[4] * point.Y * point.Y;
            var y = yw[0] + yw[1] * point.X + yw[2] * point.Y + yw[3] * point.X * point.X + yw[4] * point.Y * point.Y;
            return new Point2f((float)x, (float)-y);
        }

        public override IObservable<Tuple<Point2f, Point2f>> Process(IObservable<IplImage> source)
        {
            return Observable.Defer(() =>
            {
                var red = default(IplImage);
                var blue = default(IplImage);
                Point2f redpoint, bluepoint;
                return source.Select(input =>
                {
                    if (red == null) red = new IplImage(input.Size, input.Depth, 1);
                    if (blue == null) blue = new IplImage(input.Size, input.Depth, 1);
                    CV.Split(input, red, null, blue, null);
                    CV.Threshold(red, red, ThresholdRed, 255, ThresholdTypes.Binary);
                    CV.Threshold(blue, blue, ThresholdBlue, 255, ThresholdTypes.Binary);
                    var redled = ConnectedComponent.FromImage(red).Centroid;
                    var blueled = ConnectedComponent.FromImage(blue).Centroid;
                    redpoint = UndistortPoint(redled, UndistortX, UndistortY);
                    bluepoint = UndistortPoint(blueled, UndistortX, UndistortY);
                    return Tuple.Create(redpoint, bluepoint);
                });
            });
        }
    }
}
