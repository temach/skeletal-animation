﻿using System;
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

    struct BoundingVectors
    {
        public Vector3D ZeroNear;
        public Vector3D ZeroFar;
    }

    class AxiAlignedBoundingBox
    {
        public RectangleF _rect;
        public object Source;

        public Vector2 Center
        {
            get { return new Vector2(_rect.X + _rect.Width/2.0f, _rect.Y + _rect.Height/2.0f); }
        }
        public Vector2 ZeroNear
        {
            get { return new Vector2(_rect.X, _rect.Y); }
        }
        public Vector2 ZeroFar
        {
            get { return new Vector2(_rect.Right, _rect.Bottom); }
        }

        public AxiAlignedBoundingBox(object source, BoundingVectors bounds)
        {
            this.Source = source;
            // change from the 3d model into 2d program space just discard Z coordinate
            _rect = new RectangleF(bounds.ZeroNear.X, bounds.ZeroNear.Y
                , bounds.ZeroFar.X - bounds.ZeroNear.X
                , bounds.ZeroFar.Y - bounds.ZeroNear.Y
            );
        }

        public AxiAlignedBoundingBox(object source, Vector3D zero_near, Vector3D zero_far)
        {
            this.Source = source;
            // change from the 3d model into 2d program space just discard Z coordinate
            _rect = new RectangleF(zero_near.X, zero_near.Y
                , zero_far.X - zero_near.X
                , zero_far.Y - zero_near.Y
            );
        }

        public bool CheckContainsPoint(Vector2 p)
        {
            if ((ZeroNear.X < p.X && p.X < ZeroFar.X)
                && (ZeroNear.Y < p.Y && p.Y < ZeroFar.Y))
            {
                return true;
            }
            return false;
        }

        public void Render()
        {
            Util.GR.DrawRectangle(Util.pp4, Rectangle.Round(this._rect));
        }

    }

    /// <summary>
    /// Stores info on extra geometry of the entity
    /// </summary>
    class Geometry
    {

        // public List<TriangularFace> _face_borders = new List<TriangularFace>();
        public List<AxiAlignedBoundingBox> _mesh_borders = new List<AxiAlignedBoundingBox>();
        public AxiAlignedBoundingBox _entity_border = null;

        /// <summary>
        /// Build geometry data for node (usually use only for one of the children of scene.RootNode)
        /// </summary>
        public Geometry(Scene sc, Node nd)
        {
            Matrix4x4 id = Matrix4x4.Identity;
            Vector3D zero_near = new Vector3D(1e10f, 1e10f, 1e10f);
        	Vector3D zero_far = new Vector3D(-1e10f, -1e10f, -1e10f);
            CalcBoundingRect(sc, nd, ref zero_near, ref zero_far, ref id);
            // change from the 3d model into 2d program space
            _entity_border = new AxiAlignedBoundingBox(nd, zero_near, zero_far);
        }

        /// <summary>
        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
        /// </summary>
        private void CalcBoundingRect(Scene sc, Node node, ref Vector3D scene_min, ref Vector3D scene_max, ref Matrix4x4 cur_mat)
        {
            Matrix4x4 prev = cur_mat;
            cur_mat = prev * node.Transform;
            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = sc.Meshes[index];
                    var bounds = GetMinMaxForMesh(mesh, cur_mat);
                    // build mesh bounding box
                    _mesh_borders.Add(new AxiAlignedBoundingBox(mesh, bounds));
                    // check for new min/max with respect to whole scene
                    // find min
                    scene_min.X = Math.Min(scene_min.X, bounds.ZeroNear.X);
                    scene_min.Y = Math.Min(scene_min.Y, bounds.ZeroNear.Y);
                    // find max
                    scene_max.X = Math.Max(scene_max.X, bounds.ZeroFar.X);
                    scene_max.Y = Math.Max(scene_max.Y, bounds.ZeroFar.Y);
                }
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                CalcBoundingRect(sc, node.Children[i], ref scene_min, ref scene_max, ref cur_mat);
            }
            // unwind the matrix stack before we calculate the box for the next leaf
            cur_mat = prev;
        }

        /// <summary>
        /// Get minimum and maximum vectors from the mesh after trasnformation by matrix.
        /// </summary>
        public BoundingVectors GetMinMaxForMesh(Mesh mesh, Matrix4x4 mat)
        {
            Vector3D zero_near = new Vector3D(1e10f, 1e10f, 1e10f);
            Vector3D zero_far = new Vector3D(-1e10f, -1e10f, -1e10f);
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vector3D vertex = mesh.Vertices[i];
                vertex = mat.eTransformVector(vertex);
                // find min
                zero_near.X = Math.Min(zero_near.X, vertex.X);
                zero_near.Y = Math.Min(zero_near.Y, vertex.Y);
                // find max
                zero_far.X = Math.Max(zero_far.X, vertex.X);
                zero_far.Y = Math.Max(zero_far.Y, vertex.Y);
            }
            return new BoundingVectors { ZeroNear = zero_near , ZeroFar = zero_far };
        }

        public AxiAlignedBoundingBox IntersectWithMesh(Vector2 point)
        {
            foreach (AxiAlignedBoundingBox border in _mesh_borders)
            {
                if (border.CheckContainsPoint(point))
                {
                    return border;
                }
            }
            return null;
        }

        public bool MeshBorderContainsPoint(Vector2 point)
        {
            foreach (AxiAlignedBoundingBox border in _mesh_borders)
            {
                if (border.CheckContainsPoint(point))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool EntityBorderContainsPoint(Vector2 point)
        {
            return _entity_border.CheckContainsPoint(point);
        }

        public void RenderEntityBorder()
        {
            _entity_border.Render();
        }

    }


    /// <summary>
    /// Represents the currently loaded object.
    /// We will have lots of these.
    /// </summary>
    class Entity
    {
        public Node _armature;
        public Node _node;
        public Scene _scene;
        public Geometry _extra_geometry;
        public DrawConfig _draw_conf;
        public Matrix _matrix = new Matrix();
        private readonly float _motion_speed = 10.0f;

        // binding offset for each bone, but relative to the parent
        public Dictionary<string, Matrix4x4> _local_bind_offset;

        public string Name
        {
            get { return _node.Name; }
            set { _node.Name = value; }
        }
        public double GetRotationAngleDeg
        {
            get { return _matrix.eGetRotationAngle(); }
        }
        public Point GetTranslation
        {
            get { return Point.Round(_matrix.eGetTranslationPoint()); }
        }

        // the only public constructor
        public Entity(Scene sc, Node nd, Node arma)
        {
            _scene = sc;
            _node = nd;
            _extra_geometry = new Geometry(sc, nd);
            _armature = arma;
            InitCalcLocalOffset(arma);
        }

        public void InitCalcLocalOffset(Node armature_root)
        {
            _local_bind_offset = new Dictionary<string, Matrix4x4>();
            RecurCalcLocalOffset(armature_root);
        }

        public void RecurCalcLocalOffset(Node nd)
        {
            // the local offset for each bone is its transform when the bone is in initial position
            var local = nd.Transform;
            local.Inverse();
            _local_bind_offset[nd.Name] = local;
            foreach (var child in nd.Children)
            {
                RecurCalcLocalOffset(child);
            }
        }

        public void RotateBy(double angle_degrees)
        {
            _matrix.Rotate((float)angle_degrees);
        }

        public void RotateByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.I:
                    RotateBy(17);
                    break;
                case Keys.O:
                    RotateBy(-17);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            this._matrix.Translate((_motion_speed * x), (_motion_speed * y));
        }

        public void MoveByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    MoveBy(-1, 0);
                    break;
                case Keys.Up:
                    MoveBy(0, -1);
                    break;
                case Keys.Right:
                    MoveBy(1, 0);
                    break;
                case Keys.Down:
                    MoveBy(0, 1);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Change the matrices for each node to 
        /// match the orientaitons in the keyframe data.
        /// Then during rendering the mesh will look different.
        /// </summary>
        public void NextKeyframe()
        {
            return;
        }

        public bool ContainsPoint(Vector2 p)
        {
            // modify the point so it is in entity space
            var tmp = _matrix.Clone();
            tmp.Invert();
            var entity_coord_space_point = tmp.eTransformSingleVector2(p);
            return _extra_geometry.EntityBorderContainsPoint(entity_coord_space_point);
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
            // this should be the matrix of root bone. 
            // The root bone should store in world transaltion/rotation/scale.
            // currently it is just _matrix.
            //Util.GR.MultiplyTransform(_matrix);
            Util.GR.MultiplyTransform(_armature.Transform.eTo3x2());
            _extra_geometry.RenderEntityBorder();
            Util.PushMatrix();
            NewRecursiveRenderSystemDrawing(_node);
            Util.PopMatrix();
            OldRecursiveRenderSystemDrawing(_node);
        }

        public List<Bone> GetBonesAffectingVertex(int vertex_id, Mesh mesh)
        {
            var ret = new List<Bone>();
            foreach (var bone in mesh.Bones)
            {
                foreach (var vw in bone.VertexWeights)
                {
                    if (vw.VertexID == vertex_id)
                    {
                        ret.Add(bone);
                    }
                }
            }
            return ret;
        }

        private Node InnerRecurFindNodeInScene(Node cur_node, string node_name)
        {
            if (cur_node.Name == node_name)
            {
                return cur_node;
            }
            foreach (var child in cur_node.Children)
            {
                var tmp =  InnerRecurFindNodeInScene(child, node_name);
                if (tmp != null)
                {
                    return tmp;
                }
            }
            return null;
        }


        public void RenderTriangle(List<Vector2> vertices)
        {
            Brush br = Util.GetNextBrush();
            var to_draw = vertices.Select(vec => vec.eToPointFloat()).ToArray();
            Util.GR.FillPolygon(br, to_draw);
            // Bad code to draw a single point
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

        public void RenderMesh(Mesh mesh)
        {
            foreach (Face cur_face in mesh.Faces)
            {
                foreach (var vert_id in cur_face.Indices)
                {
                    Vector2 pos = mesh.Vertices[vert_id].eAs2DandOpenTK();
                    RenderVertex(pos);
                }
            }
        }

        /// <summary>
        /// Get the armature node and trace back its changes
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        public Tuple<Matrix4x4,Matrix4x4> GetArmatureGlobalTransform(Node bone)
        {
            Matrix4x4 ret_other = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            Matrix4x4 ret = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            ret *= bone.Transform;
            ret_other *= bone.Transform;
            Node cur = bone;
            while (cur.Parent != null)
            {
                Matrix4x4 tmp = ret * cur.Parent.Transform;
                ret = tmp;
                ret_other = cur.Parent.Transform * ret_other;
                cur = cur.Parent;
            }
            return Tuple.Create(ret, ret_other);
        }



        //-------------------------------------------------------------------------------------------------
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        // one mesh, one bone policy
        private void OldRecursiveRenderSystemDrawing(Node nd)
        {
            Util.PushMatrix();
            Matrix4x4 nd_local_mat = nd.Transform;
            Util.GR.MultiplyTransform(nd_local_mat.eTo3x2());
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene.Meshes[mesh_id];
                Util.PushMatrix();
                Bone mesh_bone = cur_mesh.Bones[0];
                // a bone transform is more than by what we need to trasnform the model
                Node armature_node = InnerRecurFindNodeInScene(_scene.RootNode, mesh_bone.Name);
                /// Transform tells delta in local coords
                // Matrix4x4 armature_bone = armature_node.Transform;
                var tmp = GetArmatureGlobalTransform(armature_node);
                Matrix4x4 bone_global_mat = tmp.Item1;
                /// bind tells the original delta in global coord, so we can find current delta
                Matrix4x4 bind = mesh_bone.OffsetMatrix;
                /// This does not work because even in initial state bind != armature_bone.Inverse()
                /// but it should be equal for this to work. so let's find global transofrm for armature_bone\
                /// then it will work ok. 
                Matrix4x4 delta_roto = bind * bone_global_mat;
                Util.GR.MultiplyTransform(delta_roto.eTo3x2());
                RenderMesh(cur_mesh);
                Util.PopMatrix();
            }
            foreach (Node child in nd.Children)
            {
                OldRecursiveRenderSystemDrawing(child);
            }
            // we don't want to apply this branch transform to the next branch
            Util.PopMatrix();
        }

        //-------------------------------------------------------------------------------------------------
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        // one mesh, one bone policy
        private void NewRecursiveRenderSystemDrawing(Node nd)
        {
            Util.PushMatrix();
            Matrix4x4 nd_local_mat = nd.Transform;
            //Util.GR.MultiplyTransform(nd_local_mat.eTo3x2());
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene.Meshes[mesh_id];
                Bone mesh_bone = cur_mesh.Bones[0];
                // a bone transform is more than by what we need to trasnform the model
                Node armature_node = InnerRecurFindNodeInScene(_scene.RootNode, mesh_bone.Name);
                // Transform tells delta in local coords
                Matrix4x4 local_bone = armature_node.Transform;
                Matrix4x4 local_bind = _local_bind_offset[mesh_bone.Name];
                Matrix4x4 delta_roto =  local_bone * local_bind;
                Util.GR.MultiplyTransform(delta_roto.eTo3x2());
                RenderMesh(cur_mesh);
            }
            foreach (Node child in nd.Children)
            {
                NewRecursiveRenderSystemDrawing(child);
            }
            // we don't want to apply this branch transform to the next branch
            Util.PopMatrix();
        }
    } // end of class

}
