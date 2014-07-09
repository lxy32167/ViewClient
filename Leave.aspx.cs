using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using Client.App_Code;

namespace Client
{
    public partial class Leave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UMsgDefine.FM_UserLogout fmLogout;
            TBeaconThreadClass TBeaconClass;
            TUDPListenedClass TUDPClass;
            TRcvPaperThreadClass TRcvPaperClass;
            TSaveScoreThreadClass TSaveScoreClass;
            //大部分交给GC处理，这里要做的内容，清理目录，停止线程，关闭socket
            TDM_Client DM_Client = (TDM_Client)Session["DM_Client"];
            TBeaconClass = (TBeaconThreadClass)Session["TBeaconClass"];
            TUDPClass = (TUDPListenedClass)Session["TUDPClass"];
            TSaveScoreClass = (TSaveScoreThreadClass)Session["TSaveScoreClass"];
            TRcvPaperClass = (TRcvPaperThreadClass)Session["TRcvPaperClass"];

            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (tmpstr1.Length != 0)
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1, true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "DaAn"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "DaAn", true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "XiZe"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "XiZe", true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "BiaoZhun"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "BiaoZhun", true);
            }
            if (DM_Client.OldPaperList.Count > 0)
            {
                UMyRecords.stMyPaper PTmpPaper;
                int i;

                if (DM_Client.OldPaperList.Count > 0)
                {
                    lock (DM_Client.CsOldPaper)
                    {
                        lock (DM_Client.CsSavePaper)
                        {
                            for (i = DM_Client.OldPaperList.Count; i > 0; i--)
                            {
                                PTmpPaper = DM_Client.OldPaperList[0]; //已改分第一张
                                if (PTmpPaper.Status)
                                {
                                    DM_Client.OldPaperList.RemoveAt(0);
                                    DM_Client.SaveScoreList.Add(PTmpPaper);
                                }
                            }
                        }
                    }
                }
                Session["DM_Client"] = DM_Client; //将现有的分数存回服务器
            }
            if (DM_Client.MyRecords.ThdStatus.UDP)
            {
                TUDPClass.TUDPListenedThread.Abort();
            }
            fmLogout.MsgHead.MsgType = UConstDefine.TM_USER_LOGOUT;
            fmLogout.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_UserLogout));
            fmLogout.UserID = DM_Client.MyRecords.UserInfo.UserID;
            byte[] buf = UMsgDefine.StructToBytes(fmLogout);
            DM_Client.TCPSocket.Send(buf, buf.Length, SocketFlags.None);

            if (DM_Client.MyRecords.ThdStatus.Beacon)
            {
                TBeaconClass.TBeaconThread.Abort();
            }
            if (DM_Client.MyRecords.ThdStatus.Rcv)
            {
                TRcvPaperClass.TRcvPaperThread.Abort();
            }
            if (DM_Client.MyRecords.ThdStatus.Save)
            {
                TSaveScoreClass.TSaveScoreThread.Abort();
            }

            UMyFuncs.ClearUpInfo(DM_Client, 1);
            UMyFuncs.ClearUpInfo(DM_Client, 2);

            DM_Client.NewPaperList.Clear();
            DM_Client.SaveScoreList.Clear();
            DM_Client.OldPaperList.Clear();
            DM_Client.JunkList.Clear();
            DM_Client.StatusList.Clear();

            DM_Client.UDPSocket.Close();
            DM_Client.TCPSocket.Shutdown(SocketShutdown.Both);
            Thread.Sleep(10);
            DM_Client.TCPSocket.Close();
            DM_Client.MyRecords.ArrSocket.Close();
        }
    }
}