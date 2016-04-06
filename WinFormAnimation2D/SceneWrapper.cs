using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Assimp;
using OpenTK;
using Quaternion = Assimp.Quaternion;

namespace WinFormAnimation2D
{
    class SceneWrapper
    {
        public Scene _inner;
        public Dictionary<Node,NodeWrapper> _node2wrapper = new Dictionary<Node,NodeWrapper>();

        public SceneWrapper(Scene sc)
        {
            _inner = sc;
        }

        public NodeWrapper GetWrapperForNode(Node nd)
        {
            return _node2wrapper[nd];
        }

        public NodeWrapper BuildArmatureWrapper(string armature_root_name)
        {
            Node armature_root = InnerRecurFindNode(_inner.RootNode, armature_root_name);
            NodeWrapper root = InnerRecurBuildArmature(armature_root);
            return root;
        }

        private NodeWrapper InnerRecurBuildArmature(Node nd)
        {
            var w_current = new NodeWrapper(nd);
            w_current.GlobTrans = GetArmatureGlobalTransform(nd);
            w_current.LocTrans = nd.Transform;
            // add to dict for faster lookup
            _node2wrapper[nd] = w_current;
            foreach (var child in nd.Children)
            {
                NodeWrapper w_child = InnerRecurBuildArmature(child);
                w_child.Parent = w_current;
                w_current.Children.Add(w_child);
            }
            return w_current;
        }

        /// <summary>
        /// Get the armature node and trace back its changes
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        public Matrix4x4 GetArmatureGlobalTransform(Node bone)
        {
            Matrix4x4 ret = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            ret *= bone.Transform;
            Node cur = bone;
            while (cur.Parent != null)
            {
                ret *= cur.Parent.Transform;
                cur = cur.Parent;
            }
            return ret;
        }

        // for getting the bone nodes
        public NodeWrapper FindNodeWrapper(string node_name)
        {
            return GetWrapperForNode(FindNode(node_name));
        }

        // Use only NodeWrapper in the interface to outside
        public Node FindNode(string node_name)
        {
            return InnerRecurFindNode(_inner.RootNode, node_name);
        }

        private Node InnerRecurFindNode(Node cur_node, string node_name)
        {
            if (cur_node.Name == node_name)
            {
                return cur_node;
            }
            foreach (var child in cur_node.Children)
            {
                var tmp =  InnerRecurFindNode(child, node_name);
                if (tmp != null)
                {
                    return tmp;
                }
            }
            return null;
        }

        /// <summary>
        /// Make sure that all meshes are named.
        /// </summary>
        public void NameUnnamedMeshes()
        {
            for (int i = 0; i < _inner.MeshCount; i++)
            {
                Mesh mesh = _inner.Meshes[i];
                if (mesh.Name.Length == 0)
                {
                    mesh.Name = i.ToString();
                }
            }
        }

        public void NodeNamesAreUnique()
        {
            var name_set = new HashSet<string>();
            InnerRecurCheckNamesUnique(_inner.RootNode, name_set);
        }

        public void InnerRecurCheckNamesUnique(Node nd, HashSet<string> nd_names)
        {
            if (nd_names.Contains(nd.Name))
            {
                throw new Exception("Node names in scene are not unique. Can not proceed.");
            }
            else
            {
                nd_names.Add(nd.Name);
            }
            foreach (var child in nd.Children)
            {
                InnerRecurCheckNamesUnique(child, nd_names);
            }
        }

    }
}
