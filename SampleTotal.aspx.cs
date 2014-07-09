using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Client.App_Code;
using System.Data;
using System.Runtime.InteropServices;
using System.Net.Sockets;
namespace Client
{
    public partial class SampleTotal : System.Web.UI.Page
    {
        int PaperNum,SampleSelect;
        TDM_Client DM_Client;
        DataTable Sampledt = new DataTable();
        DataTable SamPaperNo = new DataTable();
        char[] SGPaperNo;
        UMyRecords.stPaperNo[] SGPaperNoByGrp;
        UCheck Check;
        protected void Page_Load(object sender, EventArgs e)
        {
            UMyRecords.stFullBlockInfo tmpPBlkInfo;
            DM_Client = (TDM_Client)Session["DM_Client"];
            
            if (!IsPostBack)
            {
                GridViewCheckSample.Attributes.Add("style", "word-break:keep-all;word-wrap:normal");
           //     PaperNum = DM_Client.MyVars.SampleNum;//样卷数与样卷制作中保存的样卷数量同步
           //     SampleSelect = PaperNum;

                //DropDownList1.Items.Add("请选题块号");
                if (DM_Client.MyRecords.BlockInfoList.Count > 0)
                {
                    for (int i = 0; i < DM_Client.MyRecords.BlockInfoList.Count; i++)
                    {
                        tmpPBlkInfo = DM_Client.MyRecords.BlockInfoList[i];
                        DropDownList1.Items.Add(new String(tmpPBlkInfo.BlockInfo.VolName));
               //         Label1.Text = new String(tmpPBlkInfo.BlockInfo.VolName);
                    }
                }
                if (DropDownList1.Items.Count > 0)
                {
                    DropDownList1.SelectedIndex = 0;
                    Combox_BlkSelect();
                    Session["DM_Client"] = DM_Client;

                }

            }
            
            //SamPaperNo = DM_Client.SamPaperNo;

            //GridViewCheck.DataSource = SamPaperNo;
            //GridViewCheck.DataBind();
            Sampledt = DM_Client.Sampledt;
            GridViewSampleScore.DataSource = Sampledt;
            GridViewSampleScore.DataBind();
            UpdateImage.Update();
            UpdateImage1.Update();
   
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (ListItem listitem in DropDownList1.Items)
            {
                ClientScript.RegisterForEventValidation(DropDownList1.UniqueID, listitem.Value);
            }
            base.Render(writer);
        }
        private void Combox_BlkSelect()
        {
            int i, j;
            UMsgDefine.FM_GETSAMPAPERNO_REQ SamPaperNoRes;     //取某个题块全部样卷号
            UMsgDefine.FM_GETSAMPAPERNO_RSP SamPaperNoRsp;
            UMsgDefine.FM_GETSGPAPERNO_REQ fmGetSGPaperNoReq;
            UMsgDefine.FM_GETSGPAPERNO_RSP fmGetSGPaperNoRsp;
            string tmp1 = "";
            UMyRecords.stPaperNo tmpPSGPaperNoByGrp;
            int[] tempPaperNo = new int[UConstDefine.SamPaperLen];
  
            UMyRecords.stFullBlockInfo PMyBlock;
            UMyRecords.StExamRefScore PExamRefScore;
            string tmpstring,s,p;

            if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.DaZuZhang)
            {
                //TODO:
            }
            else
            {
                if (!new string(DM_Client.MyRecords.CurBlockInfo.BlockInfo.VolName).Equals(""))
                {
                    DM_Client.MyVars.QuestionType = DM_Client.MyRecords.CurBlockInfo.BlockInfo.QuestionType;
                    DM_Client.MyVars.CurFullScore = -1;    //初始化
                    DecodeInfo(new string(DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckRule),5);   //解析题组长得分要点到StrGrd_ProScore
                 //   DecodeInfoZJ(DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckRule,1,1); //解析大组长得分要点到StrGrd_ReScore(普通用户无）

               //     RowCount = StrGrd_ProScore.RowCount;
                    if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.DaZuZhang)
                    {
                        //TODO:setCurRate();
                    }
                    DropDownSampleGroup.Enabled = false;
                    if ((DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong && DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing) || (DM_Client.MyRecords.UserInfo.Role == UConstDefine.XiaoZuZhang))
                    {
                        fmGetSGPaperNoReq.MsgHead.MsgType = UConstDefine.TM_GETSGPAPERNO_REQ;
                        fmGetSGPaperNoReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GETSGPAPERNO_REQ));
                        fmGetSGPaperNoReq.UserID = DM_Client.MyRecords.UserInfo.UserID;
                        fmGetSGPaperNoReq.VolName = DropDownList1.SelectedValue.PadRight(32,'\0').ToCharArray();

                        lock (DM_Client.CsSocket)
                        {
                            byte[] buf = UMsgDefine.StructToBytes(fmGetSGPaperNoReq);
                            DM_Client.TCPSocket.Send(buf, fmGetSGPaperNoReq.MsgHead.MsgLength, SocketFlags.None);

                            byte[] Receivebuf = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GETSGPAPERNO_RSP))];
                            DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                            fmGetSGPaperNoRsp = (UMsgDefine.FM_GETSGPAPERNO_RSP)UMsgDefine.BytesToStruct(Receivebuf, typeof(UMsgDefine.FM_GETSGPAPERNO_RSP));

                            if (fmGetSGPaperNoRsp.MsgHead.MsgType == UConstDefine.TM_GETSGPAPERNO_RSP)
                            {
                                if (fmGetSGPaperNoRsp.RspCode == UConstDefine.RM_RSP_OK)
                                {
                                    if (fmGetSGPaperNoRsp.GroupNum == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('当前题块没有样卷分组,请关闭此窗体回到阅卷状态');</script>", false);
                                        return;
                                    }

                                    tmpPSGPaperNoByGrp = new UMyRecords.stPaperNo();
                                    SGPaperNo = new char[fmGetSGPaperNoRsp.DataLen];
                                    Receivebuf = new byte[fmGetSGPaperNoRsp.DataLen];
                                    DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                                    SGPaperNo = System.Text.Encoding.Default.GetChars(Receivebuf);

                                    DropDownSampleGroup.Enabled = true;

                                    tmpstring = new string(SGPaperNo);
                                    SGPaperNoByGrp = new UMyRecords.stPaperNo[fmGetSGPaperNoRsp.GroupNum];
                                    for (i = 0; i < fmGetSGPaperNoRsp.GroupNum; i++)
                                    {
                                        s = tmpstring.Substring(tmpstring.IndexOf('(') + 1, tmpstring.IndexOf(')') - tmpstring.IndexOf('(') - 1);
                                        p = s.Substring(0, s.IndexOf(','));
                                        SGPaperNoByGrp[i].GroupName = p.PadRight(64, '\0').ToCharArray();
                                        p = s.Substring(s.IndexOf(',') + 1);
                                        SGPaperNoByGrp[i].PaperNo = p;
                                        tmpstring = tmpstring.Substring(tmpstring.IndexOf(')') + 1);
                                    }
                                    DM_Client.SGPaperNoByGrp = SGPaperNoByGrp;
                                    for (i = 0; i < fmGetSGPaperNoRsp.GroupNum; i++)
                                    {
                                        tmp1 = new string(SGPaperNoByGrp[i].GroupName);
                                        tmp1 = tmp1.Substring(0, tmp1.IndexOf('\0'));
                                        if (tmp1.Equals("-1"))
                                        {
                                            DropDownSampleGroup.Items.Add("未分组样卷");
                                        }
                                        else
                                        {
                                            DropDownSampleGroup.Items.Add(tmp1);
                                        }
                                    }
                                }
                                else
                                {
                                    switch (fmGetSGPaperNoRsp.RspCode)  //原客户端bug?
                                    {
                                        case UConstDefine.RM_ERR_NOSAMNO:
                                            ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('当前题块没有样卷!');</script>", false);
                                            return;
                                            
                                        default:
                                            ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取样卷号时响应帧代码错误!');</script>", false);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取响应帧类型错误!');</script>", false);
                            }
                        }
                    }
                    SamPaperNoRes.MsgHead.MsgType = UConstDefine.TM_GETSAMPAPERNO_REQ;
                    SamPaperNoRes.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GETSAMPAPERNO_REQ));
                    SamPaperNoRes.UserID = DM_Client.MyRecords.UserInfo.UserID;

                    SamPaperNoRes.VolName = DropDownList1.SelectedValue.PadRight(32,'\0').ToCharArray();
<<<<<<< HEAD
                    
=======
                    tmp1 = new string(SGPaperNoByGrp[0].GroupName);
>>>>>>> origin/master
                    
                    SamPaperNoRes.GroupName = "".PadRight(64, '\0').ToCharArray();

                    lock (DM_Client.CsSocket)
                    {

                        byte[] buf = UMsgDefine.StructToBytes(SamPaperNoRes);
                        DM_Client.TCPSocket.Send(buf, SamPaperNoRes.MsgHead.MsgLength, SocketFlags.None);

                        byte[] Receivebuf = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GETSAMPAPERNO_RSP))];
                        DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                        SamPaperNoRsp = (UMsgDefine.FM_GETSAMPAPERNO_RSP)UMsgDefine.BytesToStruct(Receivebuf, typeof(UMsgDefine.FM_GETSAMPAPERNO_RSP));

                        if (SamPaperNoRsp.MsgHead.MsgType == UConstDefine.TM_GETSAMPAPERNO_RSP)
                        {
                            if (SamPaperNoRsp.RspCode == UConstDefine.RM_RSP_OK)
                            {
                                for (int k = 0; k < 256; k++)
                                {
                                    tempPaperNo[k] = SamPaperNoRsp.PaperNo[k];
                                }
                                DataTable SamPaperNo = new DataTable();
                                SamPaperNo.Columns.Add("试卷号");
                                SamPaperNo.Columns.Add("状态");
                                if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                                {
                                    for (int k = 0; k < SamPaperNoRsp.FinalLength; k++)
                                    {
                                        SamPaperNo.Rows.Add(tempPaperNo[k], "已确认");
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < SamPaperNoRsp.TempLength; k++)
                                    {
                                        SamPaperNo.Rows.Add(tempPaperNo[SamPaperNoRsp.FinalLength + k], "待定");
                                    }
                                    for (int k = SamPaperNoRsp.TempLength; k < SamPaperNoRsp.FinalLength + SamPaperNoRsp.TempLength; k++)
                                    {
                                        SamPaperNo.Rows.Add(tempPaperNo[k - SamPaperNoRsp.TempLength], "已确认");
                                    }
                                }
                                DM_Client.SamPaperNo = SamPaperNo;
                                GridViewCheckSample.DataSource = SamPaperNo;
                                GridViewCheckSample.DataBind();
                            }
                            else
                            {
                                switch (SamPaperNoRsp.RspCode)
                                {
                                    case UConstDefine.RM_ERR_NOSAMNO:
                                        ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('当前题块没有样卷!');</script>", false);
                                        return;
                                    default:
                                        ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取样卷号时响应帧代码错误!');</script>", false);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取响应帧类型错误!');</script>", false);
                        }

                    }

                }
            }
        }

        private void DecodeInfoZJ(UMyRecords.StExamRefScore tmpExamRefScore, int ARow, int OperationType)
        {
            string tmpDetail,Points,s,TmpStr,tmpstring,tmps;
            int i,j,k,code;
            Single TmpScore;
            long tmp;
            Sampledt = DM_Client.Sampledt.Copy();
           // ScoreDetail:TStringList;
            code = Sampledt.Rows.Count;
            DataRow dr = Sampledt.NewRow();
            tmp = tmpExamRefScore.UserID;
            tmpstring = tmp.ToString().Substring(0,1);
            if(tmpstring.Equals("4"))    //大组长给分
            {
                tmps = tmp.ToString().Substring(1,3);
                tmpstring=tmp.ToString().Substring(8,2);
                dr["步骤"] = "d"+tmps + tmpstring+"给分";
            }
            else if(tmpstring.Equals("3"))     //题组长给分
            {
                tmps = tmp.ToString().Substring(1,5);
                tmpstring = tmp.ToString().Substring(8,2);
                dr["步骤"] = "t"+tmps+tmpstring+"给分";
            }
            dr["档次"] = tmpExamRefScore.Rate.ToString();
            tmpDetail = new string(tmpExamRefScore.Reason);
            tmpDetail = tmpDetail.Substring(0,tmpDetail.IndexOf('\0'));
            dr["理由"] = tmpDetail;
            switch (OperationType)
            {
                case 2:  //解析详细分数到样卷界面的专家详细给分StrGrd_ProScore
                    tmpDetail = new string(tmpExamRefScore.DetailScore);
                    tmpDetail = tmpDetail.Substring(0, tmpDetail.IndexOf('\0'));
                    i = 1;
                    switch (DM_Client.MyVars.QuestionType)
                    {
                        case 1:
                        case 2:
                        case 3:
                            while (tmpDetail.Length > 0)
                            {
                                if (tmpDetail.IndexOf(',') > 0)
                                {
                                    dr[i] = tmpDetail.Substring(0, tmpDetail.IndexOf(','));
                                    tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf(',') + 1);
                                }
                                else
                                {
                                    if (tmpDetail.IndexOf('{') > 0)
                                    {
                                        tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf('{') + 1, tmpDetail.IndexOf('}') - tmpDetail.IndexOf('{') - 1);
                                    }
                                    while (tmpDetail.Length > 0)
                                    {
                                        if (tmpDetail.IndexOf('|') > 0)
                                        {
                                            dr[i] = tmpDetail.Substring(0, tmpDetail.IndexOf('|'));
                                            tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf('|') + 1);
                                        }
                                        else
                                        {
                                            dr[i] = tmpDetail;
                                            tmpDetail = "";
                                        }
                                        i++;
                                    }
                                    tmpDetail = "";
                                    break;
                                }
                                i++;
                            }


                            Sampledt.Rows.Add(dr);
                            break;
                        case 4:
                        case 5: //TODO:
                            break;
                    }
                    break;
            }
        }

        private void DecodeInfo(string Info, int p_2)
        {
            string TmpStr,Points,TmpStr2;
            int colcount,j;
            string[] ScoreDetail;
            Single[] ArrFull;
            string[] PointsName;
            TmpStr = Info;
          
            colcount = Int32.Parse(TmpStr.Substring(1, TmpStr.IndexOf('}') - 1));
            ArrFull = new Single[colcount];
            PointsName = new String[colcount];
            TmpStr = TmpStr.Substring(TmpStr.IndexOf('}') + 2);
            DM_Client.MyVars.CurFullScore = 0;
            Sampledt.Columns.Add("步骤");
            switch (DM_Client.MyVars.QuestionType)
            {
                case 1:
                case 2:
                case 3:
                    j = 0;
                    TmpStr2 = TmpStr;
                    while (TmpStr.IndexOf('(') >= 0)
                    {
                        Points = TmpStr.Substring(1, TmpStr.IndexOf(')') - 1);
                        ScoreDetail = Points.Split(',');
                        
                        ArrFull[j] = float.Parse(ScoreDetail[1]);
                        if (ArrFull[j] > 0)
                        {
                            DM_Client.MyVars.CurFullScore += ArrFull[j];
                        }
                        PointsName[j] = ScoreDetail[0];
                        
                        BoundField newcolumn = new BoundField();
                        newcolumn.HeaderText = ScoreDetail[0];
                        newcolumn.DataField = ScoreDetail[0];
                        this.GridViewSampleScore.Columns.Insert(j+1, newcolumn);
                        
                        Sampledt.Columns.Add(ScoreDetail[0]);
                        
                        j++;
                        TmpStr = TmpStr.Substring(TmpStr.IndexOf(')') + 2);
                    }
                    Sampledt.Columns.Add("档次");
                    Sampledt.Columns.Add("理由");

                    DataRow dr = Sampledt.NewRow();
                    dr["步骤"] = "满分";
                    for (int i = 0; i < ArrFull.Length; i++)
                    {
                        dr[PointsName[i]] = ArrFull[i].ToString();
                    }
                    dr["档次"] = "";
                    dr["理由"] = "";
                    Sampledt.Rows.Add(dr);
                    DM_Client.Sampledt = Sampledt;
                    GridViewSampleScore.DataSource = Sampledt;
                    GridViewSampleScore.DataBind();
                    break;
                    //TODO:解析是否允许扣分
                case 4:
                case 5:
                    //TODO:
                    break;
            }
        }
        protected void LinkButtonSelect_Click(object sender, EventArgs e)
        {
            int i, j, code;
            Int64 tmp;
            string tmpstring, tmpscore, tmps;
            UMyRecords.stMyUser PTmpUser,tmpPMyUser;
            UMsgDefine.FM_GetPaper_Req fmGetPaperReq;
            UMsgDefine.FM_GetPaper_Rsp fmGetPaperRsp;
            UMyRecords.stMyPaper tmpPMyPaper = new UMyRecords.stMyPaper();
            UMyRecords.StExamRefScore tmpPExamRefScore;
            UMyRecords.StExamRefScore[] tmpExamRefScore;
            int len, tmplen, curRow;
            Boolean isSuccess = false;
            UMyRecords.stStatInfo tmpPStatInfo;
         //   UMyRecords.stBlkQty tmpPBlkQty;
            double v;
            int RecCount;
            char[] DetailScoreforPT = new char[64];
            string Content;
            System.IO.MemoryStream RcvMS;

            LinkButton b = (LinkButton)sender;
            GridViewRow row = (GridViewRow)b.Parent.Parent;
            int index = row.RowIndex;    //确定点击的行

            tmpExamRefScore = new UMyRecords.StExamRefScore[1];

       //     DM_Client.MyVars.curPaper.PaperInfo.PaperNo = 0;

            fmGetPaperReq.MsgHead.MsgType = UConstDefine.TM_GETPAPER_REQ;
            fmGetPaperReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GetPaper_Req));
            fmGetPaperReq.GetPaperTask.UserID=DM_Client.MyRecords.UserInfo.UserID;
            fmGetPaperReq.GetPaperTask.PaperType=UConstDefine.PM_PAPERTYPE_SAMPLE;
            fmGetPaperReq.GetPaperTask.DispOrBlkNo = UMyFuncs.GetBlkNo(DropDownList1.SelectedValue);
            fmGetPaperReq.GetPaperTask.PaperNoOrReason = Int32.Parse(((Label)row.FindControl("GridPaperNo")).Text);
            fmGetPaperReq.GetPaperTask.VolName = DropDownList1.SelectedValue.PadRight(32, '\0').ToCharArray();

            lock (DM_Client.CsSocket)
            {
                byte[] buf = UMsgDefine.StructToBytes(fmGetPaperReq);
                DM_Client.TCPSocket.Send(buf, fmGetPaperReq.MsgHead.MsgLength, SocketFlags.None);

                byte[] Receivebuf = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GetPaper_Rsp))];
                DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                fmGetPaperRsp = (UMsgDefine.FM_GetPaper_Rsp)UMsgDefine.BytesToStruct(Receivebuf, typeof(UMsgDefine.FM_GetPaper_Rsp));

                if (fmGetPaperRsp.MsgHead.MsgType == UConstDefine.TM_GETPAPER_RSP)
                {
                    if (fmGetPaperRsp.RspCode == UConstDefine.RM_RSP_OK)
                    {
                        tmpPMyPaper = new UMyRecords.stMyPaper();
                        tmpPMyPaper.PaperInfo = fmGetPaperRsp.PaperData;
                 //       DM_Client.MyVars.curPaper.PaperInfo = fmGetPaperRsp.PaperData;

                        tmpPExamRefScore = new UMyRecords.StExamRefScore();
                        tmplen = Marshal.SizeOf(typeof(UMyRecords.StExamRefScore));

                        Receivebuf = new byte[tmplen];
                        DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                        tmpPExamRefScore = (UMyRecords.StExamRefScore)UMsgDefine.BytesToStruct(Receivebuf, typeof(UMyRecords.StExamRefScore));
                        RecCount = tmpPExamRefScore.RecCount;

                        tmpExamRefScore = new UMyRecords.StExamRefScore[RecCount];
                        tmpExamRefScore[0] = tmpPExamRefScore;
                        
                        if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                        {
                            if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                            {
                                Receivebuf = new byte[64];
                                DM_Client.TCPSocket.Receive(Receivebuf, RecCount, SocketFlags.None);
                                DetailScoreforPT = System.Text.Encoding.Default.GetChars(Receivebuf);
                            }
                            if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing)
                            {//普通评卷员正评状态下浏览样卷库时不需要获取自己的参考分
                            }

                        }
                        else //题组长浏览样卷库，能看到包含好几份成绩的待定的样卷 若有n份题组长给分，则再接收n-1次
                        {
                            i = 1;
                            while (RecCount > 1)
                            {
                                Receivebuf = new byte[tmplen];
                                DM_Client.TCPSocket.Receive(Receivebuf, Receivebuf.Length, SocketFlags.None);
                                tmpExamRefScore[i] = (UMyRecords.StExamRefScore)UMsgDefine.BytesToStruct(Receivebuf, typeof(UMyRecords.StExamRefScore));
                                RecCount--;
                                i++;
                            }
                            RecCount = tmpExamRefScore[0].RecCount;  //还原
                        }
                        
                        tmpPMyPaper.PaperImage = new System.IO.MemoryStream();
                        //接收试题图片  第一种为从文件服务器读取
                        if (tmpPMyPaper.PaperInfo.ImageLen == 0)
                        {
                            UMyFuncs.GetPicFromServer(DM_Client, tmpPMyPaper.PaperImage, tmpPMyPaper.PaperInfo.PicPath);
                            if (tmpPMyPaper.PaperImage.Length > 0)
                            {
                                isSuccess = true;
                                Content = "成功读取共享试卷图片" + new string(tmpPMyPaper.PaperInfo.PicPath);
                                UMyFuncs.WriteLogFile(DM_Client, Content);
                            }
                            else
                            {
                                Content = "读取共享试卷图片失败" + tmpPMyPaper.PaperInfo.PaperNo.ToString() + new string(tmpPMyPaper.PaperInfo.PicPath);
                                UMyFuncs.WriteLogFile(DM_Client, Content);
                                isSuccess = false;
                            }
                        }
                        else
                        {
                            RcvMS = new System.IO.MemoryStream();
                            RcvMS.Flush();
                            while (RcvMS.Length < tmpPMyPaper.PaperInfo.ImageLen)
                            {
                                //对网络中断的控制
                                len = DM_Client.TCPSocket.Receive(buf, 8192, SocketFlags.None);


                                RcvMS.Write(buf, 0, len);
                            }
                            if (RcvMS.Length == tmpPMyPaper.PaperInfo.ImageLen)
                            {
                                RcvMS.Position = 0;

                                tmpPMyPaper.PaperImage.Flush();
                                tmpPMyPaper.PaperImage = RcvMS; //?

                                isSuccess = true;


                                RcvMS.Flush();

                                Content = "成功读取试卷图片";
                                UMyFuncs.WriteLogFile(DM_Client, Content);
                            }
                            RcvMS.Flush();
                        }
                    }
                    else
                    {
                        switch (fmGetPaperRsp.RspCode)
                        {
                            case UConstDefine.RM_ERR_GETEXAMOVER:
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('所有样卷均已浏览完，点击下一张将重新开始!');</script>", false);
                                break;
                            case UConstDefine.RM_ERR_NOPAPER:
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('当前没有样卷!');</script>", false);
                                break;
                            case UConstDefine.RM_ERR_NOACTIVE:
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('该样卷所在的样卷分组并未被激活!');</script>", false);
                                break;
                            case UConstDefine.RM_ERR_NOSAMSCORE:
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('您尚未批改过该份样卷，请评阅完所有样卷后浏览样卷!');</script>", false);
                                break;
                            default:
                                ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取响应帧代码错误!');</script>", false);
                                break;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.ButtonRandomGetPaper, this.ButtonRandomGetPaper.GetType(), "msgAns", "<script>alert('获取响应帧类型错误!');</script>", false);
                }
            }
            if (isSuccess)
            {
                string tmp1 = new string(tmpExamRefScore[0].GroupName);
                tmp1 = tmp1.Substring(0, tmp1.IndexOf('\0'));
                if (tmp1.Equals("-1"))
                {
                    if (DM_Client.MyRecords.UserInfo.Status != UConstDefine.ZhengPing && DM_Client.MyRecords.UserInfo.Role != UConstDefine.XiaoZuZhang)
                    {
                        DropDownSampleGroup.Items.Clear();
                        DropDownSampleGroup.Items.Add("未分组样卷");
                        DropDownSampleGroup.SelectedValue = "未分组样卷";
                    }
                    else
                    {
                        DropDownSampleGroup.SelectedValue = "未分组样卷";
                    }
                }
                else
                {
                    string tmp2 = new string(tmpExamRefScore[0].TrainTarget);
                    tmp2 = tmp2.Substring(0, tmp2.IndexOf('\0'));
                    if (DM_Client.MyRecords.UserInfo.Status != UConstDefine.ZhengPing && DM_Client.MyRecords.UserInfo.Role != UConstDefine.XiaoZuZhang)
                    {
                        DropDownSampleGroup.Items.Clear();
                        DropDownSampleGroup.Items.Add(tmp1);
                        DropDownSampleGroup.SelectedValue = tmp1;
                    }
                    else
                    {
                        DropDownSampleGroup.SelectedValue = tmp1;
                    }
                    TextBoxAim.Text = tmp2;
                }
                UpdatePanelGroup.Update();
                string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                string picTemp = Server.MapPath(".");
                tmpPMyPaper.PaperImage.Position = 0;
                UMyFuncs.LoadJP2(DM_Client, tmpPMyPaper, picTemp); //no matter the format is the FreeImageNET can deals with
                imagesSample.ImageUrl = "~/" + tmpstr1 + "/" + tmpPMyPaper.PaperInfo.PaperNo.ToString() + ".bmp";

                if (DM_Client.MyRecords.UserInfo.Role <= UConstDefine.XiaoZuZhang)
                {
                    ButtonRandomGetPaper.Enabled = false;
                }

                //设置开始时间
                v = ((DateTime.Now - UnitGlobalV.javaTime).TotalDays + DM_Client.sysTimeDis) * 86400 - 8 * 60 * 60; //server

          //      DM_Client.MyVars.curPaper.StartTime = Math.Round(v);
            }
            DecodeInfoZJ(tmpExamRefScore[0], 2, 2);   //解析专家详细分数

            //若大组长已给分，则不能再给分

            if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
            {
                if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                {
                    DecodeInfoZJ_2(new string(DetailScoreforPT), 3, 3);  //修改了客户端定义
                }
                if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing)
                {
                    //普通评卷员正评状态下浏览样卷库时不需要获取自己的参考分
                }
                GridViewSampleScore.DataSource = Sampledt;
                GridViewSampleScore.DataBind();
                UpdateImage.Update();
                UpdateImage1.Update();
            }
            else//题组长和大组长浏览样卷库，能看到包含好几份成绩的待定的样卷
            {
            }
        }

        private void DecodeInfoZJ_2(string p, int ARow, int OperationType)
        {
            string tmpDetail, Points, s, TmpStr, tmpstring, tmps;
            int i, j, k, code;
            Single TmpScore;
            long tmp;
            
            // ScoreDetail:TStringList;
            DataRow dr = Sampledt.NewRow();
            tmp = DM_Client.MyRecords.UserInfo.UserID;
            tmpstring = tmp.ToString().Substring(0, 1);
            if (tmpstring.Equals("1"))    //大组长给分
            {
                dr["步骤"] = "本人给分";
            }
            
            dr["档次"] = "";
            dr["理由"] = "";
            switch (OperationType)
            {
                case 3:  //解析详细分数到样卷界面的专家详细给分StrGrd_ProScore
                    tmpDetail = p;
                    tmpDetail = tmpDetail.Substring(0,tmpDetail.IndexOf('\0'));
                    i = 1;
                    switch (DM_Client.MyVars.QuestionType)
                    {
                        case 1:
                        case 2:
                        case 3:
                            while (tmpDetail.Length > 0)
                            {
                                if (tmpDetail.IndexOf(',') > 0)
                                {
                                    dr[i] = tmpDetail.Substring(0, tmpDetail.IndexOf(','));
                                    tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf(',') + 1);
                                }
                                else
                                {
                                    if (tmpDetail.IndexOf('{') > 0)
                                    {
                                        tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf('{') + 1, tmpDetail.IndexOf('}') - tmpDetail.IndexOf('{') - 1);
                                    }
                                    while (tmpDetail.Length > 0)
                                    {
                                        if (tmpDetail.IndexOf('|') > 0)
                                        {
                                            dr[i] = tmpDetail.Substring(0, tmpDetail.IndexOf('|'));
                                            tmpDetail = tmpDetail.Substring(tmpDetail.IndexOf('|') + 1);
                                        }
                                        else
                                        {
                                            dr[i] = tmpDetail;
                                            tmpDetail = "";
                                        }
                                        i++;
                                    }
                                    tmpDetail = "";
                                    break;
                                }
                                i++;
                            }
                            Sampledt.Rows.Add(dr);
                            break;


                        case 4:
                        case 5: //TODO:
                            break;
                    }
                    break;
            }
        }
        protected void GridViewCheck_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                if (IsBlankRow(gvr))
                {
                    int nCount = gvr.Cells.Count;
                    for (int i = 0; i < nCount; i++)
                    {
                        TableCell tc = gvr.Cells[i];
                        foreach (Control c in tc.Controls)
                        {
                            DisableCtrl(c);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 判断是不是空行
        /// </summary>
        /// <returns></returns>
        private static bool IsBlankRow(GridViewRow gvr)
        {
            int nCount = gvr.Cells.Count;
            for (int i = 1; i < nCount - 1; i++)
            {
                string strText = gvr.Cells[i].Text;
                if (strText != "" && strText != "&nbsp;")
                    return false;
            }
            return true;
        }
        private static void DisableCtrl(Control c)
        {
            if (c is WebControl)
            {
                WebControl wc = (WebControl)c;
                if (wc is LinkButton)
                    wc.Enabled = false;
            }
        }


        protected void DropDownSampleGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i, j;
            string tmpstring, s;
            SGPaperNoByGrp = DM_Client.SGPaperNoByGrp;

            DataTable SamPaperNo = new DataTable();
            SamPaperNo.Columns.Add("试卷号");
            SamPaperNo.Columns.Add("状态");
            string tmp1;
            for (i = 0; i < SGPaperNoByGrp.Length; i++)
            {
                tmp1 = new string(SGPaperNoByGrp[i].GroupName);
                tmp1 = tmp1.Substring(0,tmp1.IndexOf('\0'));
                if (DropDownSampleGroup.SelectedValue.Equals(tmp1) || ((DropDownSampleGroup.SelectedValue == "未分组样卷") && tmp1.Equals("-1")))
                {

                    tmpstring = SGPaperNoByGrp[i].PaperNo;
                    while (tmpstring.IndexOf(',') > 0)
                    {
                        s = tmpstring.Substring(0,tmpstring.IndexOf(','));
                        SamPaperNo.Rows.Add(s, "已确认");
                        tmpstring = tmpstring.Substring(tmpstring.IndexOf(',') + 1);
                    }
                    if (tmpstring.Length != 0)
                    {
                        SamPaperNo.Rows.Add(tmpstring, "已确认");
                    }
                }
            }
            DM_Client.SamPaperNo = SamPaperNo;
            GridViewCheckSample.DataSource = SamPaperNo;
            GridViewCheckSample.DataBind();
            UpdatePanelGroup.Update();
            UpdatePanelCheck.Update();
            imagesSample.ImageUrl = "";
            UpdateImage.Update(); 
            UpdateImage1.Update();
            Session["DM_Client"] = DM_Client;
        }
    }
}