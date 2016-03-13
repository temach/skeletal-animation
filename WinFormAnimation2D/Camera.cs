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
    class Drawing2DCamera : INotifyPropertyChanged
    {
        // boiler-plate INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool UpdateField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        // end boiler-plate

        private readonly float _zoom_speed = 1.00105f;
        // TODO: currently this limits do not work. They just affect the step size.
        private readonly float _zoom_close_limit = 0.9f;
        private readonly float _zoom_far_limit = 10.0f;
        // this is like an initial zoom factor. In OpenGL zoom is
        // done by moving objects closer/further
        // in Drawing2D it is done by scaling. 
        // Actually in OpenGL we can use scaling as well. No? interesting idea....
        private readonly float _init_zoom = 0.9f;
        private readonly float _rotation_speed = 1.5f;
        private readonly float _motion_speed = 10.0f;
        private Matrix _cam_mat = new Matrix();

        public double GetRotationAngleDeg
        {
            get { return _cam_mat.eGetRotationAngle(); }
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            Debug.Assert(_cam_mat.IsInvertible == true, "can not change into world coordinates.");
            var tmp = _cam_mat.Clone();
            tmp.Invert();
            return tmp.eTransformPoint(screen_coords);
        }

        public Matrix CamMatrix {
            get { return _cam_mat; }
            set { _cam_mat = value; }
        }

        public Drawing2DCamera()
        {
            _cam_mat.Scale(_init_zoom, _init_zoom);
        }

        public void RotateBy(double angle_degrees)
        {
            _cam_mat.Rotate((float)angle_degrees);
            OnPropertyChanged("GetRotationAngleDeg");
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
            this.CamMatrix.Translate((_motion_speed * x), (_motion_speed * y));
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
        /// Scrolls along the camera z axis. In and out of the scene.
        /// </summary>
        /// <param name="z">Scroll factor.</param>
        public void ProcessScroll(int z)
        {
            float factor = Math.Max((float)Math.Pow(_zoom_speed, z), _zoom_close_limit);
            _cam_mat.Scale(factor, factor);
        }

    }

}