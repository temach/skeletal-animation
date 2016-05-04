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
    class CameraDrawing2D : ITransformState
    {
        // we need half the size of picture box
        public float rotate_offset_x;
        public float rotate_offset_y;

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

        public void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right (x > 0) we perform a clockwise rotation.
            if (x != 0)
            {
                RotateBy(x * _transform.RotateSpeedDegrees);
            }
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        public Vector2 ConvertScreen2WorldCoordinates(Point screen_coords)
        {
            Vector3 tmp = new Vector3(screen_coords.X, screen_coords.Y, 0.0f);
            tmp = Vector3.Transform(tmp, _transform._matrix);
            return new Vector2(tmp.X, tmp.Y);
        }

        public void RotateBy(double angle_degrees)
        {
            RotateAroundScreenCenter2D(angle_degrees);
        }

        public void MoveBy(Vector3 direction)
        {
            // x,y are direction parameters one of {-1, 0, 1}
            direction.Z = 0;
            if (direction.eIsZero())
            {
                return;
            }
            var translate = _transform.TranslationFromDirection(direction);
            _transform.ApplyTranslation(translate);
        }

        public CameraDrawing2D(Matrix4 draw2d_init_mat, Size window_size)
        {
            _transform = new TransformState(draw2d_init_mat, 10.0, 1.5);
            rotate_offset_x = window_size.Width / 2.0f;
            rotate_offset_y = window_size.Height / 2.0f;
        }

        /// <summary>
        /// Get the camera matrix to be uploaded to drawing 2D
        /// </summary>
        public Matrix4 MatrixToDrawing2D()
        {
            Matrix4 cam_inverted = _transform._matrix;
            cam_inverted.Invert();
            return cam_inverted;
        }

        // when doing a rotation we want to perform it around the screen center.
        public void RotateAroundScreenCenter2D(double angle_degrees)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            // we would remove the translation in OpenGL because its screen center is at (0,0,0)
            // in 2D camera screen center is at (Width/2.0, Height/2.0)
            // so translate to screen center 
            _transform._matrix = Matrix4.CreateTranslation(rotate_offset_x, rotate_offset_y, 0.0f) * _transform._matrix;
            _transform._matrix = Matrix4.CreateRotationZ(angle_radians) * _transform._matrix;
            // translate back
            _transform._matrix = Matrix4.CreateTranslation(-rotate_offset_x, -rotate_offset_y, 0.0f) * _transform._matrix;
        }

    }
}
