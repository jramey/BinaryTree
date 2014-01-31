using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class RedBlackTree<T> : IEnumerable<T>
             where T : IComparable<T>
    {
        public RedBlackNode<T> RootNode { get; set; }
        public Int32 NodeCount { get; set; }
        public Boolean IsEmpty { get { return RootNode == null; } }

        public T GetMinValue()
        {
            if (IsEmpty)
                throw new Exception("Error: Cannot determine minimum value of an empty tree");

            var node = RootNode;

            while (node.LeftNode != null)
                node = node.LeftNode;

            return node.NodeValue;
        }

        public T GetMaxValue()
        {
            if (IsEmpty)
                throw new Exception("Error: Cannot determine maximum value of an empty tree");

            var node = RootNode;

            while (node.RightNode != null)
                node = node.RightNode;

            return node.NodeValue;
        }

        public void Insert(T value)
        {
            if (RootNode == null)
            {
                var node = new RedBlackNode<T>(value) { ParentNode = null, NodeColor = NodeColor.Black };
                RootNode = node;
                NodeCount++;
            }
            else
            {
                InsertNode(value, RootNode);
            }
        }

        private void InsertNode(T value, RedBlackNode<T> currentNode)
        {
            if (value.CompareTo(currentNode.NodeValue) == -1)
            {
                if (currentNode.LeftNode == null)
                {
                    var node = new RedBlackNode<T>(value)
                    {
                        NodeColor = NodeColor.Red,
                        ParentNode = currentNode,
                    };

                    currentNode.LeftNode = node;
                    NodeCount++;
                }
                else
                {
                    InsertNode(value, currentNode.LeftNode);
                    return;
                }
            }
            else if (value.CompareTo(currentNode.NodeValue) == 1)
            {
                if (currentNode.RightNode == null)
                {
                    var node = new RedBlackNode<T>(value)
                    {
                        NodeColor = NodeColor.Red,
                        ParentNode = currentNode,
                    };

                    currentNode.RightNode = node;
                    NodeCount++;
                }
                else
                {
                    InsertNode(value, currentNode.RightNode);
                    return;
                }
            }

            CheckNode(currentNode);
            RootNode.NodeColor = NodeColor.Black;
        }

        private void CheckNode(RedBlackNode<T> currentNode)
        {
            if (currentNode == null)
                return;

            if (currentNode.NodeColor != NodeColor.Red) return;

            var uncleNode = GetSiblingNode(currentNode);
            if (uncleNode != null && uncleNode.NodeColor == NodeColor.Red)
            {
                uncleNode.NodeColor = NodeColor.Black;
                currentNode.NodeColor = NodeColor.Black;
                currentNode.ParentNode.NodeColor = NodeColor.Red;

                if (currentNode.ParentNode.ParentNode != null
                  && currentNode.ParentNode.ParentNode.NodeValue.CompareTo(RootNode.NodeValue) != 0)
                {
                    var node = currentNode.ParentNode.ParentNode;
                    CheckNode(node);
                }
            }
            else
            {
                var redChild =
                   (currentNode.LeftNode != null && currentNode.LeftNode.NodeColor == NodeColor.Red)
                       ? Direction.Left : Direction.Right;

                if (redChild == Direction.Left)
                {
                    if (currentNode.GetParentDirection() == Direction.Right)
                        RotateLeftChildRightParent(currentNode);
                    else
                        RotateLeftChildLeftParent(currentNode);
                }
                else
                {
                    if (currentNode.RightNode.NodeColor == NodeColor.Red)
                    {
                        if (currentNode.GetParentDirection() == Direction.Right)
                            RotateRightChildRightParent(currentNode);
                        else
                            RotateRightChildLeftParent(currentNode);
                    }
                }
            }
        }

        private void RotateRightChildLeftParent(RedBlackNode<T> currentNode)
        {
            if (currentNode.IsRoot())
                return;

            if (currentNode.LeftNode != null)
            {
                currentNode.ParentNode.RightNode = currentNode.LeftNode;
                currentNode.LeftNode.ParentNode = currentNode.ParentNode;
            }
            else
            {
                currentNode.ParentNode.RightNode = currentNode.LeftNode;
            }

            var tmpNode = currentNode.ParentNode.ParentNode;
            currentNode.LeftNode = currentNode.ParentNode;
            currentNode.ParentNode.ParentNode = currentNode;

            if (tmpNode == null)
            {
                RootNode = currentNode;
                currentNode.ParentNode = null;
            }
            else
            {
                currentNode.ParentNode = tmpNode;

                if (tmpNode.NodeValue.CompareTo(currentNode.NodeValue) > 0)
                    tmpNode.LeftNode = currentNode;
                else
                    tmpNode.RightNode = currentNode;
            }

            FixChildColors(currentNode);

            var newCurrent = currentNode.ParentNode;
            CheckNode(newCurrent);
        }

        private void RotateRightChildRightParent(RedBlackNode<T> currentNode)
        {
            if (currentNode.IsRoot())
                return;

            var tmpNode = currentNode.RightNode.LeftNode;
            currentNode.RightNode.ParentNode = currentNode.ParentNode;
            currentNode.ParentNode.LeftNode = currentNode.RightNode;
            currentNode.ParentNode = currentNode.RightNode;
            currentNode.RightNode.LeftNode = currentNode;

            if (tmpNode != null)
            {
                currentNode.RightNode = tmpNode;
                tmpNode.ParentNode = currentNode;
            }
            else
            {
                currentNode.RightNode = tmpNode;
            }

            var newCurrent = currentNode.ParentNode;
            CheckNode(newCurrent);
        }

        private RedBlackNode<T> GetSiblingNode(RedBlackNode<T> currentNode)
        {
            if (currentNode == null || currentNode.ParentNode == null)
                return null;

            if (currentNode.ParentNode.LeftNode != null
              && currentNode.ParentNode.LeftNode.NodeValue.CompareTo(currentNode.NodeValue) == 0)
                return currentNode.ParentNode.RightNode;

            return currentNode.ParentNode.LeftNode;
        }

        private void RotateLeftChildLeftParent(RedBlackNode<T> currentNode)
        {
            if (currentNode.IsRoot())
                return;

            var tmpNode = currentNode.LeftNode.RightNode;
            currentNode.LeftNode.ParentNode = currentNode.ParentNode;
            currentNode.ParentNode.RightNode = currentNode.LeftNode;
            currentNode.ParentNode = currentNode.LeftNode;
            currentNode.LeftNode.RightNode = currentNode;

            if (tmpNode != null)
            {
                currentNode.LeftNode = tmpNode;
                tmpNode.ParentNode = currentNode;
            }
            else
            {
                currentNode.LeftNode = tmpNode;
            }

            var newCurrent = currentNode.ParentNode;
            CheckNode(newCurrent);
        }

        private void RotateLeftChildRightParent(RedBlackNode<T> currentNode)
        {
            if (currentNode.IsRoot())
                return;

            if (currentNode.RightNode != null)
            {
                currentNode.ParentNode.LeftNode = currentNode.RightNode;
                currentNode.RightNode.ParentNode = currentNode.ParentNode;
            }
            else
            {
                currentNode.ParentNode.LeftNode = currentNode.RightNode;
            }

            var tmpNode = currentNode.ParentNode.ParentNode;
            currentNode.RightNode = currentNode.ParentNode;
            currentNode.ParentNode.ParentNode = currentNode;

            if (tmpNode == null)
            {
                RootNode = currentNode;
                currentNode.ParentNode = null;
            }
            else
            {
                currentNode.ParentNode = tmpNode;

                if (tmpNode.NodeValue.CompareTo(currentNode.NodeValue) > 0)
                    tmpNode.LeftNode = currentNode;
                else
                    tmpNode.RightNode = currentNode;
            }

            FixChildColors(currentNode);

            var newCurrent = currentNode.ParentNode;
            CheckNode(newCurrent);
        }

        private void FixChildColors(RedBlackNode<T> current)
        {
            if (current.NodeColor == NodeColor.Red)
            {
                if (current.LeftNode != null && current.LeftNode.NodeColor == NodeColor.Black)
                {
                    current.LeftNode.NodeColor = NodeColor.Red;
                    current.NodeColor = NodeColor.Black;
                }
                else if (current.RightNode != null && current.RightNode.NodeColor == NodeColor.Black)
                {
                    current.RightNode.NodeColor = NodeColor.Red;
                    current.NodeColor = NodeColor.Black;
                }
            }
        }

        //public Boolean DepthContains(T value)
        //{
        //    if (IsEmpty)
        //        return false;

        //    var currentNode = GetLeftMostNode();

        //        if (value.CompareTo(currentNode.NodeValue) == -1)
        //            return false;
        //        else
        //            DepthRecurse(

        //    return false;
        //}

        //public Boolean DepthRecurse(RedBlackNode<T> currentNode)
        //{
        //    while (currentNode != null && currentNode != RootNode)
        //    {
        //        if (value.CompareTo(currentNode.NodeValue) == -1)
        //            currentNode = currentNode.LeftNode;
        //        else if (value.CompareTo(currentNode.NodeValue) == 1 && currentNode.RightNode != null)
        //            currentNode = currentNode.RightNode;
        //        else if (value.CompareTo(currentNode.NodeValue) == 1 && currentNode.RightNode == null)
        //            currentNode = currentNode.ParentNode;
        //        else
        //            return true;
        //    }

        //    return false;
        //}

        private RedBlackNode<T> GetLeftMostNode()
        {
            var tempNode = RootNode;

            while (tempNode.LeftNode != null)
                tempNode = tempNode.LeftNode;

            return tempNode;
        }

        public Boolean BredthContains(T value)
        {
            if (IsEmpty)
                return false;

            var currentNode = RootNode;

            while (currentNode != null)
            {
                if (value.CompareTo(currentNode.NodeValue) == -1)
                    currentNode = currentNode.LeftNode;
                else if (value.CompareTo(currentNode.NodeValue) == 1)
                    currentNode = currentNode.RightNode;
                else
                    return true;
            }

            return false;
        }

        private static IEnumerable<T> InOrderTraversal(RedBlackNode<T> node)
        {
            if (node.LeftNode != null)
            {
                foreach (T nodeVal in InOrderTraversal(node.LeftNode))
                    yield return nodeVal;
            }

            yield return node.NodeValue;

            if (node.RightNode != null)
            {
                foreach (T nodeVal in InOrderTraversal(node.RightNode))
                    yield return nodeVal;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T value in InOrderTraversal(RootNode)) { yield return value; };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
