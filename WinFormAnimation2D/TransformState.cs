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
        void RotateByKey(KeyEventArgs e);

        // x,y,z should be direction parameters, one of {-1, 0, 1}
        // void MoveBy(int x, int y);
        void MoveBy(Vector3 direction);
        void MoveByKey(KeyEventArgs e);
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
            RotateAroundAxis(angle_degrees, Vector3.UnitX);
        }
        public void RotateAroundAxis(double angle_degrees, Vector3 axis)
        {
            float angle_radians = (float)(angle_degrees * Math.PI / 180.0);
            _matrix = Matrix4.CreateFromAxisAngle(axis, angle_radians) * _matrix;
        }

        public double GetAngleDegreesFromKeyEventArg(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.I:
                case Keys.K:
                case Keys.Oemcomma:
                    return RotateSpeedDegrees;
                case Keys.O:
                case Keys.L:
                case Keys.OemPeriod:
                    return -1 * RotateSpeedDegrees;
                default:
                    Debug.Assert(false);
                    break;
            }
            return double.NaN;
        }

        public Vector3 GetRotationAxisFromKeys(KeyEventArgs e)
        {
            // we must use global vectors here, becasue we pre-multiply the cmarea matrix and then invert it
            switch (e.KeyData)
            {
                case Keys.I:
                case Keys.O:
                    // return _local_x;
                    return Vector3.UnitX;
                case Keys.K:
                case Keys.L:
                    // return _local_y;
                    return Vector3.UnitY;
                case Keys.Oemcomma:
                case Keys.OemPeriod:
                    // return _local_z;
                    return Vector3.UnitZ;
                default:
                    Debug.Assert(false);
                    break;
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
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

        public Vector3 GetDirectionNormalizedFromKey(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A:
                    return new Vector3(-1, 0, 0);
                case Keys.D:
                    return new Vector3(1, 0, 0);
                case Keys.W:
                    return new Vector3(0, -1, 0);
                case Keys.S:
                    return new Vector3(0, 1, 0);
                case Keys.E:
                    return new Vector3(0, 0, -1);
                case Keys.Q:
                    return new Vector3(0, 0, 1);
                default:
                    Debug.Assert(false);
                    break;
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

    } 

}
