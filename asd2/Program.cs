using System;
using System.Collections.Generic;

namespace asd2
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>(5);
            Random random = new Random();
            for (int i = 1; i <= 78; i++)
            {
                tree.Insert(random.Next(-100, 100), i);
            }

            Console.WriteLine(tree);
            tree.Delete(1);
            Console.WriteLine(tree);
            tree.Delete(2);
            Console.WriteLine(tree);

            tree.Delete(3);
            Console.WriteLine(tree);

            tree.Delete(4);
            Console.WriteLine(tree);

            tree.Delete(12);
            Console.WriteLine(tree);
        }
    }
}