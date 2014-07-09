using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Client.App_Code
{
    public class UMsgDefine
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct stMsgHead
        {
            public int MsgType;
            public int MsgLength;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_QueryUser_Req
        {
            public stMsgHead MsgHead;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_QueryUser_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int RecCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Auth_Req
        {
            public stMsgHead MsgHead;
            public int AuthType;
            public int UDPPort;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.NameLen)]
            public char[] UserName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.PwdLen)]
            public char[] UserPwd;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Auth_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public uint ServerTime;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_UserLogout
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ChgPwd_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.PwdLen)]
            public char[] OldPwd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.PwdLen)]
            public char[] NewPwd;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UConstDefine.NameLen)] //注意有汉字，必须采用ansi字符串与客户端相同
            public string TrueName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = UConstDefine.ServeForLen)]
            public string ServeFor;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ChgPwd_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfo_Req
        {
            public stMsgHead MsgHead;
            public int InfoType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfo_Rsp_Login
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stBlockInfoLogin BlockInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfo_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stBlockInfo BlockInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETCLASSROOMMAP_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETCLASSROOMMAP_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int ClsMapLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETRATEMAP_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETRATEMAP_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public char[] RateMap;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_EXAMSAVESCORE_REQ
        {
            public stMsgHead MsgHead;
            public UMyRecords.stEXAMSAVESCORE Score;
            public int AdoptedID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_EXAMSAVESCORE_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SUBMITSTANDARD_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int PaperNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SUBMITSTANDARD_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMBLKINFO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMBLKINFO_RSP
        {
            public stMsgHead MsgHead;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.BLKNUM)]
            public UMyRecords.stBlockInfo[] BLKINFO;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_DELETESAM_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int PaperNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_DELETESAM_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_MODIFYSAM_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int PaperNo;
            public int Rate;
            public Single TotalScore;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] Reason;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_MODIFYSAM_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGROUPINFO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGROUPINFO_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int SamGroupNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNO)]
            public UMyRecords.stSamGroupInfo[] AllSamGroupInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SAMGROUP_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public UMyRecords.stSamGroupInfo TmpSamGroupInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] OldSamGroupName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SAMGROUP_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_DELETESAMGROUP_REQ
        {
            public stMsgHead MsgHead;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] SamGroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_DELETESAMGROUP_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ACTIVESAMGROUP_REQ
        {
            public stMsgHead MsgHead;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] SamGroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int IsActive;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ACTIVESAMGROUP_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGROUPPAPER_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] SamGroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGROUPPAPER_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;

            public int SamGroupPaperNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SamPaperNoLen)]
            public int[] SamGroupPaperNo;

            public int NoSamGroupPaperNum;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SAVESAMGROUPPAPER_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int AddSamGroupPaperNum;
            public int DelSamGroupPaperNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SamPaperLen)]
            public int[] SamGroupPaper;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SAVESAMGROUPPAPER_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMTOTAL_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMTOTAL_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SamNum)]
            public int[] SamPaperNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGRPNAME_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGRPNAME_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int Count;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.ALLSAMGROUPNAMELEN)]
            public char[] SamGrpName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGRPPAPERNO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMGRPPAPER_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int DataLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SamPaperLen)]
            public int[] PaperNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_CHGSTAT_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int ReqType;
            public int TopTeam;
            public int MidTeam;
            public int BtmTeam;
            public int TargetID;
            public int OldStatus;
            public int NewStatus;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_CHGSTAT_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetPaper_Req
        {
            public stMsgHead MsgHead;
            public UMyRecords.stGetPaperTask GetPaperTask;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetPaper_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stPaperInfo PaperData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScore_Req
        {
            public stMsgHead MsgHead;
            public int RecCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public UMyRecords.stSaveScore[] Score;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveExcp_Req
        {
            public stMsgHead MsgHead;
            public int RecCount;
            public UMyRecords.stSaveExcp Score;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScore_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetSMTask_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int Role;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetSMTask_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int RecCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetSimilarPaper_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int Role;
            public int ClassRoomNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetSimilarPaper_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stPaperInfo PaperInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveSMPaper_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int Role;
            public int ClassRoomNo;
            public int StringLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.BufferSize - 20)]
            public char[] StudentIDGrp;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveSMPaper_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetExcpPaper_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int Role;
            public int BlockNo;
            public int Reason;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetExcpPaper_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stExampleRec ExcpRec;
            public UMyRecords.stPaperInfo PaperInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetGrpInfo_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetGrpInfo_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int BufLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetQtyFile_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int FileType;
            public int Num;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetQtyFile_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int FileFormat;
            public int count;
            public int FileLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_QtyStat_Req
        {
            public stMsgHead MsgHead;
            public int QueryType;
            public int LeaderID;
            public Int64 UserID;
            public int Num;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_QtyStat_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int RecCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SETRATIO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int RegType;
            public int RationType;
            public int TargetID;
            public int TopTeam;
            public int MidTeam;
            public int BtmTeam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public char[] RationDate;
            public Single RejudgeRation;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] SamGrpName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SetRatio_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SelectPaper_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int Role;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
            public int PaperNo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SelectPaper_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stPaperInfo PaperInfo;
            public UMyRecords.stRefScore RefScore;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Beacon_Client
        {
            public stMsgHead MsgHead;
            public int nFlag;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Beacon_Server
        {
            public stMsgHead MsgHead;
            public uint ServerTime;
            public int Status;
            public int LogOut;       
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Beacon_Server_Assignment
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
            public char[] Assignment;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ProveSrv_Req
        {
            public stMsgHead MsgHead;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_ProveSrv_Rsp
        {
            public stMsgHead MsgHead;
            public Int32 IPAddr;
            public int UserCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SrvInfoLen)]
            public char[] ServerInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_Broadcase_Srv
        {
            public stMsgHead MsgHead;
            public UMyRecords.stBcMsgData BcMsgData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_UDPMsg_Srv
        {
            public stMsgHead MsgHead;
            public UMyRecords.stUcMsgData UcMsgData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_UDPMsg_ACK
        {
            public stMsgHead MsgHead;
            public int MsgID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_UDPMsg_IM
        {
            public stMsgHead MsgHead;
            public int msgID;
            public int RecvNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public char[] msgData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfoYc_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfoYcData_Req
        {
            public stMsgHead MsgHead;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetBlkInfoYc_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int BlkCount;
            public int nLen;
            public int JunkInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetYcBook_Req
        {
            public stMsgHead MsgHead;
            public int TopLevelTeam;
            public int BookNum;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GetYcBook_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public char[] StuIdList;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScoreYc_Req
        {
            public stMsgHead MsgHead;
            public UMyRecords.stSaveYcScore Score;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScoreYc_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScoreYcDZ_Req
        {
            public stMsgHead MsgHead;
            public Boolean Ret;
            public UMyRecords.stSaveYcScore Score;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_SaveScoreYcDZ_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSTANDARD_REQ
        {
            public stMsgHead MsgHead;
            public int RequestType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSTANDARD_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int StandardLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXCPStuINFO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXCPStuINFO_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int StuCount;
            public int StuIDLen;
            public int nLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXPAPER_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int VolumeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] StuID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXPAPER_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stPaperInfo PaperData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct getBlockInfo_Req
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolumeName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct getBlockInfo_Rsp
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stBlockRateInfo paperRateInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXAMQTY_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int QuestionNo;
            public int BlkNo;
            public int Num;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETEXAMQTY_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int FileFormat;
            public int FileLen;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMPAPERNO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SAMGROUPNAMELEN)]
            public char[] GroupName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMPAPERNO_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int FinalLength;
            public int TempLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.SamPaperLen)]
            public int[] PaperNo;

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSGPAPERNO_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSGPAPERNO_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int DataLen;
            public int GroupNum;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMSCORESTATS_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            public int PaperNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETSAMSCORESTATS_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public int len;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_MAKESAMPAPEROVER_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UConstDefine.VolLen)]
            public char[] VolName;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_MAKESAMPAPEROVER_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETPERQTY_REQ
        {
            public stMsgHead MsgHead;
            public Int64 UserID;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct FM_GETPERQTY_RSP
        {
            public stMsgHead MsgHead;
            public int RspCode;
            public UMyRecords.stPerQtyInfo PerQtyInfo;
        }
        public static byte[] StructToBytes(object obj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(obj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;

        }
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            //得到结构的大小
            int size = Marshal.SizeOf(type);
            //    Log(size.ToString(), 1);
            //byte数组长度小于结构的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构
            return obj;
        }
        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }    
    }
}