using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using FreeImageAPI;
using Client.App_Code;

namespace Client
{
    public partial class Answer : System.Web.UI.Page
    {
        TDM_Client DM_Client;
        UCheck Check;
        int i;
        Boolean flag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            DM_Client = (TDM_Client)Session["DM_Client"];
            Check = (UCheck)Session["Check"];
            if (!IsPostBack)
            {
                if (DM_Client.MyVars.CurVolName.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('卷名为空');window.close();</script>", false);
          //          Response.Write("<script>window.close();</script>");
                }

                Check.DaAnNum = 0;
                Check.XiZeNum = 0;
                Check.BiaoZhunNum = 0;
                Check.PMyBlock = new UMyRecords.stFullBlockInfo();

                if (DM_Client.MyRecords.BlockInfoList.Count > 0)
                {
                    for (i = 0; i < DM_Client.MyRecords.BlockInfoList.Count; i++)
                    {
                        Check.PMyBlock = DM_Client.MyRecords.BlockInfoList[i];
                        if (new string(Check.PMyBlock.BlockInfo.VolName).TrimEnd('\0').Equals(DM_Client.MyVars.CurVolName))
                        {
                            flag = true;
                            break;
                        }
                        else
                        {
                            flag = false;
                        }
                    }

                    if (flag == true)
                    {
                        GetXiZe();
                        GetDaAn();

                        Check.BiaoZhunNum = Check.PMyBlock.TipsList.Count;
                        Check.XiZeNum = Check.PMyBlock.PingFenXiZeList.Count;
                        Check.DaAnNum = Check.PMyBlock.DaAnList.Count;

                        if (Check.BiaoZhunNum <= 0)
                        {
                            if (Check.XiZeNum <= 0)
                            {
                                if (Check.DaAnNum <= 0)
                                {
                                    Check.YiJu = false;
                                    Session["Check"] = Check;
                                    ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('没有可用的评分依据');window.close();</script>", false);
                               //     Response.Write("<script></script>");
                                }
                                else //只有答案
                                {
                                    lButtonBiaoZhun.Enabled = false;
                                    lButtonXiZe.Enabled = false;
                                    mvCompany.ActiveViewIndex = 2;
                                    Cell1.Attributes["class"] = "TopBorder";
                                    Cell2.Attributes["class"] = "TopBorder";
                                    Cell3.Attributes["class"] = "SelectedTopBorder";
                                    Check.DaAnNo = 0;
                                    ShowDaAn(Check.DaAnNo);
                                    Page.ClientScript.RegisterStartupScript(this.UpdateSystemAnswer.GetType(), "myscript", "<script>winresize('" + Check.DaAnWidth.ToString() + "','" + Check.DaAnHeight.ToString() + "');</script>");
                                    UpdateSystemAnswer.Update();
                                }
                            }
                            else //无标准，有细则
                            {
                                lButtonBiaoZhun.Enabled = false;
                                mvCompany.ActiveViewIndex = 1;
                                Cell1.Attributes["class"] = "TopBorder";
                                Cell2.Attributes["class"] = "SelectedTopBorder";
                                Cell3.Attributes["class"] = "TopBorder";
                                Check.XiZeNo = 0;
                                Check.DaAnNo = 0;
                                ShowXiZe(Check.XiZeNo);
                                UpdateSystemAnswer.Update();
                            }
                        }
                        else
                        {
                            mvCompany.ActiveViewIndex = 0;
                            Cell1.Attributes["class"] = "SelectedTopBorder";
                            Cell2.Attributes["class"] = "TopBorder";
                            Cell3.Attributes["class"] = "TopBorder";
                            Check.BiaoZhunNo = 0;
                            Check.XiZeNo = 0;
                            Check.DaAnNo = 0;
                            ShowBiaoZhun(Check.XiZeNo);
                            UpdateSystemAnswer.Update();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('当前没有可用的题块评分依据信息');window.close();</script>", false);
       //                 Response.Write("<script></script>");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('当前没有可用的题块信息');window.close();</script>", false);
           //         Response.Write("<script>window.close();</script>");
                }
                Session["Check"] = Check;
            }
        }
        public void GetXiZe()
        {
            UMsgDefine.FM_GETSTANDARD_REQ fmGetStandardReq;
            UMsgDefine.FM_GETSTANDARD_RSP fmGetStandardRsp;
            MemoryStream RcvMS;
            byte[] buf = new byte[8192];
            int len,PicFormat;
            DateTime tmpTime;
            UMyRecords.stPingFenXiZe PPingFenXiZe;

            fmGetStandardReq.MsgHead.MsgType = UConstDefine.TM_GETSTANDARD_REQ;
            fmGetStandardReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_REQ));
            fmGetStandardReq.RequestType = UConstDefine.FM_GETPINGFENXIZE;
            fmGetStandardReq.VolName = DM_Client.MyVars.CurVolName.PadRight(UConstDefine.VolLen,'\0').ToCharArray();

            lock (DM_Client.CsSocket)
            {
                byte[] Message = UMsgDefine.StructToBytes(fmGetStandardReq);
                DM_Client.TCPSocket.Send(Message, fmGetStandardReq.MsgHead.MsgLength, SocketFlags.None);

                byte[] RcvMessage = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_RSP))];
                DM_Client.TCPSocket.Receive(RcvMessage, Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_RSP)), SocketFlags.None);
                fmGetStandardRsp = (UMsgDefine.FM_GETSTANDARD_RSP)UMsgDefine.BytesToStruct(RcvMessage, typeof(UMsgDefine.FM_GETSTANDARD_RSP));

                if(fmGetStandardRsp.MsgHead.MsgType == UConstDefine.TM_GETSTANDARD_RSP)
                {
                    if (fmGetStandardRsp.RspCode == UConstDefine.RM_RSP_OK)
                    {
                        if (fmGetStandardRsp.StandardLen > 0)
                        {
                            RcvMS = new MemoryStream();
                            RcvMS.Flush();

                            while (RcvMS.Length < fmGetStandardRsp.StandardLen)
                            {
                                len = DM_Client.TCPSocket.Receive(buf, 8192, SocketFlags.None);
                                RcvMS.Write(buf, 0, len);
                            }

                            Check.PMyBlock.PingFenXiZeList = new List<UMyRecords.stPingFenXiZe>();
                            RcvMS.Position = 0;
                            while (RcvMS.Position < RcvMS.Length)
                            {
                                RcvMessage = new byte[Marshal.SizeOf(typeof(int))];
                                RcvMS.Read(RcvMessage, 0, 4);    //marshal.sizeof(char) = Ansi,sizeof(char) = unicode
                                len = BitConverter.ToInt32(RcvMessage, 0);

                                RcvMS.Read(RcvMessage, 0, 4);   //marshal.sizeof(char) = Ansi,sizeof(char) = unicode
                                PicFormat = BitConverter.ToInt32(RcvMessage, 0);

                                PPingFenXiZe = new UMyRecords.stPingFenXiZe();
                                PPingFenXiZe.PingFenXiZeImage = new MemoryStream();

                                byte[] buff = new byte[len];
                                RcvMS.Read(buff, 0, len);
                                PPingFenXiZe.PingFenXiZeImage.Write(buff, 0, len);

                                Check.PMyBlock.PingFenXiZeList.Add(PPingFenXiZe);
                            }
                            RcvMS.Close();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('获取评分细则响应代码出错');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('获取响应类型出错');</script>", false);
                }
            }
        }

        public void GetDaAn()
        {
            UMsgDefine.FM_GETSTANDARD_REQ fmGetStandardReq;
            UMsgDefine.FM_GETSTANDARD_RSP fmGetStandardRsp;
            MemoryStream RcvMS;
            byte[] buf = new byte[8192];
            int len, PicFormat;
            DateTime tmpTime;
            UMyRecords.stDaAn PDaAn;

            fmGetStandardReq.MsgHead.MsgType = UConstDefine.TM_GETSTANDARD_REQ;
            fmGetStandardReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_REQ));
            fmGetStandardReq.RequestType = UConstDefine.FM_GETCANKAODAAN;
            fmGetStandardReq.VolName = DM_Client.MyVars.CurVolName.PadRight(UConstDefine.VolLen, '\0').ToCharArray();

            lock (DM_Client.CsSocket)
            {
                byte[] Message = UMsgDefine.StructToBytes(fmGetStandardReq);
                DM_Client.TCPSocket.Send(Message, fmGetStandardReq.MsgHead.MsgLength, SocketFlags.None);

                byte[] RcvMessage = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_RSP))];
                DM_Client.TCPSocket.Receive(RcvMessage, Marshal.SizeOf(typeof(UMsgDefine.FM_GETSTANDARD_RSP)), SocketFlags.None);
                fmGetStandardRsp = (UMsgDefine.FM_GETSTANDARD_RSP)UMsgDefine.BytesToStruct(RcvMessage, typeof(UMsgDefine.FM_GETSTANDARD_RSP));

                if (fmGetStandardRsp.MsgHead.MsgType == UConstDefine.TM_GETSTANDARD_RSP)
                {
                    if (fmGetStandardRsp.RspCode == UConstDefine.RM_RSP_OK)
                    {
                        if (fmGetStandardRsp.StandardLen > 0)
                        {
                            RcvMS = new MemoryStream();
                            RcvMS.Flush();

                            while (RcvMS.Length < fmGetStandardRsp.StandardLen)
                            {
                                len = DM_Client.TCPSocket.Receive(buf, 8192, SocketFlags.None);
                                RcvMS.Write(buf, 0, len);
                            }

                            Check.PMyBlock.DaAnList = new List<UMyRecords.stDaAn>();
                            RcvMS.Position = 0;
                            while (RcvMS.Position < RcvMS.Length)
                            {
                                RcvMessage = new byte[Marshal.SizeOf(typeof(int))];
                                RcvMS.Read(RcvMessage, 0, 4);   //marshal.sizeof(char) = Ansi,sizeof(char) = unicode
                                len = BitConverter.ToInt32(RcvMessage, 0);

                                RcvMS.Read(RcvMessage, 0, 4);    //marshal.sizeof(char) = Ansi,sizeof(char) = unicode
                                PicFormat = BitConverter.ToInt32(RcvMessage, 0);

                                PDaAn = new UMyRecords.stDaAn();
                                PDaAn.DaAnImage = new MemoryStream();

                                byte[] buff = new byte[len];
                                RcvMS.Read(buff, 0, len);
                                PDaAn.DaAnImage.Write(buff, 0, len);

                                Check.PMyBlock.DaAnList.Add(PDaAn);
                            }
                            RcvMS.Close();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('获取评分细则响应代码出错');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdateSystemAnswer, this.UpdateSystemAnswer.GetType(), "msgAns", "<script>alert('获取响应类型出错');</script>", false);
                }
            }
        }
        public void ShowXiZe(int XiZeNo)
        {
            UMyRecords.stPingFenXiZe PPingFenXiZe;
  
            if (Check.XiZeNum > 0)
            {
                if (XiZeNo < Check.PMyBlock.PingFenXiZeList.Count)
                {
                    PPingFenXiZe = new UMyRecords.stPingFenXiZe();
                    PPingFenXiZe = Check.PMyBlock.PingFenXiZeList[XiZeNo];
                    PPingFenXiZe.PingFenXiZeImage.Position = 0;

                    if (XiZeNo == Check.XiZeNum - 1)
                    {
                        XiZeNo = 0;
                    }
                    LoadXiZe(PPingFenXiZe,XiZeNo);
                }
            }
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            xizeimage.ImageUrl = "~/" + tmpstr1 + "XiZe/" + XiZeNo.ToString() + ".bmp";
        }
        public void LoadXiZe(UMyRecords.stPingFenXiZe PPingFenXiZe,int XiZeNo)
        {
            string picTemp = Server.MapPath(".");
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (!System.IO.Directory.Exists(picTemp + "/" + tmpstr1 + "XiZe"))
            {
                System.IO.Directory.CreateDirectory(picTemp + "/" + tmpstr1 + "XiZe");
            }
            FreeImageBitmap fbm = FreeImageBitmap.FromStream(PPingFenXiZe.PingFenXiZeImage);
            Check.XiZeWidth = fbm.Width;
            Check.XiZeHeight = fbm.Height;
            fbm.Save(picTemp + "/" + tmpstr1 + "XiZe/" + XiZeNo.ToString() + ".bmp", FREE_IMAGE_FORMAT.FIF_BMP);
            string Content = "取细则请求成功";
            UMyFuncs.WriteLogFile(DM_Client, Content);
        }
        
        public void ShowDaAn(int DaAnNo)
        {
            UMyRecords.stDaAn PDaAn;
     
            if (Check.DaAnNum > 0)
            {
                if (DaAnNo < Check.PMyBlock.DaAnList.Count)
                {
                    PDaAn = new UMyRecords.stDaAn();
                    PDaAn = Check.PMyBlock.DaAnList[DaAnNo];
                    PDaAn.DaAnImage.Position = 0;

                    if (DaAnNo == Check.DaAnNum - 1)
                    {
                        DaAnNo = 0;
                    }
                    LoadDaAn(PDaAn, DaAnNo);
                }
            }
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            daanimage.ImageUrl = "~/" + tmpstr1 + "DaAn/" + DaAnNo.ToString() + ".bmp";
            
        }
        public void LoadDaAn(UMyRecords.stDaAn PDaAn, int DaAnNo)
        {
            string picTemp = Server.MapPath(".");
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (!System.IO.Directory.Exists(picTemp + "/" + tmpstr1 + "DaAn"))
            {
                System.IO.Directory.CreateDirectory(picTemp + "/" + tmpstr1 + "DaAn");
            }
            FreeImageBitmap fbm = FreeImageBitmap.FromStream(PDaAn.DaAnImage);
            Check.DaAnWidth = fbm.Width;
            Check.DaAnHeight = fbm.Height;
            fbm.Save(picTemp + "/" + tmpstr1 + "DaAn/" + DaAnNo.ToString() + ".bmp", FREE_IMAGE_FORMAT.FIF_BMP);
            string Content = "取答案请求成功";
            UMyFuncs.WriteLogFile(DM_Client, Content);
        }
        public void ShowBiaoZhun(int BiaoZhunNo)
        {
            UMyRecords.stTipsInfo PTipsInfo;
  
            if (Check.BiaoZhunNum > 0)
            {
                if (BiaoZhunNo < Check.PMyBlock.TipsList.Count)
                {
                    PTipsInfo = new UMyRecords.stTipsInfo();
                    PTipsInfo = Check.PMyBlock.TipsList[BiaoZhunNo];
                    PTipsInfo.TipsImage.Position = 0;

                    if (BiaoZhunNo == Check.BiaoZhunNum - 1)
                    {
                        BiaoZhunNo = 0;
                    }
                    LoadBiaoZhun(PTipsInfo, BiaoZhunNo);
                }
            }
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            biaozhunimage.ImageUrl = "~/"  + tmpstr1 + "BiaoZhun/" + BiaoZhunNo.ToString() + ".bmp";
        }
        public void LoadBiaoZhun(UMyRecords.stTipsInfo PTipsInfo, int BiaoZhunNo)
        {
            string picTemp = Server.MapPath(".");
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (!System.IO.Directory.Exists(picTemp + "/" + tmpstr1 + "BiaoZhun"))
            {
                System.IO.Directory.CreateDirectory(picTemp + "/" + tmpstr1 + "BiaoZhun");
            }
            FreeImageBitmap fbm = FreeImageBitmap.FromStream(PTipsInfo.TipsImage);
            Check.BiaoZhunWidth = fbm.Width;
            Check.BiaoZhunHeight = fbm.Height;
            fbm.Save(picTemp + "/" + tmpstr1 + "BiaoZhun/" + BiaoZhunNo.ToString() + ".bmp", FREE_IMAGE_FORMAT.FIF_BMP);
            string Content = "取标准请求成功";
            UMyFuncs.WriteLogFile(DM_Client, Content);
        }
        protected void lButtonBiaoZhun_Click(object sender, EventArgs e)
        {
            mvCompany.ActiveViewIndex = 0;
            Cell1.Attributes["class"] = "SelectedTopBorder";
            Cell2.Attributes["class"] = "TopBorder";
            Cell3.Attributes["class"] = "TopBorder";
        }
        protected void lButtonXiZe_Click(object sender, EventArgs e)
        {
            mvCompany.ActiveViewIndex = 1;
            Cell1.Attributes["class"] = "TopBorder";
            Cell2.Attributes["class"] = "SelectedTopBorder";
            Cell3.Attributes["class"] = "TopBorder";
        }
        protected void lButtonDaAn_Click(object sender, EventArgs e)
        {
            mvCompany.ActiveViewIndex = 2;
            Cell1.Attributes["class"] = "TopBorder";
            Cell2.Attributes["class"] = "TopBorder";
            Cell3.Attributes["class"] = "SelectedTopBorder";
        }

        protected void biaozhunnext_Click(object sender, EventArgs e)
        {
            Check.BiaoZhunNo++;
            ShowXiZe(Check.BiaoZhunNo);
            Session["Check"] = Check;
        }

        protected void biaozhunbefore_Click(object sender, EventArgs e)
        {
            Check.BiaoZhunNo--;
            ShowXiZe(Check.BiaoZhunNo);
            Session["Check"] = Check;
        }

        protected void xizebefore_Click(object sender, EventArgs e)
        {
            Check.XiZeNo--;
            ShowXiZe(Check.XiZeNo);
            Session["Check"] = Check;
        }

        protected void xizenext_Click(object sender, EventArgs e)
        {
            Check.XiZeNo++;
            ShowXiZe(Check.XiZeNo);
            Session["Check"] = Check;
        }

        protected void daanbefore_Click(object sender, EventArgs e)
        {
            Check.DaAnNo--;
            ShowXiZe(Check.DaAnNo);
            Session["Check"] = Check;
        }

        protected void daannext_Click(object sender, EventArgs e)
        {
            Check.DaAnNo++;
            ShowXiZe(Check.DaAnNo);
            Session["Check"] = Check;
        }
    }
}