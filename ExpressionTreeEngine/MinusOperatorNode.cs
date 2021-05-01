// <copyright file="MinusOperatorNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Minus Operator.
    /// </summary>
    public class MinusOperatorNode : OperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinusOperatorNode"/> class.
        /// </summary>
        /// <param name="precedence">precedence.</param>
        public MinusOperatorNode(ushort precedence)
        {
            this.Precedence = precedence;
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets the add operator char.
        /// </summary>
        public static new char Operator => '-';

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        public static Associative Associativity => Associative.Left;

        /// <summary>
        /// The Minus evaluation.
        /// </summary>
        /// <returns>the value after calculate.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
