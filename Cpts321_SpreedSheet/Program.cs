// <copyright file="Program.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cpts321_SpreedSheet
{
    using System;
    using System.Windows.Forms;
    using ExpressionTreeEngine;

    /// <summary>
    /// run main programm.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// main.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            string expression = "A+B+C1+C2+6";
            ExpressionTree tree = new ExpressionTree(expression);

            // do the while loop to print the menu.
            while (true)
            {
                // menu
                // ExpressionTree tree = new ExpressionTree(expression);
                Console.WriteLine("Menu(current expression = {0})", expression);
                Console.WriteLine("1 = Enter a new expression");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate tree");
                Console.WriteLine("4 = Quit");
                int choice = Convert.ToInt32(Console.ReadLine());

                // do the choice for the expression tree.
                switch (choice)
                {
                    // Case enter the new expression
                    case 1:
                        Console.WriteLine("Enter new expression: ");
                        string newExpression = Console.ReadLine();
                        expression = newExpression;
                        break;

                    // Case set the value
                    case 2:
                        Console.WriteLine("Enter variable name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter variable value: ");
                        string value = Console.ReadLine();
                        tree.SetVariable(name, Convert.ToDouble(value));
                        break;

                    // Case do compution for the tree.
                    case 3:
                        Console.WriteLine(tree.Evaluate());
                        break;

                    // exit programm.
                    case 4:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
