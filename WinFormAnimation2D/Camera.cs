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

    /// <summary>
    /// Maintains camera abstraction.
    /// </summary>
    class CameraDevice
    {
        public Drawing2DCamera _2d_camera;
        public OpenGLCamera _3d_camera;

        private Matrix4 _cam_mat
        {
            get { return _2d_camera.CamMatrix; }
        }
        private Matrix4 _opengl_mat
        {
            get { return _3d_camera.CamMatrix; }
        }

        public Vector2 GetTranslation
        {
            get { return _2d_camera.GetTranslation2D; }
        }

        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            return _2d_camera.ConvertScreen2WorldCoordinates(screen_coords);
        }

        public Matrix4 CamMatrix
        {
            get { return _cam_mat; }
        }

        public CameraDevice(Matrix4 draw2d_init_mat, Size window_size, Matrix4 opengl_init_mat)
        {
            _2d_camera = new Drawing2DCamera(draw2d_init_mat, window_size);
            _3d_camera = new OpenGLCamera(opengl_init_mat);
        }

        /// Get the camera matrix to be uploaded to drawing 2D
        public Matrix4 MatrixToDrawing2D()
        {
            return _2d_camera.MatrixToDrawing2D();
        }

        /// Get the camera matrix to be uploaded to drawing 2D
        public Matrix4 MatrixToOpenGL()
        {
            return _3d_camera.MatrixToOpenGL();
        }

        public void RotateBy(double angle_degrees)
        {
            _2d_camera.RotateAroundScreenCenter2D(angle_degrees);
            _3d_camera.RotateBy2D(angle_degrees);
        }

        public void RotateByKey(KeyEventArgs e)
        {
            _2d_camera.RotateByKey2D(e);
            _3d_camera.RotateByKey2D(e);
        }

        public void ProcessMouse(int x, int y)
        {
            _2d_camera.ProcessMouse(x, y);
            _3d_camera.ProcessMouse(x, y);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            _2d_camera.MoveBy2D(x, y);
            _3d_camera.MoveBy2D(x, y);
        }

        public void MoveByKey(KeyEventArgs e)
        {
            _2d_camera.MoveByKey2D(e);
            _3d_camera.MoveByKey2D(e);
        }

    }

    class OpenGLCamera : ITransformState
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

        public OpenGLCamera(Matrix4 opengl_init_mat)
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

        public void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right (x > 0) we perform a clockwise rotation.
            if (x != 0)
            {
                RotateBy2D(x * _transform._roto_speed_degrees);
            }
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

        public void RotateBy2D(double angle_degrees)
        {
            _transform.RotateBy(angle_degrees, Vector3.UnitZ);
        }
        public void RotateByKey2D(KeyEventArgs e)
        {
            double angle_degrees = _transform.GetAngleDegreesFromKeyEventArg(e);
            _transform.RotateBy2D(angle_degrees);
        }
        public void MoveBy2D(int x, int y)
        {
            // x,y are direction parameters one of {-1, 0, 1}
            _transform.MoveBy2D(x, y);
        }
        public void MoveByKey2D(KeyEventArgs e)
        {
            _transform.MoveByKey(e);
        }

    }


    class Drawing2DCamera : ITransformState
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
                RotateBy2D(x * _transform._roto_speed_degrees);
            }
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

        public void RotateBy2D(double angle_degrees)
        {
            RotateAroundScreenCenter2D(angle_degrees);
        }
        public void RotateByKey2D(KeyEventArgs e)
        {
            double angle_degrees = _transform.GetAngleDegreesFromKeyEventArg(e);
            RotateAroundScreenCenter2D(angle_degrees);
        }
        public void MoveBy2D(int x, int y)
        {
            // x,y are direction parameters one of {-1, 0, 1}
            _transform.MoveBy2D(x, y);
        }
        public void MoveByKey2D(KeyEventArgs e)
        {
            _transform.MoveByKey(e);
        }

        public Drawing2DCamera(Matrix4 draw2d_init_mat, Size window_size)
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