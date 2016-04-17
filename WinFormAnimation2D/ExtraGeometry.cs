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

namespace WinFormAnimation2D
{
    struct BoundingVectors
    {
        public Vector3 ZeroNear;
        public Vector3 ZeroFar;
        public BoundingVectors(Vector3 near, Vector3 far)
        {
            ZeroNear = near;
            ZeroFar = far;
        }
    }

    class BoneBounds
    {
        public Vector3 _start;
        public Vector3 _end;

        // arbitrary vector that is perpendicular to the _end - _start
        // in 3D this might work better Vector3(-1*(_end.Y + _end.Z), 1, 1)
        // while in 2D use this Vector3(-1 * _end.Y, 1, 0), so that Z = 0;
        public Vector3 _normal
        {
            get {
                var bone_vec = _end - _start;
                var sidevec = new Vector3(bone_vec.Y, -1 * bone_vec.X, 0.0f);
                return Vector3.Multiply(Vector3.NormalizeFast(sidevec), 5.0f);
            }
        }

        public BoneBounds()
        {
            _start = Vector3.Zero;
            _end = Vector3.Zero;
        }

        public BoneBounds(Vector3 start, Vector3 end)
        {
            _start = start;
            _end = end;
        }

        // change from the 3d model into 2d program space just discard Z coordinate
        public Vector3[] Triangle
        {
            get
            {
                return new Vector3[] {
                    _start
                    , _start + _normal
                    , _end
                    , _start - _normal
                    , _start
                };
            }
        }

        public void Render(Pen p = null)
        {
            Point[] tmp = Triangle.Select(v => v.eToPoint()).ToArray();
            Util.GR.DrawLines(p == null ? Pens.Aqua : p, tmp);
        }
    }

    class MeshBounds
    {
        public Vector3 _zero_near;
        public Vector3 _zero_far;
        public bool _updating;

        public Vector3 Center
        {
            get { return Vector3.Divide(Vector3.Add(_zero_near,_zero_far), 2.0f);  }
        }

        // change from the 3d model into 2d program space just discard Z coordinate
        public RectangleF Rect
        {
            get
            {
                return new RectangleF(_zero_near.X, _zero_near.Y
                    , _zero_far.X - _zero_near.X
                    , _zero_far.Y - _zero_near.Y
                );
            }
        }

        public MeshBounds(Vector3D zero_near, Vector3D zero_far)
        {
            _zero_far = zero_far.eToOpenTK();
            _zero_near = zero_near.eToOpenTK();
        }

        public MeshBounds()
        {
            _zero_near = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            _zero_far = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public bool CheckContainsPoint(Vector2 p)
        {
            if ((_zero_near.X < p.X && p.X < _zero_far.X)
                && (_zero_near.Y < p.Y && p.Y < _zero_far.Y))
            {
                return true;
            }
            return false;
        }

        public void Render()
        {
            RenderDrawing2D();
            RenderGL();
        }

        public void RenderDrawing2D()
        {
            Util.GR.DrawRectangle(Util.pp4, Rectangle.Round(this.Rect));
        }

        public void RenderGL()
        {
            GL.Color3(Util.cc4);
            GL.Normal3(0, 1, 1);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Begin(BeginMode.Quads);
            GL.Vertex3(Rect.Location.X, Rect.Location.Y, 1.0);
            GL.Vertex3(Rect.Location.X + Rect.Width, Rect.Location.Y, 1.0);
            GL.Vertex3(Rect.Location.X + Rect.Width, Rect.Location.Y + Rect.Height, 0.0);
            GL.Vertex3(Rect.Location.X, Rect.Location.Y + Rect.Height, 0.0);
            GL.End();
        }

        public BoundingVectors GetNearFar()
        {
            return new BoundingVectors(_zero_near, _zero_far);
        }

        // call this before starting a cycle of updates
        public void SafeStartUpdateNearFar()
        {
            if (_updating)
            {
                return;
            }
            _zero_near = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            _zero_far = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            _updating = true;
        }

        public void EndUpdateNearFar()
        {
            Debug.Assert(_updating == true, "Update was never started");
            _updating = false;
        }

        // pass in a vertex belonging to the mesh, we will 
        // check if we need to change the near/far values
        public void UpdateNearFar(Vector3 vertex)
        {
            // update frame min
            _zero_near.X = Math.Min(_zero_near.X, vertex.X);
            _zero_near.Y = Math.Min(_zero_near.Y, vertex.Y);
            _zero_near.Z = Math.Min(_zero_near.Z, vertex.Z);
            // update frame max
            _zero_far.X = Math.Max(_zero_far.X, vertex.X);
            _zero_far.Y = Math.Max(_zero_far.Y, vertex.Y);
            _zero_far.Z = Math.Max(_zero_far.Z, vertex.Z);
        }

    }

    class BoundingBoxGroup
    {
        public List<MeshBounds> Items;
        public MeshBounds _overall_box = new MeshBounds();
        public MeshBounds OverallBox
        {
            get
            {
                // Update before returning
                var tmp = GetCoveringBoundingBox(Items);
                _overall_box._zero_near = tmp._zero_near;
                _overall_box._zero_far = tmp._zero_far;
                return _overall_box;
            }
        }

        public BoundingBoxGroup(IEnumerable<MeshBounds> boxes)
        {
            Items = boxes.ToList();
        }

        public MeshBounds GetCoveringBoundingBox(IEnumerable<MeshBounds> boxes)
        {
            Vector3D zero_near = new Vector3D(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3D zero_far = new Vector3D(float.MinValue, float.MinValue, float.MinValue);
            foreach (var aabb in boxes)
            {
                // find min
                zero_near.X = Math.Min(zero_near.X, aabb._zero_near.X);
                zero_near.Y = Math.Min(zero_near.Y, aabb._zero_near.Y);
                zero_near.Z = Math.Min(zero_near.Z, aabb._zero_near.Z);
                // find max
                zero_far.X = Math.Max(zero_far.X, aabb._zero_far.X);
                zero_far.Y = Math.Max(zero_far.Y, aabb._zero_far.Y);
                zero_far.Z = Math.Max(zero_far.Z, aabb._zero_far.Z);
            }
            return new MeshBounds(zero_near, zero_far);
        }
    }

    /// Stores info on extra geometry of the entity
    class Geometry
    {

        public Dictionary<int,MeshBounds> _mesh_id2box = new Dictionary<int,MeshBounds>();
        public Dictionary<string,BoneBounds> _bone_id2triangle = new Dictionary<string,BoneBounds>();
        public Dictionary<Guid,BoundingBoxGroup> _mesh_groups = new Dictionary<Guid,BoundingBoxGroup>();
        public Guid _ent_box_id;
        public Matrix4 _matrix = Matrix4.Identity;

        /// Build geometry data for node (usually use only for one of the children of scene.RootNode)
        public Geometry(IList<Mesh> scene_meshes, Node nd, BoneNode armature)
        {
            MakeBoundingBoxes(scene_meshes, nd);
            MakeBoundingTriangles(armature);
            UpdateBonePositions(armature);
            _ent_box_id = GetCoveringGroup(_mesh_id2box.Values);
        }

        public Guid GetCoveringGroup(IEnumerable<MeshBounds> boxes)
        {
            BoundingBoxGroup tmp = new BoundingBoxGroup(boxes);
            Guid id = Guid.NewGuid();
            _mesh_groups[id] = tmp;
            return id;
        }

        public void RenderBoxGroup(BoundingBoxGroup gr)
        {
            Util.PushMatrix();
            //Util.GR.MultiplyTransform(_matrix.eTo3x2());
            gr.OverallBox.Render();
            Util.PopMatrix();
        }

        public void RenderBone(BoneBounds b, Pen p = null)
        {
            Util.PushMatrix();
            Util.GR.MultiplyTransform(_matrix.eTo3x2());
            b.Render(p);
            Util.PopMatrix();
        }

        public void UpdateBonePositions(BoneNode nd)
        {
            var triangle = _bone_id2triangle[nd._inner.Name];
            Vector3 new_start = nd.GlobalTransform.ExtractTranslation();
            if (nd.Children.Count > 0)
            {
                // this bone's end == the beginning of __any__ child bone
                Vector3 new_end = nd.Children[0].GlobalTransform.ExtractTranslation();
                triangle._start = new_start;
                triangle._end = new_end;
                foreach (var child_nd in nd.Children)
                {
                    UpdateBonePositions(child_nd);
                }
            }
            else
            {
                // this bone has no children, we don't know where it will end, so we guess.
                // strategy 1: just set a random sensible value for bone
                // strategy 2: get geometric center of the vertices that this bone acts on
                // we have to use the Y-unit vector instead of X because we defined Y_UP 
                // in the collada.dae file, so all the matrices work such that direct unit vector is unit Y
                var delta = Vector3.TransformVector(Vector3.UnitY, nd.GlobalTransform);
                Vector3 new_end = new_start + Vector3.Multiply(delta, 70.0f);
                triangle._start = new_start;
                triangle._end = new_end;
            }
        }

        // make triangles to draw for each bone
        private void MakeBoundingTriangles(BoneNode nd)
        {
            _bone_id2triangle[nd._inner.Name] = new BoneBounds();
            for (int i = 0; i < nd._inner.ChildCount; i++)
            {
                 MakeBoundingTriangles(nd.Children[i]);
            }

        }

        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
        private void MakeBoundingBoxes(IList<Mesh> scene_meshes, Node node)
        {
            foreach (int index in node.MeshIndices)
            {
                Mesh mesh = scene_meshes[index];
                _mesh_id2box[index] = new MeshBounds();
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                MakeBoundingBoxes(scene_meshes, node.Children[i]);
            }
        }

        public MeshBounds IntersectWithMesh(Vector2 point)
        {
            foreach (MeshBounds border in _mesh_id2box.Values)
            {
                if (border.CheckContainsPoint(point))
                {
                    return border;
                }
            }
            return null;
        }

        public bool EntityBorderContainsPoint(Vector2 point)
        {
            return _mesh_groups[_ent_box_id].OverallBox.CheckContainsPoint(point);
        }

    }


}
