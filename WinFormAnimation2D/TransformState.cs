using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;        // for MemoryStream
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using Quaternion = Assimp.Quaternion;

namespace WinFormAnimation2D
{

    /// <summary>
    /// Implement this when class allows local matrix transforms.
    /// (Entity, Camera)
    /// </summary>
    interface ITransformState
    {
        TransformState Transform { get; }
        Vector3 GetTranslation { get; }

        // void RotateBy(double angle_degrees, Vector3 axis);
        void RotateBy2D(double angle_degrees);
        void RotateByKey2D(KeyEventArgs e);

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        // void MoveBy(Vector3 direction);
        void MoveBy2D(int x, int y);
        void MoveByKey2D(KeyEventArgs e);
    }

    class TransformState
    {
        public float _move_speed;
        public float _roto_speed_degrees;

        public Matrix4 _matrix = Matrix4.Identity;
        public Matrix4x4 _ai_matrix
        {
            get { return _matrix.eToAssimp(); }
            set { _matrix = value.eToOpenTK(); }
        }

        public TransformState(Matrix4 init_matrix, double motion_speed, double rotation_speed_degrees)
        {
            _matrix = init_matrix;
            _move_speed = (float)motion_speed;
            _roto_speed_degrees = (float)rotation_speed_degrees;
        }

        public Vector3 GetTranslation
        {
            get { return _matrix.ExtractTranslation(); }
        }
        public Vector2 GetTranslation2D
        {
            get { return _matrix.ExtractTranslation().eTo2D(); }
        }

        public void RotateBy2D(double angle_degrees)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            _matrix = Matrix4.CreateRotationZ(angle_radians) * _matrix;
        }
        public void RotateBy(double angle_degrees, Vector3 axis)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            _matrix = Matrix4.CreateFromAxisAngle(axis, angle_radians) * _matrix;
        }

        public void RotateByKey2D(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.I:
                    RotateBy2D(_roto_speed_degrees);
                    break;
                case Keys.O:
                    RotateBy2D(-1 * _roto_speed_degrees);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        public void MoveBy(Vector3 direction)
        {
            direction.Normalize();
            direction = Vector3.Multiply(direction, _move_speed);
            _matrix = Matrix4.CreateTranslation(direction.X, direction.Y, direction.Z) * _matrix;
        }
        public void MoveBy2D(int x, int y)
        {
            _matrix = Matrix4.CreateTranslation((_move_speed * x), (_move_speed * y), 0.0f) * _matrix;
        }

        public void MoveByKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A:
                    MoveBy2D(-1, 0);
                    break;
                case Keys.W:
                    MoveBy2D(0, -1);
                    break;
                case Keys.D:
                    MoveBy2D(1, 0);
                    break;
                case Keys.S:
                    MoveBy2D(0, 1);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

    } 

}
