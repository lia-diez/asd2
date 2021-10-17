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

        public T SearchData(int key)
        {
            T result = RecursiveDataSearch(key, Root);
            return result;
        }

        private T RecursiveDataSearch(int key, Node<T> current)
        {
            T result;
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Records.Count && current.Records[keyIndex].Key == key)
            {
                return current.Records[keyIndex].Data;
            }

            if (!current.IsLeaf)
                result = RecursiveDataSearch(key, current.Children[keyIndex]);
            else
                return default;
            return result;
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
            Node<T> result;
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Records.Count && current.Records[keyIndex].Key == key)
            {
                return current;
            }

            if (!current.IsLeaf)
                result = RecursiveSearch(key, current.Children[keyIndex]);
            else
                return null;
            return result;
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

        private void DeleteLeaf(int key, Node<T> current)
        {
            int removedKeyIndex = FindIndex(key, current);
            
            if (current.Records.Count > _limit - 1)
            {
                current.Records.RemoveAt(removedKeyIndex);
                return;
            }

            Node<T> neighbour = new();
            bool leftExists = current.Left != null;
            if (leftExists && current.Left.Records.Count  > _limit - 1)
                neighbour = current.Left;
            else if (current.Right != null && current.Right.Records.Count  > _limit - 1)
            {
                neighbour = current.Right;
                leftExists = false;
            }
            
            if (neighbour.Records != null)
            {
                Record<T> parentRecord = current.Parent.Records[leftExists ? current.Index-1 : current.Index];
                
                current.Records.RemoveAt(removedKeyIndex);
                current.Records.Insert(FindIndex(parentRecord.Key, current), parentRecord);
                current.Parent.Records.Remove(parentRecord);

                Record<T> neighbourRecord = neighbour.Records[leftExists ? ^1 : 0];
                
                current.Parent.Records.Insert(FindIndex(neighbourRecord.Key, current.Parent), neighbourRecord);
                neighbour.Records.Remove(neighbourRecord);
            }
            else
            {
                neighbour = leftExists ? current.Left : current.Right;
                current.Records.InsertRange(leftExists ?  0 : current.Records.Count, neighbour.Records);
                
                Record<T> parentRecord = current.Parent.Records[leftExists ? current.Index-1 : current.Index];
                current.Parent.Children.Remove(neighbour);
                current.Records.Insert(_limit - 1, parentRecord);
                current.Parent.Records.Remove(parentRecord);
                current.Records.RemoveAt(removedKeyIndex);

            }
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