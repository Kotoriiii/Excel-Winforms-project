// <copyright file="OperatorNodeFactory.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Operator Node Factory.
    /// </summary>
    public class OperatorNodeFactory
    {
        /// <summary>
        /// Deal with each operator node.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>Operator Node.</returns>
        public OperatorNode CreateOperatorNode(char op)
        {
            if (op == '+')
            {
                return new AddOperatorNode(1);
            }
            else if (op == '-')
            {
                return new MinusOperatorNode(1);
            }
            else if (op == '*')
            {
                return new MultiplyOperatorNode(2);
            }
            else if (op == '/')
            {
                return new DivideOperatorNode(2);
            }

            return null;
        }
    }
}