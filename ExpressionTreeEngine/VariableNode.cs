// <copyright file="VariableNode.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// make vaiable node constructor.
    /// </summary>
    public class VariableNode : ExpressionTreeNode
    {
        private string name;
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">name.</param>
        /// <param name="variables">varibales.</param>
        public VariableNode(string name, ref Dictionary<string, double> variables)
        {
            this.name = name;
            this.variables = variables;
        }

        /// <summary>
        /// Gets or sets Variable Node name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// VariableNode evaluation.
        /// </summary>
        /// <returns>value of VariableNode.</returns>
        public override double Evaluate()
        {
            double value = 0.0;

            if (this.variables.ContainsKey(this.name))
            {
                value = this.variables[this.name];
            }

            return value;
        }
    }
}
