// <copyright file="SpreadSheetCell.cs" company="Jiangquan Li">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cpts321
{
    using System.ComponentModel;

    /// <summary>
    /// This cell information for Spreadsheet.
    /// </summary>
    public abstract class SpreadsheetCell : INotifyPropertyChanged
    {
        private int rowindex;
        private int columindex;

        /// <summary>
        /// text data.
        /// </summary>
        protected string text;

        /// <summary>
        /// value data.
        /// </summary>
        protected string value;

        /// <summary>
        /// color for each cell.
        /// </summary>
        public uint BGColor;

        /// <summary>
        /// Event to cell Property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event to change color.
        /// </summary>
        public event PropertyChangedEventHandler ColorPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="row">row from spreadsheet.</param>
        /// <param name="coloum">colum from spreadsheet.</param>
        public SpreadsheetCell(int row, int coloum)
        {
            this.rowindex = row;
            this.columindex = coloum;
            this.text = string.Empty;
            this.value = string.Empty;
            this.BGColor = 0xFFFFFFFF;
        }

        /// <summary>
        /// Gets or sets row constructor.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowindex; }
            set { this.rowindex = value; }
        }

        /// <summary>
        /// Gets or sets colum constructor.
        /// </summary>
        public int ColumIndex
        {
            get { return this.columindex; }
            set { this.columindex = value; }
        }

        /// <summary>
        /// Gets or sets text constructor.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value)
                {
                    return;
                }
                else
                {
                    this.text = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        /// <summary>
        /// Gets value constructor.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            internal set
            {
                /*if (this.value == value)
                {
                    return;
                }
                else
                {
                    this.value = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }*/
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets text constructor.
        /// </summary>
        public uint Color
        {
            get
            {
                return this.BGColor;
            }

            set
            {
                if (this.BGColor == value)
                {
                    return;
                }
                else
                {
                    this.BGColor = value;
                    this.ColorPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
                }
            }
        }

        /// <summary>
        /// proerty change function.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }
    }

    /// <summary>
    /// Cell program.
    /// </summary>
    public class Cell : SpreadsheetCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">row from spreadsheet.</param>
        /// <param name="col">colum from spreadsheet.</param>
        public Cell(int row, int col)
            : base(row, col)
        {
        }
    }
}