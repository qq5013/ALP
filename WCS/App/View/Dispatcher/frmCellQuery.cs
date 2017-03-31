using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Dispatcher
{
    public partial class frmCellQuery : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();
        private Dictionary<int, string> ShelfCode = new Dictionary<int, string>();
        private Dictionary<int, string> ShelfName = new Dictionary<int, string>();
        private Dictionary<int, int> ShelfRow = new Dictionary<int, int>();
        private Dictionary<int, int> ShelfColumn = new Dictionary<int, int>();

        private DataTable cellTable = null;        
        private bool needDraw = false;
        private bool filtered = false;

        private int[] Columns = new int[17];
        private int[] Rows = new int[17];
            
        private int[] Page = new int[17];
        private int[] PageShelf = new int[17];
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[3];
        private int left = 5;
        string CellCode = "";
        private bool IsWheel = true;

        public frmCellQuery()
        {
            InitializeComponent();
            //设置双缓冲
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);

            Filter.EnableFilter(dgvMain);
            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;

            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);

            this.PColor.Visible = false;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Filter.Trim().Length != 0)
                {
                    DialogResult result = MessageBox.Show("重新读入数据请选择'是(Y)',清除过滤条件请选择'否(N)'", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.No:
                            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                            return;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                ShelfCode.Clear();

                DataTable dtShelf = bll.FillDataTable("CMD.SelectShelf");
                for (int i = 0; i < dtShelf.Rows.Count; i++)
                {
                    ShelfCode.Add(i + 1, dtShelf.Rows[i]["ShelfCode"].ToString());
                    ShelfName.Add(i + 1, dtShelf.Rows[i]["ShelfName"].ToString());
                }
          

                btnRefresh.Enabled = false;
                btnChart.Enabled = false;

                pnlProgress.Top = (pnlMain.Height - pnlProgress.Height) / 3;
                pnlProgress.Left = (pnlMain.Width - pnlProgress.Width) / 2;
                pnlProgress.Visible = true;
                Application.DoEvents();

                cellTable = bll.FillDataTable("WCS.SelectCell");
                bsMain.DataSource = cellTable;

                pnlProgress.Visible = false;
                btnRefresh.Enabled = true;
                btnChart.Enabled = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入数据失败，原因：" + exp.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (cellTable != null && cellTable.Rows.Count != 0)
            {
                if (pnlData.Visible)
                {
                    this.PColor.Visible = true;
                    filtered = bsMain.Filter != null;
                    needDraw = true;
                    btnRefresh.Enabled = false;
                    pnlData.Visible = false;
                    pnlChart.Visible = true;
                    btnChart.Text = "列表";
                }
                else
                {
                    this.PColor.Visible = false;
                    needDraw = false;
                    btnRefresh.Enabled = true;
                    pnlData.Visible = true;
                    pnlChart.Visible = false;
                    btnChart.Text = "图形";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (needDraw)
                {
                    for (int i = 0; i <= PageShelf[currentPage - 1]-1; i++)
                    {
                        int key = 0;//currentPage * PageShelf[currentPage-1] + i - 1;


                        for (int j = 0; j < currentPage; j++)
                        {
                            key += PageShelf[j];
                        }
                        if (PageShelf[currentPage - 1] > 1)
                            key += i - 1;
                        if (!shelf.ContainsKey(key))
                        {
                            DataRow[] rows = cellTable.Select(string.Format("ShelfCode='{0}'", ShelfCode[key]), "CellCode desc");
                            shelf.Add(key, rows);
                            ShelfRow.Add(key, int.Parse(rows[0]["Rows"].ToString()));
                            ShelfColumn.Add(key, int.Parse(rows[0]["Columns"].ToString()));

                            SetCellSize(ShelfColumn[key], ShelfRow[key], PageShelf[currentPage - 1]);
                        }
                        Font font = new Font("微软雅黑", 9);
                        SizeF size = e.Graphics.MeasureString("A排01层", font);
                        float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                        size = e.Graphics.MeasureString("13", font);
                        float adjustWidth = (cellWidth - size.Width) / 2;

                        DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth);

                        int tmpLeft = left + Columns[currentPage - 1]* cellWidth + 5;

                        for (int j = 0; j < Rows[currentPage-1]; j++)
                        {

                            string s = string.Format("{0}排{1}层", ShelfName[key], Convert.ToString(ShelfRow[key] - j).PadLeft(2, '0'));
                            e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i] + (j + 1) * cellHeight);                            
                        }
                    }
                }
                IsWheel = false;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth)
        {
            string shelfCode = cellRows[0]["ShelfCode"].ToString();
            foreach (DataRow cellRow in cellRows)
            {
                
                int column = Convert.ToInt32(cellRow["CellColumn"]) ;
                string cellCode = cellRow["CellCode"].ToString();

                int row = Rows[currentPage-1] - Convert.ToInt32(cellRow["CellRow"]) + 1;
                int quantity = ReturnColorFlag(cellRow["PalletCode"].ToString(), cellRow["IsActive"].ToString(), cellRow["IsLock"].ToString(), cellRow["ErrorFlag"].ToString());

                int x = left + (Columns[currentPage - 1] - column) * cellWidth;
                int y = top + row * cellHeight;

                Pen pen = new Pen(Color.DarkCyan, 2);
                g.DrawRectangle(pen, new Rectangle(x, y, cellWidth, cellHeight));

                if (!filtered)
                {
                    FillCell(g, top, row, column, quantity);
                }
            }
            for (int j = 1; j <= int.Parse(cellRows[0]["Columns"].ToString()); j++)
            {
                DataRow[] drsExists = cellTable.Select(string.Format("shelfcode='{0}' and CellColumn={1}", shelfCode, j));
                if (drsExists.Length > 0)
                    g.DrawString(j.ToString(), new Font("微軟雅黑", 9), Brushes.DarkCyan, left + (Columns[currentPage - 1] - j) * cellWidth + adjustWidth, top + cellHeight * (Rows[currentPage - 1] + 1) + 3);
            }
        }

     
        private void FillCell(Graphics g, int top, int row, int column, int quantity)
        {           
            int x = left + (column-1) * cellWidth;

            int y = top + row * cellHeight;
            if (quantity == 1)  //空货位锁定
                g.FillRectangle(Brushes.Yellow, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 2) //有货未锁定
                g.FillRectangle(Brushes.Blue, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 3) //有货且锁定
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 4) //禁用
                g.FillRectangle(Brushes.Gray, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 5) //有问题
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 6) //托盘
                g.FillRectangle(Brushes.Orange, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 7) //托盘锁定
                g.FillRectangle(Brushes.Gold, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            
        }
        private void pnlChart_Resize(object sender, EventArgs e)
        {
            SetCellSize(Columns[currentPage - 1], Rows[currentPage - 1], PageShelf[currentPage - 1]);
           
        }

        private void SetCellSize(int Columns, int Rows,int PageShelf)
        {
            top[0] = 0;
            top[1] = pnlContent.Height / 2;
            if (PageShelf == 1)
                top[1] = pnlContent.Height;

            cellWidth = (pnlContent.Width - 90 - sbShelf.Width - 20) / Columns;
            cellHeight = (pnlContent.Height / PageShelf) / (Rows + PageShelf);
            
        }

        private void pnlChart_MouseClick(object sender, MouseEventArgs e)
        {
            int i = e.Y < top[1] ? 0 : 1;
             

            int shelf = 0;//currentPage * PageShelf[currentPage-1] + i - 1;


            for (int j = 0; j < currentPage; j++)
            {
                shelf += PageShelf[j];
            }
            if (PageShelf[currentPage - 1] > 1)
                shelf += i - 1;


            int column = Columns[currentPage - 1] - (e.X - left) / cellWidth;

            int row = Rows[currentPage - 1] - (e.Y - top[i]) / cellHeight + 1;

            if (column <= Columns[currentPage - 1] && row <= Rows[currentPage - 1])
            {
                DataRow[] cellRows = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                if (cellRows.Length != 0)
                    CellCode = cellRows[0]["CellCode"].ToString();
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }        
        private void pnlChart_MouseEnter(object sender, EventArgs e)
        {
            pnlChart.Focus();
        }

        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            IsWheel = true;
            if (e.Delta < 0 && currentPage + 1 <= 17)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;
        }

        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos > 17)
                return;
            if (pos != currentPage)
            {
                currentPage = pos;
                SetCellSize(Columns[currentPage - 1], Rows[currentPage - 1], PageShelf[currentPage - 1]);
                pnlChart.Invalidate();
               
            }
        }

        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvMain.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvMain.RowHeadersDefaultCellStyle.Font, rectangle, dgvMain.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private int ReturnColorFlag(string ProductCode, string IsActive, string IsLock, string ErrFlag)
        {
            int Flag = 0;
            if (ProductCode == "")
            {
                if (IsActive == "0")
                    Flag = 4;
                if (ErrFlag == "1")
                    Flag = 5;
            }
            else
                Flag = 2;
            return Flag;
        }

         

        private int X, Y;
        private void pnlChart_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsWheel) return;
            if (X != e.X || Y != e.Y)
            {
                int i = e.Y < top[1] ? 0 : 1;
                int shelf = 0;//currentPage * PageShelf[currentPage-1] + i - 1;


                for (int j = 0; j < currentPage; j++)
                {
                    shelf += PageShelf[j];
                }
                if (PageShelf[currentPage - 1] > 1)
                    shelf += i - 1;
               

                int column = Columns[currentPage - 1] - (e.X - left) / cellWidth;
                int row = Rows[currentPage - 1] - (e.Y - top[i]) / cellHeight + 1;

                DataRow[] drsExists = cellTable.Select(string.Format("shelfcode='{0}' and CellColumn={1} and CellRow={2}", ShelfCode[shelf], column, row));

                if (drsExists.Length > 0)
                {
                    if (column <= Columns[currentPage - 1] && row <= Rows[currentPage - 1] && row > 0 && column > 0)
                    {
                        string tip ="貨位："+ drsExists[0]["CellName"].ToString() +( drsExists[0]["PalletCode"].ToString().Length > 0 ? " 箱/托盤條碼：" + drsExists[0]["PalletCode"].ToString() : "");
                        toolTip1.SetToolTip(pnlChart, tip);
                    }
                    else
                        toolTip1.SetToolTip(pnlChart, null);
                }
                else
                {
                    toolTip1.SetToolTip(pnlChart, null);
                }

                X = e.X;
                Y = e.Y;
            }
        }
        private void btnReQuery_Click(object sender, EventArgs e)
        {
            cellTable = bll.FillDataTable("WCS.SelectCell");
            bsMain.DataSource = cellTable;
            pnlChart.Invalidate();
        }

        private void frmCellQuery_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                Columns[i] = 60;
                PageShelf[i] = 2;
            }
            Rows[0] = 5;
            Rows[1] = 5;
            Rows[2] = 5;
            Rows[3] = 4;
            Rows[4] = 4;
            Rows[5] = 6;
            Rows[6] = 6;
            Rows[7] = 6;
            Rows[8] = 6;
            Rows[9] = 4;
            Rows[10] = 4;
            for (int i = 11; i < 17; i++)
            {
                Columns[i] = 98;
                Rows[i] = 16;
                PageShelf[i] = 1;
            }

            
           
          
        }       
    }
}
