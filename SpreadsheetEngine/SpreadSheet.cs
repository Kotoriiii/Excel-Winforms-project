// <copyright file="SpreadSheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cpts321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Xml;
    using ExpressionTreeEngine;

    /// <summary>
    /// This main spreadsheet to work with cell.
    /// </summary>
    public class Spreadsheet
    {
        private readonly SpreadsheetCell[,] cell;

        /// <summary>
        /// Event to cell property change.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        private readonly int rowcount;
        private readonly int columcount;

        /// <summary>
        /// Undo Button change Text.
        /// </summary>
        public string UndoText;

        /// <summary>
        /// Redo Button change Text.
        /// </summary>
        public string RedoText;

        private readonly Stack<UndoRedoCollection> Undos = new Stack<UndoRedoCollection>();
        private readonly Stack<UndoRedoCollection> Redos = new Stack<UndoRedoCollection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rowIndex">row from spreedsheet.</param>
        /// <param name="coloumIndex">colum from spreedsheet.</param>
        public Spreadsheet(int rowIndex, int coloumIndex)
        {
            this.cell = new SpreadsheetCell[rowIndex, coloumIndex];
            this.rowcount = rowIndex;
            this.columcount = coloumIndex;
            for (int i = 0; i < rowIndex; i++)
            {
                for (int j = 0; j < coloumIndex; j++)
                {
                    this.cell[i, j] = new Cell(i, j);
                    this.cell[i, j].Value = Convert.ToString(0);
                    this.cell[i, j].PropertyChanged += new PropertyChangedEventHandler(this.CellPropertyChanges);
                    this.cell[i, j].ColorPropertyChanged += new PropertyChangedEventHandler(this.ColorPropertyChanged);
                }
            }
        }

        /// <summary>
        /// get the position for cell.
        /// </summary>
        /// <param name="rowIndex">row from spreedsheet.</param>
        /// <param name="coloumIndex">colum from spreedsheet.</param>
        /// <returns>cell from spreadsheet.</returns>
        public SpreadsheetCell GetCell(int rowIndex, int coloumIndex)
        {
            if (rowIndex > this.rowcount || coloumIndex > this.columcount)
            {
                throw new IndexOutOfRangeException();
            }

            return this.cell[rowIndex, coloumIndex];
        }

        /// <summary>
        /// Gets get row count.
        /// </summary>
        public int RowCount
        {
            get { return this.rowcount; }
        }

        /// <summary>
        /// Gets get colum count.
        /// </summary>
        public int ColumCount
        {
            get { return this.columcount; }
        }

        private SpreadsheetCell GetSingleValue(string varibles)
        {
            Dictionary<char, int> columsNumber = new Dictionary<char, int>();
            int count = 0;
            int row;
            for (char i = 'A'; i <= 'Z'; i++)
            {
                columsNumber[i] = count;
                count++;
            }

            string number = varibles.Substring(1);
            try
            {
                row = Convert.ToInt32(number) - 1;
            }
            catch (FormatException)
            {
                return null;
            }

            // row = Convert.ToInt32(number) - 1;
            int colum = columsNumber[varibles[0]];

            return this.GetCell(row, colum);
        }

        /// <summary>
        /// handle change color property.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected virtual void ColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Color"));
        }

        /// <summary>
        /// envent to change cell.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void CellPropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetCell spreadsheet = sender as SpreadsheetCell;

            Dictionary<int, char> alp = new Dictionary<int, char>();
            int count = 0;
            for (char i = 'A'; i <= 'Z'; i++)
            {
                alp[count] = i;
                count++;
            }

            string changeCell = alp[spreadsheet.ColumIndex].ToString() + (spreadsheet.RowIndex + 1).ToString();

            if (e.PropertyName == "Text")
            {
                if (spreadsheet.Text.StartsWith("=") == false)
                {
                    spreadsheet.Value = spreadsheet.Text;
                }
                else
                {
                    // if equal to the expreesion which need to cal by expression tree.
                    if (spreadsheet.Text.Length > 3)
                    {
                        string expression = spreadsheet.Text.Substring(1);
                        ExpressionTree tree = new ExpressionTree(expression);

                        List<string> strlist = tree.GetVaribles(expression);
                        foreach (string var in strlist)
                        {
                            SpreadsheetCell cell = this.GetSingleValue(var);
                            if (cell != null)
                            {
                                if (this.GetSingleValue(changeCell) == this.GetSingleValue(var))
                                {
                                    spreadsheet.Value = "!self reference";
                                }
                                else if (this.GetSingleValue(var).Value != "!self reference")
                                {
                                    double value = Convert.ToDouble(this.GetSingleValue(var).Value);
                                    tree.SetVariable(var, value);
                                    spreadsheet.Value = tree.Evaluate().ToString();
                                }
                                else
                                {
                                    spreadsheet.Value = Convert.ToString(0);
                                }

                                cell.PropertyChanged += spreadsheet.CellPropertyChanged;
                            }
                            else
                            {
                                spreadsheet.Value = "!bad reference";
                            }
                        }
                    }

                    // if = like B1,A1 just single cell value.
                    else
                    {
                        string name = spreadsheet.Text.Substring(1);
                        SpreadsheetCell cell = this.GetSingleValue(name);

                        // cell will be null if catch FormatException case which return to null,if not, just normall to get value else will be bad reference.
                        if (cell != null)
                        {
                            // if equal self cell value
                            if (this.GetSingleValue(changeCell) == this.GetSingleValue(name))
                            {
                                spreadsheet.Value = "!self reference";
                            }

                            // if equal the cell value which is no self reference, else is equal zero.
                            else if (this.GetSingleValue(name).Value != "!self reference")
                            {
                                spreadsheet.Value = this.GetSingleValue(name).Value;
                            }
                            else
                            {
                                spreadsheet.Value = Convert.ToString(0);
                            }

                            cell.PropertyChanged += spreadsheet.CellPropertyChanged;
                        }
                        else
                        {
                            spreadsheet.Value = "!bad reference";
                        }
                    }
                }
            }

            this.CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
        }

        /// <summary>
        /// Add undo items in the stack undos.
        /// </summary>
        /// <param name="undo">undo items.</param>
        public void AddUndo(UndoRedoCollection undo)
        {
            this.Undos.Push(undo);

            if (undo.ChangedName == "Text Changes")
            {
                this.UndoText = "Undo Text Changed";
            }
            else if (undo.ChangedName == "Color Changes")
            {
                this.UndoText = "Undo Color Changed";
            }

            this.CellPropertyChanged(this.Undos, new PropertyChangedEventArgs("Undo"));
        }

        /// <summary>
        /// Add redo items in the stack redos.
        /// </summary>
        /// <param name="redo">redo items.</param>
        public void AddRedo(UndoRedoCollection redo)
        {
            this.Redos.Push(redo);
            if (redo.ChangedName == "Text Changes")
            {
                this.RedoText = "Redo Text Changed";
            }
            else if (redo.ChangedName == "Color Changes")
            {
                this.RedoText = "Redo Color Changed";
            }

            this.CellPropertyChanged(this.Undos, new PropertyChangedEventArgs("Redo"));
        }

        /// <summary>
        /// do undo handel.
        /// </summary>
        public void Undo()
        {
            if (this.Undos.Count > 0)
            {
                UndoRedoCollection command = this.Undos.Pop();
                command.UndoChange();
                this.AddRedo(command);
            }

            if (this.Undos.Count > 0)
            {
                if (this.Undos.Peek().ChangedName == "Text Changes")
                {
                    this.UndoText = "Undo Text Changed";
                }
                else if (this.Undos.Peek().ChangedName == "Color Changes")
                {
                    this.UndoText = "Undo Color Changed";
                }

                this.CellPropertyChanged(this.Redos.Peek(), new PropertyChangedEventArgs("Undo"));
            }
            else
            {
                this.CellPropertyChanged(this.Redos.Peek(), new PropertyChangedEventArgs("Undo Cancel"));
            }
        }

        /// <summary>
        /// do redo handle.
        /// </summary>
        public void Redo()
        {
            if (this.Redos.Count > 0)
            {
                UndoRedoCollection command = this.Redos.Pop();
                command.RedoChange();
                this.AddUndo(command);
            }

            if (this.Redos.Count > 0)
            {
                if (this.Redos.Peek().ChangedName == "Text Changes")
                {
                    this.RedoText = "Redo Text Changed";
                }
                else if (this.Redos.Peek().ChangedName == "Color Changes")
                {
                    this.RedoText = "Redo Color Changed";
                }

                this.CellPropertyChanged(this.Undos.Peek(), new PropertyChangedEventArgs("Redo"));
            }
            else
            {
                this.CellPropertyChanged(this.Undos.Peek(), new PropertyChangedEventArgs("Redo Cancel"));
            }
        }

        /// <summary>
        /// load xml to spreadsheet.
        /// </summary>
        /// <param name="stream">stream.</param>
        public void TestReader(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            this.Undos.Clear();
            this.Redos.Clear();

            for (int i = 0; i < this.rowcount; i++)
            {
                for (int j = 0; j < this.columcount; j++)
                {
                    this.cell[i, j] = new Cell(i, j);
                    this.cell[i, j].PropertyChanged += new PropertyChangedEventHandler(this.CellPropertyChanges);
                    this.cell[i, j].ColorPropertyChanged += new PropertyChangedEventHandler(this.ColorPropertyChanged);
                }
            }

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                reader.ReadStartElement("Spreadsheet");
                while (reader.Name == "cell")
                {
                    reader.ReadStartElement("cell");
                    reader.ReadStartElement("Name");
                    string name = reader.ReadContentAsString();
                    SpreadsheetCell cell = this.GetSingleValue(name);
                    reader.ReadEndElement();
                    if (reader.Name == "BgColor")
                    {
                        reader.ReadStartElement("BgColor");
                        uint color = Convert.ToUInt32(reader.ReadContentAsString());
                        cell.Color = color;
                        reader.ReadEndElement();
                    }

                    if (reader.Name == "Text")
                    {
                        reader.ReadStartElement("Text");
                        string text = reader.ReadContentAsString();
                        cell.Text = text;
                        reader.ReadEndElement();
                    }

                    reader.ReadEndElement();
                }

                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// save xml to stream.
        /// </summary>
        /// <param name="stream">stream.</param>
        public void TestWriter(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Async = true;

            Dictionary<int, char> alp = new Dictionary<int, char>();
            int count = 0;
            for (char i = 'A'; i <= 'Z'; i++)
            {
                alp[count] = i;
                count++;
            }

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartElement("Spreadsheet");
                for (int i = 0; i < this.rowcount; i++)
                {
                    for (int j = 0; j < this.columcount; j++)
                    {
                        SpreadsheetCell cell = this.GetCell(i, j);
                        string name = alp[cell.ColumIndex].ToString() + (cell.RowIndex + 1).ToString();
                        if (cell.Value != string.Empty || cell.Color != 0xFFFFFFFF)
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteStartElement("Name");
                            writer.WriteString(name);
                            writer.WriteEndElement();
                            writer.WriteStartElement("BgColor");
                            writer.WriteString(cell.Color.ToString());
                            writer.WriteEndElement();
                            writer.WriteStartElement("Text");
                            writer.WriteString(cell.Value);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }

                writer.WriteEndElement();
                writer.Close();
            }
        }
    }
}
