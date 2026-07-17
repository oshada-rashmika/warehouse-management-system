using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WareHouseApp
{
    public class TablePrinter
    {
        private DataGridView grid;
        private PrintDocument printDoc;
        private string reportTitle;
        private int rowIndex = 0;
        private List<int> columnStarts;
        private List<int> columnWidths;

        public static void PrintDataGridView(DataGridView dgv, string title)
        {
            TablePrinter printer = new TablePrinter(dgv, title);
            printer.Print();
        }

        private TablePrinter(DataGridView dgv, string title)
        {
            this.grid = dgv;
            this.reportTitle = title;
            this.printDoc = new PrintDocument();

            this.printDoc.DefaultPageSettings.Landscape = true;
            
            this.printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            this.printDoc.BeginPrint += new PrintEventHandler(BeginPrint);
        }

        private void Print()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            printDialog.UseEXDialog = true;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error printing: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BeginPrint(object sender, PrintEventArgs e)
        {
            rowIndex = 0;
            columnStarts = new List<int>();
            columnWidths = new List<int>();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int margin = 50;
            int pageHeight = e.PageBounds.Height;
            int pageWidth = e.PageBounds.Width;
            int currentY = margin;

            if (rowIndex == 0)
            {
                Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
                g.DrawString(reportTitle, titleFont, Brushes.Black, new Point(margin, currentY));
                currentY += 40;
                
                string dateStr = "Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                g.DrawString(dateStr, new Font("Segoe UI", 10), Brushes.Gray, new Point(margin, currentY));
                currentY += 30;
                
                int totalWidth = pageWidth - (margin * 2);
                int numCols = grid.Columns.Count;
                if (numCols > 0)
                {
                    int widthPerCol = totalWidth / numCols;
                    int currentX = margin;
                    foreach (DataGridViewColumn col in grid.Columns)
                    {
                        columnStarts.Add(currentX);
                        columnWidths.Add(widthPerCol);
                        currentX += widthPerCol;
                    }
                }
            }

            Font headerFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font rowFont = new Font("Segoe UI", 10);
            int rowHeight = 30;

            if (rowIndex == 0)
            {
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(columnStarts[i], currentY, columnWidths[i], rowHeight);
                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(Pens.Black, rect);
                    g.DrawString(grid.Columns[i].HeaderText, headerFont, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                currentY += rowHeight;
            }

            while (rowIndex < grid.Rows.Count)
            {
                if (currentY + rowHeight > pageHeight - margin)
                {
                    e.HasMorePages = true;
                    return;
                }

                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(columnStarts[i], currentY, columnWidths[i], rowHeight);
                    g.DrawRectangle(Pens.Black, rect);
                    string cellValue = grid.Rows[rowIndex].Cells[i].Value?.ToString() ?? "";
                    Rectangle textRect = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
                    g.DrawString(cellValue, rowFont, Brushes.Black, textRect, new StringFormat { LineAlignment = StringAlignment.Center });
                }

                currentY += rowHeight;
                rowIndex++;
            }

            e.HasMorePages = false;
        }
    }
}
