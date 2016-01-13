using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using assimp = Assimp;
using Assimp.Configs;
using System.Windows.Forms;
using System.Drawing;
using draw2D = System.Drawing.Drawing2D;

namespace WinFormAnimation2D
{
    static class Util
    {
        // Useful for random value generation. 
        // We use only one Random instance in the whole program.
        static Random rand = new Random();

                
        static IEnumerator<Brush> iter_randbrush = GetRandBrush().GetEnumerator();
        static Brush NextRandBrush
        {
            get
            {
                if (iter_randbrush.MoveNext())
                {
                    return iter_randbrush.Current;
                }
                else
                {
                    return Brushes.Red;
                }
            }
        }

        // Static config fields
        public static float stepsize = 10.0f;
        public static double epsilon = 1E-8;

        /// <summary>
        /// Big + Green pen to render points on screen
        /// </summary>
        public static Pen pp1 = new Pen(Color.LawnGreen, 20.0f);

        /// <summary>
        /// Medium + Black pen to render points on screen
        /// </summary>
        public static Pen pp2 = new Pen(Color.Black, 10.0f);

        /// <summary>
        /// Small + Red pen to render points on screen
        /// </summary>
        public static Pen pp3 = new Pen(Color.Red, 0.01f);

        /// <summary>
        /// Convert assimp 4 by 4 matrix into 3 by 2 matrix from System.Drawing.Drawing2D and use it
        /// for drawing with Graphics object.
        /// </summary>
        public static draw2D.Matrix eTo3x2(this assimp.Matrix4x4 m)
        {
            return new draw2D.Matrix(m.A1, m.B1, m.A2, m.B2, m.A4, m.B4);
            // return new draw2D.Matrix(m[0, 0], m[1, 0], m[0, 1], m[1, 1], m[0, 3], m[1, 3]);
        }

        /// <summary>
        /// Draw circle with Graphics from point and radius.
        /// </summary>
        public static void eDrawCircle(this Graphics g, Pen pen, Point p, int rad)
        {
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(pen, rect);
        }

        /// <summary>
        /// Debug function to quickly draw points with Graphics
        /// </summary>
        public static void eDrawPoint(this Graphics g, Point p)
        {
            float rad = 0.3f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Debug function to quickly draw __floating__ points with Graphics
        /// </summary>
        public static void eDrawPoint(this Graphics g, PointF p)
        {
            float rad = 0.03f;        // radius
            var rect = new RectangleF(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Convert assimp 3D vector to 2D System.Drawing.Point
        /// for drawing with Graphics object.
        /// </summary>
        public static Point eToPoint(this assimp.Vector3D v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Convert assimp 3D vector to 2D System.Drawing.PointF (floating point)
        /// for drawing with Graphics object.
        /// </summary>
        public static PointF eToPointFloat(this assimp.Vector3D v)
        {
            return new PointF(v.X, v.Y);
        }

        public static draw2D.Matrix eNormaliseScale(this draw2D.Matrix mat)
        {
            var curmat = mat.Elements;

            // normalise the x and y axis to set scale to 1.0f
            var x_axis = new assimp.Vector2D(curmat[0], curmat[1]);
            var y_axis = new assimp.Vector2D(curmat[2], curmat[3]);
            x_axis.Normalize();
            y_axis.Normalize();

            // make new matrix with scale of 1.0f 
            // Do not change the translation
            var newmat = new draw2D.Matrix(x_axis[0], x_axis[1]
                , y_axis[0], y_axis[1]
                , curmat[4], curmat[5]
            );

            return newmat.Clone();
        }

        /// <summary>
        /// Get a brush of random color.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Brush> GetRandBrush()
        {
            // Note that this is Unsigned int (so overflow is ok)
            uint rand_brush_count = 0;
            while (true)
            {
                rand_brush_count++;
                switch (rand_brush_count % 4)
                {
                    case 0:
                        yield return Brushes.LawnGreen; break;
                    case 1:
                        yield return Brushes.Green; break;
                    case 2:
                        yield return Brushes.GreenYellow; break;
                    case 3:
                        yield return Brushes.LightSeaGreen; break;
                    default:
                        yield return Brushes.Red; break;
                }
            }
        }



    }
}
/**************************

    Matrix Assimp2Drawing(Matrix4x4 assimp_matrix)
    {
        return new Matrix(assimp_matrix.)
    }


    //-------------------------------------------------------------------------------------------------
    // Render the scene.
    // Begin at the root node of the imported data and traverse
    // the scenegraph by multiplying subsequent local transforms
    // together on OpenGL matrix stack.
    void RecursiveRender(Graphics g, Node nd)
    {
        int i = 0;
        int n = 0, t = 0;
        Matrix4x4 mat44 = nd.Transform;

        mat44.Transpose();
        GraphicsState st1 = g.Save();
        Matrix k = null;
        g.MultiplyTransform()
        g.MultiplyTransform()
            g.MultiplyTransform(mat44);
        glMultMatrixf((float *) &mat44);

        // draw all meshes assigned to this node
        for (n = 0; n < nd.mNumMeshes; ++n) {
            const struct aiMesh* mesh = Current_Scene->mMeshes[nd->mMeshes[n]];

            ApplyMaterial(Current_Scene->mMaterials[mesh->mMaterialIndex]);

            if(mesh->mNormals == NULL) {
                glDisable(GL_LIGHTING);
            } else {
                glEnable(GL_LIGHTING);
            }

            for (t = 0; t < mesh->mNumFaces; ++t)
            {
                const struct aiFace* face = &mesh->mFaces[t];
                GLenum face_mode;

                switch(face->mNumIndices)
                {
                    case 1: face_mode = GL_POINTS; break;
                    case 2: face_mode = GL_LINES; break;
                    case 3: face_mode = GL_TRIANGLES; break;
                    default: face_mode = GL_POLYGON; break;
                }

                glBegin(face_mode);

                for (i = 0; i < face->mNumIndices; i++)
                {
                    unsigned int index = face->mIndices[i];
                    if (mesh->mNormals != NULL) {
                        glNormal3fv(&mesh->mNormals[index].x);
                    }
                    glVertex3fv(&mesh->mVertices[index].x);
                }

                glEnd();
            }
        }

        // draw all children
        for (n = 0; n < nd->mNumChildren; ++n) {
            RecursiveRender(nd->mChildren[n]);
        }

        glPopMatrix();
    }


**************************/
