using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IServices;
using System.Data;
using Util;

namespace BLL
{
    public class Server
    {

        /// <summary>
        /// 通道字典
        /// </summary>
        private static Dictionary<string, object> Channels = new Dictionary<string, object>();

        /// <summary>
        /// 创建一个指定类型的通道
        /// </summary>
        /// <typeparam name="TChannel">WCF接口类型</typeparam>
        /// <returns></returns>
        public static TChannel GetChannel<TChannel>()
        {
            try
            {
                string endPointConfigName = typeof(TChannel).Name;
                if (Channels.ContainsKey(endPointConfigName))
                {
                    return (TChannel)Channels[endPointConfigName];
                }

                ChannelFactory<TChannel> channelFactory = new ChannelFactory<TChannel>(endPointConfigName);
                TChannel channel = channelFactory.CreateChannel();
                Channels.Add(endPointConfigName, channel);
                return channel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取库区编码
        /// </summary>
        /// <returns></returns>
        public static string GetAreaCode()
        {
            MCP.Config.Configuration confg = new MCP.Config.Configuration();
            confg.Load("Config.xml");
           string AreaCode = confg.Attributes["AreaCode"];
           confg.Release();
           return AreaCode;
        }
        public static string GetTaskTest()
        {
            MCP.Config.Configuration confg = new MCP.Config.Configuration();
            confg.Load("Config.xml");
            string AreaCode = confg.Attributes["TaskTest"];
            confg.Release();
            return AreaCode;
        }

        /// <summary>
        /// 中間數據庫插入WCS數據庫，并轉入WCS_Task
        /// </summary>
        /// <param name="dt"></param>
        internal static void InsertTaskToWcs(DataTable dt,bool blnUpdateAck)
        {
            try
            {
                //插入中間表
                BLL.BLLBase bllStock = new BLLBase("StockDB");
                bllStock.BatchInsertTable(dt, "AsrsTask_TMP");

                DataTable dtTaskID = dt.DefaultView.ToTable(true, "task_id");

                List<string> MiddleComds = new List<string>();
                List<DataParameter[]> Middleparas = new List<DataParameter[]>();

                List<string> StockComds = new List<string>();
                List<DataParameter[]> Stockparas = new List<DataParameter[]>();
                for (int i = 0; i < dtTaskID.Rows.Count; i++)
                {
                    DataRow[] drsTask = dt.Select(string.Format("task_id='{0}'", dtTaskID.Rows[i]["task_id"].ToString()));
                    //更新中間表狀態
                    string taskID = drsTask[0]["task_id"].ToString();
                    string SubtaskID = drsTask[0]["subtask_id"].ToString();

                    MiddleComds.Add("Middle.UpdateAsrsTaskAck");
                    MiddleComds.Add("Middle.UpdateAsrsSubTaskAck");
                    string strWhere = string.Format("subtask_id='{0}'", SubtaskID);
                    if (drsTask.Length > 1)
                        strWhere = "1=1";
                    Middleparas.Add(new DataParameter[] { new DataParameter("@TaskID", taskID) });
                    Middleparas.Add(new DataParameter[] { new DataParameter("@TaskID", taskID), new DataParameter("{0}", strWhere) });

                    StockComds.Add("WCS.InsertWCSTask");
                    Stockparas.Add(new DataParameter[] { new DataParameter("@TaskID", taskID) });
                }
                BLL.BLLBase bllMiddle = new BLLBase("MiddleDB");
                if (blnUpdateAck)
                    bllMiddle.ExecTran(MiddleComds.ToArray(), Middleparas);
                bllStock.ExecTran(StockComds.ToArray(), Stockparas);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
