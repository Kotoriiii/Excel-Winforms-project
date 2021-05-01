// <copyright file="ConstantNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    /// <summary>
    /// Make constant Node constructor.
    /// </summary>
    public class ConstantNode : ExpressionTreeNode
    {
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">value.</param>
        public ConstantNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets or sets Constant Node value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// ConstantNode evaluation.
        /// </summary>
        /// <returns>value of ConstantNode.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
