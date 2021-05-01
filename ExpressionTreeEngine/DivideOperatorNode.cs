// <copyright file="DivideOperatorNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Divide Operator.
    /// </summary>
    public class DivideOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivideOperatorNode"/> class.
        /// </summary>
        /// <param name="precedence">precedence.</param>
        public DivideOperatorNode(ushort precedence)
        {
            this.Precedence = precedence;
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets the divide operator char.
        /// </summary>
        public static new char Operator => '/';

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// The divide evaluation.
        /// </summary>
        /// <returns>the value after calculate.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() / this.Right.Evaluate();
        }
    }
}
