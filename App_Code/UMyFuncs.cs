using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;
using FreeImageAPI;
using Microsoft.VisualBasic;
namespace Client.App_Code
{
    public class UMyFuncs
    {
        [DllImport("kernel32")]
        static extern uint GetTickCount();
        public static void ClearUpInfo(TDM_Client DM_Client, int InfoType)
        {
            int i;
            UMyRecords.stFullBlockInfo PTmpBlk;
            switch (InfoType)
            {
                case 1:
                    DM_Client.MyRecords.UserInfo.UserID = 0;
                    DM_Client.MyRecords.UserInfo.LoginName = "".PadRight(UConstDefine.NameLen, '\0').ToCharArray();
                    DM_Client.MyRecords.UserInfo.Role = 0;
                    DM_Client.MyRecords.UserInfo.Status = 0;
                    DM_Client.MyRecords.UserInfo.Interceder = 0;
                    DM_Client.MyRecords.UserInfo.TopLevelTeam = 0;
                    DM_Client.MyRecords.UserInfo.MidLevelTeam = 0;
                    DM_Client.MyRecords.UserInfo.BtmLevelTeam = 0;
                    DM_Client.MyRecords.UserInfo.XiaoZuZhangID = 0;
                    DM_Client.MyRecords.UserInfo.TiZuZhangID = 0;
                    DM_Client.MyRecords.UserInfo.DaZuZhangID = 0;
                    DM_Client.MyRecords.UserInfo.SmpPaperTackled = 0;
                    DM_Client.MyRecords.UserInfo.TstPaperTackled = 0;
                    DM_Client.MyRecords.UserInfo.PaperTackled = 0;
                    DM_Client.MyVars.CurPaperNum = -1;
                    DM_Client.MyRecords.UserInfo.TrueName = "".PadRight(UConstDefine.NameLen, '\0').ToCharArray();
                    DM_Client.MyRecords.UserInfo.ServeFor = "".PadRight(UConstDefine.ServeForLen, '\0').ToCharArray();
                    DM_Client.MyRecords.UserInfo.AssignLen = 0;
               //     DM_Client.MyRecords.UserInfo.Assignment = "".PadRight(2000, '\0').ToCharArray();
                    DM_Client.MyRecords.AssignList.Clear();
                    break;
                case 2:
                    DM_Client.MyVars.BlockOK = false;
                    if (DM_Client.MyRecords.BlockInfoList.Count > 0)
                    {
                        for (i = 0; i < DM_Client.MyRecords.BlockInfoList.Count; i++)
                        {
                            PTmpBlk = DM_Client.MyRecords.BlockInfoList[i];
                            PTmpBlk.TipsList.Clear();
                            DM_Client.MyRecords.BlockInfoList[i] = PTmpBlk;
                        }
                        DM_Client.MyRecords.BlockInfoList.Clear();
                    }
                    break;

            }
        }
        public static void DecodeInfo(TDM_Client DM_Client, string Info, int OperationType)
        {
            String s,tmpStr,tmpDetail,Points;
            UMyRecords.stMyPaper PMyPaper;
            int i, j, k, code;
            Single TmpScore;
            List<string> ScoreDetail;
            Boolean FirstFlag;
            switch (OperationType)
            {
                case 1:
                    DM_Client.MyRecords.AssignList.Clear();
                    s = Info;
                    while (s.Length > 0)
                    {
                        if (s.IndexOf(',') > 0)
                        {
                            tmpStr = s.Substring(0, s.IndexOf(','));
                        }
                        else
                        {
                            tmpStr = s;
                            DM_Client.MyRecords.AssignList.Add(tmpStr);
                            break;
                        }
                        DM_Client.MyRecords.AssignList.Add(tmpStr);
                        s = s.Substring(s.IndexOf(',') + 1);
                    }
                    break;
               

            }
        }
        public static int GetBlockInfo(TDM_Client DM_Client, int BlockType, int InfoLen, string VolStr, string ErrorMsg)
        {
            UMsgDefine.FM_GetBlkInfo_Req fmGetBlkReq;
            UMsgDefine.FM_GetBlkInfo_Rsp fmGetBlkRsp;
            UMsgDefine.FM_GetBlkInfo_Rsp_Login fmGetBlkRspTemp;
            UMyRecords.stFullBlockInfo PAddBlk;
            UMyRecords.stTipsInfo PAddTips;
            int Len, PicFormat;
            DateTime tmpTime;
            MemoryStream RcvdStream = new MemoryStream();
            switch (BlockType)
            {
                case 1:
                    fmGetBlkReq.MsgHead.MsgType = UConstDefine.TM_GETBLKINFO_REQ;
                    fmGetBlkReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GetBlkInfo_Req));
                    fmGetBlkReq.InfoType = UConstDefine.PM_GETBLKINFO_NORMAL;
                    fmGetBlkReq.VolumeName = VolStr.PadRight(UConstDefine.VolLen, '\0').ToCharArray();
                    lock (DM_Client.CsSocket)
                    {
                        byte[] Message = UMsgDefine.StructToBytes(fmGetBlkReq);
                        DM_Client.TCPSocket.Send(Message, fmGetBlkReq.MsgHead.MsgLength, SocketFlags.None);
                        DM_Client.TCPSocket.SendTimeout = UConstDefine.TimeOutSeconds;

                        fmGetBlkRsp.BlockInfo.CheckRule = "".PadRight(256, '\0').ToCharArray();
                        fmGetBlkRsp.BlockInfo.VolName = "".PadRight(UConstDefine.VolLen, '\0').ToCharArray();
                        fmGetBlkRsp.BlockInfo.RevaluateRule = "".PadRight(24, '\0').ToCharArray();

                        byte[] RcvMessage = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GetBlkInfo_Rsp_Login))];
                        DM_Client.TCPSocket.Receive(RcvMessage, Marshal.SizeOf(typeof(UMsgDefine.FM_GetBlkInfo_Rsp_Login)), SocketFlags.None);
                        fmGetBlkRspTemp = (UMsgDefine.FM_GetBlkInfo_Rsp_Login)UMsgDefine.BytesToStruct(RcvMessage, typeof(UMsgDefine.FM_GetBlkInfo_Rsp_Login));

                        if (fmGetBlkRspTemp.MsgHead.MsgType == UConstDefine.TM_GETBLKINFO_RSP)
                        {
                            if (fmGetBlkRspTemp.RspCode == UConstDefine.RM_RSP_OK)
                            {
                                PAddBlk = new UMyRecords.stFullBlockInfo();
                                PAddBlk.BlockInfo.VolName = fmGetBlkRspTemp.BlockInfo.VolName;
                                PAddBlk.BlockInfo.QuestionNo = fmGetBlkRspTemp.BlockInfo.QuestionNo;
                                PAddBlk.BlockInfo.BlkSeqNo = fmGetBlkRspTemp.BlockInfo.BlkSeqNo;
                                PAddBlk.BlockInfo.nNEv = fmGetBlkRspTemp.BlockInfo.nNEv;
                                PAddBlk.BlockInfo.CheckRule = fmGetBlkRspTemp.BlockInfo.CheckRule;
                                PAddBlk.BlockInfo.RevaluateRule = fmGetBlkRspTemp.BlockInfo.RevaluateRule;
                                PAddBlk.BlockInfo.CheckScore = fmGetBlkRspTemp.BlockInfo.CheckScore;
                                PAddBlk.BlockInfo.DispScore = fmGetBlkRspTemp.BlockInfo.DispScore;
                                PAddBlk.BlockInfo.BufSize = fmGetBlkRspTemp.BlockInfo.BufSize;
                                PAddBlk.BlockInfo.TimeOut = fmGetBlkRspTemp.BlockInfo.TimeOut;
                                PAddBlk.BlockInfo.QuestionType = fmGetBlkRspTemp.BlockInfo.QuestionType;
                                PAddBlk.BlockInfo.SampleTestMin = fmGetBlkRspTemp.BlockInfo.SampleTestMin;
                                PAddBlk.BlockInfo.MaxSize = fmGetBlkRspTemp.BlockInfo.MaxSize;
                                PAddBlk.BlockInfo.MinSize = fmGetBlkRspTemp.BlockInfo.MinSize;
                                PAddBlk.BlockInfo.AvgSize = fmGetBlkRspTemp.BlockInfo.AvgSize;
                                PAddBlk.BlockInfo.MinTime = fmGetBlkRspTemp.BlockInfo.MinTime;
                                //DM_Client.MyVars.CurImageLimit.MaxSize = fmGetBlkRspTemp.BlockInfo.MaxSize;
                                //DM_Client.MyVars.CurImageLimit.MinSize = fmGetBlkRspTemp.BlockInfo.MinSize;
                                //DM_Client.MyVars.CurImageLimit.AvgSize = fmGetBlkRspTemp.BlockInfo.AvgSize;
                                //DM_Client.MyVars.CurImageLimit.SpeedLimit = fmGetBlkRspTemp.BlockInfo.MinTime;
                                PAddBlk.BlockInfo.TotalTestMin = fmGetBlkRspTemp.BlockInfo.TotalTestMin;
                                PAddBlk.BlockInfo.ServerID = fmGetBlkRspTemp.BlockInfo.ServerID;
                                PAddBlk.BlockInfo.TipsLen = fmGetBlkRspTemp.BlockInfo.TipsLen;
                                PAddBlk.BlockInfo.JunkInfo = fmGetBlkRspTemp.BlockInfo.JunkInfo;
                                PAddBlk.BlockInfo.nMarkModelLen = fmGetBlkRspTemp.BlockInfo.nMarkModelLen;
                                PAddBlk.BlockInfo.MarkModelInfo = fmGetBlkRspTemp.BlockInfo.MarkModelInfo;

                                PAddBlk.TipsList = new List<UMyRecords.stTipsInfo>();
                                PAddBlk.TipsList.Clear();

                                PAddBlk.PingFenXiZeList = new List<UMyRecords.stPingFenXiZe>();
                                PAddBlk.PingFenXiZeList.Clear();

                                PAddBlk.DaAnList = new List<UMyRecords.stDaAn>();
                                PAddBlk.DaAnList.Clear();

                                RcvMessage = new byte[PAddBlk.BlockInfo.nMarkModelLen];
                                DM_Client.TCPSocket.Receive(RcvMessage, PAddBlk.BlockInfo.nMarkModelLen, SocketFlags.None);
                                char[] buf = new char[PAddBlk.BlockInfo.nMarkModelLen];
                                buf = System.Text.Encoding.Default.GetChars(RcvMessage);
                                PAddBlk.BlockInfo.MarkModelList = (char[])buf.Clone();

                                DM_Client.MyRecords.BlockInfoList.Add(PAddBlk);

                                

                                return PAddBlk.BlockInfo.TipsLen;
                            }
                            else
                            {
                                ErrorMsg = "获取题块信息时出现错误!";
                                return -1;
                            }
                        }
                        else
                        {
                            ErrorMsg = "获取题块信息时收到错误的响应帧";
                            return -1;
                        }
                    }
                case 2:
                    fmGetBlkReq.MsgHead.MsgType = UConstDefine.TM_GETBLKINFO_REQ;
                    fmGetBlkReq.MsgHead.MsgLength = fmGetBlkReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GetBlkInfo_Req));
                    fmGetBlkReq.InfoType = UConstDefine.PM_GETBLKINFO_TIPS;
                    fmGetBlkReq.VolumeName = VolStr.PadRight(UConstDefine.VolLen, '\0').ToCharArray();


                    lock (DM_Client.CsSocket)
                    {
                        byte[] Message = UMsgDefine.StructToBytes(fmGetBlkReq);
                        DM_Client.TCPSocket.Send(Message, fmGetBlkReq.MsgHead.MsgLength, SocketFlags.None);
                        DM_Client.TCPSocket.SendTimeout = UConstDefine.TimeOutSeconds;

                        //for adding --network stopping
                        byte[] buff = new byte[UConstDefine.BufferSize];
                        while (RcvdStream.Length < InfoLen)
                        {
                            Len = DM_Client.TCPSocket.Receive(buff, UConstDefine.BufferSize, SocketFlags.None);
                            RcvdStream.Write(buff, 0, Len);
                        }
                        if (RcvdStream.Length == InfoLen)
                        {
                            RcvdStream.Position = 0;
                            if (DM_Client.MyRecords.BlockInfoList.Count > 0)
                            {
                                PAddBlk = DM_Client.MyRecords.BlockInfoList.Last();
                                PAddBlk.TipsList.Clear();

                                while (RcvdStream.Position < RcvdStream.Length)
                                {
                                    byte[] RcvMessage = new byte[Marshal.SizeOf(typeof(int))];
                                    RcvdStream.Read(RcvMessage, 0, Marshal.SizeOf(typeof(int)));
                                    Len = BitConverter.ToInt32(RcvMessage, 0);
                                    RcvdStream.Read(RcvMessage, 0, Marshal.SizeOf(typeof(int)));
                                    PicFormat = BitConverter.ToInt32(RcvMessage, 0);

                                    PAddTips = new UMyRecords.stTipsInfo();

                                    PAddTips.TipsImage = new MemoryStream();

                                    RcvMessage = new byte[Len];
                                    RcvdStream.Read(RcvMessage, 0, Len);
                                    PAddTips.TipsImage.Write(RcvMessage, 0, Len);

                                    PAddBlk.TipsList.Add(PAddTips);
                                }
                            }
                        }
                        RcvdStream.Close();
                        return -1;
                    }
            }
            return -1;
        }
        public static void GetNextVol(TDM_Client DM_Client)
        {
            int index;
            UMyRecords.stFullBlockInfo PTmpBlk;

            DM_Client.MyVars.CurVolName = "";
            if (DM_Client.MyRecords.AssignList.Count > 0)
            {
                if (DM_Client.MyRecords.AssignList.Count == 1)
                {
                    DM_Client.MyVars.CurVolName = DM_Client.MyRecords.AssignList.ElementAt(0);
                }
                else
                {
                    index = DM_Client.MyRecords.AssignList.IndexOf(new string(DM_Client.MyRecords.CurBlockInfo.BlockInfo.VolName));
                    if(index < 0)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = (index+1) % DM_Client.MyRecords.AssignList.Count;
                    }
                    if((index >=0 ) && (index < DM_Client.MyRecords.AssignList.Count))
                    {
                        DM_Client.MyVars.CurVolName = DM_Client.MyRecords.AssignList.ElementAt(index);
                    }
                }

                if (!(DM_Client.MyVars.CurVolName.Equals("")) && (!new String(DM_Client.MyRecords.CurBlockInfo.BlockInfo.VolName).TrimEnd('\0').Equals(DM_Client.MyVars.CurVolName)))
                {
                    for(index = 0;index <DM_Client.MyRecords.BlockInfoList.Count;index++)
                    {
                        PTmpBlk = DM_Client.MyRecords.BlockInfoList.ElementAt(index);
                        string tmpstr1 = new String(PTmpBlk.BlockInfo.VolName);
                        tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                        if(tmpstr1.Equals(DM_Client.MyVars.CurVolName))
                        {
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.VolName = PTmpBlk.BlockInfo.VolName;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.QuestionNo = PTmpBlk.BlockInfo.QuestionNo;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.BlkSeqNo = PTmpBlk.BlockInfo.BlkSeqNo;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.nNEv = PTmpBlk.BlockInfo.nNEv;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckRule = PTmpBlk.BlockInfo.CheckRule;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckScore = PTmpBlk.BlockInfo.CheckScore;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.DispScore = PTmpBlk.BlockInfo.DispScore;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize = PTmpBlk.BlockInfo.BufSize;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.TimeOut = PTmpBlk.BlockInfo.TimeOut;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.QuestionType = PTmpBlk.BlockInfo.QuestionType;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.TotalTestMin = PTmpBlk.BlockInfo.TotalTestMin;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.SampleTestMin = PTmpBlk.BlockInfo.SampleTestMin;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.MaxSize = PTmpBlk.BlockInfo.MaxSize;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.MinSize = PTmpBlk.BlockInfo.MinSize;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.AvgSize = PTmpBlk.BlockInfo.AvgSize;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.MinTime = PTmpBlk.BlockInfo.MinTime;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.SampleAmount = PTmpBlk.BlockInfo.SampleAmount;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.ServerID = PTmpBlk.BlockInfo.ServerID;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.TipsLen = PTmpBlk.BlockInfo.TipsLen;
                            DM_Client.MyRecords.CurBlockInfo.BlockInfo.JunkInfo = PTmpBlk.BlockInfo.JunkInfo;
                            break;
                        }
                    }
                }
            }
        }
        public static void ClearPaperNode(UMyRecords.stMyPaper PPaper)
        {
            PPaper.PaperInfo.PaperNo = 0;
            PPaper.PaperInfo.ImageFormat = -1;
            PPaper.PaperInfo.VolumeName = "".PadRight(UConstDefine.VolLen, '\0').ToCharArray();
            PPaper.PaperInfo.PaperType = -1;
            PPaper.PaperInfo.ImageLen = -1;

            PPaper.PaperImage.Flush();

            PPaper.SaveType = -1;

            PPaper.ScoreInfo.BlkNo = -1;
            PPaper.ScoreInfo.UserIDOrPaperSeq = -1;
            PPaper.ScoreInfo.ExcpReason = -1;
            PPaper.ScoreInfo.TotalScore = -1;
            PPaper.ScoreInfo.Txt = "".PadRight(UConstDefine.DetailLen, '\0').ToCharArray();

            PPaper.RefScore.UserID1 = -1;
            PPaper.RefScore.UserID2 = -1;
            PPaper.RefScore.UserID3 = -1;
            PPaper.RefScore.UserID4 = -1;

            PPaper.TotalScore = -1;
            PPaper.DetailScore = "";
            PPaper.StartTime = 0;
            PPaper.TimeStamp = 0;
            PPaper.RStartTime = 0;
            PPaper.REndTime = 0;
            PPaper.Status = false;
            PPaper.Flag = false;
        }
        public static void WriteLogFile(TDM_Client DM_Client,string Content)
        {
            Content = DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss") + " " + Content;
            lock (DM_Client.CsLogFile)
            {
                string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                FileStream ds = File.Open(@"C:/Debug_" + tmpstr1 + @".txt", FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(ds);
                sw.WriteLine(Content);
                sw.Dispose();
                sw.Close();
                ds.Dispose();
                ds.Close();
            }
        }
        public static int GetBlkNo(string VolStr)
        {
            int i;
            for (i = 0; i < 2; i++)
            {
                VolStr = VolStr.Substring(VolStr.IndexOf('_') + 1);
            }
            VolStr = VolStr.Substring(0, VolStr.IndexOf('_'));


            i = (int)Microsoft.VisualBasic.Conversion.Val(VolStr); 
            if(IsNum(VolStr))
                return i;
            else
                return -1;
        }
        public static bool IsNum(String str)
        {
            if (str.ElementAt(0) == '-')
            {
                for (int i = 1; i < str.Length; i++)
                {
                    if (!Char.IsNumber(str, i))
                        return false;
                }
            }
            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!Char.IsNumber(str, i))
                        return false;
                }
            }
            return true;
        }
        public static void GetPicFromServer(TDM_Client DM_Client, MemoryStream stream, char[] URI)
        {
            //TODO:直接修改？
            //HTTP

            //SHARE
            WebClient webClient = new WebClient();
            byte[] buf = webClient.DownloadData(new string(URI).TrimEnd('\0'));
            stream.Write(buf, 0, buf.Length);
            //TFTP
        }
        public static int CheckDetail(TDM_Client DM_Client, int MidNo, string Detail)
        {
            UMyRecords.stFullBlockInfo PTmpBlk;
            int i, j, k, Count, Code, Pos1, Pos2;
            string CheckRule, Points, score1, score2;
            Single[] ArrFull;
            string[] strPt,strPt1,strPt2;
            Boolean AllowMinus;
            Single sumScore, Score;
            CheckRule = "";
            if (Detail.Equals(""))
            {
                return 1;
            }
            for (i = 0; i < DM_Client.MyRecords.BlockInfoList.Count; i++)
            {
                PTmpBlk = DM_Client.MyRecords.BlockInfoList[i];
                if (PTmpBlk.BlockInfo.BlkSeqNo == MidNo)
                {
                    CheckRule = new String(PTmpBlk.BlockInfo.CheckRule);
                    break;
                }
            }

            i = 0;

            CheckRule = CheckRule.Substring(0,CheckRule.IndexOf('\0'));

            Count = Int32.Parse(CheckRule.Substring(1, CheckRule.IndexOf('}') - 1));
            ArrFull = new Single[Count];
         
            CheckRule = CheckRule.Substring(CheckRule.IndexOf('}') + 2);
            while (CheckRule.IndexOf('(') >= 0)
            {
                Points = CheckRule.Substring(1, CheckRule.IndexOf(')') - 1);
                strPt = Points.Split(',');
                ArrFull[i] = float.Parse(strPt[1]);
                i++;
                CheckRule = CheckRule.Substring(CheckRule.IndexOf(')') + 2);
            }
            
            AllowMinus = false;
            if (CheckRule.ToCharArray(0,1)[0] == 'Y')
            {
                AllowMinus = true;
            }
          
            score1 = Detail;
            score2 = "";
            if (AllowMinus)
            {
                Pos1 = Detail.IndexOf('{');
                Pos2 = Detail.IndexOf('}');
                if (Pos1 < 0 || Pos2 < 0)
                {
                    return 1;
                }
                score1 = Detail.Substring(0, Detail.IndexOf('{') - 2);
                score2 = Detail.Substring(Pos1 + 1, Pos2 - Pos1 - 1);

            }
            strPt1 =  score1.Split(',');
            j = 0;
            sumScore = 0;
            for (i = 0; i < strPt1.Length; i++)
            {
                Score = (float)Conversion.Val(strPt1[i]);
                if (!IsNum(strPt1[i]))
                    return 1;
                if ((Score < 0) || (Score > ArrFull[i]))
                    return 2;
                j++;
                sumScore = sumScore + Score;
            }

            if (score2.Length != 0)         //防止score为空引起的索引出错
            {
                strPt2 = score2.Split('|');
                k = j;
                for (i = 0; i < strPt2.Length; i++)
                {
                    Score = (float)Microsoft.VisualBasic.Conversion.Val(strPt2[i]);
                    if (!IsNum(strPt2[i]))
                        return 1;
                    if (Score > 0 || Score < ArrFull[i + k])
                        return 2;
                    j++;
                    sumScore = sumScore + Score;
                }
            }
            if(j!=Count)
                return 1;
            if (sumScore < 0)
                return 3;
            return 0;
        }
        public static void LoadJP2(TDM_Client DM_Client,UMyRecords.stMyPaper PTmpPaper,string picTemp)
        {
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (!System.IO.Directory.Exists(picTemp + "/" + tmpstr1))
            {
                System.IO.Directory.CreateDirectory(picTemp + "/" + tmpstr1);
            }
            FreeImageBitmap fbm = FreeImageBitmap.FromStream(PTmpPaper.PaperImage);
            fbm.Save(picTemp + "/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp", FREE_IMAGE_FORMAT.FIF_BMP);
            string Content = "普通取卷请求成功";
            UMyFuncs.WriteLogFile(DM_Client, Content);
        }

        public static void LoadJP2(TDM_Client DM_Client, UMyRecords.stMyPaper PTmpPaper, string picTemp,Boolean NextPaper)  //重载
        {
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (!System.IO.Directory.Exists(picTemp + "/" + tmpstr1))
            {
                System.IO.Directory.CreateDirectory(picTemp + "/" + tmpstr1);
            }
            FreeImageBitmap fbm = FreeImageBitmap.FromStream(PTmpPaper.PaperImage);
            fbm.Save(picTemp + "/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp", FREE_IMAGE_FORMAT.FIF_BMP);
            DM_Client.NextPaper = true;     //已预取
            DM_Client.NextPaperNo = PTmpPaper.PaperInfo.PaperNo;
            string Content = "预取请求成功";
            UMyFuncs.WriteLogFile(DM_Client, Content);
        } 

        public static void FetchNextPaper(TDM_Client DM_Client,int index, int FetchType,string picTemp)
        {
            UMyRecords.stMyPaper PNextPaper;
            uint t1, t2;
            Boolean NextPaper = false;
            switch (FetchType)
            {
                case 0:
                    lock (DM_Client.CsNewPaper)
                    {
                        if (index < 0)
                            return;
                        PNextPaper = DM_Client.NewPaperList[index];
                        lock (DM_Client.CsNextPaper)
                        {
                            if (PNextPaper.Flag)
                            {
                                PNextPaper.PaperImage.Position = 0;
                                t1 = GetTickCount();
                                LoadJP2(DM_Client, PNextPaper, picTemp,NextPaper);
                                t2 = GetTickCount();
                            }
                        }
                    }
                    break;
                case 1: //处于复查状态时试卷的下一张
                    lock (DM_Client.CsOldPaper)
                    {
                        lock (DM_Client.CsNextPaper)
                        {
                            PNextPaper = DM_Client.OldPaperList.Last();
                            if (!PNextPaper.Status)//该试卷还未给分
                            {
                                PNextPaper.PaperImage.Position = 0;
                                LoadJP2(DM_Client, PNextPaper, picTemp);
                            }
                        }
                    }
                    break;
                case 2://异常试卷的下一张(普通客户端改不了)
                    lock (DM_Client.CsNextPaper)
                    {
                        lock (DM_Client.CsExpPaper)
                        {
                        }
                    }
                    break;
            }

        }
    }
}