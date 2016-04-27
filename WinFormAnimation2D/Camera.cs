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
        public CameraDrawing2D _2d_camera;
        public CameraFreeFly3D _3d_camera;

        public Vector2 GetTranslation
        {
            get { return _2d_camera.GetTranslation2D; }
        }

        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            return _2d_camera.ConvertScreen2WorldCoordinates(screen_coords);
        }

        public CameraDevice(Matrix4 draw2d_init_mat, Size window_size, Matrix4 opengl_init_mat)
        {
            _2d_camera = new CameraDrawing2D(draw2d_init_mat, window_size);
            _3d_camera = new CameraFreeFly3D(opengl_init_mat);
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

        public void RotateBy(double angle_degrees, Vector3 axis)
        {
            _2d_camera.RotateAroundScreenCenter2D(angle_degrees);
            _3d_camera.RotateByAxis(angle_degrees, axis);
        }

        public void ProcessMouse(int x, int y)
        {
            _2d_camera.ProcessMouse(x, y);
            _3d_camera.ProcessMouse(x, y);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(Vector3 direction)
        {
            _2d_camera.MoveBy(direction);
            _3d_camera.MoveBy(direction);
        }

    }

}