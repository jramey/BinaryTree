using System;
using System.Diagnostics;
using BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryTreeTests
{
    [TestClass]
    public class RedBlackTreeTests
    {
        [TestMethod]
        public void ContainsSuccessTest()
        {
            var tree = new RedBlackTree<Int32>();

            tree.Insert(10);
            tree.Insert(25);
            tree.Insert(7);

            Assert.IsTrue(tree.BredthContains(25));
        }

        [TestMethod]
        public void RotationTests()
        {
            var tree = new RedBlackTree<Int32>();

            tree.Insert(35);
            tree.Insert(25);
            tree.Insert(70);
            tree.Insert(10);
            tree.Insert(100);
            tree.Insert(20);
            tree.Insert(5);
            tree.Insert(85);
            tree.Insert(55);
            tree.Insert(40);

            Assert.AreEqual(35, tree.RootNode.NodeValue);
            Assert.AreEqual(20, tree.RootNode.LeftNode.NodeValue);
            Assert.AreEqual(25, tree.RootNode.LeftNode.RightNode.NodeValue);
            Assert.AreEqual(10, tree.RootNode.LeftNode.LeftNode.NodeValue);
            Assert.AreEqual(5, tree.RootNode.LeftNode.LeftNode.LeftNode.NodeValue);

            Assert.AreEqual(85, tree.RootNode.RightNode.NodeValue);
            Assert.AreEqual(100, tree.RootNode.RightNode.RightNode.NodeValue);
            Assert.AreEqual(55, tree.RootNode.RightNode.LeftNode.NodeValue);
            Assert.AreEqual(40, tree.RootNode.RightNode.LeftNode.LeftNode.NodeValue);
            Assert.AreEqual(70, tree.RootNode.RightNode.LeftNode.RightNode.NodeValue);
        }

        [TestMethod]
        public void ContainsFailureTest()
        {
            var tree = new RedBlackTree<Int32>();

            tree.Insert(10);
            tree.Insert(25);
            tree.Insert(7);

            Assert.IsFalse(tree.BredthContains(2));
        }

        [TestMethod]
        public void EmptyTreeTest()
        {
            var tree = new RedBlackTree<Int32>();

            Assert.IsTrue(tree.IsEmpty);
            Assert.IsFalse(tree.BredthContains(2));
        }

        [TestMethod]
        public void RootNodeTest()
        {
            var tree = new RedBlackTree<Int32>();
            tree.Insert(10);

            Assert.IsFalse(tree.IsEmpty);
            Assert.IsTrue(tree.BredthContains(10));
            Assert.AreEqual(10, tree.RootNode.NodeValue);
        }

        [TestMethod]
        public void SpeedTest()
        {
            
            var tree = new RedBlackTree<Int32>();

            var stopWatch = Stopwatch.StartNew();
            InsertMultipleNodes(tree, 100000);
            stopWatch.Stop();
            Debug.WriteLine(String.Format("Insert Time: {0} milliseconds ", stopWatch.ElapsedMilliseconds));

            var searchStopWatch = Stopwatch.StartNew();

            var found = false;

            found = tree.BredthContains(777);

            searchStopWatch.Stop();
            Debug.WriteLine(String.Format("Search Time: {0} ticks ", searchStopWatch.ElapsedTicks));

            Assert.IsTrue(found);
        }

        private void InsertMultipleNodes(RedBlackTree<Int32> tree, Int32 numberOfNodes)
        {
            for (var i = 0; i < numberOfNodes; i++)
            {
                tree.Insert(i);
            }
        }
    }
}
