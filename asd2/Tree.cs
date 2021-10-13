#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

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

        public bool Search(int key, ref T result)
        {
            return RecursiveSearch(key, Root, ref result);
        }

        private bool RecursiveSearch(int key, Node<T> current, ref T result)
        {
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Records.Count && current.Records[keyIndex].Key == key)
            {
                result = current.Records[keyIndex].Data;
                return true;
            }
            if (!current.IsLeaf)
                RecursiveSearch(key, current.Children[keyIndex], ref result);
            else
                return false;
            return true;
        }

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

            Node<T> left = new (current.Records.GetRange(0, _limit - 1), current.Parent);
            Node<T> right = new (current.Records.GetRange(_limit, _limit - 1), current.Parent);

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