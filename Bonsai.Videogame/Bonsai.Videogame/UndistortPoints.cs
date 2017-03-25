using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCV.Net;
using System.Runtime.InteropServices;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.Videogame
{
    [Description("Undistorts the observed point coordinates using the specified intrinsic camera matrix.")]
    public class UndistortPoints : Transform<Mat, Mat>
    {
        Point2d focalLength;
        Point2d principalPoint;
        Point3d radialDistortion;
        Point2d tangentialDistortion;
        Mat intrinsics;
        Mat distortion;

        public UndistortPoints()
        {
            UpdateIntrinsics();
            UpdateDistortion();
        }

        [Description("The focal length of the camera, expressed in pixel units.")]
        public Point2d FocalLength
        {
            get { return focalLength; }
            set
            {
                focalLength = value;
                UpdateIntrinsics();
            }
        }

        [Description("The principal point of the camera, usually at the image center.")]
        public Point2d PrincipalPoint
        {
            get { return principalPoint; }
            set
            {
                principalPoint = value;
                UpdateIntrinsics();
            }
        }

        [Description("The radial distortion coefficients.")]
        public Point3d RadialDistortion
        {
            get { return radialDistortion; }
            set
            {
                radialDistortion = value;
                UpdateDistortion();
            }
        }

        [Description("The tangential distortion coefficients.")]
        public Point2d TangentialDistortion
        {
            get { return tangentialDistortion; }
            set
            {
                tangentialDistortion = value;
                UpdateDistortion();
            }
        }

        void UpdateIntrinsics()
        {
            intrinsics = Mat.FromArray(new double[,]
            {
                {focalLength.X, 0, principalPoint.X},
                {0, focalLength.Y, principalPoint.Y},
                {0, 0, 1}
            });
        }

        void UpdateDistortion()
        {
            distortion = Mat.FromArray(new[]
            {
                radialDistortion.X,
                radialDistortion.Y,
                tangentialDistortion.X,
                tangentialDistortion.Y,
                radialDistortion.Z
            });
        }

        void UndistortPoint(
            ref Point2f point,
            Mat cameraMatrix,
            double[] cameraMatrixInverse,
            ref Point3d radialDistortion,
            ref Point2d tangentialDistortion,
            out Point2f result)
        {
            var k1 = radialDistortion.X;
            var k2 = radialDistortion.Y;
            var p1 = tangentialDistortion.X;
            var p2 = tangentialDistortion.Y;
            var k3 = radialDistortion.Z;
            var k4 = 0;
            var k5 = 0;
            var k6 = 0;

            var u0 = cameraMatrix.GetReal(0, 2);
            var v0 = cameraMatrix.GetReal(1, 2);
            var fx = cameraMatrix.GetReal(0, 0);
            var fy = cameraMatrix.GetReal(1, 1);

            var i = point.Y;
            var j = point.X;
            var _x = i * cameraMatrixInverse[1] + cameraMatrixInverse[2] + j * cameraMatrixInverse[0];
            var _y = i * cameraMatrixInverse[4] + cameraMatrixInverse[5] + j * cameraMatrixInverse[3];
            var _w = i * cameraMatrixInverse[7] + cameraMatrixInverse[8] + j * cameraMatrixInverse[6];
            double w = 1.0 / _w, x = _x * w, y = _y * w;
            double x2 = x * x, y2 = y * y;
            double r2 = x2 + y2, _2xy = 2 * x * y;
            double kr = (1 + ((k3 * r2 + k2) * r2 + k1) * r2) / (1 + ((k6 * r2 + k5) * r2 + k4) * r2);
            double u = fx * (x * kr + p1 * _2xy + p2 * (r2 + 2 * x2)) + u0;
            double v = fy * (y * kr + p1 * (r2 + 2 * y2) + p2 * _2xy) + v0;
            result.X = (float)u;
            result.Y = (float)v;
        }

        public IObservable<Point2f> Process(IObservable<Point2f> source)
        {
            return Process(source.Select(x => new[] { x })).Select(xs => xs[0]);
        }

        public IObservable<Tuple<Point2f, Point2f>> Process(IObservable<Tuple<Point2f, Point2f>> source)
        {
            return Process(source.Select(input => new[] { input.Item1, input.Item2 })).Select(xs => Tuple.Create(xs[0], xs[1]));
        }

        public IObservable<Point2f[]> Process(IObservable<Point2f[]> source)
        {
            return Observable.Defer(() =>
            {
                Mat cameraMatrix = null;
                double[] inverseCameraMatrix = null;
                Mat distortionCoefficients = null;
                return source.Select(input =>
                {
                    if (cameraMatrix != intrinsics || distortionCoefficients != distortion)
                    {
                        cameraMatrix = intrinsics;
                        distortionCoefficients = distortion;
                        inverseCameraMatrix = new double[cameraMatrix.Rows * cameraMatrix.Cols];
                        using (var inverseHeader = Mat.CreateMatHeader(inverseCameraMatrix, cameraMatrix.Rows, cameraMatrix.Cols, Depth.F64, 1))
                        {
                            CV.Invert(cameraMatrix, inverseHeader, InversionMethod.LU);
                        }
                    }

                    var output = new Point2f[input.Length];
                    for (int i = 0; i < output.Length; i++)
                    {
                        UndistortPoint(
                            ref input[i],
                            cameraMatrix,
                            inverseCameraMatrix,
                            ref radialDistortion,
                            ref tangentialDistortion,
                            out output[i]);
                    }
                    return output;
                });
            });
        }

        public override IObservable<Mat> Process(IObservable<Mat> source)
        {
            return Observable.Defer(() =>
            {
                Mat cameraMatrix = null;
                Mat distortionCoefficients = null;
                return source.Select(input =>
                {
                    if (cameraMatrix != intrinsics || distortionCoefficients != distortion)
                    {
                        cameraMatrix = intrinsics;
                        distortionCoefficients = distortion;
                    }

                    var output = new Mat(input.Size, input.Depth, input.Channels);
                    CV.UndistortPoints(input, output, cameraMatrix, distortionCoefficients);
                    return output;
                });
            });
        }
    }
}
