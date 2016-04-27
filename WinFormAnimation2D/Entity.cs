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
        public Entity(SceneWrapper sc, Node mesh, BoneNode armature, ActionState state)
        {
            _scene = sc;
            _node = mesh;
            _extra_geometry = new Geometry(sc._inner.Meshes, mesh, armature);
            _armature = armature;
            _action = state;
            _transform = new TransformState(Matrix4.Identity, 10, 17);
        }

        public void RotateBy(double angle_degrees)
        {
            _transform.Rotate(angle_degrees);
        }

        public void RotateByKey(KeyEventArgs e)
        {
            double angle_degrees = _transform.GetAngleDegreesFromKeyEventArg(e);
            _transform.Rotate(angle_degrees);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            var translate = _transform.TranslationFromDirection(new Vector3(x, y, 0));
            _transform.ApplyTranslation(translate);
        }

        public void MoveByKey2D(KeyEventArgs e)
        {
            _transform.GetDirectionNormalizedFromKey(e);
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
                Util.GR.CompositingQuality = CompositingQuality.HighQuality;
                Util.GR.InterpolationMode = InterpolationMode.HighQualityBilinear;
                Util.GR.SmoothingMode = SmoothingMode.AntiAlias;
            }
            // first pass: calculate a matrix for each vertex
            RecursiveTransformVertices(_node, Matrix4.Identity.eToAssimp());
            // second pass: render with this matrix
            RecursiveRenderSystemDrawing(_node);
            // apply the matrix to graphics just to draw the rectangle
            // TODO: we should just transform the border according to the RecursiveTransformVertices
            RenderBoundingBoxes(_extra_geometry);
        }

        public void RenderTriangle(List<Vector2> vertices)
        {
            Brush br = Util.GetNextBrush();
            var to_draw = vertices.Select(vec => vec.eToPointFloat()).ToArray();
            Util.GR.FillPolygon(br, to_draw);
            foreach (var p in to_draw)
            {
                Util.GR.eDrawPoint(p);
            }
        }
        public List<Vector2> cur_triangle = new List<Vector2>();
        public void RenderVertex(Vector2 vec)
        {
            cur_triangle.Add(vec);
            if (cur_triangle.Count() == 3)
            {
                RenderTriangle(cur_triangle);
                cur_triangle.Clear();
            }
        }

        // Render the scene.
        // each vertex at most one bone policy
        private void RecursiveRenderSystemDrawing(Node nd)
        {
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene._inner.Meshes[mesh_id];
                MeshBounds aabb = _extra_geometry._mesh_id2box[mesh_id];
                aabb.SafeStartUpdateNearFar();
                foreach (Face cur_face in cur_mesh.Faces)
                {
                    GL.Normal3(0, 0, 1);
                    GL.Color3(Util.GetNextColor());
                    GL.Begin(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
                    foreach (var vert_id in cur_face.Indices)
                    {
                        // we must get the new vertex position here
                        Matrix4 delta = _vertex2matrix[cur_mesh.Vertices[vert_id]].eToOpenTK();
                        Vector3 default_pose = cur_mesh.Vertices[vert_id].eToOpenTK();
                        Vector3 trans;
                        Vector3.Transform(ref default_pose, ref delta, out trans);
                        aabb.UpdateNearFar(trans);
                        // 2d
                        RenderVertex(trans.eTo2D());
                        // 3d
                        GL.Vertex3(trans.X, trans.Y, trans.Z);
                    }
                    GL.End();
                }
            }
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(child);
            }
        }

        // First pass: transform all vertices in a mesh according to bone
        // here we must associate a matrix with each bone (maybe with each vertex_id??)
        // then we multiply the current_bone matrix with the one we had before 
        // (perhaps it was identity, perhaps it was already some matrix (if 
        // the bone influences many vertices) )
        // then we store this multiplied matrix.
        // in the render function we get a vertex_id, so we can find the matrix to apply 
        // to the vertex, then we send the vertex to OpenGL
        public void RecursiveTransformVertices(Node nd, Matrix4x4 current)
        {
            Matrix4x4 current_node = current * nd.Transform;
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene._inner.Meshes[mesh_id];
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
                        Vector3D vertex = cur_mesh.Vertices[pair.VertexID];
                        _vertex2matrix[vertex] = current_bone;
                    }
                }
            }
            foreach (Node child in nd.Children)
            {
                 RecursiveTransformVertices(child, current_node);
            }
        }

        public void RenderBoundingBoxes(Geometry geom)
        {
            foreach (var aabb in geom._mesh_id2box.Values)
            {
                aabb.EndUpdateNearFar();
                if (Properties.Settings.Default.RenderAllMeshBounds)
                {
                    aabb.Render();
                }
            }
            //_extra_geometry.RenderEntityBorder();
        }

    } // end of class

}
