using System;

namespace Nodes
{
    class Program
    {
        static void Main(string[] args)
        {
            var first = new Node { Value = 3 };
            var middle = new Node { Value = 5 };

            // chaining nodes
            first.Next = middle;
        }
    }

    public class Node
    {
        public int Value { get; set; }
        public Node Next { get; set; }
    }
}
 