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
    public class CreateContourTriMesh : Transform<Contour, TriMeshData>
    {
        public CreateContourTriMesh()
        {
            Depth = 1;
            FlipContour = FlipMode.Vertical;
        }

        public Size ImageSize { get; set; }

        public float Depth { get; set; }

        public FlipMode? FlipContour { get; set; }

        public override IObservable<TriMeshData> Process(IObservable<Contour> source)
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
                var output = new TriMeshData();
                var vertices = new float[points.Length * 6];
                var indices = new int[(points.Length - 1) * 6];
                var scaleX = flipContour.HasValue && flipContour.Value != FlipMode.Vertical ? -2.0f : 2.0f;
                var scaleY = flipContour.HasValue && flipContour.Value != FlipMode.Horizontal ? -2.0f : 2.0f;
                for (int i = 0, vcount = 0; i < points.Length; i++, vcount += 2)
                {
                    var x = scaleX * (points[i].X / width - 0.5f);
                    var y = scaleY * (points[i].Y / height - 0.5f);
                    vertices[vi++] = x; // v0.x
                    vertices[vi++] = y; // v0.y
                    vertices[vi++] = -depth; // v0.z
                    vertices[vi++] = x; // v1.x
                    vertices[vi++] = y; // v1.y
                    vertices[vi++] = +depth; // v1.z

                    // clock-wise
                    if (i > 0)
                    {
                        indices[ii++] = vcount + 1; // p1v1
                        indices[ii++] = vcount + 1; // p1v1
                        indices[ii++] = vcount + 0; // p1v0
                        indices[ii++] = vcount - 2; // p0v0
                    }
                    if (i < points.Length - 1)
                    {
                        indices[ii++] = vcount + 0; // p0v0
                        indices[ii++] = vcount + 1; // p0v1
                    }

                    // counter clock-wise
                    //if (i > 0)
                    //{
                    //    indices[ii++] = vcount + 0; // p1v0
                    //    indices[ii++] = vcount + 0; // p1v0
                    //    indices[ii++] = vcount + 1; // p1v1
                    //    indices[ii++] = vcount - 1; // p0v1
                    //}
                    //if (i < points.Length - 1)
                    //{
                    //    indices[ii++] = vcount + 1; // p0v1
                    //    indices[ii++] = vcount + 0; // p0v0
                    //}
                }
                output.BuildSingle(vertices, indices);
                return output;
            });
        }
    }
}
