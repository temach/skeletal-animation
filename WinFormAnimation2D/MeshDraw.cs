using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace WinFormAnimation2D
{

    struct Vbo
    {
        public int VertexBufferId;
        public int ColorBufferId;
        public int TexCoordBufferId;
        public int NormalBufferId;
        public int TangentBufferId;
        public int ElementBufferId;
        public int NumIndices;
        public int BitangentBufferId;
        public bool Is32BitIndices; 
    }

    /// <summary>
    /// Mesh rendering using VBOs.
    /// 
    /// Based on http://www.opentk.com/files/T08_VBO.cs
    /// </summary>
    class MeshDraw
    {
        public Mesh _mesh;
        public Vbo _vbo;

        /// <summary>
        /// Uploads the data to the GPU.
        /// </summary>
        public MeshDraw(Mesh mesh)
        {
            Debug.Assert(mesh != null);

            _mesh = mesh;
            // Upload(out _vbo);
        }


        /// <summary>
        /// Draws the mesh geometry given the current pipeline state. 
        /// 
        /// The pipeline is restored afterwards.
        /// </summary>
        /// <param name="flags">Rendering mode</param>
        public void Render()
        {
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            Debug.Assert(_vbo.VertexBufferId != 0);
            Debug.Assert(_vbo.ElementBufferId != 0);
            // normals
            if (Properties.Settings.Default.RenderNormals)
            {
                if (_vbo.NormalBufferId != 0)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.NormalBufferId);
                    GL.NormalPointer(NormalPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
                    GL.EnableClientState(ArrayCap.NormalArray);
                }
            }
            // vertex colors
            if (Properties.Settings.Default.RenderVertexColors)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.ColorBufferId);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);
                GL.EnableClientState(ArrayCap.ColorArray);
            }
            // UV coordinates
            if (Properties.Settings.Default.RenderTexture)
            {
                if (_vbo.TexCoordBufferId != 0)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.TexCoordBufferId);
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, IntPtr.Zero);
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                }
            }
            // vertex position
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.VertexBufferId);
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);
            GL.EnableClientState(ArrayCap.VertexArray);
            // primitives
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vbo.ElementBufferId);
            GL.DrawElements(BeginMode.Triangles, _vbo.NumIndices /* actually, count(indices) */,
                DrawElementsType.UnsignedInt, IntPtr.Zero);
            // Restore the state
            GL.PopClientAttrib();
        }


        /// <summary>
        /// Currently only called during construction, this method uploads the input mesh (
        /// the RenderMesh instance is bound to) to a VBO.
        /// </summary>
        /// <param name="vboToFill"></param>
        private void Upload(out Vbo vboToFill)
        {
            vboToFill = new Vbo();     
      
            UploadVertices(out vboToFill.VertexBufferId);
            if (_mesh.HasNormals)
            {
                UploadNormals(out vboToFill.NormalBufferId);
            }

            if (_mesh.HasVertexColors(0))
            {
                UploadColors(out vboToFill.ColorBufferId);
            }

            if (_mesh.HasTextureCoords(0))
            {
                UploadTextureCoords(out vboToFill.TexCoordBufferId);
            }

            if (_mesh.HasTangentBasis)
            {
                UploadTangentsAndBitangents(out vboToFill.TangentBufferId, out vboToFill.BitangentBufferId);
            }

            UploadPrimitives(out vboToFill.ElementBufferId, out vboToFill.NumIndices);
            // TODO: upload bone weights
        }


        /// <summary>
        /// Generates and populates an Gl vertex array buffer given 3D vectors as source data
        /// </summary>
        private void NewServerBufferWithFloats(out int outGlBufferId, List<Vector3D> dataBuffer) 
        {
            GL.GenBuffers(1, out outGlBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, outGlBufferId);
            int sizeof_vec3d = 12; // X,Y,Z = 3 floats, 4 bytes each
            var byteCount = dataBuffer.Count * sizeof_vec3d;
            var temp = new float[byteCount];
            var n = 0;
            foreach(var v in dataBuffer)
            {
                temp[n++] = v.X;
                temp[n++] = v.Y;
                temp[n++] = v.Z;
            }
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)byteCount, temp, BufferUsageHint.StaticDraw);
            VerifyBufferSize(byteCount);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }


        /// <summary>
        /// Verifies that the size of the currently bound vertex array buffer matches
        /// a given parameter and throws if it doesn't.
        /// </summary>
        private void VerifyBufferSize(int byteCount)
        {
            int bufferSize;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (byteCount != bufferSize)
            {
                throw new Exception("Vertex data array not uploaded correctly - buffer size does not match upload size");
            }
        }


        /// <summary>
        /// Uploads vertex indices to a newly generated Gl vertex array
        /// </summary>
        private void UploadPrimitives(out int elementBufferId, out int indicesCount)
        {
            Debug.Assert(_mesh.HasTextureCoords(0));

            GL.GenBuffers(1, out elementBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferId);

            var faces = _mesh.Faces;

            // TODO account for other primitives than triangles
            var triCount = 0;
            int byteCount;
            foreach(var face in faces)
            {
                Debug.Assert(face.IndexCount == 3);
                ++triCount;
            }
            var intCount = triCount * 3;
            var temp = new uint[intCount];
            byteCount = intCount * sizeof(uint);
            var n = 0;
            foreach (var idx in faces.Where(face => face.IndexCount == 3).SelectMany(face => face.Indices))
            {
                temp[n++] = (uint)idx;
            }

            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)byteCount
                , temp, BufferUsageHint.StaticDraw);

            int bufferSize;
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (byteCount != bufferSize)
            {
                throw new Exception("Index data array not uploaded correctly - buffer size does not match upload size");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            indicesCount = triCount * 3;
        }


        /// <summary>
        /// Uploads UV coordinates to a newly generated Gl vertex array.
        /// </summary>
        private void UploadTextureCoords(out int texCoordBufferId)
        {
            Debug.Assert(_mesh.HasTextureCoords(0));

            GL.GenBuffers(1, out texCoordBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordBufferId);

            var uvs = _mesh.TextureCoordinateChannels[0];
            var floatCount = uvs.Count * 2;
            var temp = new float[floatCount];
            var n = 0;
            foreach (var uv in uvs)
            {
                temp[n++] = uv.X;
                temp[n++] = uv.Y;
            }

            var byteCount = floatCount*sizeof (float);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(byteCount), temp, BufferUsageHint.StaticDraw);
            VerifyBufferSize(byteCount);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }


        /// <summary>
        /// Uploads vertex positions to a newly generated Gl vertex array.
        /// </summary>
        private void UploadVertices(out int verticesBufferId)
        {
            NewServerBufferWithFloats(out verticesBufferId, _mesh.Vertices);
        }


        /// <summary>
        /// Uploads normal vectors to a newly generated Gl vertex array.
        /// </summary>
        private void UploadNormals(out int normalBufferId)
        {
            Debug.Assert(_mesh.HasNormals);
            NewServerBufferWithFloats(out normalBufferId, _mesh.Normals);
        }


        /// <summary>
        /// Uploads tangents and bitangents to newly generated Gl vertex arrays.
        /// </summary>
        private void UploadTangentsAndBitangents(out int tangentBufferId, out int bitangentBufferId)
        {
            Debug.Assert(_mesh.HasTangentBasis);

            var tangents = _mesh.Tangents;
            NewServerBufferWithFloats(out tangentBufferId, tangents);

            var bitangents = _mesh.BiTangents;
            Debug.Assert(bitangents.Count == tangents.Count);

            NewServerBufferWithFloats(out bitangentBufferId, bitangents);
        }


        /// <summary>
        /// Uploads vertex colors to a newly generated Gl vertex array.
        /// </summary>
        /// <param name="colorBufferId"></param>
        private void UploadColors(out int colorBufferId)
        {
            Debug.Assert(_mesh.HasVertexColors(0));

            GL.GenBuffers(1, out colorBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferId);

            var colors = _mesh.VertexColorChannels[0];
            // convert to 32Bit RGBA
            var byteCount = colors.Count*4;
            var byteColors = new byte[byteCount];
            var n = 0;
            foreach(var c in colors)
            {
                byteColors[n++] = (byte)(c.R * 255);
                byteColors[n++] = (byte)(c.G * 255);
                byteColors[n++] = (byte)(c.B * 255);
                byteColors[n++] = (byte)(c.A * 255);
            }

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(byteCount), byteColors, BufferUsageHint.StaticDraw);
            VerifyBufferSize(byteCount);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }       
    }
}
