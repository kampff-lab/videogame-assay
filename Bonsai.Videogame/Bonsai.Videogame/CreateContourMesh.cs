using Bonsai.Vision;
using Ode.Net.Collision;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class CreateContourMesh : Transform<Contour, Tuple<float[], int[]>>
    {
        public CreateContourMesh()
        {
            Depth = 1;
            FlipContour = FlipMode.Vertical;
        }

        public Size ImageSize { get; set; }

        public float Depth { get; set; }

        public FlipMode? FlipContour { get; set; }

        public IObservable<Tuple<float[], int[]>> Process(IObservable<Contours> source)
        {
            return Observable.Defer(() =>
            {
                var vertices = new List<float>();
                var indices = new List<int>();
                return source.Select(input =>
                {
                    if (input == null) return null;

                    var vcount = 0;
                    var depth = Depth;
                    var size = input.ImageSize;
                    var flipContour = FlipContour;
                    var width = (float)size.Width;
                    var height = (float)size.Height;
                    var contour = input.FirstContour;
                    while (contour != null)
                    {
                        var points = new Point[contour.Count + 1];
                        contour.CopyTo(points);
                        points[contour.Count] = points[0];

                        var vi = 0;
                        var ii = 0;
                        var scaleX = flipContour.HasValue && flipContour.Value != FlipMode.Vertical ? -2.0f : 2.0f;
                        var scaleY = flipContour.HasValue && flipContour.Value != FlipMode.Horizontal ? -2.0f : 2.0f;
                        for (int i = 0; i < points.Length; i++, vcount += 2)
                        {
                            var x = scaleX * (points[i].X / width - 0.5f);
                            var y = scaleY * (points[i].Y / height - 0.5f);
                            vertices.Add(x);
                            vertices.Add(y);
                            vertices.Add(-depth);
                            vertices.Add(x);
                            vertices.Add(y);
                            vertices.Add(+depth);
                            if (i > 0)
                            {
                                indices.Add(vcount + 0); // t1v0
                                indices.Add(vcount + 0); // t1v0
                                indices.Add(vcount + 1); // t1v1
                                indices.Add(vcount - 1); // t0v1
                            }
                            if (i < points.Length - 1)
                            {
                                indices.Add(vcount + 1); // t0v1
                                indices.Add(vcount + 0); // t0v0
                            }
                        }

                        contour = contour.HNext;
                    }

                    var output = Tuple.Create(vertices.ToArray(), indices.ToArray());
                    vertices.Clear();
                    indices.Clear();
                    return output;
                });
            });
        }

        public override IObservable<Tuple<float[], int[]>> Process(IObservable<Contour> source)
        {
            return source.Select(input =>
            {
                if (input == null) return null;

                var depth = Depth;
                var size = ImageSize;
                var flipContour = FlipContour;
                var width = (float)size.Width;
                var height = (float)size.Height;
                var points = new Point[input.Count + 1];
                input.CopyTo(points);
                points[input.Count] = points[0];

                var vi = 0;
                var ii = 0;
                var vertices = new float[points.Length * 6];
                var indices = new int[(points.Length - 1) * 6];
                var scaleX = flipContour.HasValue && flipContour.Value != FlipMode.Vertical ? -2.0f : 2.0f;
                var scaleY = flipContour.HasValue && flipContour.Value != FlipMode.Horizontal ? -2.0f : 2.0f;
                for (int i = 0, vcount = 0; i < points.Length; i++, vcount += 2)
                {
                    var x = scaleX * (points[i].X / width - 0.5f);
                    var y = scaleY * (points[i].Y / height - 0.5f);
                    vertices[vi++] = x;
                    vertices[vi++] = y;
                    vertices[vi++] = -depth;
                    vertices[vi++] = x;
                    vertices[vi++] = y;
                    vertices[vi++] = +depth;
                    if (i > 0)
                    {
                        indices[ii++] = vcount + 0; // t1v0
                        indices[ii++] = vcount + 0; // t1v0
                        indices[ii++] = vcount + 1; // t1v1
                        indices[ii++] = vcount - 1; // t0v1
                    }
                    if (i < points.Length - 1)
                    {
                        indices[ii++] = vcount + 1; // t0v1
                        indices[ii++] = vcount + 0; // t0v0
                    }
                }

                return Tuple.Create(vertices, indices);
            });
        }
    }
}
