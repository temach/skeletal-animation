using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace WinFormAnimation2D
{
    class Camera
    {
        private Matrix4x4 _cam_mat;
        private float _cameraDistance;

        // Zoom parameters
        private readonly float ZoomSpeed = 1.00105f;
        private readonly float MinZeroDistance = 0.1f;

        // Rotation parameters
        private const float RotationSpeed = 0.5f;
        private const float InitialCameraDistance = 10.0f;

        public Matrix4x4 CamMatrix {
            get { return _cam_mat; }
        }

        // get or set the translation part of the matrix separately
        private Vector3D _trans
        {
            get { return new Vector3D(_cam_mat.D1, _cam_mat.D2, _cam_mat.D3); }
            set {
                _cam_mat.D1 = value.X;
                _cam_mat.D2 = value.Y;
                _cam_mat.D3 = value.Z;
            }
        }

        public Camera()
        {
            _cam_mat = Matrix4x4.FromTranslation(new Vector3D(0.0f, 0.0f, 1.0f));
            _cameraDistance = InitialCameraDistance;

            UpdateViewMatrix();
        }

        public void ProcessMouse(int x, int y)
        {
            var unit_y = new Vector3D(0.0f, 1.0f, 0.0f);
            if (y != 0)
            {
                var matrix = Matrix4x4.FromAngleAxis( 
                    (float)(y * RotationSpeed * Math.PI / 180.0)
                    , Vector3D.Cross(_trans, unit_y)
                );
                _trans = matrix.eTransformVector(_trans);
            }

            if (x != 0)
            {
                var matrix = Matrix4x4.FromAngleAxis( 
                    (float)(-x * RotationSpeed * Math.PI / 180.0)
                    , unit_y
                );
                _trans = matrix.eTransformVector(_trans);
            }

            // limit how high up can you go. This prevents swapping 
            // of axies and confusion, (and gimbal lock)
            if (_trans.Y > 0.8f)
            {
                _trans = new Vector3D(_trans.X, 0.8f, _trans.Z);
            }
            if (_trans.Y < -0.8f)
            {
                _trans = new Vector3D(_trans.X, -0.8f, _trans.Z);
            }
               
            UpdateViewMatrix();
        }

        public void Scroll(int z)
        {
            _cameraDistance *= (float)Math.Pow(ZoomSpeed, -z);
            _cameraDistance = Math.Max(_cameraDistance, MinimumCameraDistance);
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            var old_trans = _trans;
            _cam_mat = Matrix4x4.Lo
            _view = Matrix4.LookAt(_offset * _cameraDistance, Vector3.Zero, Vector3.UnitY);
        }
    }
}
