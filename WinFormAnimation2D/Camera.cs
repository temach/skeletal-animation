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
        public CameraFreeFly3D _3d_freefly;
        public OrbitCameraController _3d_orbital;

        public Vector3 GetTranslation
        {
            get
            { return (_cam_mode == CamMode.Orbital) 
                    ? _3d_orbital.GetTranslation 
                    : _3d_freefly.GetTranslation;
            }
        }

        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        public Vector3 ConvertScreen2WorldCoordinates(Point screen_coords)
        {
            return Vector3.Zero;
        }

        public CameraDevice(Matrix4 opengl_init_mat)
        {
            _3d_freefly = new CameraFreeFly3D(opengl_init_mat);
            _3d_orbital = new OrbitCameraController();
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
            _3d_freefly.ClockwiseRotateAroundAxis(axis);
            _3d_orbital.MouseMove((int)axis.X, (int)axis.Y);
            _3d_orbital.Scroll(axis.Z);
        }

        public void OnMouseMove(int x, int y)
        {
            _3d_freefly.ProcessMouse(x, y);
            _3d_orbital.MouseMove(x, y);
        }

        public void Scroll(float scroll)
        {
            _3d_freefly.MoveBy(new Vector3(0, 0, -1 * scroll));
            _3d_orbital.Scroll(scroll);
        }

        // x,y are direction parameters one of {-1, 0, 1}
        public void MoveBy(Vector3 direction)
        {
            _3d_freefly.MoveBy(direction);
            _3d_orbital.Pan(direction.X, direction.Y);
        }

    }

}