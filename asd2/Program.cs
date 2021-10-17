using System;
using System.Collections.Generic;

namespace asd2
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>(10);
            Tree<Tree<int>> ubertree = new Tree<Tree<int>>(3);
            Random random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                tree.Insert(random.Next(-100, 100), i);
            }

            Console.WriteLine(tree);
            Console.WriteLine(tree.Search(30));

            tree.Delete(99);
        }
    }
}