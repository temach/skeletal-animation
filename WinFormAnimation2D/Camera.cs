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
            get { return _2d_camera.GetTranslation; }
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
            _2d_camera.RotateBy(angle_degrees);
            _3d_camera.RotateBy(angle_degrees);
        }

        public void RotateByKey(KeyEventArgs e)
        {
            _2d_camera.RotateByKey(e);
            _3d_camera.RotateByKey(e);
        }

        public void ProcessMouse(int x, int y)
        {
            _2d_camera.ProcessMouse(x, y);
            _3d_camera.ProcessMouse(x, y);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            _2d_camera.MoveBy(x, y);
            _3d_camera.MoveBy(x, y);
        }

        public void MoveByKey(KeyEventArgs e)
        {
            _2d_camera.MoveByKey(e);
            _3d_camera.MoveByKey(e);
        }

        /// Scrolls along the camera z axis. In and out of the scene.
        public void ProcessScroll(int z)
        {
            _3d_camera.ProcessScroll(z);
        }

    }

    class OpenGLCamera
    {
        private readonly float _zoom_speed = 1.00105f;
        private readonly float _zoom_close_limit = 0.9f;
        private readonly float _zoom_far_limit = 10.0f;
        // this is like an initial zoom factor. In OpenGL zoom is
        // done by moving objects closer/further
        // in Drawing2D it is done by scaling. 
        // Actually in OpenGL we can use scaling as well. No? interesting idea....
        private readonly float _rotation_speed = 1.5f;
        private readonly float _motion_speed = 10.0f;
        private Matrix4 _opengl_mat;

        // just for debug, we need half the size of picture box
        public float shift_x = 360.5f;
        public float shift_y = 233f;

        public Vector2 GetTranslation
        {
            get { return _opengl_mat.ExtractTranslation().eTo2D(); }
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            Vector3 tmp = new Vector3(screen_coords.X, screen_coords.Y, 0.0f);
            tmp = Vector3.Transform(tmp, _opengl_mat);
            return new PointF(tmp.X, tmp.Y);
        }

        public Matrix4 CamMatrix
        {
            get { return _opengl_mat; }
        }

        public OpenGLCamera(Matrix4 opengl_init_mat)
        {
            _opengl_mat = opengl_init_mat;
        }

        /// <summary>
        /// Get the camera matrix to be uploaded to drawing 2D
        /// </summary>
        public Matrix4 MatrixToOpenGL()
        {
            Matrix4 opengl_cam_inverted = _opengl_mat;
            opengl_cam_inverted.Invert();
            return opengl_cam_inverted;
        }

        public void RotateBy(double angle_degrees)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            _opengl_mat = Matrix4.CreateRotationZ(angle_radians) * _opengl_mat;
        }

        public void RotateByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.I:
                    RotateBy(-10);
                    break;
                case Keys.O:
                    RotateBy(10);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right (x > 0) we perform a clockwise rotation.
            if (x != 0)
            {
                RotateBy(x * _rotation_speed);
            }
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            _opengl_mat = Matrix4.CreateTranslation((_motion_speed * x), (_motion_speed * y), 0.0f) * _opengl_mat;
        }

        public void MoveByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A:
                    MoveBy(1, 0);
                    break;
                case Keys.W:
                    MoveBy(0, 1);
                    break;
                case Keys.D:
                    MoveBy(-1, 0);
                    break;
                case Keys.S:
                    MoveBy(0, -1);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Scrolls along the camera z axis. In and out of the scene.
        /// </summary>
        /// <param name="z">Scroll factor.</param>
        public void ProcessScroll(int z)
        {
            float factor = Math.Max((float)Math.Pow(_zoom_speed, z), _zoom_close_limit);
            //_cam_mat.Scale(factor, factor);
        }

    }


    class Drawing2DCamera
    {

        private readonly float _rotation_speed = 1.5f;
        private readonly float _motion_speed = 10.0f;
        private Matrix4 _cam_mat;

        // we need half the size of picture box
        public float rotate_offset_x;
        public float rotate_offset_y;

        public Vector2 GetTranslation
        {
            get { return _cam_mat.ExtractTranslation().eTo2D(); }
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            Vector3 tmp = new Vector3(screen_coords.X, screen_coords.Y, 0.0f);
            tmp = Vector3.Transform(tmp, _cam_mat);
            return new PointF(tmp.X, tmp.Y);
        }

        public Matrix4 CamMatrix {
            get { return _cam_mat; }
            set { _cam_mat = value; }
        }

        public Drawing2DCamera(Matrix4 draw2d_init_mat, Size window_size)
        {
            _cam_mat = draw2d_init_mat;
            rotate_offset_x = window_size.Width / 2.0f;
            rotate_offset_y = window_size.Height / 2.0f;
        }

        /// <summary>
        /// Get the camera matrix to be uploaded to drawing 2D
        /// </summary>
        public Matrix4 MatrixToDrawing2D()
        {
            Matrix4 cam_inverted = _cam_mat;
            cam_inverted.Invert();
            return cam_inverted;
        }

        // when doing a rotation we want to perform it around the screen center.
        public void RotateBy(double angle_degrees)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            // we would remove the translation in OpenGL because its screen center is at (0,0,0)
            // in 2D camera screen center is at (Width/2.0, Height/2.0)
            // so translate to screen center 
            _cam_mat = Matrix4.CreateTranslation(rotate_offset_x, rotate_offset_y, 0.0f) * _cam_mat;
            _cam_mat = Matrix4.CreateRotationZ(angle_radians) * _cam_mat;
            // translate back
            _cam_mat = Matrix4.CreateTranslation(-rotate_offset_x, -rotate_offset_y, 0.0f) * _cam_mat;
        }

        public void RotateByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.I:
                    RotateBy(-10);
                    break;
                case Keys.O:
                    RotateBy(10);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right (x > 0) we perform a clockwise rotation.
            if (x != 0)
            {
                RotateBy(x * _rotation_speed);
            }
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(int x, int y)
        {
            _cam_mat = Matrix4.CreateTranslation((_motion_speed * x), (_motion_speed * y), 0.0f) * _cam_mat;
        }

        public void MoveByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A:
                    MoveBy(1, 0);
                    break;
                case Keys.W:
                    MoveBy(0, 1);
                    break;
                case Keys.D:
                    MoveBy(-1, 0);
                    break;
                case Keys.S:
                    MoveBy(0, -1);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

    }

}