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
        }

        public Size ImageSize { get; set; }

        public float Depth { get; set; }

        public override IObservable<TriMeshData> Process(IObservable<Contour> source)
        {
            return source.Select(input =>
            {
                if (input == null) return null;

                var depth = Depth;
                var size = ImageSize;
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
                for (int i = 0, vcount = 0; i < points.Length; i++, vcount += 2)
                {
                    var x = 2.0f * (points[i].X / width - 0.5f);
                    var y = -2.0f * (points[i].Y / height - 0.5f);
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
                output.BuildSingle(vertices, indices);
                return output;
            });
        }
    }
}
