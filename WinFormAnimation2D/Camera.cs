using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing.Drawing2D;

namespace WinFormAnimation2D
{
    abstract class AbsCamera
    {

        /// <summary>
        /// Motion in X-Y plane.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        abstract public void ProcessMouse(int x, int y);

        /// <summary>
        /// Scrolls along the camera z axis. In and out of the scene.
        /// </summary>
        /// <param name="z">Scroll factor.</param>
        abstract public void ProcessScroll(int z);
    }


    class Drawing2DCamera : AbsCamera
    {
        // Zoom parameters
        private readonly float ZoomSpeed = 1.00105f;
        // TODO: currently this limits do not work. They just affect the step size.
        private readonly float ZoomCloseLimit = 0.9f;
        private readonly float ZoomFarLimit = 10.0f;
        // this is like an initial zoom factor. In OpenGL zoom is
        // done by moving objects closer/further
        // in Drawing2D it is done by scaling. 
        // Actually in OpenGL we can use scaling as well. No? interesting idea....
        private readonly float InitZoom = 0.5f;

        // Rotation parameters
        private readonly float RotationSpeed = 1.5f;

        // we use matrix 4 even thougth MAtrix3 would also be ok.
        private Matrix _cam_mat = new Matrix();

        // Since Matrix is a class this get/set crap does not protect anything.
        public Matrix CamMatrix {
            get { return _cam_mat; }
            set { _cam_mat = value; }
        }

        public Drawing2DCamera()
        {
            _cam_mat.Scale(InitZoom, InitZoom);
        }

        /// <summary>
        /// Respond to mouse motion event by snapping the rotation 
        /// of the object to the specified delta.
        /// </summary>
        /// <param name="x">The absolute delta between mouse click and current mouse position</param>
        /// <param name="y">The absolute delta between mouse click and current mouse position</param>
        public override void ProcessMouse(int x, int y)
        {
            // when user pulls mouse to the right x > 0 so we perform a clockwise rotation.
            // along the global y axis
            if (x != 0)
            {
                _cam_mat = _cam_mat.eSnapRotate((float)(x * RotationSpeed));
                //_cam_mat.Rotate((float)(x * RotationSpeed * Math.PI / 180.0f));
            }
        }

        /// <summary>
        /// Scrolls along the camera z axis. In and out of the scene.
        /// </summary>
        /// <param name="z">Scroll factor.</param>
        public override void ProcessScroll(int z)
        {
            float factor = Math.Max((float)Math.Pow(ZoomSpeed, z), ZoomCloseLimit);
            _cam_mat.Scale(factor, factor);
        }
    }


    class OpenGLCamera : AbsCamera
    {
        // Camera's translation in Y axis:  -_y_limit < |camra.Row3.Y| < +_y_limit
        protected float _y_mod_limit = 0.8f;

        // Zoom parameters
        protected readonly float ZoomSpeed = 1.00105f;
        protected readonly float MinZeroDistance = 0.3f;

        // Rotation parameters
        protected const float RotationSpeed = 0.5f;
        protected const float InitialCameraDistance = 10.0f;

        // get or set the translation part of the matrix separately
        protected Vector3 _trans
        {
            get { return _cam_mat.Row3.Xyz; }
            set {
                // set the translation as given Vector3, last value
                // is 1.0 to keep last column in matrix as (0; 0; 0; 1)
                _cam_mat.Row3 = new Vector4(value, 1.0f);
            }
        }

        protected Matrix4 _cam_mat;

        public Matrix4 CamMatrix {
            get { return _cam_mat; }
        }

        public OpenGLCamera()
        {
            _cam_mat = Matrix4.CreateTranslation(0.0f, 0.0f, InitialCameraDistance);
            UpdateCameraMatrix();
        }

        public override void ProcessMouse(int x, int y)
        {
            // when user pulls mouse up we get y < 0 and we want to rotate "upwards"
            // So the angle is (y * RotoSpeed) (since FromAxisAngle does a clockwise 
            // rotoation when looking along the axis by default.
            if (y != 0)
            {
                // rotate around the direction of the camera's X-axis
                // we just calculate the matrix that would perform such a rotation
                var roto_cam_x = Matrix4.CreateFromAxisAngle( _cam_mat.Row0.Xyz
                    , (float)(y * RotationSpeed * Math.PI / 180.0)
                );

                // use that mtrix to change the camera's location.
                // Don't touch rotation yet. Leave that to UpdateCameraMatrix() function
                _trans = Vector3.TransformVector(_trans, roto_cam_x);
            }
            // when user pulls mouse to the right x > 0 so we perform a clockwise rotation.
            // along the global y axis
            if (x != 0)
            {
                var roto_global_y = Matrix4.CreateFromAxisAngle( Vector3.UnitY
                    , (float)(x * RotationSpeed * Math.PI / 180.0)
                );

                _trans = Vector3.TransformVector(_trans, roto_global_y);
            }

            // limit how high up can you go. This prevents swapping 
            // of axies and confusion, (and gimbal lock)
            if (_trans.Y > _y_mod_limit)
            {
                _trans = new Vector3(_trans.X, _y_mod_limit, _trans.Z);
            }
            else if (_trans.Y < (-1 * _y_mod_limit))
            {
                _trans = new Vector3(_trans.X, -1 * _y_mod_limit, _trans.Z);
            }

            // Configure the rotation so that's its looking at entity in the middle
            UpdateCameraMatrix();
        }

        /// <summary>
        /// Scrolls along the camera z axis. In and out of the scene.
        /// </summary>
        /// <param name="z">Scroll factor.</param>
        public override void ProcessScroll(int z)
        {
            float factor = Math.Max((float)Math.Pow(ZoomSpeed, -z), MinZeroDistance);
            _trans = Vector3.Multiply(_trans, factor);
            UpdateCameraMatrix();
        }

        /// <summary>
        /// Reorient (only rotations) the camera to look at entity in (0,0,0) position.
        /// </summary>
        private void UpdateCameraMatrix()
        {
            // var old_trans = _trans;
            // _trans.Normalize();
            // up vector is camera's Y-axis
            _cam_mat = Matrix4.LookAt(_trans, Vector3.Zero, _cam_mat.Row1.Xyz);
            // _cam_mat = Util.eMatrixLookAt(_trans * _cameraDistance, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            // // for some reason we don't want to change the translation part of the matrix
            // _trans = old_trans;
        }
    }
}