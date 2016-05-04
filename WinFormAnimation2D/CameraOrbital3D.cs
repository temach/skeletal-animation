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
    public class OrbitCameraController
    {
        private Matrix4 _view;
        private Matrix4 _viewWithOffset;
        private float _cameraDistance;
        private Vector3 _right;
        private Vector3 _up;
        private Vector3 _front;

        private Vector3 _panVector;

        private bool _dirty = true;

        private float ZoomSpeed = 2.00105f;
        private float MinimumCameraDistance = 0.1f;
        /// <summary>
        /// Rotation speed, in degrees per pixels
        /// </summary>
        private float RotationSpeed = 0.5f;
        private float PanSpeed = 2.0f; // 0.004f;
        private float InitialCameraDistance = 200.0f;

        private Vector3 _pivot;


        public OrbitCameraController()
        {
            // _view = Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 0.9f);
            _view = Matrix4.Identity;
            _viewWithOffset = Matrix4.Identity;
            _cameraDistance = InitialCameraDistance;
            _right = Vector3.UnitX;
            _up = Vector3.UnitY;
            _front = Vector3.UnitZ;
            SetOrbitOrConstrainedMode();           
        }

        public Matrix4 MatrixToOpenGL()
        {
            return this.GetView(); // this.GetView().Inverted();
        }

        public void SetPivot(Vector3 pivot)
        {
            _pivot = pivot;
            _dirty = true;
        }

        public Matrix4 GetView()
        {
            if (_dirty)
            {
                  UpdateViewMatrix();
            }
            return _viewWithOffset;
        }

        public void MouseMove(int x, int y)
        {
            if(x == 0 && y == 0)
            {
                return;
            }
            if (x != 0)
            {
                _view *= Matrix4.CreateFromAxisAngle(_up, (float)(x * RotationSpeed * Math.PI / 180.0));
            }
            if (y != 0)
            {
                _view *= Matrix4.CreateFromAxisAngle(_right, (float)(y * RotationSpeed * Math.PI / 180.0));
            }
            _dirty = true;
            SetOrbitOrConstrainedMode();
        }

        public void Scroll(float z)
        {
            _cameraDistance *= (float)Math.Pow(ZoomSpeed, -z);
            _cameraDistance = Math.Max(_cameraDistance, MinimumCameraDistance);
            _dirty = true;
        }

        public void Pan(float x, float y)
        {
            _panVector.X += x * PanSpeed;
            _panVector.Y += -y * PanSpeed;
            _dirty = true;
        }

        public void MovementKey(float x, float y, float z)
        {
            // TODO switch to FPS camera at current position?
        }

        public Vector3 _local_x { get { return _view.Row0.Xyz; } }
        public Vector3 _local_y { get { return _view.Row1.Xyz; } }
        public Vector3 _local_z { get { return _view.Row2.Xyz; } }
        public Vector3 _local_trans { get { return _view.Row3.Xyz; } }

        private void UpdateViewMatrix()
        {
            // for othagonal matrices T^(-1) = T^(transposed), so here we are applying a global rotation
            _viewWithOffset = Matrix4.LookAt(_view.Column2.Xyz * _cameraDistance + _pivot, _pivot, _view.Column1.Xyz);
            _viewWithOffset *= Matrix4.CreateTranslation(_panVector);
            _dirty = false;
        }

        /// Switches the camera controller between the X,Z,Y and Orbit modes.
        public void SetOrbitOrConstrainedMode()
        {
            _dirty = true;
        }

    }
}
