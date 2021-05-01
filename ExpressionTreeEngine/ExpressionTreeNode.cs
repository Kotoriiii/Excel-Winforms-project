// <copyright file="ExpressionTreeNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Abstract Node class.
    /// </summary>
    public abstract class ExpressionTreeNode
    {
        /// <summary>
        /// evaluataion.
        /// </summary>
        /// <returns>return value of each node.</returns>
       public abstract double Evaluate();
    }
}
