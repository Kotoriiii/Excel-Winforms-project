// <copyright file="ExpressionTree.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ExpressionTreeEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The main Expression Tree.
    /// </summary>
    public class ExpressionTree
    {
        private ExpressionTreeNode root = null;
        private Dictionary<string, double> variables = new Dictionary<string, double>();
        private OperatorNodeFactory factory = new OperatorNodeFactory();
        private Stack<ExpressionTreeNode> outputstack = new Stack<ExpressionTreeNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">string of the expression.</param>
        public ExpressionTree(string expression)
        {
            this.root = this.MakeTree(expression);
        }

        /// <summary>
        /// get varibles for expression.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>collections for varibles.</returns>
        public List<string> GetVaribles(string expression)
        {
            List<string> strlist = new List<string>();
            string name = string.Empty;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    name += expression[i];
                    string temp = expression.Substring(i + 1);
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (char.IsDigit(temp[j]))
                        {
                            name += temp[j];
                        }

                        if (char.IsLetter(temp[j]) || temp[j] == '+' || temp[j] == '-' || temp[j] == '*' || temp[j] == '/')
                        {
                            break;
                        }
                    }

                    strlist.Add(name);
                    name = string.Empty;
                }
            }

            return strlist;
        }

        /// <summary>
        /// Make tree function.
        /// </summary>
        /// <param name="s">expression.</param>
        /// <returns>return the node of tree.</returns>
        private ExpressionTreeNode MakeTree(string s)
        {
            List<string> expression = this.ShuntingYardAlg(s);
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i] != "+" && expression[i] != "-" && expression[i] != "*" && expression[i] != "/")
                {
                    // if the Contant node.
                    if (Regex.IsMatch(expression[i], @"^[+-]?\d*[.]?\d*$"))
                    {
                        this.outputstack.Push(new ConstantNode(Convert.ToDouble(expression[i])));
                    }

                    // if the Variable Node.
                    else
                    {
                        this.outputstack.Push(new VariableNode(expression[i], ref this.variables));
                    }
                }
                else
                {
                    // opeartor node.
                    OperatorNode node = this.factory.CreateOperatorNode(Convert.ToChar(expression[i]));
                    if (this.outputstack.Count != 0)
                    {
                        // set the left child and right child under operator.
                        node.Right = this.outputstack.Pop();
                        node.Left = this.outputstack.Pop();
                    }

                    this.outputstack.Push(node);
                }
            }

            return this.outputstack.Pop();
        }

        /// <summary>
        /// Get opeartor precedence.
        /// </summary>
        /// <param name="ch">char of opeartor.</param>
        /// <returns>return precedence.</returns>
        private int GetPrecedence(char ch)
        {
            if (ch == '+' || ch == '-')
            {
                return 1;
            }
            else if (ch == '*' || ch == '/')
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Make tree helper.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>return the string.</returns>
        private List<string> ShuntingYardAlg(string expression)
        {
            Stack<string> stack = new Stack<string>();
            List<string> strlist = new List<string>();
            string name = string.Empty;
            string number = string.Empty;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    name += expression[i];
                    string temp = expression.Substring(i + 1);
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (char.IsDigit(temp[j]))
                        {
                            name += temp[j];
                            i++;
                        }

                        if (char.IsLetter(temp[j]) || temp[j] == '+' || temp[j] == '-' || temp[j] == '*' || temp[j] == '/')
                        {
                            break;
                        }
                    }

                    strlist.Add(name);
                    name = string.Empty;
                }
                else if (expression[i] != '+' && expression[i] != '-' && expression[i] != '*' && expression[i] != '/')
                {
                    // if the token is a left bracket
                    if (expression[i] == '(')
                    {
                        stack.Push(expression[i].ToString());
                    }

                    // if the token is a right bracket
                    else if (expression[i] == ')')
                    {
                        while (stack.Peek() != "(" && stack.Count != 0)
                        {
                            strlist.Add(stack.Pop());
                        }

                        stack.Pop();
                    }

                    // if the token is letter and digit number together.
                    /*else if (char.IsLetter(expression[i]))
                    {
                        /*name += expression[i];
                        if (i != expression.Length - 1)
                        {
                            if (char.IsDigit(expression[i + 1]))
                            {
                                name += expression[i + 1];
                                strlist.Add(name);
                                i++;
                                name = string.Empty;
                            }
                            else
                            {
                                strlist.Add(name);
                                name = string.Empty;
                            }
                        }
                        else
                        {
                            strlist.Add(expression[i].ToString());
                        }
                    }*/

                    // if the token is number.
                    else if (char.IsDigit(expression[i]))
                    {
                        if (i != expression.Length - 1)
                        {
                            number += expression[i];
                            if (char.IsDigit(expression[i + 1]))
                            {
                                number += expression[i + 1];
                                strlist.Add(number);
                                i++;
                                number = string.Empty;
                            }
                            else
                            {
                                strlist.Add(number);
                                number = string.Empty;
                            }
                        }
                        else
                        {
                            strlist.Add(expression[i].ToString());
                        }
                    }
                }
                else
                {
                    // else then all the tokens are operator.
                    /*if (stack.Count == 0 || stack.Peek() == "(")
                    {
                        stack.Push(expression[i].ToString());
                    }*/
                    // else if (stack.Count != 0)
                    // {
                        // OperatorNode inputnode = this.factory.CreateOperatorNode(expression[i]);
                        // OperatorNode outputnode = this.factory.CreateOperatorNode(Convert.ToChar(stack.Peek()));
                    while (stack.Count != 0 && this.GetPrecedence(expression[i]) <= this.GetPrecedence(Convert.ToChar(stack.Peek())))
                    {
                        strlist.Add(stack.Pop());
                    }

                    stack.Push(expression[i].ToString());

                   // }
                }
            }

            // pop all the stack back to list.
            while (stack.Count != 0)
            {
                if (stack.Peek() != "(")
                {
                    strlist.Add(stack.Pop());
                }
            }

            return strlist;
        }

        /// <summary>
        /// Set name with value in dic.
        /// </summary>
        /// <param name="variableName">string for the name.</param>
        /// <param name="variableValue">double for the value.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// do the evaluate for expression tree.
        /// </summary>
        /// <returns>the final compute value.</returns>
        public double Evaluate()
        {
            return this.root.Evaluate();
        }
    }
}
