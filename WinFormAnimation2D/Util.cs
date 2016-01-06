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
        public static Pen pp3 = new Pen(Color.Red, 5.0f);

        /// <summary>
        /// Convert assimp 4 by 4 matrix into 3 by 2 matrix from System.Drawing.Drawing2D and use it
        /// for drawing with Graphics object.
        /// </summary>
        public static draw2D.Matrix To3x2(this assimp.Matrix4x4 m)
        {
            return new draw2D.Matrix(m.A1, m.B1, m.A2, m.B2, m.A4, m.B4);
            // return new draw2D.Matrix(m[0, 0], m[1, 0], m[0, 1], m[1, 1], m[0, 3], m[1, 3]);
        }

        /// <summary>
        /// Draw circle with Graphics from point and radius.
        /// </summary>
        public static void DrawCircle(this Graphics g, Pen pen, Point p, int rad)
        {
            Rectangle rect = new Rectangle(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(pen, rect);
        }

        /// <summary>
        /// Debug function to quickly draw points with Graphics
        /// </summary>
        public static void DrawPoint(this Graphics g, Point p)
        {
            int rad = 1;        // radius
            Rectangle rect = new Rectangle(p.X - rad, p.Y - rad, 2 * rad, 2 * rad);
            g.DrawEllipse(Util.pp3, rect);
        }

        /// <summary>
        /// Convert assimp 3D vector to 2D System.Drawing.Point
        /// for drawing with Graphics object.
        /// </summary>
        public static Point ToPoint(this assimp.Vector3D v)
        {
            return new Point((int)v.X, (int)v.Y);
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
