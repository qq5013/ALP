using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using DataGridViewAutoFilter;

namespace App.View.Dispatcher
{
    public partial class frmTaskQuery :BaseForm
    {
        
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmTaskQuery()
        {
            InitializeComponent();
        }
        private void BindData()
        {

            string filter = " 1=1 ";
            filter += string.Format(" AND TO_DATE('{0}','yyyy/mm/dd hh24:mi:ss')<=Readtime", dtpStartTime.Value.ToString("yyyy/MM/dd HH:mm:00"));
            filter += string.Format(" AND Readtime<=TO_DATE('{0}','yyyy/mm/dd hh24:mi:ss')", dtpEndTime.Value.ToString("yyyy/MM/dd HH:mm:59"));
            if (cmbTaskType.Text != "")
            {
                filter += string.Format(" AND TASKTYPE='{0}'", cmbTaskType.Text);
            }
            if (cmbCraneNo.Text != "")
            {
                filter += string.Format(" AND ASRSID='{0}'", cmbCraneNo.Text);
            }
            if ( txtPalletCode.Text.Trim() != "")
            {
                filter += string.Format(" AND PALLETID LIKE '%{0}%'", txtPalletCode.Text.Trim());
            }
            if (txtCellCode.Text.Trim() != "")
            {
                filter += string.Format(" AND (SLOCATION LIKE '%{0}%' OR DLOCATION LIKE '%{0}%') ", txtCellCode.Text.Trim());
            }

            DataTable dt = bll.FillDataTable("WCS.SelectTaskQuery", new DataParameter[] { new DataParameter("{0}", filter) });
            bsMain.DataSource = dt;
        }

        private void frmTaskQuery_Load(object sender, EventArgs e)
        {
            this.dtpStartTime.Value = DateTime.Now.AddHours(-1);
            for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}

