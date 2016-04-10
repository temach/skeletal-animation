using Assimp;
using Assimp.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WinFormAnimation2D
{
    public enum NodeType
    {
        Entity = 0,
        Mesh,
        TriangleFace,
        Other,
    }

    class CustomTreeNode : TreeNode
    {
        public NodeType NodeType;
        public AxiAlignedBoundingBox DrawData;

        public CustomTreeNode(NodeType nt)
        {
            this.NodeType = nt;
        }
    }

}
