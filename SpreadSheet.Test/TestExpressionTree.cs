// <copyright file="TestExpressionTree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadSheet.Test
{
    using System.Collections.Generic;
    using ExpressionTreeEngine;

    // NUnit 3 tests
    // See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
    using NUnit.Framework;

    /// <summary>
    /// test for the Expression Tree.
    /// </summary>
    [TestFixture]
    public class TestExpressionTree
    {
        /// <summary>
        /// Test Method for Expression Tree.
        /// </summary>
        [Test]
        public void TestExpressionTree1()
        {
            // TODO: Test eveluation for expression Tree
            string expression1 = "A+B+C1+C2+6";
            ExpressionTree tree = new ExpressionTree(expression1);
            tree.SetVariable("A", 2.0);
            tree.SetVariable("B", 3.0);
            tree.SetVariable("C1", 4.0);
            tree.SetVariable("C2", 5.0);
            double value1 = tree.Evaluate();
            Assert.AreEqual(20.0, value1);

            string expression2 = "C2-9-B2-27";
            ExpressionTree tree2 = new ExpressionTree(expression2);
            tree2.SetVariable("C2", 50.0);
            tree2.SetVariable("B2", 10.0);
            double value2 = tree2.Evaluate();
            Assert.AreEqual(4.0, value2);
        }

        /// <summary>
        /// Test Method for Expression Tree.
        /// </summary>
        [Test]
        public void TestExpressionTree2()
        {
            // TODO: Test eveluation for expression Tree
            string expression1 = "A*B+C1/C2+6";
            ExpressionTree tree = new ExpressionTree(expression1);
            tree.SetVariable("A", 9.0);
            tree.SetVariable("B", 8.0);
            tree.SetVariable("C1", 10.0);
            tree.SetVariable("C2", 5.0);
            double value1 = tree.Evaluate();
            Assert.AreEqual(80.0, value1);

            string expression2 = "C2*9+B2-27";
            ExpressionTree tree2 = new ExpressionTree(expression2);
            tree2.SetVariable("C2", 20.0);
            tree2.SetVariable("B2", 10.0);
            double value2 = tree2.Evaluate();
            Assert.AreEqual(163.0, value2);

            string expression3 = "C2*(9+B2)-27";
            ExpressionTree tree3 = new ExpressionTree(expression3);
            tree3.SetVariable("C2", 20.0);
            tree3.SetVariable("B2", 10.0);
            double value3 = tree3.Evaluate();
            Assert.AreEqual(353.0, value3);

            string expression4 = "(A1+B2-5)/5";
            ExpressionTree tree4 = new ExpressionTree(expression4);
            tree4.SetVariable("A1", 5.0);
            tree4.SetVariable("B2", 10.0);
            double value4 = tree4.Evaluate();
            Assert.AreEqual(2, value4);

            string expression5 = "1+2/3*6";
            ExpressionTree tree5 = new ExpressionTree(expression5);
            double value5 = tree5.Evaluate();
            Assert.AreEqual(5, value5);

            string expression6 = "(((1+2)/3)*6)";
            ExpressionTree tree6 = new ExpressionTree(expression6);
            double value6 = tree6.Evaluate();
            Assert.AreEqual(6, value6);
        }

        /// <summary>
        /// Test for get varibles.
        /// </summary>
        [Test]
        public void TestGetVaribles()
        {
            string expression1 = "A*B+C1/C2+6";
            ExpressionTree tree = new ExpressionTree(expression1);
            List<string> test = tree.GetVaribles(expression1);
            Assert.That(test[0], Is.EqualTo("A"));
            Assert.That(test[1], Is.EqualTo("B"));
            Assert.That(test[2], Is.EqualTo("C1"));
            Assert.That(test[3], Is.EqualTo("C2"));

            string expression2 = "C2*9+B2-27";
            ExpressionTree tree2 = new ExpressionTree(expression2);
            List<string> test2 = tree2.GetVaribles(expression2);
            Assert.That(test2[0], Is.EqualTo("C2"));
            Assert.That(test2[1], Is.EqualTo("B2"));

            string expression3 = "B11+C22";
            ExpressionTree tree3 = new ExpressionTree(expression3);
            List<string> test3 = tree3.GetVaribles(expression3);
            Assert.That(test3[0], Is.EqualTo("B11"));
            Assert.That(test3[1], Is.EqualTo("C22"));
        }
    }
}
