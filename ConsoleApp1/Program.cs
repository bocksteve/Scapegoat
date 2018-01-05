using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapegoat
{
    class Program
    {
        private static Tree _scapegoat = null;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("tree.txt");
            string[] firstLine = lines[0].Split(' ');
            if (firstLine[0] == "BuildTree")
            {
                float alpha = float.Parse(firstLine[1]);
                int key = int.Parse(firstLine[2]);
                _scapegoat = new Tree(key, alpha);
            }
            else
            {
                Console.WriteLine("Invalid text file, no BuildTree command");
                return;
            }
            for (int i = 1; i<lines.Length; i++)
            {
                //lines[i] will be a command for each
                string[] tokens = lines[i].Split('\t');
                if (tokens[0] == "Insert")
                {
                    //Inserting new node
                    _scapegoat.Insert(int.Parse(tokens[1]));
                }
                else if (tokens[0] == "Search")
                {
                    //Search for key
                    _scapegoat.Search(int.Parse(tokens[1]));
                }
                else if (tokens[0] == "Delete")
                {
                    //Delete node with key
                    _scapegoat.Delete(int.Parse(tokens[1]));
                }
                else if (tokens[0] == "Print")
                {
                    //Print tree
                    Print(_scapegoat.root, 0);
                }
                else if (tokens[0] == "Done")
                {
                    //Finished
                    Console.WriteLine("Text file complete, press any key to continue");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    //Didn't find a valid command on that line
                    Console.WriteLine("Line {0} invalid command", i);
                }
            }
        }

        public static void Print(Node root, int level)
        {
            if (root != null)
            {
                Print(root.left, level + 1);
                for (int i = 0; i < level; i++)
                {
                    Console.Write("\t");
                }
                Console.WriteLine("{0}", root.key);
                Print(root.right, level + 1);
            }
        }
    }
}
