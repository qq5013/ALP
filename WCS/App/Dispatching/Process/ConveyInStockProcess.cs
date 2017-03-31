using System;
using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;

namespace App.Dispatching.Process
{
    /// <summary>
    /// 托盤到達入庫端處理
    /// </summary>
    public class ConveyInStockProcess : AbstractProcess
    {
        private Timer tmWorkTimer = new Timer();

        public override void Initialize(Context context)
        {
            base.Initialize(context);
            tmWorkTimer.Interval = 2000;
            tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);
            base.Initialize(context);
        }

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
                string ConveyID = stateItem.ItemName.Substring(0, 4);
                string TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, ConveyID + "RTaskNo")));
                string PalletCode = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, ConveyID + "RPalletCode")));
                
                List<DataParameter[]> paras = new List<DataParameter[]>();
                string[] Comds = new string[2];
                Comds[0] = "WCS.UpdateTaskState";
                Comds[1] = "WCS.UpdateTaskAsileNo";


               paras.Add(new DataParameter[] { new DataParameter("{0}", "State=2,Convey_FinishDate=getdate()"),
                                               new DataParameter("{1}", string.Format("TaskNo='{0}' and State in (1,6)", TaskNo)) });
               paras.Add(new DataParameter[] { new DataParameter("@TaskNo", TaskNo),
                                               new DataParameter("@InStationNo",ConveyID) });

                                          
                BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                bllStock.ExecTran(Comds, paras);
                Logger.Error("任務號：" + TaskNo + " 托盤條碼：" + PalletCode + " 到達入庫站台" + ConveyID);
                
            }
            catch (Exception ex)
            {
                Logger.Error("ConveyInStockProcess出現錯誤，錯誤原因：" + ex.Message);
            }
        }
        //并板托盤，下達輸送線任務
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {

                tmWorkTimer.Stop();
                DataParameter[] paras = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='12' and (Flag=2 and Type='MRG' or Flag=9)  and State=5 and AreaCode='UL' ")) };
                BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                DataTable dt = bllStock.FillDataTable("WCS.SelectConveyTask", paras);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //判斷任務是否任務完成
                    string taskid = dt.Rows[i]["TaskID"].ToString();
                    string subtaskid = dt.Rows[i]["SubTaskID"].ToString();
                    string Flag = dt.Rows[i]["Flag"].ToString();
                    string Ttype = dt.Rows[i]["Type"].ToString();
                    string TaskNo = dt.Rows[i]["TaskNo"].ToString();
                    string PalletCode = dt.Rows[i]["PalletCode"].ToString();
                    string fromStation = dt.Rows[i]["fromStation"].ToString();
                    string Destination = dt.Rows[i]["ToStation"].ToString();
                    if (Flag == "2")
                    {
                        //判斷目的地址後方是否有正在盤庫的任務
                        int count = bllStock.GetRowCount("WCS_TASK", string.Format(string.Format("location_id<='{0}' and State=0 and AreaCode='UL'", Destination)));
                        if (count == 0)
                        {
                            count = bllStock.GetRowCount("WCS_TASK", string.Format(string.Format("TaskType='12' and State=6 and ToStation<='{0}' and Flag=9 and AreaCode='UL'", Destination)));
                        }
                        if (count > 0)
                            continue;


                        WriteToService("Convey", fromStation + "WTaskNo", TaskNo);
                        WriteToService("Convey", fromStation + "WPalletCode", PalletCode);
                        WriteToService("Convey", fromStation + "Destination", Destination); //目的地
                        if (WriteToService("Convey", fromStation + "WriteFinished", 1))
                        {
                            paras = new DataParameter[] { new DataParameter("{0}", "State=6,Convey_StartDate=getdate()"),
                                                               new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))
                                                             };
                            bllStock.ExecNonQuery("WCS.UpdateTaskState", paras);
                        }

                         
                    }
                    else
                    {
                        int count = bllStock.GetRowCount("WCS_TASK", string.Format("TaskID={0} and SubTaskID!={1} and State=7"));
                        if (count > 1)
                        {
                            WriteToService("Convey", fromStation + "WTaskNo", TaskNo);
                            WriteToService("Convey", fromStation + "WPalletCode", PalletCode);
                            WriteToService("Convey", fromStation + "Destination", Destination); //目的地
                            if (WriteToService("Convey", fromStation + "WriteFinished", 1))
                            {
                                paras = new DataParameter[] { new DataParameter("{0}", "State=6,Convey_StartDate=getdate()"),
                                                               new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))
                                                             };
                                bllStock.ExecNonQuery("WCS.UpdateTaskState", paras);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error("ConveyStackProcess中tmWorker出現錯誤，錯誤原因：" + ex.Message);
            }
            finally
            {

                tmWorkTimer.Start();
            }
        }

    }
}
