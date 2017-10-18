using Ode.Net.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class CreateTriMeshData : Transform<Tuple<float[], int[]>, TriMeshData>
    {
        public override IObservable<TriMeshData> Process(IObservable<Tuple<float[], int[]>> source)
        {
            return source.Select(input =>
            {
                if (input == null) return null;
                var output = new TriMeshData();
                output.BuildSingle(input.Item1, input.Item2);
                return output;
            });
        }
    }
}
