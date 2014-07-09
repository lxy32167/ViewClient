using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Client.App_Code
{
    public class UMyRecords
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct stTipsInfo
        {
            public MemoryStream TipsImage;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stPingFenXiZe
        {
            public MemoryStream PingFenXiZeImage;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stDaAn
        {
            public MemoryStream DaAnImage;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBookList
        {
            public string BkList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stStatInfoYangPing
        {
            public Int64 UserID;
            public int PaperNo;
            public int QtyType;
            public Single ExpertScore;
            public uint TimeStamp1;
            public Single ReValuateScore;
            public uint TimeStamp2;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stSaveYcScore
        {
            public uint TimeStamp;
            public Int64 UserID;
            public int BookNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] StuID;
            public int BlkCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 497)]
            public char[] DetailScore;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBlockRateInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] volumeName;
            public int allPaperNum;
            public int oneScoreNum;
            public int twoScoreNum;
            public int threeScoreNum;
            public int threeScoreFinishNum;
            public int fourScoreNum;
            public int fourScoreFinishNum;
            public int fiveScoreNum;
            public int fiveScoreFinishNum;
            public int sixScoreNum;
            public int sixScoreFinishNum;
            public int sevenScoreNum;
            public int sevenScoreFinishNum;
            public int eightScoreNum;
            public int eightScoreFinishNum;
            public int quesPaperNum;
            public int quesPaperFinishNum;
            public int delayPaperNum;
            public int delayPaperFinishNum;
            public int finalnum;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stUcMsgData
        {
            public int MsgID;
            public int MsgType;
            public Int64 UserID;
            public int MsgFlag;
            public int TagTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public char[] MsgData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBcMsgData
        {
            public int MsgID;
            public int NotifyType;
            public int TargetScope;
            public int TargetID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public char[] NotifyText;
            public int MsgFlag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BlkGridYc
        {
            public string BlkSeqNo;
            public int QuestionNo;
            public int QNoRow;
            public int BlkRow;
            public Single BlkTScr;
            public Single QTScr;
            public Single BlkNowScr;
            public Single QTNowScr;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BlkInfoYc
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] BlkSeqNo;
            public int QuestionNo;
            public int QuestionType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] CheckRule;
        }
        //[StructLayout(LayoutKind.Sequential)]
        //public struct stBlkQty
        //{
        //    public int QuestionNo;
        //    public int BlkNo;

        //}
        [StructLayout(LayoutKind.Sequential)]
        public struct stClassRoom
        {
            public int ClassNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] ClassName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stRate
        {
            public int rate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] rateDes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBlkRate
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
            public List<stRate> RateList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBlockInfoLogin
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
            public int QuestionNo;
            public int BlkSeqNo;
            public int nNEv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] CheckRule;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] RevaluateRule;
            public int CheckScore;
            public int DispScore;
            public int BufSize;
            public int TimeOut;
            public int QuestionType;
            public int TotalTestMin;
            public int SampleTestMin;
            public int MaxSize;
            public int MinSize;
            public int AvgSize;
            public Single MinTime;
            public int SampleAmount;
            public Int32 ServerID;
            public int TipsLen;
            public int JunkInfo;
            public int nMarkModelLen;
            public int MarkModelInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stBlockInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
            public int QuestionNo;
            public int BlkSeqNo;
            public int nNEv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] CheckRule;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] RevaluateRule;
            public int CheckScore;
            public int DispScore;
            public int BufSize;
            public int TimeOut;
            public int QuestionType;
            public int TotalTestMin;
            public int SampleTestMin;
            public int MaxSize;
            public int MinSize;
            public int AvgSize;
            public Single MinTime;
            public int SampleAmount;
            public Int32 ServerID;
            public int TipsLen;
            public int JunkInfo;
            public int nMarkModelLen;
            public int MarkModelInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] MarkModelList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stFullBlockInfo
        {
            public stBlockInfo BlockInfo;
            public List<stTipsInfo> TipsList;
            public List<stPingFenXiZe> PingFenXiZeList;
            public List<stDaAn> DaAnList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stMyScoreData
        {
            public int BlkNo;
            public Int64 UserIDOrPaperSeq;
            public int ExcpReason;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] Txt;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stPaperInfo
        {
            public int PaperNo;
            public int ImageFormat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int PaperType;
            public int ImageLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SHAREDPICPATHLEN)]
            public char[] PicPath;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stRefScore
        {
            public Int64 UserID1;
            public Single TotalScore1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore1;
            public Int64 UserID2;
            public Single TotalScore2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore2;
            public Int64 UserID3;
            public Single TotalScore3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore3;
            public Int64 UserID4;
            public Single TotalScore4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore4;
            public Int64 UserID5;
            public Single TotalScore5;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore5;
            public Int64 UserID_Ti;
            public Single TotalScore_Ti;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore_Ti;
            public Int64 UserID_Da;
            public Single TotalScore_Da;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore_Da;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stMyPaper
        {
            public stPaperInfo PaperInfo;
            public MemoryStream PaperImage;
            public int SaveType;
            public stMyScoreData ScoreInfo;
            public stRefScore RefScore;
            public Single TotalScore;
            public string DetailScore;
            public double TimeStamp;
            public uint RStartTime;
            public uint REndTime;
            public double StartTime;
            public Boolean Status;
            public Boolean Flag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SamplePaper
        {
            public int No;
            public int Rate;
            public string Reason;
            public string score;
            public int Blk;
            public string Group;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stMyUser
        {
            public Int64 UserID;
            public int UserRole;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserTrueName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int TopGrpNo;
            public int MidGrpNo;
            public int BtmGrpNo;
            public Single RejudgeSmpRatio;
            public Single SelfRejudgeRatio;
            public int TaskAmount;
            public int SmpAmount;
            public Boolean Flag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stClassQty
        {
            public int TopGrpNo;
            public int MidGrpNo;
            public int BtmGrpNo;
            public string VolName;
            public List<string> UserIDList;
            public MemoryStream QtyStream;
            public Boolean Active;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stQueryResult
        {
            public Int64 UserID;
            public string LogName;
            public string UserName;
            public int TotalPaperNum;
            public int ValidPaperNum;
            public Single TimeCost;
            public Single MaxScore;
            public Single MinScore;
            public Single AvgScore;
            public Boolean Flag;
            public Single nor;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stStatusInfo
        {
            public Int64 UserID;
            public int UserRole;
            public uint ReqTime;
            public uint LeaderID;
            public int OldStatus;
            public int NewStatus;
            public int ValidStatus;
            public uint Accept;
            public Boolean Flag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stResult
        {
            public uint starttime;
            public uint endtime;
            public int TNo;
            public int MNo;
            public int BNo;
            public Int64 UserID;
            public int Count;
            public Single avg;
            public Single nor;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stGrpTotalInfo
        {
            public int TotalNum;
            public int CheckTimes;
            public int ValidTimes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stEXAMSAVESCORE
        {
            public Int64 UserID;
            public int PaperNo;
            public int QuestionNo;
            public int BlkNo;
            public int ScoreType;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.ReasonLen)]
            public char[] Reason;
            public uint TimeStamp;
            public int TimeCost;
            public int Rate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stPapeNoAndGrpName
        {
            public int PaperNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stSamGroupInfo
        {
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.TRAINTARGETLEN)]
            public char[] TrainTarget;
            public int Active;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stUserInfo
        {
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] LoginName;
            public int Role;
            public int Status;
            public int Interceder;
            public int TopLevelTeam;
            public int MidLevelTeam;
            public int BtmLevelTeam;
            public Int64 XiaoZuZhangID;
            public Int64 TiZuZhangID;
            public Int64 DaZuZhangID;
            public int SmpPaperTackled;
            public int TstPaperTackled;
            public int PaperTackled;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] TrueName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.ServeForLen)]
            public char[] ServeFor;
            public int AssignLen;
   //         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
   //         public char[] Assignment;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stUserInfoAssignment
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
            public char[] Assignment;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stGetPaperTask
        {
            public Int64 UserID;
            public int PaperType;
            public int DispOrBlkNo;
            public int PaperNoOrReason;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stScoreForExcp
        {
            public int PaperNo;
            public int BlkNo;
            public Int64 UserID;
            public int ExcpReason;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] ExcpTxt;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stScoreForScore
        {
            public int PaperNo;
            public int BlkNo;
            public Int64 PaperSeq;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stExcpMap
        {
            public int ReasonCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] Reason;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stSaveScore
        {
            public Int64 UserID;
            public int PaperNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int ScoreType;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore;
            public uint TimeStamp;
            public int TimeCost;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stSaveExcp
        {
            public Int64 UserID;
            public int PaperNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int ScoreType;
            public int ReasonType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UConstDefine.DetailLen)]
            public string DetailScore;
            public uint TimeStamp;
            public int TimeCost;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stServerInfo
        {
            public Int32 IPAddr;
            public int TCPPort;
            public int UDPUniPort;
            public int UDPSBrdPort;
            public int UDPCBrdPort;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stExampleRec
        {
            public int PaperNo;
            public int PaperSeq;
            public int BlockNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stSimilarRec
        {
            public Int64 UserID;
            public int ClassRoomNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int Checked;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stGrpUserInfo
        {
            public Int64 UserID;
            public int UserRole;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserTrueName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int TopGrpNo;
            public int MidGrpNo;
            public int BtmGrpNo;
            public Single RejudgeSmpRatio;
            public Single SelfRejudgeRatio;
            public int TaskAmount;
            public int SmpAmount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stQtyInfo
        {
            public int PaperNo;
            public uint Time;
            public Int64 UserID;
            public Single TotalScore;
            public int CheckID;
            public Int16 TimeCost;
            public char VaildFlag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stStatInfo
        {
            public Int64 UserID;
            public int PaperNo;
            public Single ExpertScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] DetailScore1;
            public uint TimeStamp1;
            public Single ReValuateScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] DetailScore2;
            public uint TimeStamp2;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct StExamRefScore
        {
            public Int64 UserID;
            public int PaperNo;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.DetailLen)]
            public char[] DetailScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.ReasonLen)]
            public char[] Reason;
            public int Rate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.TRAINTARGETLEN)]
            public char[] TrainTarget;
            public int RecCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stPaperNo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
            public string PaperNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stPerQtyInfo
        {
            public int GPaperCount;
            public int PChecked;
            public int PValid;
            public int GFinished;
            public int GChecked;
            public int PSumTime;
            public int GSumTime;
            public Single PSumScore;
            public Single GSumScore;
        }
    }

}