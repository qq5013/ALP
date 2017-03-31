using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;

namespace App.Dispatching.Process
{
    public class MiniLoadProcess : AbstractProcess
    {
        private class rCrnStatus
        {
            
            public int Action { get; set; }
            public int io_flag { get; set; }
            public string ServiceName { get; set; }
            public string DeviceNo { get; set; }
            public string AisleNo { get; set; }
            public string OutStationNo1 { get; set; }
            public string OutStationNo2 { get; set; }

            public rCrnStatus()
            {
                Action = 0;
                io_flag = 0;
                ServiceName = "";
                DeviceNo = "";
                AisleNo = "";
                OutStationNo1 = "";
                OutStationNo2 = "";
            }
        }

        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        private Dictionary<int, rCrnStatus> dCrnStatus = new Dictionary<int, rCrnStatus>();
        private Timer tmWorkTimer = new Timer();
        private bool blRun = false;
        private string AreaCode;
        private string ConveyServer = "MConvey";

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
                        dCrnStatus[i].OutStationNo1 = dt.Rows[i - 1]["OutStationNo"].ToString();
                        dCrnStatus[i].OutStationNo2 = dt.Rows[i - 1]["OutStationNo1"].ToString();
                    }
                }

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);
                

                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("MiniLoadProcess堆垛機初始化出錯，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            //object obj = ObjectUtil.GetObject(stateItem.State);            
            //if (obj == null)
            //    return;

            switch (stateItem.ItemName)
            {
                case "CraneTaskFinished1":
                case "CraneTaskFinished2":
                    try
                    {
                        object obj = ObjectUtil.GetObject(stateItem.State);
                        string TaskFinish = obj.ToString();
                        if (TaskFinish.Equals("True") || TaskFinish.Equals("1"))
                        {
                            int taskIndex = int.Parse(stateItem.ItemName.Substring(stateItem.ItemName.Length - 1, 1));
                            string TaskNo = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(stateItem.Name, "CraneTaskNo" + taskIndex)).ToString();

                            if (TaskNo.Length == 0)
                                return;
                            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskNo='{0}'", TaskNo)), new DataParameter("{1}", "1") };
                            DataTable dtTask=bll.FillDataTable("WCS.SelectCraneTask", param);
                           
                            string TaskType = dtTask.Rows[0]["TaskType"].ToString() ;
                            int Flag =int.Parse( dtTask.Rows[0]["Flag"].ToString());

                            Logger.Info(stateItem.ItemName + "任務完成,任務號:" + TaskNo);

                            DataTable dtConveyTask = bll.FillDataTable("WCS.SelectConveyTask", param);
                            string Destination = dtConveyTask.Rows[0]["ToStation"].ToString();
                            string PalletCode = dtConveyTask.Rows[0]["Palletcode"].ToString();
                            string ConveyID = dtTask.Rows[0]["OutStationNo"].ToString(); //出庫站台

                            if (TaskType == "11")
                            {
                                bll.ExecNonQuery("WCS.Sp_TaskProcess", param);
                            }
                            else if (TaskType == "12")
                            {
                                WriteToService(stateItem.Name, ConveyID + "WTaskNo", TaskNo);
                                WriteToService(stateItem.Name, ConveyID + "WPalletCode", PalletCode);
                                WriteToService(stateItem.Name, ConveyID + "Destination", Destination); //目的地
                                if (WriteToService(stateItem.Name, ConveyID + "WriteFinished", 1))
                                {
                                    Logger.Info("任務號：" + TaskNo + "已經下達輸送線：" + ConveyID + " 目的地址：" + Destination);
                                    bll.ExecNonQuery("WCS.UpdateTaskState", new DataParameter[] { new DataParameter("{0}", "State=6,Crane_FinishDate=getdate(), Convey_StartDate=getdate()"), new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo)) });

                                }
                                bll.ExecNonQuery("WCS.UpdateCellEmpty", new DataParameter[] { new DataParameter("@CellCode", dtTask.Rows[0]["CellCode"].ToString()) });


                            }
                            else if (TaskType == "13")
                            {
                                if (Flag == 3)
                                {
                                    //同巷道內出庫
                                    bll.ExecNonQuery("WCS.Sp_TaskProcess", param);
                                }
                                else if (Flag == 6)
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
                            }
                            WriteToService(stateItem.Name, "TaskNo" + taskIndex, "");
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
                        dCrnStatus[i].Action = int.Parse(dt.Rows[i - 1]["State"].ToString());
                
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

                string plcTaskNo1 =Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CraneTaskNo1")));
                string craneMode1 = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneMode1")).ToString();
                string CraneState = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneState")).ToString();
                string CraneAlarmCode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneAlarmCode")).ToString();

                string plcTaskNo2 = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CraneTaskNo2")));
                string craneMode2 = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "CraneMode2")).ToString();


                if (plcTaskNo1 == "" && craneMode1 == "1" && CraneAlarmCode == "0" && CraneState == "1" && plcTaskNo2 == "" && craneMode2 == "1")
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
                    return;
                //切换入库优先
                dCrnStatus[craneNo].io_flag = 1;
            }
            catch (Exception e)
            {
                Logger.Debug("Crane out 状态检查错误:" + e.Message.ToString());
                return;
            }

            string serviceName = dCrnStatus[craneNo].ServiceName;
            DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskType in ('12','13') and WCS_Task.State='0' and WCS_TASK.AisleNo='{0}' and WCS_TASK.AreaCode='{1}'", dCrnStatus[craneNo].AisleNo, AreaCode)), new DataParameter("{1}", "2") };
            DataTable dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);


            bool blnHasTask = false;
            if (dt.Rows.Count == 0)
            {

                BLL.BLLBase bllMiddle = new BLL.BLLBase("MiddleDB");
                DataTable dtMiddle = bllMiddle.FillDataTable("Middle.SelectCraneOutTask", new DataParameter[] { new DataParameter("@Device", AreaCode), new DataParameter("{0}", string.Format("from_aisle='{0}'", dCrnStatus[craneNo].AisleNo)), new DataParameter("{1}", 2) });
                if (dtMiddle.Rows.Count > 0)
                {
                    BLL.Server.InsertTaskToWcs(dtMiddle, true);
                    blnHasTask = true;
                }

            }
            if (blnHasTask)
                dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);
            //判断出库站台无货
            if (dt.Rows.Count > 0)
            {
                
                string StationLoad1 = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ConveyServer, dCrnStatus[craneNo].OutStationNo1 + "_RTaskNO")));
                string StationLoad2 = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ConveyServer, dCrnStatus[craneNo].OutStationNo2 + "_RTaskNO")));
                if (StationLoad1.Length > 0 || StationLoad2.Length > 0)
                {
                    Logger.Info("站台狀態不符合堆垛機出庫！");
                    return;
                }
            }

            string filter = "TaskType in('12','13') and State='0'";
            DataRow[] drs = dt.Select(filter, "CellCode desc");
            Send2PLC(craneNo, drs);

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
                //判断堆垛机
                if (!Check_Crane_Status_IsOk(craneNo))
                    return;

                //切换入库优先
                dCrnStatus[craneNo].io_flag = 0;
            }
            catch (Exception e)
            {
                Logger.Error("MiniLoadProcess中Craneout狀態檢查錯誤:" + e.Message.ToString());
                return;
            }
            DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("(WCS_TASK.TaskType='11' or WCS_TASK.TaskType='13') and WCS_Task.State in('1','2') and WCS_TASK.AreaCode='{1}' and DeviceNo='{0}'", dCrnStatus[craneNo].DeviceNo, AreaCode)), new DataParameter("{1}", "2") };
            DataTable dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);



            bool TaskOK = false;
            if (dt.Rows.Count > 0)
            {
                string filter = "TaskType in('11','13') and State='2'";
                DataRow[] drs = dt.Select(filter, "TaskDate desc");
                if (drs.Length <= 0)
                    TaskOK = false;
                else if (drs.Length == 1)
                {
                    //如果有在途的入库任务，等待
                    filter = "TaskType in('11','13') and State='1'";
                    DataRow[] drsOnLine = dt.Select(filter, "TaskDate desc");
                    if (drsOnLine.Length == 0)
                        TaskOK = true;
                    else
                    {
                        TaskOK = false;
                        for (int i = 0; i < drsOnLine.Length; i++)
                        {
                            string Cellcode = drsOnLine[i]["CellCode"].ToString().Substring(4, 3);
                            if (Cellcode == "98")
                            {
                                TaskOK = true;
                                break;
                            }
                        }
                    }
                }
                else if (drs.Length == 2)
                    TaskOK = true;

                if (TaskOK)
                {
                    filter = "TaskType in('11','13') and State='2'";
                    drs = dt.Select(filter, "CellCode desc");
                    Send2PLC(craneNo, drs);
                }
            }
        }

        //入庫
       
        private void Send2PLC(int CraneNo, DataRow[] drs)
        {
            List<string> comds = new List<string>();
            List<DataParameter[]> Paras = new List<DataParameter[]>();
            string strWhere = "";
            //更新A任務，及B任務
            if (drs.Length == 1)
            {
                bll.ExecNonQuery("WCS.UpdateTaskAB", new DataParameter[] { new DataParameter("@TaskAB", "A"), 
                                                                           new DataParameter("@MergeTaskNo", ""), 
                                                                           new DataParameter("@TaskNo", drs[0]["TaskNo"].ToString()) });
                strWhere = string.Format("TaskNo='{0}'", drs[0]["TaskNo"].ToString());
            }
            else
            {
                comds.Add("WCS.UpdateTaskAB");
                Paras.Add(new DataParameter[] { new DataParameter("@TaskAB", "A"), 
                                                new DataParameter("@MergeTaskNo",drs[1]["TaskNo"].ToString()), 
                                                new DataParameter("@TaskNo", drs[0]["TaskNo"].ToString()) });

                comds.Add("WCS.UpdateTaskAB");
                Paras.Add(new DataParameter[] { new DataParameter("@TaskAB", "B"), 
                                                new DataParameter("@MergeTaskNo",drs[0]["TaskNo"].ToString()), 
                                                new DataParameter("@TaskNo", drs[1]["TaskNo"].ToString()) });


                strWhere = string.Format("TaskNo in ('{0}','{1}')", drs[0]["TaskNo"].ToString(), drs[1]["TaskNo"].ToString());
            }

            DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.AreaCode='{1}' and DeviceNo='{0}' and {2}", dCrnStatus[CraneNo].DeviceNo, AreaCode, strWhere)), new DataParameter("{1}", "2") };
            DataTable dt = bll.FillDataTable("WCS.SelectCraneTask", parameter);
            string serviceName = dCrnStatus[CraneNo].ServiceName;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                comds.Clear();
                Paras.Clear();
                int TaskIndex = i + 1;
                DataRow dr = drs[0];
                string TaskNo = dr["TaskNo"].ToString();
                byte taskType = byte.Parse(drs[0]["TaskType"].ToString().Substring(1, 1));
                string fromStation = drs[0]["FromStation"].ToString();
                string toStation = drs[0]["ToStation"].ToString();
                string SubTaskID = drs[0]["subtask_id"].ToString();
                string state = dr["State"].ToString();
               

                int[] cellAddr = new int[6];
                cellAddr[0] = int.Parse(fromStation.Substring(4, 3));
                cellAddr[1] = int.Parse(fromStation.Substring(7, 3));
                cellAddr[2] = int.Parse(fromStation.Substring(1, 3));
                cellAddr[3] = int.Parse(toStation.Substring(4, 3));
                cellAddr[4] = int.Parse(toStation.Substring(7, 3));
                cellAddr[5] = int.Parse(toStation.Substring(1, 3));

                sbyte[] staskNo = new sbyte[10];
                Util.ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);

                WriteToService(serviceName, "TaskAddress" + TaskIndex, cellAddr);
                WriteToService(serviceName, "TaskNo" + TaskIndex, staskNo);
                if (WriteToService(serviceName, "WriteFinished" + TaskIndex, 1))
                {
                    comds.Add("WCS.UpdateTaskState");
                    if (state == "0") //出庫
                    {
                       
                        Paras.Add(new DataParameter[] { new DataParameter("{0}", "State=4,Crane_StartDate=getdate()"),
                                                        new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))});

                    }
                    else //入庫
                    {
                        Paras.Add(new DataParameter[] { new DataParameter("{0}", "State=3,Crane_StartDate=getdate()"),
                                                        new DataParameter("{1}", string.Format("TaskNo='{0}'", TaskNo))});
                    }
                    comds.Add("WCS.UpdateTaskTmpStatus");
                    Paras.Add(new DataParameter[] { new DataParameter("@status", "STR"),
                                                    new DataParameter("@subtaskid", SubTaskID)});
                    bll.ExecTran(comds.ToArray(), Paras);
                }
                Logger.Info("任務:" + dr["TaskNo"].ToString() + " 托盤:" + dr["Palletcode"].ToString() + "已經下發給" + CraneNo + "堆垛機;起始地址:" + fromStation + ",目標地址:" + toStation);
            }

        }
    }
}