using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace WinFormAnimation2D
{
    class CameraFreeFly3D : ITransformState
    {
        public TransformState _transform;
        public TransformState Transform
        {
            get { return _transform; }
        }

        public Vector3 GetTranslation
        {
            get { return _transform.GetTranslation; }
        }

        public Vector2 GetTranslation2D
        {
            get { return _transform.GetTranslation2D; }
        }

        public Matrix4 CamMatrix
        {
            get { return _transform._matrix; }
        }

        public CameraFreeFly3D(Matrix4 opengl_init_mat)
        {
            _transform = new TransformState(opengl_init_mat, 10, 1.5);
        }

        /// <summary>
        /// Get the camera matrix to be uploaded to drawing 2D
        /// </summary>
        public Matrix4 MatrixToOpenGL()
        {
            Matrix4 opengl_cam_inverted = _transform._matrix;
            opengl_cam_inverted.Invert();
            return opengl_cam_inverted;
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            Vector3 tmp = new Vector3(screen_coords.X, screen_coords.Y, 0.0f);
            tmp = Vector3.Transform(tmp, _transform._matrix);
            return new PointF(tmp.X, tmp.Y);
        }

        // Movement in ZY plane
        public void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right (x > 0) we perform a clockwise rotation.
            if (x != 0)
            {
                _transform.Rotate(x * _transform.RotateSpeedDegrees);
            }
        }

        public void RotateBy(double angle_degrees)
        {
            RotateByAxis(angle_degrees, Vector3.UnitX);
        }
        public void RotateByAxis(double angle_degrees, Vector3 axis)
        {
            _transform.RotateAroundAxis(angle_degrees, axis);
        }

        // x,y,z are direction parameters one of {-1, 0, 1}
        public void MoveBy(Vector3 dir)
        {
            if (Properties.Settings.Default.FixCameraPlane)
            {
                var translate = _transform.TranslationFromDirectionInPlaneYZ(dir.Xy);
                _transform.ApplyTranslation(translate);
            }
            else
            {
                _transform.MoveBy(dir);
            }
        }

    }
}
