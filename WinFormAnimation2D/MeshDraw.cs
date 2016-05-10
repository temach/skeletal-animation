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
using OpenTK.Graphics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormAnimation2D
{

    struct Vbo
    {
        public int VertexBufferId;
        public int ColorBufferId;
        public int TexCoordBufferId;
        public int NormalBufferId;
        public int ElementBufferId;
        public int NumIndices;
    }

    /// <summary>
    /// Mesh rendering using VBOs.
    /// 
    /// Based on http://www.opentk.com/files/T08_VBO.cs
    /// </summary>
    class MeshDraw
    {
        public Mesh _mesh;
        public Dictionary<int,Matrix4x4> _vertex_id2matrix = new Dictionary<int,Matrix4x4>();
        public Vbo _vbo;
        public Material _material;
        public int _apply_material_id;

        /// <summary>
        /// Uploads the data to the GPU.
        /// </summary>
        public MeshDraw(Mesh mesh, IList<Material> materials)
        {
            Debug.Assert(mesh != null);
            _mesh = mesh;
            _material = materials[mesh.MaterialIndex];
            Upload(out _vbo);
            _apply_material_id = CompileMaterialDisplayList();
        }

        /// <summary>
        /// Render mesh from GPU memory.
        /// The pipeline is restored afterwards.
        /// </summary>
        public void RenderVBO()
        {
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            Debug.Assert(_vbo.VertexBufferId != 0);
            Debug.Assert(_vbo.ElementBufferId != 0);
            // material
            if (Properties.Settings.Default.OpenGLMaterial)
            {
                GL.CallList(_apply_material_id);
            }
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
                DrawElementsType.UnsignedShort, IntPtr.Zero);
            // Restore the state
            GL.PopClientAttrib();
        }

        bool _buffer_mapped = false;
        public void BeginModifyVertexData(out IntPtr data, out int qty_vertices)
        {
            Debug.Assert(_buffer_mapped == false, "Forgot to unmap the buffer with GL.UnmapBuffer()");
            _buffer_mapped = true;
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.VertexBufferId);
            data = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.ReadWrite);
            // note: number of floats in "data" = (qty_vertices * 3)
            qty_vertices = _mesh.VertexCount;
        }
        public void EndModifyVertexData()
        {
            bool data_upload_ok = GL.UnmapBuffer(BufferTarget.ArrayBuffer);
            if (! data_upload_ok)
            {
                // data store contents have become corrupt while the data store was mapped
                // This can occur for system-specific reasons that affect the availability 
                // of graphics memory, such as screen mode changes. 
                // Then GL_FALSE is returned and data contents are undefined
                // An application must detect this rare condition and reinitialize the data store. 
                // We will not reinitialise the store, but simply bail out.
                throw new Exception("OpenGL driver has failed.");
            }
            _buffer_mapped = false;
        }

        public void BeginModifyNormalData(out IntPtr data, out int qty_normals)
        {
            Debug.Assert(_buffer_mapped == false, "Forgot to unmap the buffer with GL.UnmapBuffer()");
            _buffer_mapped = true;
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo.NormalBufferId);
            data = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.ReadWrite);
            // note: number of floats in "data" = (qty_normals * 3)
            qty_normals = _mesh.Normals.Count;
        }
        public void EndModifyNormalData()
        {
            bool data_upload_ok = GL.UnmapBuffer(BufferTarget.ArrayBuffer);
            if (! data_upload_ok)
            {
                // data store contents have become corrupt while the data store was mapped
                // This can occur for system-specific reasons that affect the availability 
                // of graphics memory, such as screen mode changes. 
                // Then GL_FALSE is returned and data contents are undefined
                // An application must detect this rare condition and reinitialize the data store. 
                // We will not reinitialise the store, but simply bail out.
                throw new Exception("OpenGL driver has failed.");
            }
            _buffer_mapped = false;
        }

        public int CompileMaterialDisplayList()
        {
            int id = GL.GenLists(1);
            GL.NewList(id, ListMode.Compile);
            ApplyMaterial();
            GL.EndList();
            return id;
        }

        public OpenTK.Graphics.Color4 Assimp2OpenTK(Assimp.Color4D input)
        {
            return new Color4(input.R, input.G, input.B, input.A);
        }

        double AlphaSuppressionThreshold = 0.01;
        private void ApplyMaterial()
        {
            var hasColors = _mesh != null && _mesh.HasVertexColors(0);
            if (hasColors)
            {
                GL.Enable(EnableCap.ColorMaterial);
                GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            }
            else
            {
                GL.Disable(EnableCap.ColorMaterial);
            }
            // note: keep semantics of hasAlpha consistent with IsAlphaMaterial()
            var hasAlpha = false;
            var hasTexture = false;

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Normalize);

            var alpha = 1.0f;
            if (_material.HasOpacity)
            {
                alpha = _material.Opacity;
                // ignore zero alpha channel
                if (alpha < AlphaSuppressionThreshold) 
                {
                    alpha = 1.0f;
                }
            }

            var color = new Color4(.8f, .8f, .8f, 1.0f);
            if (_material.HasColorDiffuse)
            {
                color = Assimp2OpenTK(_material.ColorDiffuse);
                if (color.A < AlphaSuppressionThreshold) // s.a.
                {
                    color.A = 1.0f;
                }
            }
            color.A *= alpha;
            hasAlpha = hasAlpha || color.A < 1.0f;

            // if the material has a texture but the diffuse color texture is all black,
            // then heuristically assume that this is an import/export flaw and substitute
            // white.
            if (hasTexture && color.R < 1e-3f && color.G < 1e-3f && color.B < 1e-3f)
            {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, Color4.White);
            }
            else
            {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, color);
            }

            color = new Color4(0, 0, 0, 1.0f);
            if (_material.HasColorSpecular)
            {
                color = Assimp2OpenTK(_material.ColorSpecular);              
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, color);

            color = new Color4(.2f, .2f, .2f, 1.0f);
            if (_material.HasColorAmbient)
            {
                color = Assimp2OpenTK(_material.ColorAmbient);              
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, color);

            color = new Color4(0, 0, 0, 1.0f);
            if (_material.HasColorEmissive)
            {
                color = Assimp2OpenTK(_material.ColorEmissive);              
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, color);

            float shininess = 1;
            float strength = 1;
            if (_material.HasShininess)
            {
                shininess = _material.Shininess;

            }
            // todo: I don't even remember how shininess strength was supposed to be handled in assimp
            if (_material.HasShininessStrength)
            {
                strength = _material.ShininessStrength;
            }

            var exp = shininess*strength;
            if (exp >= 128.0f) // 128 is the maximum exponent as per the Gl spec
            {
                exp = 128.0f;
            }

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, exp);

            if (hasAlpha)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.DepthMask(false);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
                GL.DepthMask(true);
            }
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
            //if (_mesh.HasVertexColors(0))
            //{
            //    UploadColors(out vboToFill.ColorBufferId);
            //}
            //if (_mesh.HasTextureCoords(0))
            //{
            //    UploadTextureCoords(out vboToFill.TexCoordBufferId);
            //}
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
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)byteCount, temp, BufferUsageHint.StreamDraw);
            VerifyArrayBufferSize(byteCount);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }


        /// <summary>
        /// Verifies that the size of the currently bound vertex array buffer matches
        /// a given parameter and throws if it doesn't.
        /// </summary>
        private void VerifyArrayBufferSize(int byteCount)
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
            //Debug.Assert(_mesh.HasTextureCoords(0));

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

            // since we are 64 bit compile target
            var temp = new ushort[intCount];
            byteCount = intCount * sizeof(ushort);
            var n = 0;
            foreach (var idx in faces.Where(face => face.IndexCount == 3).SelectMany(face => face.Indices))
            {
                Debug.Assert(idx <= 0xffff);
                temp[n++] = (ushort)idx;
            }
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)byteCount, temp, BufferUsageHint.StaticDraw);


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
            VerifyArrayBufferSize(byteCount);
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
            VerifyArrayBufferSize(byteCount);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }       
    }
}
