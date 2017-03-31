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
    /// 托盤到達入庫端處理
    /// </summary>
    public class MConveyInStockProcess : AbstractProcess
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
                //到達入庫站台，同時更新任務所在巷道
                 BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                string ConveyID = stateItem.ItemName.Substring(0, 4);
                string TaskNo = ObjectUtil.GetObject(WriteToService(stateItem.Name, ConveyID + "RTaskNo")).ToString();
                string TaskAB = "A";

                DataTable dtTask=bllStock.FillDataTable("",new DataParameter[]{new DataParameter("","")});
                if (dtTask.Rows.Count > 0)
                {
                    if (ConveyID == "101") //判斷是否為入庫站台工位2
                    {
                        TaskAB = "B";
                        string cellCode = dtTask.Rows[0]["CellCode"].ToString();
                        //判斷該任務是否為98列貨位，如果是，則不更新為完成，待取走工位1后，繼續前行。
                        if (cellCode.Substring(2, 2) == "98")
                            return;
                    }

                    List<DataParameter[]> paras = new List<DataParameter[]>();
                    string[] Comds = new string[2];
                    Comds[0] = "WCS.UpdateTaskState";
                    Comds[1] = "WCS.UpdateTaskAsileNo";


                    paras.Add(new DataParameter[] { new DataParameter("{0}", "State=2,Convey_FinishDate=getdate()"),
                                               new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo)) });
                    paras.Add(new DataParameter[] { new DataParameter("@TaskNo", TaskNo),
                                               new DataParameter("@InStationNo",ConveyID) });



                    bllStock.ExecTran(Comds, paras);
                }
                else
                {
                    Logger.Error("MConveyInStockProcess中找不到任務,任務號：" + TaskNo);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("ConveyInStockProcess出現錯誤，錯誤原因：" + ex.Message);
            }
        }
       
    }
}
