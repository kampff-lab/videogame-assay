using Bonsai.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Videogame
{
    public class UpdateMeshData : Sink<Tuple<float[], int[]>>
    {
        readonly VertexAttributeMappingCollection vertexAttributes = new VertexAttributeMappingCollection();

        public UpdateMeshData()
        {
            Usage = BufferUsageHint.DynamicDraw;
            DrawMode = PrimitiveType.Triangles;
        }

        [Description("The name of the mesh geometry to update.")]
        [Editor("Bonsai.Shaders.Configuration.Design.MeshConfigurationEditor, Bonsai.Shaders.Design", typeof(UITypeEditor))]
        public string MeshName { get; set; }

        [Description("Specifies the kind of primitives to render with the vertex buffer data.")]
        public PrimitiveType DrawMode { get; set; }

        [Description("Specifies the expected usage pattern of the vertex buffer.")]
        public BufferUsageHint Usage { get; set; }

        [Description("Specifies the attributes used to interpret the vertex buffer data.")]
        public VertexAttributeMappingCollection VertexAttributes
        {
            get { return vertexAttributes; }
        }

        public override IObservable<Tuple<float[], int[]>> Process(IObservable<Tuple<float[], int[]>> source)
        {
            return Observable.Defer(() =>
            {
                var name = MeshName;
                if (string.IsNullOrEmpty(name))
                {
                    throw new InvalidOperationException("A mesh name must be specified.");
                }


                Mesh mesh = null;
                return source.CombineEither(
                    ShaderManager.WindowSource.Do(window =>
                    {
                        window.Update(() =>
                        {
                            mesh = window.Meshes[name];
                            mesh.EnsureElementArray();
                            mesh.ElementArrayType = DrawElementsType.UnsignedInt;
                            VertexHelper.BindVertexAttributes(
                                mesh.VertexBuffer,
                                mesh.VertexArray,
                                BlittableValueType<float>.Stride,
                                vertexAttributes);
                        });
                    }),
                    (input, window) =>
                    {
                        window.Update(() =>
                        {
                            mesh.DrawMode = DrawMode;
                            VertexHelper.UpdateVertexBuffer(mesh.VertexBuffer, input.Item1, Usage);
                            mesh.VertexCount = VertexHelper.UpdateIndexBuffer(mesh.ElementArray, input.Item2, Usage);

                            for (int i=0; i < input.Item1.Length; i++)
                            {
                                Console.WriteLine(input.Item1[i].ToString());
                            }
                        });
                        return input;
                    });
            });
        }
    }
}
