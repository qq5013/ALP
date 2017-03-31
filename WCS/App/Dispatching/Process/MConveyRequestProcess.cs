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
    /// 入庫讀取條碼后，請求入庫
    /// </summary>
    public class MConveyRequestProcess : AbstractProcess
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
                string PalletCode = ObjectUtil.GetObject(WriteToService(stateItem.Name, ConveyID + "RPalletCode")).ToString();
                //根據條碼，獲取任務；先在WCS_Task中獲取任務，如無任務，則在中間表獲取
                DataParameter[] paras = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='11' and State in (0,1) and Palletcode='{0}'", PalletCode)) };
                BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                DataTable dt = bllStock.FillDataTable("WCS.SelectConveyTask", paras);
                bool blnHasTask = false;
                if (dt.Rows.Count == 0)
                {
                    BLL.BLLBase bllMiddle = new BLL.BLLBase("MiddleDB");
                    dt = bllMiddle.FillDataTable("Middle.SelectInStockRequestTask", new DataParameter[] { new DataParameter("@Device", "ML"), new DataParameter("{0}", string.Format("hu_id='{0}'", PalletCode)) });
                    if (dt.Rows.Count > 0)
                    {
                        blnHasTask = true;
                        DataRow[] drs = dt.Select("len(from_location_id)=3");
                        if (drs.Length > 0)
                        {
                            drs[0]["location_id"] = ConveyID;
                            dt.AcceptChanges();
                        }
                        BLL.Server.InsertTaskToWcs(dt, true);
                    }

                }
                if (!blnHasTask)
                    dt = bllStock.FillDataTable("WCS.SelectConveyTask", paras);
                if (dt.Rows.Count > 0)
                {
                    //判斷所在巷道是否擁堵
                    bool bln = false;
                    if (bln)
                    {
                        WriteToService(stateItem.Name, ConveyID + "WriteFinished", 2);
                    }
                    else
                    {




                        string TaskNo = dt.Rows[0]["TaskNo"].ToString();
                        string SubTaskID = dt.Rows[0]["subtask_id"].ToString();
                        string Destination = dt.Rows[0]["ToStation"].ToString();
                        //更新開始入庫

                        WriteToService(stateItem.Name, ConveyID + "WTaskNo", TaskNo);
                        WriteToService(stateItem.Name, ConveyID + "WPalletCode", PalletCode);
                        WriteToService(stateItem.Name, ConveyID + "Destination", Destination); //目的地
                        if (WriteToService(stateItem.Name, ConveyID + "WriteFinished", 1))
                        {
                            List<string> comds = new List<string>();
                            List<DataParameter[]> Paras = new List<DataParameter[]>();
                            comds.Add("WCS.UpdateTaskState");
                            Paras.Add(new DataParameter[] { new DataParameter("{0}", "State=1,Convey_StartDate=getdate()"),
                                                            new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))}
                                     );

                            comds.Add("WCS.UpdateTaskTmpStatus");
                            Paras.Add(new DataParameter[] { new DataParameter("@status", "STR"),
                                                            new DataParameter("@subtaskid", SubTaskID)});


                            bllStock.ExecTran(comds.ToArray(), Paras);
                        }

                    }
                }
                else
                {
                    //報警，無任務，返回出口
                    WriteToService(stateItem.Name, ConveyID + "WriteFinished", 3);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ConveyRequestProcess出現錯誤，錯誤原因：" + ex.Message);
            }
        }
    }
}
