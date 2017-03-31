using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;

namespace App.Dispatching.Process
{
    public class CraneProcess : AbstractProcess
    {
        private class rCrnStatus
        {
            
            public int Action { get; set; }
            public int io_flag { get; set; }
            public string ServiceName { get; set; }
            public string DeviceNo { get; set; }
            public string AisleNo { get; set; }
            public string OutStationNo { get; set; }

            public rCrnStatus()
            {
                Action = 0;
                io_flag = 0;
                ServiceName = "";
                DeviceNo = "";
                AisleNo = "";
                OutStationNo = "";
            }
        }

        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        private Dictionary<int, rCrnStatus> dCrnStatus = new Dictionary<int, rCrnStatus>();
        private Timer tmWorkTimer = new Timer();
        private bool blRun = false;
        private string AreaCode;
        private string ConveyServer = "Convey";


        public override void Initialize(Context context)
        {
            try
            {
                AreaCode = BLL.Server.GetAreaCode();
                //获取堆垛机信息
                DataTable dt = bll.FillDataTable("CMD.SelectDevice", new DataParameter[] { new DataParameter("@AreaCode", AreaCode) });
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    if (!dCrnStatus.ContainsKey(i))
                    {
                        rCrnStatus crnsta = new rCrnStatus();
                        dCrnStatus.Add(i, crnsta);

                    
                     
                        dCrnStatus[i].io_flag = 0;
                        dCrnStatus[i].ServiceName = dt.Rows[i - 1]["ServiceName"].ToString();
                        dCrnStatus[i].Action = int.Parse(dt.Rows[i - 1]["State"].ToString());
                        dCrnStatus[i].DeviceNo = dt.Rows[i - 1]["DeviceNo"].ToString();
                        dCrnStatus[i].AisleNo = dt.Rows[i - 1]["AisleNo"].ToString();
                        dCrnStatus[i].OutStationNo = dt.Rows[i - 1]["OutStationNo"].ToString();
                    }
                }

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);
                

                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess堆垛機初始化出錯，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            //object obj = ObjectUtil.GetObject(stateItem.State);            
            //if (obj == null)
            //    return;

            switch (stateItem.ItemName)
            {
                case "CraneTaskFinished":
                    try
                    {
                        object obj = ObjectUtil.GetObject(stateItem.State);
                        string TaskFinish = obj.ToString();
                        if (TaskFinish.Equals("True") || TaskFinish.Equals("1"))
                        {
                            string TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(stateItem.Name, "CraneTaskNo")));

                            if (TaskNo == "0")
                                return;
                            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskNo='{0}'", TaskNo)), new DataParameter("{1}", "1") };
                            DataTable dtTask = bll.FillDataTable("WCS.SelectCraneTask", param);

                            string TaskType = dtTask.Rows[0]["TaskType"].ToString();
                            string TType = dtTask.Rows[0]["Type"].ToString();
                            int Flag = int.Parse(dtTask.Rows[0]["Flag"].ToString());
                            string State=dtTask.Rows[0]["State"].ToString();

                            Logger.Info(stateItem.ItemName + "任務完成,任務號:" + TaskNo);

                            DataTable dtConveyTask = bll.FillDataTable("WCS.SelectConveyTask", param);
                            string Destination = dtConveyTask.Rows[0]["ToStation"].ToString();
                            string PalletCode = dtConveyTask.Rows[0]["Palletcode"].ToString();
                            string ConveyID = dtTask.Rows[0]["OutStationNo"].ToString(); //出庫站台

                            bll.ExecNonQuery("WCS.Sp_TaskProcess", param);

                            //更新中間庫，狀態為CMP
                            if (TaskType == "11" || (TaskType == "13" && Flag == 3) || (TaskType == "13" && Flag == 6 && State=="3" ))
                            {
                                string TaskID = dtTask.Rows[0]["TaskID"].ToString();
                                string SubTaskID = dtTask.Rows[0]["SubTaskID"].ToString();

                                List<DataParameter[]> paras = new List<DataParameter[]>();
                                string[] Comds = new string[2];
                                Comds[0] = "Middle.UpdateAsrsTaskRTN";
                                Comds[1] = "Middle.UpdateAsrsSubTaskRTN";
                                paras.Add(new DataParameter[] { new DataParameter("@TaskID", TaskID) });
                                paras.Add(new DataParameter[] { new DataParameter("@SubTaskID", SubTaskID) });

                                BLL.BLLBase bllMiddle = new BLL.BLLBase("MiddleDB");
                                bllMiddle.ExecTran(Comds, paras);

                            }

                            if (TaskType == "12")
                            {
                                if ((Flag == 2 && TType != "MRG") || Flag == 4)
                                {
                                    //出庫，整版出庫，寫入輸送線，直接到達指定位置

                                    WriteToService(stateItem.Name, ConveyID + "WTaskNo", TaskNo);
                                    WriteToService(stateItem.Name, ConveyID + "WPalletCode", PalletCode);
                                    WriteToService(stateItem.Name, ConveyID + "Destination", Destination); //目的地
                                    if (WriteToService(stateItem.Name, ConveyID + "WriteFinished", 1))
                                    {
                                        Logger.Info("任務號：" + TaskNo + "已經下達輸送線：" + ConveyID + " 目的地址：" + Destination);
                                        bll.ExecNonQuery("WCS.UpdateTaskState", new DataParameter[] { new DataParameter("{0}", "State=6,Crane_FinishDate=getdate(), Convey_StartDate=getdate()"), new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo)) });

                                    }
                                }
                                else
                                {
                                    //并板出庫，寫入輸送線任務號，不確認直接到達目的地。
                                    WriteToService(stateItem.Name, ConveyID + "WTaskNo", TaskNo);
                                    WriteToService(stateItem.Name, ConveyID + "WriteFinished", 0);

                                }

                            }
                            else if (TaskType == "13" && Flag == 6 && State=="4") //移庫出庫
                            {
                                //不同巷道移庫，寫入輸送線任務，直接到目的巷道入庫站台。
                                WriteToService(stateItem.Name, ConveyID + "WTaskNo", TaskNo);
                                WriteToService(stateItem.Name, ConveyID + "WPalletCode", PalletCode);
                                WriteToService(stateItem.Name, ConveyID + "Destination", Destination); //目的地
                                if (WriteToService(stateItem.Name, ConveyID + "WriteFinished", 1))
                                {
                                    Logger.Info("任務號：" + TaskNo + "已經下達輸送線：" + ConveyID + " 目的地址：" + Destination);
                                    bll.ExecNonQuery("WCS.UpdateTaskState", new DataParameter[] { new DataParameter("{0}", "State=6,Crane_FinishDate=getdate(), Convey_StartDate=getdate()"), new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo)) });
                                    bll.ExecNonQuery("WCS.UpdateCellEmpty", new DataParameter[] { new DataParameter("@CellCode", dtTask.Rows[0]["CellCode"].ToString()) });
                                }

                            }
                            sbyte[] staskNo = new sbyte[10];
                            Util.ConvertStringChar.stringToBytes("", 10).CopyTo(staskNo, 0);
                            WriteToService(stateItem.Name, "TaskNo", 0);
                        }
                    }
                    catch (Exception ex1)
                    {
                        Logger.Info("CraneProcess中CraneTaskFinished出錯：" + ex1.Message);
                    }
                    break;
                case "Run":
                    blRun = (int)stateItem.State == 1;
                    if (blRun)
                    {
                        tmWorkTimer.Start();
                        Logger.Info("堆垛机联机");
                    }
                    else
                    {
                        tmWorkTimer.Stop();
                        Logger.Info("堆垛机脱机");
                    }
                    break;
                default:
                    break;
            }
            
            
            return;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!blRun)
                    return;
                tmWorkTimer.Stop();

                DataTable dt = bll.FillDataTable("CMD.SelectDevice", new DataParameter[] { new DataParameter("@AreaCode", AreaCode) });
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    if (dCrnStatus.ContainsKey(i))
                    {
                        dCrnStatus[i].Action = int.Parse(dt.Rows[i - 1]["State"].ToString());
                    }

                    if (dCrnStatus[i].Action != 1)
                        continue;
                    if (dCrnStatus[i].io_flag == 0)
                    {
                        CraneOut(i);
                    }
                    else
                    {
                        CraneIn(i);
                    }
                }
                
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        /// <summary>
        /// 检查堆垛机入库状态
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <returns></returns>
        private bool Check_Crane_Status_IsOk(int craneNo)
        {
            try
            {
                string serviceName = dCrnStatus[craneNo].ServiceName;

                string plcTaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CraneTaskNo")));
                string craneMode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneMode")).ToString();
                string CraneState = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneState")).ToString();
                string CraneAlarmCode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneAlarmCode")).ToString();
              

                if (plcTaskNo != "0" && craneMode == "1" && CraneAlarmCode == "0" && CraneState == "1" )
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }            
        }        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="craneNo"></param>
        private void CraneOut(int craneNo)
        {
            // 判断堆垛机的状态 自动  空闲
            //Logger.Debug("判断堆垛机" + piCrnNo.ToString() + "能否出库");
            try
            {
                
                //判断堆垛机
                if (!Check_Crane_Status_IsOk(craneNo))
                {
                    //Logger.Info("堆垛机状态不符合出库");
                    return;
                }
                //切换入库优先
                dCrnStatus[craneNo].io_flag = 1;
            }
            catch (Exception e)
            {
                Logger.Debug("Crane out 状态检查错误:" + e.Message.ToString());
                return;
            }

            string serviceName = dCrnStatus[craneNo].ServiceName;
            DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskType in ('12','13') and WCS_Task.State='0' and WCS_TASK.AisleNo='{1}' and WCS_TASK.AreaCode='{1}'", dCrnStatus[craneNo].AisleNo, AreaCode)), new DataParameter("{1}", "1") };
            DataTable dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);


            bool blnHasTask = false;
            if (dt.Rows.Count == 0)
            {
                BLL.BLLBase bllMiddle = new BLL.BLLBase("MiddleDB");
                DataTable dtMiddle = bllMiddle.FillDataTable("Middle.SelectCraneOutTask", new DataParameter[] { new DataParameter("@Device", AreaCode), new DataParameter("{0}", string.Format("from_aisle='{0}'", dCrnStatus[craneNo].AisleNo)), new DataParameter("{1}", 1) });
                if (dtMiddle.Rows.Count > 0)
                {
                    BLL.Server.InsertTaskToWcs(dtMiddle,true);
                    blnHasTask = true;
                }

            }
            if (blnHasTask)
                dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);
            //出库
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string TaskNo = dr["TaskNo"].ToString();
                byte taskType = byte.Parse(dt.Rows[0]["TaskType"].ToString().Substring(1, 1));
                string SubTaskID = dt.Rows[0]["subtask_id"].ToString();
                string fromStation = dt.Rows[0]["FromStation"].ToString();
                string toStation = dt.Rows[0]["ToStation"].ToString();
                string stationNo = dt.Rows[0]["StationNo"].ToString();

                if (taskType != 3 && dt.Rows[0]["Flag"].ToString() != "3")
                {
                    string StationLoad = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ConveyServer, dCrnStatus[craneNo].OutStationNo + "_RTaskNO")));
                    //判断出库站台无货
                    if (StationLoad.Length > 0)
                    {
                        Logger.Info("站台狀態不符合堆垛機出庫！");
                        return;
                    }
                }

                int[] cellAddr = new int[6];
                cellAddr[0] = int.Parse(fromStation.Substring(4, 3));
                cellAddr[1] = int.Parse(fromStation.Substring(7, 3));
                cellAddr[2] = int.Parse(fromStation.Substring(1, 3));
                cellAddr[3] = int.Parse(toStation.Substring(4, 3));
                cellAddr[4] = int.Parse(toStation.Substring(7, 3));
                cellAddr[5] = int.Parse(toStation.Substring(1, 3));

                sbyte[] staskNo = new sbyte[10];
                Util.ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);

                WriteToService(serviceName, "TaskAddress", cellAddr);
                WriteToService(serviceName, "TaskNo", staskNo);
                if (WriteToService(serviceName, "WriteFinished", 1))
                {
                    //更新任务状态为执行中
                    List<string> comds = new List<string>();
                    List<DataParameter[]> Paras = new List<DataParameter[]>();
                    comds.Add("WCS.UpdateTaskState");
                    Paras.Add(new DataParameter[] { new DataParameter("{0}", "State=4,Crane_StartDate=getdate()"),
                                                            new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))}
                             );

                    comds.Add("WCS.UpdateTaskTmpStatus");
                    Paras.Add(new DataParameter[] { new DataParameter("@status", "STR"),
                                                    new DataParameter("@subtaskid", SubTaskID)});
                    bll.ExecTran(comds.ToArray(), Paras);
                }
                Logger.Info("任務:" + dr["TaskNo"].ToString() +" 托盤:" + dr["Palletcode"].ToString() + "已下發給" + craneNo + "堆垛機;起始地址:" + fromStation + ",目標地址:" + toStation);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="craneNo"></param>
        private void CraneIn(int craneNo)
        {
            // 判断堆垛机的状态 自动  空闲
            try
            {
               
                if (!Check_Crane_Status_IsOk(craneNo))
                    return;
                dCrnStatus[craneNo].io_flag = 0;
            }
            catch (Exception e)
            {
                Logger.Debug("Crane out 状态检查错误:" + e.Message.ToString());
                return;
            }

            string serviceName = dCrnStatus[craneNo].ServiceName;
            //获取任务，排序优先等级、任务时间
            DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("(WCS_TASK.TaskType='11' or WCS_TASK.TaskType='13') and WCS_TASK.State='2' and WCS_TASK.AreaCode='UL' and DeviceNo='{0}'", dCrnStatus[craneNo].DeviceNo)), new DataParameter("{1}", "1") };
            DataTable dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);

            //出库
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                string TaskNo = dr["TaskNo"].ToString();
                byte taskType = byte.Parse(dt.Rows[0]["TaskType"].ToString().Substring(1, 1));
                string fromStation = dt.Rows[0]["FromStation"].ToString();
                string toStation = dt.Rows[0]["ToStation"].ToString();
                string SubTaskID = dt.Rows[0]["subtask_id"].ToString();

                int[] cellAddr = new int[6];
                cellAddr[0] = int.Parse(fromStation.Substring(4, 3));
                cellAddr[1] = int.Parse(fromStation.Substring(7, 3));
                cellAddr[2] = int.Parse(fromStation.Substring(1, 3));
                cellAddr[3] = int.Parse(toStation.Substring(4, 3));
                cellAddr[4] = int.Parse(toStation.Substring(7, 3));
                cellAddr[5] = int.Parse(toStation.Substring(1, 3));

                sbyte[] staskNo = new sbyte[10];
                Util.ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);

                WriteToService(serviceName, "TaskAddress", cellAddr);
                WriteToService(serviceName, "TaskNo", staskNo);
                if (WriteToService(serviceName, "WriteFinished", 1))
                { 
                    List<string> comds = new List<string>();
                    List<DataParameter[]> Paras = new List<DataParameter[]>();
                    comds.Add("WCS.UpdateTaskState");
                    Paras.Add(new DataParameter[] { new DataParameter("{0}", "State=3,Crane_StartDate=getdate()"),
                                                            new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))}
                             );

                    comds.Add("WCS.UpdateTaskTmpStatus");
                    Paras.Add(new DataParameter[] { new DataParameter("@status", "STR"),
                                                            new DataParameter("@subtaskid", SubTaskID)}
                             );
                    bll.ExecTran(comds.ToArray(), Paras);
                }
                Logger.Info("任務:" + dr["TaskNo"].ToString() + " 托盤:" + dr["Palletcode"].ToString() + "已經下發給" + craneNo + "堆垛機;起始地址:" + fromStation + ",目標地址:" + toStation);
            }
        }
    }
}