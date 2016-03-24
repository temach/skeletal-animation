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

        public SceneWrapper(Scene sc)
        {
            _inner = sc;
        }

        public Node FindNode(string node_name)
        {
            return InnerRecurFindNodeInScene(_inner.RootNode, node_name);
        }

        private Node InnerRecurFindNodeInScene(Node cur_node, string node_name)
        {
            if (cur_node.Name == node_name)
            {
                return cur_node;
            }
            foreach (var child in cur_node.Children)
            {
                var tmp =  InnerRecurFindNodeInScene(child, node_name);
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


    }
}
