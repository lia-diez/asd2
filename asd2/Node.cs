using System.Collections.Generic;

namespace asd2
{
    public class Node<T>
    {
        public List<Record<T>> Records;
        public List<Node<T>> Children;
        public bool IsLeaf => Children.Count == 0;
        public Node<T> Parent;

        public Node(List<Record<T>> records,Node<T> parent)
        {
            Records = records;
            Parent = parent;
            Children = new List<Node<T>>();
        }
    }
}