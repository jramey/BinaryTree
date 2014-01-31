using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class RedBlackNode<T> where T : IComparable<T>
    {
        public T NodeValue { get; set; }
        public RedBlackNode<T> ParentNode { get; set; }
        public RedBlackNode<T> LeftNode { get; set; }
        public RedBlackNode<T> RightNode { get; set; }
        public NodeColor NodeColor { get; set; }

        public RedBlackNode(T nodeValue, RedBlackNode<T> leftNode, RedBlackNode<T> rightNode)
        {
            NodeValue = nodeValue;
            LeftNode = leftNode;
            RightNode = rightNode;
            ParentNode = null;
            NodeColor = NodeColor.Red;
        }

        public RedBlackNode(T nodeValue)
            : this(nodeValue, null, null)
        {

        }

        public Direction GetParentDirection()
        {
            if (ParentNode == null || NodeValue.CompareTo(ParentNode.NodeValue) > 0)
                return Direction.Left;

            return Direction.Right;
        }

        public Boolean IsRoot()
        {
            return ParentNode == null;
        }

        public Boolean IsLeaf()
        {
            return LeftNode == null && RightNode == null;
        }
    }

    public enum NodeColor
    {
        Red = 0,
        Black = 1
    }

    public enum Direction
    { 
        Left = 0,
        Right = 1
    }
}
