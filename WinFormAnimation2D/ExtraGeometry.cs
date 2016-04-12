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

    class VectorBoundingTriangle
    {
        public Vector3 _start;
        public Vector3 _end;

        // arbitrary vector that is perpendicular to the _end - _start
        // in 3D this might work better Vector3(-1*(_end.Y + _end.Z), 1, 1)
        // while in 2D use this Vector3(-1 * _end.Y, 1, 0), so that Z = 0;
        public Vector3 _normal
        {
            get { return Vector3.Multiply(new Vector3(-1 * _end.Y, 1, 0), 0.2f); }
        }

        public VectorBoundingTriangle(Vector3 start, Vector3 end)
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

        public void Render()
        {
            Point[] tmp = Triangle.Select(v => v.eToPoint()).ToArray();
            Util.GR.DrawLines(Pens.Aqua, tmp);
            //Util.GR.DrawClosedCurve(Pens.Aqua, tmp);
        }
    }

    class AxiAlignedBoundingBox
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

        public AxiAlignedBoundingBox(Vector3D zero_near, Vector3D zero_far)
        {
            _zero_far = zero_far.eToOpenTK();
            _zero_near = zero_near.eToOpenTK();
        }

        public AxiAlignedBoundingBox()
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
            Util.GR.DrawRectangle(Util.pp4, Rectangle.Round(this.Rect));
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
            _zero_far.Z = Math.Min(_zero_far.Z, vertex.Z);
        }

    }

    /// Stores info on extra geometry of the entity
    class Geometry
    {

        // public List<TriangularFace> _face_borders = new List<TriangularFace>();
        public Dictionary<int,AxiAlignedBoundingBox> _mesh_id2box = new Dictionary<int,AxiAlignedBoundingBox>();
        private AxiAlignedBoundingBox _entity_box = new AxiAlignedBoundingBox();
        public AxiAlignedBoundingBox EntityBox
        {
            get {
                // Update before returning
                var tmp = GetCoveringBoundingBox(_mesh_id2box.Values);
                _entity_box._zero_far = tmp._zero_far;
                _entity_box._zero_near = tmp._zero_near;
                return _entity_box;
            }
        }

        /// Build geometry data for node (usually use only for one of the children of scene.RootNode)
        public Geometry(IList<Mesh> scene_meshes, Node nd)
        {
            MakeBoundingBoxes(scene_meshes, nd);
        }

        public AxiAlignedBoundingBox GetCoveringBoundingBox(IEnumerable<AxiAlignedBoundingBox> boxes)
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
            return new AxiAlignedBoundingBox(zero_near, zero_far);
        }

        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
        private void MakeBoundingBoxes(IList<Mesh> scene_meshes, Node node)
        {
            foreach (int index in node.MeshIndices)
            {
                Mesh mesh = scene_meshes[index];
                _mesh_id2box[index] = new AxiAlignedBoundingBox();
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                MakeBoundingBoxes(scene_meshes, node.Children[i]);
            }
        }

        public AxiAlignedBoundingBox IntersectWithMesh(Vector2 point)
        {
            foreach (AxiAlignedBoundingBox border in _mesh_id2box.Values)
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
            return EntityBox.CheckContainsPoint(point);
        }

        public void RenderEntityBorder()
        {
            EntityBox.Render();
        }

    }


}
