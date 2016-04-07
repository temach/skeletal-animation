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

    /// Stores info on extra geometry of the entity
    class Geometry
    {

        // public List<TriangularFace> _face_borders = new List<TriangularFace>();
        public List<AxiAlignedBoundingBox> _mesh_borders = new List<AxiAlignedBoundingBox>();
        public AxiAlignedBoundingBox _entity_border = null;

        /// Build geometry data for node (usually use only for one of the children of scene.RootNode)
        public Geometry(Scene sc, Node nd)
        {
            Matrix4x4 id = Matrix4x4.Identity;
            Vector3D zero_near = new Vector3D(1e10f, 1e10f, 1e10f);
        	Vector3D zero_far = new Vector3D(-1e10f, -1e10f, -1e10f);
            CalcBoundingRect(sc, nd, ref zero_near, ref zero_far, ref id);
            // change from the 3d model into 2d program space
            _entity_border = new AxiAlignedBoundingBox(nd, zero_near, zero_far);
        }

        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
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

        /// Get minimum and maximum vectors from the mesh after trasnformation by matrix.
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


}
