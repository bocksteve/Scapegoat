using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapegoat
{
    class Tree
    {
        public double size;
        public double max_size;
        public Node root;
        public float alpha;       //the alpha weight value between 0.5 and 1

        public Tree(int key, float alpha)
        {
            this.alpha = alpha;
            root = new Node(key);
            size = 1;
            max_size = 1;
        }

        public void Insert(int key)
        {
            Node cur = root;
            int depth = 0;
            while (cur != null)
            {
                depth++;
                if (cur.key == key)
                {
                    Console.WriteLine("ERROR: Key {0} already exists", key);
                    return;
                }
                else if (cur.key < key)
                {
                    //key is greater then current node's key, go right
                    if (cur.right == null)
                    {
                        cur.right = new Node(key);
                        cur.right.parent = cur;
                        size++;
                        if (size > max_size)
                        {
                            max_size = size;
                        }
                        //Checking if new node is deep, Log(1/alpha) size;
                        double check = Math.Log(size, (1/alpha));
                        if (depth > check)
                        {
                            //Rebalance at root
                            //Console.WriteLine("depth = {0}, check = {1}", depth, check);
                            //Console.WriteLine("Needs rebalance at below");
                            root = Rebalance(root);
                        }
                        //Console.WriteLine("Inserted {0} into tree", key);
                        return;
                    }
                    cur = cur.right;
                }
                else
                {
                    //key is less then current node's key, go left
                    if (cur.left == null)
                    {
                        cur.left = new Node(key);
                        cur.left.parent = cur;
                        size++;
                        if (size > max_size)
                        {
                            max_size = size;
                        }
                        double check = Math.Log(size, (1 / alpha));
                        if (depth > check)
                        {
                            //Rebalance at root
                            //Console.WriteLine("depth = {0}, check = {1}", depth, check);
                            //Console.WriteLine("Needs rebalance at below");
                            root = Rebalance(root);
                        }
                        //Console.WriteLine("Inserted {0} into tree", key);
                        return;
                    }
                    cur = cur.left;
                }
            }
            //Shouldn't be here
            Console.WriteLine("Out of while loop while trying to insert {0}", key);
            return;
        }

        public void Search(int key)
        {
            Node cur = root;
            while (cur != null)
            {
                if (cur.key == key)
                {
                    Console.WriteLine("Key {0} found!", key);
                    return;
                }
                else if (cur.key < key)
                {
                    //key is greater then current node's key, go right
                    if (cur.right == null)
                    {
                        Console.WriteLine("Key {0} not found...", key);
                        return;
                    }
                    cur = cur.right;
                }
                else
                {
                    //key is less then current node's key, go left
                    if (cur.left == null)
                    {
                        Console.WriteLine("Key {0} not found...", key);
                        return;
                    }
                    cur = cur.left;
                }
            }
            //if we do happen to get down here, which we shouldn't, key was not found
            Console.WriteLine("Key {0} not found... (out of while loop)", key);
            return;
        }

        public void Delete(int key)
        {
            Node cur = root;
            while (cur != null)
            {
                if (cur.key == key)
                {
                    //Console.WriteLine("Key {0} found!  Deleting now", key);
                    if ((cur.left == null) && (cur.right == null))
                    {
                        //Case 1: no children
                        if (cur.key < cur.parent.key)
                        {
                            //Its parents left child
                            cur.parent.left = null;
                        }
                        else
                        {
                            //Its parents right child
                            cur.parent.right = null;
                        }
                    }
                    else if ((cur.left != null) && (cur.right == null))
                    {
                        //Case 2: has left child but no right child
                        if (cur.key < cur.parent.key)
                        {
                            //Its parents left child
                            cur.parent.left = cur.left;
                            cur.left.parent = cur.parent;
                        }
                        else
                        {
                            //Its parents right child
                            cur.parent.right = cur.left;
                            cur.left.parent = cur.parent;
                        }
                    }
                    else if ((cur.left == null) && (cur.right != null))
                    {
                        //Case 3: has right child but no left child
                        if (cur.key < cur.parent.key)
                        {
                            //Its parents left child
                            cur.parent.left = cur.right;
                            cur.right.parent = cur.parent;
                        }
                        else
                        {
                            //Its parents right child
                            cur.parent.right = cur.right;
                            cur.right.parent = cur.parent;
                        }
                    }
                    else
                    {
                        //Case 4: Has both children, find minimum of right subtree
                        Node temp = cur;
                        temp = temp.right;
                        while (temp.left != null)
                        {
                            temp = temp.left;
                        }

                        //temp now == minimum of right subtree, therefore its either Case 1 or Case 3
                        //Simply give its key to the temp, then run delete again to get rid of the now duplicate node
                        //Delete key first, otherwise end in infinite loop 
                        int x = temp.key;
                        Delete(temp.key);
                        cur.key = x;
                    }
                    size--;
                    //check for rebalance
                    //Console.WriteLine("Check to rebalance (delete) size = {0}, alpha*max_size = {1}", size, (alpha * max_size));
                    if (size <= (alpha*max_size))
                    {
                        //rebalance at root
                        //Console.WriteLine("Rebalance (delete) size = {0}, alpha*max_size = {1}", size, (alpha*max_size));
                        root = Rebalance(root);
                        max_size = size;
                    }
                    return;
                }
                else if (cur.key < key)
                {
                    //key is greater then current node's key, go right
                    if (cur.right == null)
                    {
                        Console.WriteLine("Key {0} not found, cannot delete...", key);
                        return;
                    }
                    cur = cur.right;
                }
                else
                {
                    //key is less then current node's key, go left
                    if (cur.left == null)
                    {
                        Console.WriteLine("Key {0} not found, cannot delete...", key);
                        return;
                    }
                    cur = cur.left;
                }
            }
            //if we do happen to get down here, which we shouldn't, key was not found
            Console.WriteLine("Key {0} not found... (out of delete while loop)", key);
            return;

        }

        private Node Rebalance(Node subtree)
        {
            List<Node> nodes = new List<Node>();
            StoreNodes(ref nodes, subtree);

            //Now the nodes list is full of the nodes of the subtree to be rebalanced, in sorted order
            int numNodes = nodes.Count;
            return rebalanceHelp (nodes, 0, numNodes - 1);

            //Rebalance at the subtree, then return the new root of the subtree
        }

        private Node rebalanceHelp(List<Node> nodes, int start, int end)
        {
            if (start > end)
            {
                return null;
            }

            //find the middle node to start the subtree
            int mid = (start + end) / 2;
            Node root = nodes[mid];

            root.left = rebalanceHelp(nodes, start, mid - 1);
            if (root.left != null)
            {
                root.left.parent = root;
            }
            root.right = rebalanceHelp(nodes, mid+1, end);
            if (root.right != null)
            {
                root.right.parent = root;
            }

            return root;
        }

        private void StoreNodes(ref List<Node> nodes, Node root)
        {
            //Used to put all of the nodes in the subtree into a list to be rebalanced. List will be in sorted order
            if (root.left != null)
            {
                StoreNodes(ref nodes, root.left);
            }
            nodes.Add(root);
            if (root.right != null)
            {
                StoreNodes(ref nodes, root.right);
            }
        }
    }
}
