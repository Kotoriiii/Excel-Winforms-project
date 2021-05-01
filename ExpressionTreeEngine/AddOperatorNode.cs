// <copyright file="AddOperatorNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Add Operator.
    /// </summary>
    public class AddOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddOperatorNode"/> class.
        /// </summary>
        /// <param name="precedence">precedence.</param>
        public AddOperatorNode(ushort precedence)
        {
            this.Precedence = precedence;
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets the add operator char.
        /// </summary>
        public static new char Operator => '+';

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// The add evaluation.
        /// </summary>
        /// <returns>the value after calculate.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() + this.Right.Evaluate();
        }
    }
}
