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
        // collada file stores armature as a separate node
        public Dictionary<string,BoneNode> _name2bone_node = new Dictionary<string,BoneNode>();
        public Dictionary<string,Node> _name2node = new Dictionary<string, Node>();
        public Dictionary<int,MeshDraw> _mesh_id2mesh_draw = new Dictionary<int, MeshDraw>();

        public SceneWrapper(Scene sc)
        {
            _inner = sc;
            InnerBuildNodeDict(sc.RootNode);
            MakeMeshDraw(sc.Meshes);
        }

        // Make a class that will be responsible for managind the buffer lists
        public void MakeMeshDraw(IList<Mesh> meshes)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                _mesh_id2mesh_draw[i] = new MeshDraw(meshes[i]);
            }
        }

        public void InnerBuildNodeDict(Node nd)
        {
            _name2node[nd.Name] = nd;
            foreach (var child in nd.Children)
            {
                InnerBuildNodeDict(child);
            }
        }

        public Node GetNode(string name)
        {
            return _name2node[name];
        }

        public BoneNode GetBoneNode(string node_name)
        {
            return _name2bone_node[node_name];
        }

        public BoneNode BuildBoneNodes(string armature_root_name)
        {
            Node armature_root = InnerRecurFindNode(_inner.RootNode, armature_root_name);
            BoneNode root = InnerRecurBuildBones(armature_root);
            return root;
        }

        private BoneNode InnerRecurBuildBones(Node nd)
        {
            var current = new BoneNode(nd);
            current.GlobTrans = GetNodeGlobalTransform(nd);
            current.LocTrans = nd.Transform;
            // add to dict for faster lookup
            _name2bone_node[nd.Name] = current;
            foreach (var child in nd.Children)
            {
                BoneNode w_child = InnerRecurBuildBones(child);
                w_child.Parent = current;
                current.Children.Add(w_child);
            }
            return current;
        }

        /// <summary>
        /// Get the bone transform and trace back its changes
        /// </summary>
        /// <param name="nd"></param>
        /// <returns></returns>
        public Matrix4x4 GetNodeGlobalTransform(Node nd)
        {
            Matrix4x4 ret = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            ret *= nd.Transform;
            Node cur = nd;
            while (cur.Parent != null)
            {
                ret *= cur.Parent.Transform;
                cur = cur.Parent;
            }
            return ret;
        }

        // Use only NodeWrapper in the interface to outside
        // Deprecated use GetNode
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
