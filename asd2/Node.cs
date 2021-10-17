using System;
using System.Collections.Generic;
using System.Linq;

namespace asd2
{
    public class Node<T>
    {
        public List<Record<T>> Records;
        private List<Node<T>> _children;

        public List<Node<T>> Children
        {
            get => _children;
            set
            {
                foreach (var child in value)
                {
                    child.Parent = this;
                }
                _children = value;
            }
        }
        
        public bool IsLeaf => Children.Count == 0;
        public Node<T> Parent;
        public int Index => Parent.Children.IndexOf(this);
        public Node<T> Left => Index > 0 ? Parent.Children[Index - 1] : null;
        public Node<T> Right => Index < Parent.Children.Count-1 ? Parent.Children[Index + 1] : null;
        

        public Node(List<Record<T>> records,Node<T> parent)
        {
            Records = records;
            Parent = parent;
            Children = new List<Node<T>>();
        }

        public Node()
        {
        }

        public override string ToString()
        {
            return String.Join(", ", Records);
        }
    }
}