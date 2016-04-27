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
        void RotateBy(double angle_degrees);

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        void MoveBy(Vector3 direction);
    }

    class TransformState
    {
        public float MoveSpeed;
        public float RotateSpeedDegrees;

        public Matrix4 _matrix = Matrix4.Identity;
        public Matrix4x4 _ai_matrix
        {
            get { return _matrix.eToAssimp(); }
            set { _matrix = value.eToOpenTK(); }
        }

        public TransformState(Matrix4 init_matrix, double motion_speed, double rotation_speed_degrees)
        {
            _matrix = init_matrix;
            MoveSpeed = (float)motion_speed;
            RotateSpeedDegrees = (float)rotation_speed_degrees;
        }

        public Vector3 GetTranslation
        {
            get { return _matrix.ExtractTranslation(); }
        }
        public Vector2 GetTranslation2D
        {
            get { return _matrix.ExtractTranslation().eTo2D(); }
        }

        public void Rotate(double angle_degrees)
        {
            // we must use global vectors here, becasue we pre-multiply the camera matrix and then invert it
            RotateAroundAxis(angle_degrees, Vector3.UnitX);
        }
        public void RotateAroundAxis(double angle_degrees, Vector3 axis)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            _matrix = Matrix4.CreateFromAxisAngle(axis, angle_radians) * _matrix;
        }

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        public Vector3 TranslationFromDirectionInPlaneYZ(Vector2 direction)
        {
            // becasue we move perpendicular to camera direction, i.e. perpendicular to camera's X axis
            Vector3 _local_y = _matrix.Row1.Xyz;
            Vector3 _local_z = _matrix.Row2.Xyz;
            Vector3 dir = direction.X * _local_y + direction.Y * _local_z;
            return Vector3.Multiply(dir, MoveSpeed);
        }

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        public Vector3 TranslationFromDirection(Vector3 direction)
        {
            Debug.Assert(direction.Length > 0);
            direction.Normalize();
            return Vector3.Multiply(direction, MoveSpeed);
        }

        public void ApplyTranslation(Vector3 trans)
        {
            _matrix = Matrix4.CreateTranslation(trans) * _matrix;
        }

        public void MoveBy(Vector3 direction)
        {
            Vector3 trans = TranslationFromDirection(direction);
            ApplyTranslation(trans);
        }

    } 

}
