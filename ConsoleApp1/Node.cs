using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapegoat
{
    class Node
    {
        public int key;
        public Node left, right, parent;

        public Node(int key)
        {
            this.key = key;
            left = null;
            right = null;
            parent = null;
        }
    }
}
