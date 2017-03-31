using System;
using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;

namespace App.Dispatching.Process
{
    /// <summary>
    /// 出庫到達撿貨站台
    /// 空托盤移動完成
    /// </summary>
    public class MConveyPickProcess : AbstractProcess
    {


        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            object obj = ObjectUtil.GetObject(stateItem.State);
            if (obj == null)
                return;
            if (obj.ToString() != "1")
                return;
            try
            {
                string ConveyID = stateItem.ItemName.Substring(0, 4);
                string TaskNo = ObjectUtil.GetObject(WriteToService(stateItem.Name, ConveyID + "RTaskNo")).ToString();

                BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                DataTable dtTask = bllStock.FillDataTable("WCS.SelectConveyTask", new DataParameter[] { new DataParameter("{0}", string.Format("TaskNo='{0}'", TaskNo)) });
                string TaskID = dtTask.Rows[0]["TaskID"].ToString();
                string SubTaskID = dtTask.Rows[0]["SubTaskID"].ToString();
                string PalletCode = dtTask.Rows[0]["PalletCode"].ToString();

                //找出該托盤的任務，然後插入WCS
                BLL.BLLBase bllMiddle = new BLL.BLLBase("Middle");
                DataTable dtMiddle = bllMiddle.FillDataTable("Middle.SelectConveyMoveTask", new DataParameter[] { new DataParameter("@Device", "ML"), new DataParameter("{0}", string.Format("main.task_id={0} and hu_id='{1}' and subtask_id!={1}", TaskID, PalletCode, SubTaskID)) });
                if (dtMiddle.Rows.Count > 0)
                {
                    dtMiddle.Rows[0]["location_id"] = ConveyID;
                    BLL.Server.InsertTaskToWcs(dtMiddle, false);
                }
                else
                {
                    Logger.Error("MConveyPickProcess找不到料盒：" + PalletCode+" 的後續處理方式！"); 
                }


                bllStock.ExecNonQuery("WCS.Sp_TaskProcess", new DataParameter[] { new DataParameter("@TaskNo", TaskNo) });



                List<DataParameter[]> paras = new List<DataParameter[]>();
                List<string> comds = new List<string>();
                comds.Add("Middle.UpdateAsrsSubTaskRTN");
                paras.Add(new DataParameter[] { new DataParameter("@SubTaskID", SubTaskID) });

                //int count = bllMiddle.GetRowCount("si_asrs_task_detail", string.Format("task_id={0} and status not in ('RTN','CMP') and subtask_id<>{1}", TaskID, SubTaskID));
                //if (count == 0)
                //{
                comds.Add("Middle.UpdateAsrsTaskRTN");
                paras.Add(new DataParameter[] { new DataParameter("@TaskID", TaskID) });
                //}
                bllMiddle.ExecTran(comds.ToArray(), paras);
            }
            catch (Exception ex)
            {
                Logger.Error("MConveyInStockProcess出現錯誤，錯誤原因：" + ex.Message);
            }
        }

    }
}
