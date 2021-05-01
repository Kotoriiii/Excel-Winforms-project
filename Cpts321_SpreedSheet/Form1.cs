// <copyright file="Form1.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cpts321_SpreedSheet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Cpts321;

    /// <summary>
    /// This is Form work.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly Spreadsheet table = new Spreadsheet(50, 26);

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.Load += new System.EventHandler(this.OnLoad);
        }

        /// <summary>
        /// Load the change in Form.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            string col_name = "Column";

            // creat colum from A to Z
            for (char i = 'A'; i <= 'Z'; i++)
            {
                this.dataGridView1.Columns.Add(col_name + Convert.ToString(i), Convert.ToString(i));
            }

            // create row from 0 to 50
            this.dataGridView1.RowHeadersWidth = 50;
            for (int i = 0; i < 50; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }

            // Handle cell change on dataGridView.
            this.table.CellPropertyChanged += this.OnCellPropertyChanged;
            this.dataGridView1.CellBeginEdit += this.DataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.DataGridView1_CellEndEdit;
            this.dataGridView1.BackgroundColorChanged += this.ChangeColor_Clicked;
            this.saveToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Handle cell change funtion.
        /// </summary>
        /// <param name="sender">sendr.</param>
        /// <param name="e">e.</param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell newCell = sender as Cell;

            if (e.PropertyName == "Value")
            {
                this.dataGridView1.Rows[newCell.RowIndex].Cells[newCell.ColumIndex].Value = newCell.Value;
                this.saveToolStripMenuItem.Enabled = true;
            }

            if (e.PropertyName == "Color")
            {
                this.dataGridView1.Rows[newCell.RowIndex].Cells[newCell.ColumIndex].Style.BackColor = Color.FromArgb((int)newCell.Color);
            }

            if (e.PropertyName == "Redo")
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = this.table.RedoText;
            }

            if (e.PropertyName == "Undo")
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = this.table.UndoText;
            }

            if (e.PropertyName == "Redo Cancel")
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = "Redo";
            }

            if (e.PropertyName == "Undo Cancel")
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Text = "Undo";
            }
        }

        /// <summary>
        /// Click button demo handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void DemoClick(object sender, EventArgs e)
        {
            Random r1 = new Random();
            Random r2 = new Random();

            // write word in random 50 cells.
            for (int i = 0; i < 50; i++)
            {
                int row = r1.Next(50);
                int colum = r2.Next(26);
                SpreadsheetCell index = this.table.GetCell(row, colum);
                index.Text = "Hello World";
            }

            // write word in all B cell.
            for (int i = 0; i < this.table.RowCount; i++)
            {
                SpreadsheetCell index = this.table.GetCell(i, 1);
                index.Text = "This is cell B" + Convert.ToString(i + 1);
            }

            // use to test = way to copy cell.
            SpreadsheetCell index2 = this.table.GetCell(0, 0);
            index2.Text = "=B" + Convert.ToString(1);
        }

        /// <summary>
        /// Begin to edit handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string msg = string.Format("Editing Cell at ({0}, {1})", e.ColumnIndex, e.RowIndex);
            this.Text = msg;
            SpreadsheetCell index = this.table.GetCell(e.RowIndex, e.ColumnIndex);
            this.dataGridView1.CurrentCell.Value = index.Text;
        }

        /// <summary>
        /// End to edit handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string msg = string.Format("Finished Editing Cell at ({0}, {1})", e.ColumnIndex, e.RowIndex);
            this.Text = msg;
            SpreadsheetCell index = this.table.GetCell(e.RowIndex, e.ColumnIndex);
            if (this.dataGridView1.CurrentCell.Value != null)
            {
                string prevText = index.Text;
                index.Text = this.dataGridView1.CurrentCell.Value.ToString();
                UndoRedoCollection UndoCommand = new UndoRedoCollection("Text Changes");
                UndoCommand.SetTextVariable(index, prevText, index.Text);
                this.table.AddUndo(UndoCommand);
                this.undoToolStripMenuItem.Text = this.table.UndoText;
            }
            else
            {
                this.dataGridView1.CurrentCell.Value = string.Empty;
            }
        }

        /// <summary>
        /// change color button clicked handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void ChangeColor_Clicked(object sender, EventArgs e)
        {
            List<SpreadsheetCell> colorCollect = new List<SpreadsheetCell>();
            ColorDialog MyDialog = new ColorDialog();
            List<uint> prevColor = new List<uint>();

            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;

            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;

            // Update the text box color if the user clicks OK
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewTextBoxCell cell in this.dataGridView1.SelectedCells)
                {
                    colorCollect.Add(this.table.GetCell(cell.RowIndex, cell.ColumnIndex));
                    prevColor.Add(this.table.GetCell(cell.RowIndex, cell.ColumnIndex).Color);
                    this.table.GetCell(cell.RowIndex, cell.ColumnIndex).Color = (uint)MyDialog.Color.ToArgb();
                }

                UndoRedoCollection UndoCommand = new UndoRedoCollection("Color Changes");
                UndoCommand.SetColortVarible(colorCollect, prevColor, (uint)MyDialog.Color.ToArgb());
                this.table.AddUndo(UndoCommand);
                this.undoToolStripMenuItem.Text = this.table.UndoText;
            }
        }

        /// <summary>
        /// redo button clicked handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Redo_Clicked(object sender, EventArgs e)
        {
            this.table.Redo();
        }

        /// <summary>
        /// undo button clicked handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Undo_Clicked(object sender, EventArgs e)
        {
            this.table.Undo();
        }

        /// <summary>
        /// load button clicked handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Load_Click(object sender, EventArgs e)
        {
            Stream file;
            OpenFileDialog load = new OpenFileDialog
            {
                Filter = "XML File|*.xml",
                Title = "Open",
            };
            if (load.ShowDialog() == DialogResult.OK)
            {
                this.dataGridView1.Refresh();
                this.undoToolStripMenuItem.Text = "Undo";
                this.undoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = "Redo";
                this.redoToolStripMenuItem.Enabled = false;
                if ((file = load.OpenFile()) != null)
                {
                    this.table.TestReader(file);
                    file.Close();
                }
            }
        }

        /// <summary>
        /// save button clicked handle.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Save_Click(object sender, EventArgs e)
        {
            Stream file;
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "XML File|*.xml",
                Title = "Save",
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
                if ((file = save.OpenFile()) != null)
                {
                    this.table.TestWriter(file);
                    file.Close();
                }
            }
        }
    }
}
