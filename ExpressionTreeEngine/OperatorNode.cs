// <copyright file="OperatorNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// make opeartor Node constructor.
    /// </summary>
    public class OperatorNode : ExpressionTreeNode
    {
        private OperatorNodeFactory factory = new OperatorNodeFactory();

        /// <summary>
        /// Left Right node.
        /// </summary>
        public enum Associative
        {
            /// <summary>
            /// Right child of expression tree.
            /// </summary>
            Right,

            /// <summary>
            /// Left child of expression tree.
            /// </summary>
            Left,
        }

        /// <summary>
        /// Gets or Sets left child.
        /// </summary>
        public ExpressionTreeNode Left
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets right child.
        /// </summary>
        public ExpressionTreeNode Right
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets operator.
        /// </summary>
        public char Operator
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets precedence.
        /// </summary>
        public ushort Precedence
        {
            get; set;
        }

        /// <summary>
        /// OpeartorNode evaluation.
        /// </summary>
        /// <returns>the value after calculate.</returns>
        public override double Evaluate()
        {
            // return this.factory.CreateOperatorNode(this.Operator);
            return 0.0;
        }
    }
}
