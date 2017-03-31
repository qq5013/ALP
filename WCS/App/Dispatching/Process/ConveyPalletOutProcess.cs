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
    /// 空托盤出庫口
    /// </summary>
    public class ConveyPalletOutProcess : AbstractProcess
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
                string ConveyID = stateItem.ItemName.Substring(0, 4);
                string TaskNo = ObjectUtil.GetObject(WriteToService(stateItem.Name, ConveyID + "RTaskNo")).ToString();
                BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
                DataTable dtTask = bllStock.FillDataTable("WCS.SelectConveyTask", new DataParameter[] { new DataParameter("{0}", string.Format("TaskNo='{0}'", TaskNo)) });
                string TaskID = dtTask.Rows[0]["TaskID"].ToString();
                string SubTaskID = dtTask.Rows[0]["SubTaskID"].ToString();

                bllStock.ExecNonQuery("WCS.Sp_TaskProcess", new DataParameter[] { new DataParameter("@TaskNo", TaskNo) });

                List<DataParameter[]> paras = new List<DataParameter[]>();
                string[] Comds = new string[2];
                Comds[0] = "Middle.UpdateAsrsTaskRTN";
                Comds[1] = "Middle.UpdateAsrsSubTaskRTN";
                paras.Add(new DataParameter[] { new DataParameter("@TaskID", TaskID) });
                paras.Add(new DataParameter[] { new DataParameter("@SubTaskID", SubTaskID) });

                BLL.BLLBase bllMiddle = new BLL.BLLBase("MiddleDB");
                bllMiddle.ExecTran(Comds, paras);
            }
            catch (Exception ex)
            {
                Logger.Error("ConveyPalletOutProcess出現錯誤，錯誤原因：" + ex.Message);
            }


        }
        //托盤回庫，空托盤出庫，
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();
               DataParameter[] paras = new DataParameter[] { new DataParameter("{0}", string.Format("(TaskType='11' and Flag=5 or TaskType='99') and State=0 and AreaCode='UL'")) };
               BLL.BLLBase bllStock = new BLL.BLLBase("StockDB");
               DataTable dt = bllStock.FillDataTable("WCS.SelectConveyTask", paras);
               BLL.BLLBase bllMiddle = new BLL.BLLBase("Middle");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //判斷WMS是否已經改變狀態為ISS
                    string taskid = dt.Rows[i]["TaskID"].ToString();
                    string PalletCode = dt.Rows[i]["PalletCode"].ToString();
                    string subtaskid = dt.Rows[i]["SubTaskID"].ToString();
                    int count = bllMiddle.GetRowCount("si_asrs_task_detail", "status='ISS' and subtaskid=" + subtaskid);
                    if (count > 0)
                    {
                        string TaskNo = dt.Rows[i]["TaskNo"].ToString();
                        string fromStation = dt.Rows[i]["fromStation"].ToString();
                        string Destination = dt.Rows[i]["ToStation"].ToString();
                        string TaskType = dt.Rows[i]["TaskType"].ToString();

                        WriteToService("Convey", fromStation + "WTaskNo", TaskNo);
                        WriteToService("Convey", fromStation + "WPalletCode", PalletCode);
                        WriteToService("Convey", fromStation + "Destination", Destination); //目的地
                        if (WriteToService("Convey", fromStation + "WriteFinished", 1))
                        {
                            string state = "1";
                            if (TaskType == "99")
                                state = "10";
                            List<string> comds = new List<string>();
                            List<DataParameter[]> Paras = new List<DataParameter[]>();
                            comds.Add("WCS.UpdateTaskState");
                            Paras.Add(new DataParameter[] { new DataParameter("{0}", string.Format( "State={0},Convey_StartDate=getdate()",state)),
                                                            new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))}
                                     );

                            comds.Add("WCS.UpdateTaskTmpStatus");
                            Paras.Add(new DataParameter[] { new DataParameter("@status", "STR"),
                                                            new DataParameter("@subtaskid", subtaskid)});
                            bllStock.ExecTran(comds.ToArray(), Paras);

                            List<string> MiddleComds = new List<string>();
                            List<DataParameter[]> Middleparas = new List<DataParameter[]>();
                            MiddleComds.Add("Middle.UpdateAsrsTaskAck");
                            MiddleComds.Add("Middle.UpdateAsrsSubTaskAck");


                            Middleparas.Add(new DataParameter[] { new DataParameter("@TaskID", taskid) });
                            Middleparas.Add(new DataParameter[] { new DataParameter("@TaskID", taskid), new DataParameter("{0}", string.Format("subtask_id='{0}'", subtaskid)) });
                            bllMiddle.ExecTran(MiddleComds.ToArray(), Middleparas);


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error("ConveyPalletOutProcess中tmWorker出現錯誤，錯誤原因：" + ex.Message);
            }
            finally
            {

                tmWorkTimer.Start();
            }
        }
    }
}
