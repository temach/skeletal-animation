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

    struct Segment1
    {
        public double _start;
        public double _end;
        public Segment1(double start, double end)
        {
            _start = start;
            _end = end;
        }
    }

    struct Segment2
    {
        public Vector2 _start;
        public Vector2 _end;
        public Vector2 _unit;

        public Segment2(Vector2 start, Vector2 end)
        {
            _start = start;
            _end = end;
            _unit = (end - start).Normalized();
        }

    }

    class BoundingRect
    {

        public Vector2[] _vertices = new Vector2[4];
        // vectors perpendicular to the sides. Will be projecting on them.
        public Vector2 _across;  // forward
        public Vector2 _along;  // side

        public Vector2[] ProjectionAxis
        {
            get { return new Vector2[] { this._across, this._along }; }
        }

        public BoundingRect(List<Vector2> v4list)
        {
            this._vertices = v4list.ToArray();
            CalculateProjectionAxis();
        }

        // axis should have unit length
        public Segment1 ProjectOn(Vector2 axis)
        {
            Debug.Assert(axis.Length == 1, "Axis is not of unit length!");
            var dp = new List<double>();
            foreach (Vector2 v in this._vertices)
            {
                dp.Add(Vector2.Dot(v, axis));
            }
            double min = dp.Min();
            double max = dp.Max();
            return new Segment1(min, max);
        }

        public void CalculateProjectionAxis()
        {
            _across = (_vertices[0] - _vertices[1]).Normalized();
            _along = (_vertices[0] - _vertices[3]).Normalized();
            Debug.Assert(Vector2.Dot(_across, _along) == 0
                , "You chose non perpendicular vector");
        }

        public bool CheckContainsPoint(Vector2 p)
        {
            foreach (var axis in ProjectionAxis)
            {
                var box_project = this.ProjectOn(axis);
                double box_min = box_project._start;
                double box_max = box_project._end;
                double dp = Vector2.Dot(p, axis);
                if ((dp <= box_min) || (box_max <= dp))
                {
                    // if point is out of bounds then no collision
                    return false;
                }
            }
            return true;
        }

        // apply transform to this bounding box
        public void Transform(Matrix mat)
        {
            mat.eTransformVector2(this._vertices);
            CalculateProjectionAxis();
        }

    }

    class AxiAlignedBoundingBox
    {
        public RectangleF _rect;

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

        public AxiAlignedBoundingBox(Scene sc)
        {
            Matrix4x4 id = Matrix4x4.Identity;
            Vector3D zero_near = new Vector3D(1e10f, 1e10f, 1e10f);
        	Vector3D zero_far = new Vector3D(-1e10f, -1e10f, -1e10f);
            CalcBoundingRect(sc, sc.RootNode, ref zero_near, ref zero_far, ref id);
            // change from the 3d model into 2d program space
            _rect = new RectangleF(zero_near.X, zero_near.Y
                , zero_far.X - zero_near.X
                , zero_far.Y - zero_near.Y
            );
        }

        /// <summary>
        /// For each node calculate the bounding box.
        /// This is used to align the viewport nicely when the scene is imported.
        /// </summary>
        private void CalcBoundingRect(Scene sc, Node node, ref Vector3D min, ref Vector3D max, ref Matrix4x4 cur_mat)
        {
            Matrix4x4 prev = cur_mat;
            cur_mat = prev * node.Transform;
            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = sc.Meshes[index];
                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        Vector3D cur_vertex = mesh.Vertices[i];
                        cur_vertex = cur_mat.eTransformVector(cur_vertex);
                        // find min
                        min.X = Math.Min(min.X, cur_vertex.X);
                        min.Y = Math.Min(min.Y, cur_vertex.Y);
                        // find max
                        max.X = Math.Max(max.X, cur_vertex.X);
                        max.Y = Math.Max(max.Y, cur_vertex.Y);
                    }
                }
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                CalcBoundingRect(sc, node.Children[i], ref min, ref max, ref cur_mat);
            }
            // unwind the matrix stack before we calculate the box for the next leaf
            cur_mat = prev;
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

        public AxiAlignedBoundingBox _bounding_rect = null;

        public Geometry(Scene sc)
        {
            _bounding_rect = new AxiAlignedBoundingBox(sc);
        }
        
        public bool ContainsPoint(Vector2 point)
        {
            return _bounding_rect.CheckContainsPoint(point);
        }

        public void Render()
        {
            _bounding_rect.Render();
        }

    }


    /// <summary>
    /// Represents the currently loaded object.
    /// We will have lots of these.
    /// </summary>
    class Entity
    {
        public Matrix _matrix = new Matrix();
        public Scene _scene;
        public Geometry _extra_geometry;
        public DrawConfig _draw_conf;
        public string Name;
        private readonly float _motion_speed = 10.0f;

        public double GetRotationAngleDeg
        {
            get { return _matrix.eGetRotationAngle(); }
        }
        public Point GetTranslation
        {
            get { return Point.Round(_matrix.eGetTranslationPoint()); }
        }

        // the only public constructor
        public Entity(Scene sc, string entity_name)
        {
            Name = entity_name;
            _scene = sc;
            _extra_geometry = new Geometry(sc);
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
            // why? because instead of modifying the vertices we modify the entity matrix.
            // this means that every time we want to interact with the entity, we must send
            // all interaction througth the filter of entity's matrix
            var tmp = _matrix.Clone();
            tmp.Invert();
            var entity_coord_space_point = tmp.eTransformSingleVector2(p);
            return _extra_geometry.ContainsPoint(entity_coord_space_point);
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
            Util.GR.MultiplyTransform(_matrix);
            _extra_geometry.Render();
            RecursiveRenderSystemDrawing(_scene.RootNode);
        }

        //-------------------------------------------------------------------------------------------------
        // TODO: move all the drawing code elsewhere. This should only show the logic of drawing not the actual OpenGL commands.
        // Render the scene.
        // Begin at the root node of the imported data and traverse
        // the scenegraph by multiplying subsequent local transforms
        // together on OpenGL matrix stack.
        private void RecursiveRenderSystemDrawing(Node nd)
        {
            Matrix4x4 mat44 = nd.Transform;
            var tmp = mat44.eTo3x2();
            var saved = Util.GR.Save();
            Util.GR.MultiplyTransform(tmp);
            foreach(int mesh_id in nd.MeshIndices)
            {
                Mesh cur_mesh = _scene.Meshes[mesh_id];
                foreach (Face cur_face in cur_mesh.Faces)
                {
                    // list of 3 vertices
                    var tri_vertices = cur_face.Indices.Select(i => cur_mesh.Vertices[i].eToPointFloat()).ToArray();
                    Brush br;
                    // Enable random colored light to emit from the render into your eyes
                    if (_draw_conf.EnableLight) {
                        // choose random brush color for this triangle
                        br = Util.GetNextBrush();
                    } else {
                        br = _draw_conf.DefaultBrush;
                    }
                    // Fill or draw wireframe
                    if (_draw_conf.EnablePolygonModeFill) {
                        Util.GR.FillPolygon(br, tri_vertices);
                    } else {
                        Util.GR.DrawLines(_draw_conf.DefaultPen, tri_vertices);
                    }
                    // Bad code to draw a single point. Better use DrawEllipse. But too lazy.
                    foreach(var p in tri_vertices)
                    {
                        Util.GR.eDrawPoint(p);
                    }
                }
            }
            // draw all children
            foreach (Node child in nd.Children)
            {
                RecursiveRenderSystemDrawing(child);
            }
            // After drawing the left node. unwind the matrix stack.
            // so we don't apply this leaf's transform to the next leaf.
            Util.GR.Restore(saved);
        }

    } // end of class

}
