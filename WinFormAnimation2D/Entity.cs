using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;        // for MemoryStream
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using Quaternion = Assimp.Quaternion;

// TODO: this is piece of text taken from function that no longer exists.
// but it is trying to describe my architechture. But it is getting old and useless. 
// HERE GOES:
// setup specific to this scene what other objects do not know about. (wireframe, texture,material,scale...)
// GetRenderSettings gets the currently active globale settings for the program.
// it looks at them and chooses the best settings for itself taking into 
// consideration the globals. So it tries to get the scene looking ideal while 
// still respecting global user settings (like: draw in wireframe, or without texture)
// that are currently turned on in the application. This settings are 
// activated back in the DrawToOpenGL class. After their activation we call the 
// render method on this particular object that pushes vertices (not settings) to OpenGL.
// This object should have some render code.

namespace WinFormAnimation2D
{

    /// <summary>
    /// Represents the currently loaded object.
    /// We will have lots of these.
    /// </summary>
    class Entity
    {
        public ActionState _action;
        public BoneNode _armature;
        public Node _node;
        public SceneWrapper _scene;
        public Geometry _extra_geometry;
        public DrawConfig _draw_conf;
        public TransformState _transform;
        public Dictionary<int,MeshDraw> _mesh_id2mesh_draw = new Dictionary<int,MeshDraw>();
        public Matrix4 Matrix
        {
            get { return _transform._matrix; }
            set { _transform._matrix = value; }
        }

        public Dictionary<Vector3D,Matrix4x4> _vertex2matrix = new Dictionary<Vector3D, Matrix4x4>();

        public string Name
        {
            get { return _node.Name; }
            set { _node.Name = value; }
        }
        public Vector2 GetTranslation
        {
            get { return Matrix.ExtractTranslation().eTo2D(); }
        }

        // the only public constructor
        // TODO: change the "Node mesh". This should point to MeshDraw object which is unique to each entity.
        public Entity(SceneWrapper sc, Node mesh, BoneNode armature, ActionState state)
        {
            _scene = sc;
            _node = mesh;
            _extra_geometry = new Geometry(sc._inner.Meshes, mesh, armature);
            _armature = armature;
            _action = state;
            _transform = new TransformState(Matrix4.Identity, 10, 17);
        }

        public void UploadMeshVBO(IList<Material> materials)
        {
            InnerMakeMeshDraw(_scene._inner.Meshes, materials);
        }

        // Make a class that will be responsible for managind the buffer lists
        public void InnerMakeMeshDraw(IList<Mesh> meshes, IList<Material> materials)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                _mesh_id2mesh_draw[i] = new MeshDraw(meshes[i], materials);
            }
        }

        public void RotateBy(double angle_degrees)
        {
            _transform.Rotate(angle_degrees);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            var translate = _transform.TranslationFromDirection(new Vector3(x, y, 0));
            _transform.ApplyTranslation(translate);
        }

        public bool ContainsPoint(Vector2 p)
        {
            // modify the point so it is in entity space
            Vector3 tmp = new Vector3(p.X, p.Y, 0.0f);
            return _extra_geometry.EntityBorderContainsPoint(tmp.eTo2D());
        }

        /// Render the model stored in EntityScene useing the Graphics object.
        public void RenderModel(DrawConfig settings)
        {
            _draw_conf = settings;
            if (_draw_conf.EnablePerspectiveCorrectionHint)
            {
                // all are from System.Drawing.Drawing2D.
            }
            // second pass: render with this matrix
            RecursiveRenderSystemDrawing(_node);
            // apply the matrix to graphics just to draw the rectangle
            // TODO: we should just transform the border according to the RecursiveTransformVertices
            RenderBoundingBoxes(_extra_geometry);
        }

        // Render the scene.
        // each vertex at most one bone policy
        private void RecursiveRenderSystemDrawing(Node nd)
        {
            foreach(int mesh_id in nd.MeshIndices)
            {
                MeshDraw mesh_draw = _mesh_id2mesh_draw[mesh_id];
                mesh_draw.RenderVBO();
            }
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(child);
            }
        }

        public void RenderBoundingBoxes(Geometry geom)
        {
            foreach (var aabb in geom._mesh_id2box.Values)
            {
                if (Properties.Settings.Default.RenderAllMeshBounds)
                {
                    aabb.Render();
                }
            }
        }

        public void UpdateModel(double dt_ms)
        {
            // first pass: calculate a matrix for each vertex
            RecursiveCalculateVertexTransform(_node, Matrix4.Identity.eToAssimp());
            RecursiveTransformVertices(_node);
        }

        // First pass: calculate the transofmration matrix for each vertex
        // here we must associate a matrix with each bone (maybe with each vertex_id??)
        // then we multiply the current_bone matrix with the one we had before 
        // (perhaps it was identity, perhaps it was already some matrix (if 
        // the bone influences many vertices) )
        // then we store this multiplied matrix.
        // in the render function we get a vertex_id, so we can find the matrix to apply 
        // to the vertex, then we send the vertex to OpenGL
        public void RecursiveCalculateVertexTransform(Node nd, Matrix4x4 current)
        {
            Matrix4x4 current_node = current * nd.Transform;
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene._inner.Meshes[mesh_id];
                MeshDraw mesh_draw = _mesh_id2mesh_draw[mesh_id];
                foreach (Bone bone in cur_mesh.Bones)
                {
                    // a bone transform is more than by what we need to trasnform the model
                    BoneNode armature_node = _scene.GetBoneNode(bone.Name);
                    Matrix4x4 bone_global_mat = armature_node.GlobTrans;
                    /// bind tells the original delta in global coord, so we can find current delta
                    Matrix4x4 bind = bone.OffsetMatrix;
                    Matrix4x4 delta_roto = bind * bone_global_mat;
                    Matrix4x4 current_bone = delta_roto * current_node;
                    foreach (var pair in bone.VertexWeights)
                    {
                        // Can apply bone weight here
                        mesh_draw._vertex_id2matrix[pair.VertexID] = current_bone;
                    }
                }
            }
            foreach (Node child in nd.Children)
            {
                 RecursiveCalculateVertexTransform(child, current_node);
            }
        }

        /// <summary>Transform a Position by the given Matrix</summary>
        /// <param name="pos">The position to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed position</param>
        public static void TransformPositionAssimp(ref Vector3D pos, ref Matrix4x4 mat, out Vector3D result)
        {
            // this is taken from https://github.com/opentk/opentk/blob/32665ca1cbdccb1c3be109ed0b7ff3f7cb5cb5b7/Source/Compatibility/Math/Vector3.cs
            // Note that assimp is row major, while opentk is column major
            result.X = pos.X * mat.A1 +
                       pos.Y * mat.A2 +
                       pos.Z * mat.A3 +
                       mat.A4;

            result.Y = pos.X * mat.B1 +
                       pos.Y * mat.B2 +
                       pos.Z * mat.B3 +
                       mat.B4;

            result.Z = pos.X * mat.C1 +
                       pos.Y * mat.C2 +
                       pos.Z * mat.C3 +
                       mat.C4;
        }

        // Second pass: transform all vertices in a mesh according to bone
        // just apply the previously caluclated matrix
        public void RecursiveTransformVertices(Node nd)
        {
            foreach (int mesh_id in nd.MeshIndices)
            {
                MeshDraw mesh_draw = _mesh_id2mesh_draw[mesh_id];
                // map data from VBO
                IntPtr data;
                int qty_vertices;
                mesh_draw.BeginModifyVertexData(out data, out qty_vertices);
                // iterate over inital vertex positions
                Mesh cur_mesh = _scene._inner.Meshes[mesh_id];
                MeshBounds aabb = _extra_geometry._mesh_id2box[mesh_id];
                // go over every vertex in the mesh
                unsafe
                {
                    // array of floats: X,Y,Z.....
                    int sz = 3; // size of step
                    float* coords = (float*)data;
                    for (int vertex_id = 0; vertex_id < qty_vertices; vertex_id++)
                    {
                        Matrix4x4 matrix_with_offset = mesh_draw._vertex_id2matrix[vertex_id];
                        // get the initial position of vertex when scene was loaded
                        Vector3D vertex_default = cur_mesh.Vertices[vertex_id];
                        Vector3D vertex;
                        Entity.TransformPositionAssimp(ref vertex_default, ref matrix_with_offset, out vertex);
                        // write new coords back into array
                        coords[vertex_id*sz + 0] = vertex.X;
                        coords[vertex_id*sz + 1] = vertex.Y;
                        coords[vertex_id*sz + 2] = vertex.Z;
                    }
                }
                mesh_draw.EndModifyVertexData();

                foreach (Node child in nd.Children)
                {
                    RecursiveTransformVertices(child);
                }
            }
        }

    } // end of class

}
