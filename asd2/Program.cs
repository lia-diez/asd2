using System;

namespace asd2
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>(3);
            for (int i = 0; i < 100; i++)
            {
                tree.Insert(i, i);
                if (i == 15) ;
            }

            int result = 0;
            if (tree.Search(80, ref result))
                Console.WriteLine(result);
        }
    }
}