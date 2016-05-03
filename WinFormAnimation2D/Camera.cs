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

    enum CamMode
    {
        FreeFly
        , Orbital
    }

    /// <summary>
    /// Maintains camera abstraction.
    /// </summary>
    class CameraDevice
    {
        public CamMode _cam_mode
        {
            get { return Properties.Settings.Default.OrbitingCamera ? CamMode.Orbital : CamMode.FreeFly; }
        }
        public CameraDrawing2D _2d;
        public CameraFreeFly3D _3d_freefly;
        public  OrbitCameraController _3d_orbital;

        public Vector2 GetTranslation
        {
            get { return _2d.GetTranslation2D; }
        }

        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            return _2d.ConvertScreen2WorldCoordinates(screen_coords);
        }

        public CameraDevice(Matrix4 draw2d_init_mat, Size window_size, Matrix4 opengl_init_mat)
        {
            _2d = new CameraDrawing2D(draw2d_init_mat, window_size);
            _3d_freefly = new CameraFreeFly3D(opengl_init_mat);
            _3d_orbital = new OrbitCameraController();
            // _3d_orbital = new CameraOrbital3D(opengl_init_mat);
        }

        /// Get the camera matrix to be uploaded to drawing 2D
        public Matrix4 MatrixToDrawing2D()
        {
            return _2d.MatrixToDrawing2D();
        }

        /// Get the camera matrix to be uploaded to drawing 2D
        public Matrix4 MatrixToOpenGL()
        {
            return _cam_mode == CamMode.Orbital 
                    ? _3d_orbital.MatrixToOpenGL() 
                    : _3d_freefly.MatrixToOpenGL();
        }

        public void RotateAround(Vector3 axis)
        {
            bool has_neg = (axis.X < 0) || (axis.Y < 0) || (axis.Z < 0);
            float direction = has_neg ? -1 : 1;
            _2d.RotateAroundScreenCenter2D(direction);
            _3d_freefly.ClockwiseRotateAroundAxis(axis);
            //_3d_orbital.RotateByAxis(angle_degrees, axis);
            // _3d_orbital.RotateByAxis(angle_degrees, axis);
            // _3d_orbital.Pan(axis.X, axis.Y);
            _3d_orbital.MouseMove((int)axis.X, (int)axis.Y);
            _3d_orbital.Scroll(axis.Z);
        }

        public void ProcessMouse(int x, int y)
        {
            _2d.ProcessMouse(x, y);
            _3d_freefly.ProcessMouse(x, y);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(Vector3 direction)
        {
            _2d.MoveBy(direction);
            _3d_freefly.MoveBy(direction);
            _3d_orbital.Pan(direction.X, direction.Y);
            // _3d_orbital.MoveBy(direction);
            // _3d_orbital.MoveBy(direction);
        }

    }

}