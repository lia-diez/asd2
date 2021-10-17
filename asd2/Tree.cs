#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asd2
{
    public class Tree<T>
    {
        public Node<T> Root { get; private set; }
        private int _limit;

        public Tree(int limit)
        {
            _limit = limit;
        }

        #region Search

        #region Searching for data

        public T Search(int key, out Node<T> resultNode)
        {
            resultNode = new();
            T result = RecursiveSearch(key, Root, ref resultNode);
            return result;
        }

        private T RecursiveSearch(int key, Node<T> current, ref Node<T> resultNode)
        {
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Records.Count && current.Records[keyIndex].Key == key)
            {
                resultNode = current;
                return current.Records[keyIndex].Data;
            }

            if (!current.IsLeaf)
                RecursiveSearch(key, current.Children[keyIndex], ref resultNode);
            else
                return default;
            return default;
        }

        #endregion

        #region Searching for result node

        public Node<T> Search(int key)
        {
            Node<T> result = RecursiveSearch(key, Root);
            return result;
        }
        private Node<T> RecursiveSearch(int key, Node<T> current)
        {
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Records.Count && current.Records[keyIndex].Key == key)
            {
                return current;
            }

            if (!current.IsLeaf)
                RecursiveSearch(key, current.Children[keyIndex]);
            else
                return null;
            return null;
        }

        #endregion

        #endregion

        #region Insert

        public void Insert(T data, int key)
        {
            if (Root == null)
                Root = new Node<T>(new List<Record<T>> {new(data, key)}, null);
            else
            {
                Node<T> current = Root;
                RecursiveInsert(new Record<T>(data, key), current);
            }
        }

        private void RecursiveInsert(Record<T> newRecord, Node<T> current)
        {
            int index = FindIndex(newRecord.Key, current);
            if (!current.IsLeaf)
            {
                RecursiveInsert(newRecord, current.Children[index]);
            }
            else
                current.Records.Insert(FindIndex(newRecord.Key, current), newRecord);

            if (current.Records.Count == 2 * _limit - 1)
            {
                Split(current);
            }
        }

        private void Split(Node<T> current)
        {
            Record<T> middleRecord = current.Records[_limit - 1];

            Node<T> left = new(current.Records.GetRange(0, _limit - 1), current.Parent);
            Node<T> right = new(current.Records.GetRange(_limit, _limit - 1), current.Parent);

            if (!current.IsLeaf)
            {
                left.Children = current.Children.GetRange(0, _limit);
                right.Children = current.Children.GetRange(_limit, _limit);
            }

            current.Records.RemoveAt(_limit - 1);
            if (current.Parent != null)
            {
                int index = FindIndex(current.Records[_limit].Key, current.Parent);
                current.Parent.Records.Insert(index, middleRecord);
                current.Parent.Children[index] = left;
                current.Parent.Children.Insert(index + 1, right);
            }
            else
            {
                Root = new Node<T>(new List<Record<T>> {middleRecord}, null);
                left.Parent = Root;
                right.Parent = Root;
                Root.Children.Add(left);
                Root.Children.Add(right);
            }
        }

        #endregion

        #region Output

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            RecursiveOutput(ref output, Root, 0, false);
            return output.ToString();
        }

        private void RecursiveOutput(ref StringBuilder current, Node<T> node, int depth, bool isLast)
        {
            current.Append(Multiply("│ ", depth) + (isLast ? "└─ " : "├─ ") +
                           "(" + node + ")" + "\n");
            if (!node.IsLeaf)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    RecursiveOutput(ref current, node.Children[i], depth + 1,
                        i == node.Children.Count - 1 && node.Children[i].IsLeaf);
                }
            }
        }

        private static string Multiply(string str, int times) =>
            string.Concat(Enumerable.Repeat(str, times));

        #endregion

        #region Delete

        public bool Delete(int key)
        {
            Node<T> removedNode = Search(key);
            if (removedNode.IsLeaf) 
                DeleteLeaf(key, removedNode);
            else
                DeleteInside();
            return true;
        }

        private void DeleteLeaf(int key, Node<T> removedNode)
        {
            if (removedNode.Records.Count > _limit - 1)
            {
                removedNode.Records.RemoveAt(FindIndex(key, removedNode));
                return;
            }
            
            int nodeIndex = FindIndex(key, removedNode.Parent);
            
        }
        
        private void DeleteInside(){}

        #endregion
        
        private int FindIndex(int key, Node<T> current)
        {
            int index;
            for (index = 0; index < current.Records.Count; index++)
            {
                if (current.Records[index].Key >= key) break;
            }

            return index;
        }
    }
}