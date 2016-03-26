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
    class Drawing2DCamera
    {

        private readonly float _zoom_speed = 1.00105f;
        // TODO: currently this limits do not work. They just affect the step size.
        private readonly float _zoom_close_limit = 0.9f;
        private readonly float _zoom_far_limit = 10.0f;
        // this is like an initial zoom factor. In OpenGL zoom is
        // done by moving objects closer/further
        // in Drawing2D it is done by scaling. 
        // Actually in OpenGL we can use scaling as well. No? interesting idea....
        private readonly float _rotation_speed = 1.5f;
        private readonly float _motion_speed = 10.0f;
        private Matrix _cam_mat;

        public double GetRotationAngleDeg
        {
            get { return _cam_mat.eGetRotationAngle(); }
        }
        public Point GetTranslation
        {
            get { return Point.Round(_cam_mat.eGetTranslationPoint()); }
        }

        /// <summary>
        /// Get the mouse position and calculate the world coordinates based on the screen coordinates.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PointF ConvertScreen2WorldCoordinates(PointF screen_coords)
        {
            return _cam_mat.eTransformSinglePointF(screen_coords);
        }

        public Matrix CamMatrix {
            get { return _cam_mat; }
            set { _cam_mat = value; }
        }

        public Drawing2DCamera(Matrix init_mat)
        {
            _cam_mat = init_mat;
        }

        /// <summary>
        /// Get the camera matrix to be uploaded to drawing 2D
        /// </summary>
        public Matrix MatrixToDrawing2D()
        {
            Matrix cam_inverted = CamMatrix.Clone();
            // this translation is only needed in 2D where camera is not positioned at (0,0)
            //cam_inverted.Translate(-360.5f, -233f);
            cam_inverted.Invert();
            return cam_inverted;
        }

        public void RotateBy(double angle_degrees)
        {
            // rotation is applied last
            //Vector2 cur_pos = new Vector2(_cam_mat.OffsetX, _cam_mat.OffsetY);
            // translate _half_ a screen back to the origin.
            _cam_mat.Translate(360.5f,233f);
            // do rotation
            _cam_mat.Rotate((float)angle_degrees);
            _cam_mat.Translate(-360.5f,-233f);
            // translate back half a screen
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
            this.CamMatrix.Translate((_motion_speed * x), (_motion_speed * y));
        }

        public void MoveByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    MoveBy(1, 0);
                    break;
                case Keys.Up:
                    MoveBy(0, 1);
                    break;
                case Keys.Right:
                    MoveBy(-1, 0);
                    break;
                case Keys.Down:
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

}