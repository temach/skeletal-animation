﻿using Assimp;
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
    public enum TreeNodeType
    {
        Entity = 0
        , Mesh
        , TriangleFace
        , Armature
        , Other
        ,
    }

    interface IHighlightableNode
    {
        void Render();
    }

    class SceneTreeNode : TreeNode, IHighlightableNode
    {
        public TreeNodeType NodeType = TreeNodeType.Other;

        public void Render()
        {
        }

        public SceneTreeNode(string name)
        {
            this.Name = name;
            this.Text = name;
        }
    }

    class EntityTreeNode : TreeNode, IHighlightableNode
    {
        public TreeNodeType NodeType = TreeNodeType.Entity;
        public BoundingBoxGroup DrawMeshBounds;

        public void Render()
        {
            DrawMeshBounds.OverallBox.Render();
        }
        public EntityTreeNode(string name)
        {
            this.Name = name;
            this.Text = name;
        }
    }

    class MeshTreeNode : TreeNode, IHighlightableNode
    {
        public TreeNodeType NodeType = TreeNodeType.Mesh;
        public BoundingBoxGroup DrawData;

        public void Render()
        {
            DrawData.OverallBox.Render();
        }

        public MeshTreeNode(string name)
        {
            this.Name = name;
            this.Text = name;
        }
    }

    class ArmatureTreeNode : TreeNode, IHighlightableNode
    {
        public TreeNodeType NodeType = TreeNodeType.Armature;
        public BoneBounds DrawData;

        public void Render()
        {
            DrawData.Render();
        }
        public ArmatureTreeNode(string name)
        {
            this.Name = name;
            this.Text = name;
        }
    }

}
