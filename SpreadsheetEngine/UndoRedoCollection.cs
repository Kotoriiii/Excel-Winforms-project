// <copyright file="UndoRedoCollection.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cpts321
{
    using System.Collections.Generic;

    /// <summary>
    /// Class for UndoRedo.
    /// </summary>
    public class UndoRedoCollection
    {
        private string curText;
        private string prevText;
        private SpreadsheetCell cell;
        private string changedName;
        private uint curColor;
        private List<uint> prevColor;
        private List<SpreadsheetCell> listcell;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// </summary>
        /// <param name="changed">change name.</param>
        public UndoRedoCollection(string changed)
        {
            this.changedName = changed;
        }

        /// <summary>
        /// Set varible when the property is text.
        /// </summary>
        /// <param name="changeCell">cell need change.</param>
        /// <param name="prev">prev text.</param>
        /// <param name="cur">current text.</param>
        public void SetTextVariable(SpreadsheetCell changeCell, string prev, string cur)
        {
            this.cell = changeCell;
            this.prevText = prev;
            this.curText = cur;
        }

        /// <summary>
        /// Set varible when the property is color.
        /// </summary>
        /// <param name="changeCell">cell need change.</param>
        /// <param name="prev">prev color.</param>
        /// <param name="cur">current color.</param>
        public void SetColortVarible(List<SpreadsheetCell> changeCell, List<uint> prev, uint cur)
        {
            this.listcell = changeCell;
            this.prevColor = prev;
            this.curColor = cur;
        }

        /// <summary>
        /// Gets change name.
        /// </summary>
        public string ChangedName
        {
            get
            {
                return this.changedName;
            }
        }

        /// <summary>
        /// change value when button is redo.
        /// </summary>
        public void RedoChange()
        {
            if (this.changedName == "Text Changes")
            {
                this.cell.Text = this.curText;
            }
            else if (this.changedName == "Color Changes")
            {
                foreach (Cell cell in this.listcell)
                {
                    cell.Color = this.curColor;
                }
            }
        }

        /// <summary>
        /// change value when button is undo.
        /// </summary>
        public void UndoChange()
        {
            if (this.changedName == "Text Changes")
            {
                this.cell.Text = this.prevText;
            }
            else if (this.changedName == "Color Changes")
            {
                for (int i = 0; i < this.listcell.Count; i++)
                {
                    this.listcell[i].Color = this.prevColor[i];
                }
            }
        }
    }
}
