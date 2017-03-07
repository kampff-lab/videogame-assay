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
    public class UndistortPoints : Transform<Tuple<Point2f, Point2f>, Tuple<Point2f, Point2f>>
    {
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

        public override IObservable<Tuple<Point2f, Point2f>> Process(IObservable<Tuple<Point2f, Point2f>> source)
        {
            return source.Select(input =>
            {
                var point1 = UndistortPoint(input.Item1, UndistortX, UndistortY);
                var point2 = UndistortPoint(input.Item2, UndistortX, UndistortY);
                return Tuple.Create(point1, point2);
            });
        }
    }
}
