// <copyright file="MultiplyOperatorNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Minus Operator.
    /// </summary>
    public class MultiplyOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyOperatorNode"/> class.
        /// </summary>
        /// <param name="precedence">precedence.</param>
        public MultiplyOperatorNode(ushort precedence)
        {
            this.Precedence = precedence;
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets the multiply operator char.
        /// </summary>
        public static new char Operator => '*';

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// The multiply evaluation.
        /// </summary>
        /// <returns>the value after calculate.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() * this.Right.Evaluate();
        }
    }
}