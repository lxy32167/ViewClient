using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
namespace Client.App_Code
{
    public class DataModule
    {
        public struct stImageLimit
        {
            public int MaxSize;
            public int MinSize;
            public int AvgSize;
            public Single SpeedLimit;
        }
        public struct stThdStatus
        {
            public Boolean Rcv;
            public Boolean Save;
            public Boolean Qty;
            public Boolean Beacon;
            public Boolean UDP;
            public Boolean group;
            public Boolean selectgroup;
            public Boolean selectchart;
            public Boolean selectpaper;
            public Boolean SelectAllteacher;
        }
        public struct stVars
        {
            public Boolean UdpCreated;
            public Boolean UdpBinded;
            public int UdpPort;
            public Boolean UdpAvailable;
            public Boolean BlockOK;
            public Boolean RawQtyOK;
            public Boolean AllowHalfScore;
            public Boolean AllowMinus;
            public Boolean AllowDisplay;
            public int QuestionType;
    //        public Single FullScore;
    //        public stImageLimit CurImageLimit;
            public int CurPaperNum;
            public string CurVolName;
            public int CurPaperID;
    //        public int CurCheckID;
            public Single CurFullScore;
            public Single[] ArrFullScore;// = new Single[UConstDefine.BlockCount];
    //        public int SampleNum;
            //public List<UMyRecords.SamplePaper> Paper_Sample;
     //       public UMyRecords.stMyPaper curPaper;
      //      public int GroupNum;
     //       public int CurImageScale;
            public int PositiveEndRow;
        }
        public struct stRecords
        {
            public UMyRecords.stUserInfo UserInfo;
            public stThdStatus ThdStatus;
            public List<UMyRecords.stFullBlockInfo> BlockInfoList;
            public UMyRecords.stFullBlockInfo CurBlockInfo;
            public List<string> AssignList;
            public List<UMyRecords.stExcpMap> ExcpRsnList;
           // public List<string> Levellist;
            //public List<UMyRecords.stMyUser> GrpUserList;
            //public List<> ClassQtyList;
            //public List<> QtyInfoList;
            //public List<> QtyInfoListTotal;
            public List<UMyRecords.StExamRefScore> SampleInfoList;
            //public List<> BlkQtyList;
            //public List<UMyRecords.StExamRefScore> SampleGroupList;
            //public List<UMyRecords.stClassRoom> ClassRoomList;
            //public List<UMyRecords.stBlkRate> BlkRateList;
            public int[] ArrRsnCode;//= new int[31];
            public Socket ArrSocket;
       //     public Single[,] ArrScoreLevel; //= new Single[UConstDefine.LvlCol, UConstDefine.LvlRow];
        }
        public struct stIM
        {
            public int curMsgID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4097)]
            public char[] curMsgbuf;
            public Boolean IsSuccess;
        }
        [Serializable]
        public struct stCheck
        {
            public int PaperNo;
            public string[] DetailScore;
        }
    }
    public class TDM_Client
    {
        public Socket TCPSocket;
        //public Socket ClientSocket1;
        //public Socket ClientSocket2;
        //public Socket ClientSocket3;
        //public Socket ClientSocket4;
        //public Socket ClientSocket5;
        //public Socket ClientSocket6;
        //public Socket ClientSocket7;
        //public Socket UDPClient;

        //public Boolean ToClose;
        public Socket UDPSocket;
        //public Int32 LocalIP;
        //public int LTcpPort;
        public DataModule.stVars MyVars;
        public List<UMyRecords.stServerInfo> ServerInfoList;
        public DataModule.stRecords MyRecords;
        //public DataModule.stIM MyIM;
        public List<UMyRecords.stMyPaper> NewPaperList;
        public List<UMyRecords.stMyPaper> OldPaperList;
        public List<UMyRecords.stMyPaper> SaveScoreList;
        public List<UMyRecords.stMyPaper> JunkList;
        public List<UMyRecords.stStatusInfo> StatusList;
        public Object CsSocket, CsNewPaper, CsLogFile, CsOldPaper, CsSavePaper, CsJunk, CsCheck, CsExcpRsn;
        public Object CsGrpUser, CsQtyInfo, CsStatusInfo;
        public Object CsNextPaper, CsPreviewPaper, CsExpPaper;
        public Boolean LastPaper; //原本是F_Check的变量,但ASP.NET无法传值给页面类
        public double sysTimeDis;
        public Boolean NextPaper; //预取下一张标志
        public int NextPaperNo; 
        public DataTable tblScore; //分数
        public DataTable Sampledt;
        public DataTable SamPaperNo;
        public UMyRecords.stPaperNo[] SGPaperNoByGrp;
      //  public int WindowTime;
        public int excp;
        public string reason;
        public DataModule.stCheck[] stCheck;
        public TDM_Client()
        {
            //ToClose = false;
            LastPaper = false;
            NextPaper = false;
            NextPaperNo = -1;
            MyVars.UdpCreated = false;
            MyVars.UdpBinded = false;
            MyVars.UdpAvailable = false;
            MyVars.BlockOK = false;
            MyRecords.ArrRsnCode = new int[31];
            MyVars.ArrFullScore = new Single[UConstDefine.BlockCount];
     //       MyRecords.ArrScoreLevel = new Single[UConstDefine.LvlCol, UConstDefine.LvlRow];
            for (int i = 0; i < UConstDefine.BlockCount; i++)
            {
                MyVars.ArrFullScore[i] = -1;
            }
            ServerInfoList = new List<UMyRecords.stServerInfo>();

            TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MyRecords.ArrSocket = TCPSocket;
            MyRecords.ThdStatus.Rcv = false;
            MyRecords.ThdStatus.Save = false;
            MyRecords.ThdStatus.Qty = false;
            MyRecords.ThdStatus.Beacon = false;
            MyRecords.ThdStatus.UDP = false;
            MyRecords.ThdStatus.group = false;
            MyRecords.ThdStatus.selectgroup = false;
            MyRecords.ThdStatus.selectchart = false;
            MyRecords.ThdStatus.selectpaper = false;
            MyRecords.ThdStatus.SelectAllteacher = false;

            MyRecords.AssignList = new List<string>();
            MyRecords.BlockInfoList = new List<UMyRecords.stFullBlockInfo>();
            MyRecords.ExcpRsnList = new List<UMyRecords.stExcpMap>();

            CsSocket = new Object();
            CsNewPaper = new Object();
            CsNextPaper = new Object();
            CsOldPaper = new Object();
            CsLogFile = new Object();
            CsSavePaper = new Object();
            CsPreviewPaper = new Object();
            CsJunk = new Object();
            CsCheck = new Object();
            CsGrpUser = new Object();
            CsExcpRsn = new Object();
            CsExpPaper = new Object();
            CsQtyInfo = new Object();
            CsStatusInfo = new Object();

            NewPaperList = new List<UMyRecords.stMyPaper>();
            OldPaperList = new List<UMyRecords.stMyPaper>();
            SaveScoreList = new List<UMyRecords.stMyPaper>();
            JunkList = new List<UMyRecords.stMyPaper>();
            StatusList = new List<UMyRecords.stStatusInfo>();

            UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            MyVars.UdpCreated = true;

   //         WindowTime = 5 * 60;
            reason = "";
            excp = -1;

        }
    }
}