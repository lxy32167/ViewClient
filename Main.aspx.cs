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
using System.Web.UI.DataVisualization.Charting;
using System.Web.Services;


<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
using System.Drawing;
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master

namespace Client
{

    public partial class Main : System.Web.UI.Page
    {
        [DllImport("kernel32")]
        static extern uint GetTickCount();
        TDM_Client DM_Client;
        TBeaconThreadClass TBeaconClass;
        TUDPListenedClass TUDPClass;
        TRcvPaperThreadClass TRcvPaperClass;
        TSaveScoreThreadClass TSaveScoreClass;
        TPreFetchThreadClass TPreFetchClass;
        int Review_RowCount;
        UCheck Check = new UCheck();
     //   UnitGlobalV GlobalV;

        protected void Page_Load(object sender, EventArgs e)
        {
            form1.DefaultButton = "ButtonSubmit";
<<<<<<< HEAD
          //  this.Buttonzhuxiao.Attributes.Add("onclick", "return confirm('确定返回登陆界面?');");
=======
            this.Buttonzhuxiao.Attributes.Add("onclick", "return confirm('确定返回登陆界面?');");
>>>>>>> origin/master
            this.ButtonBlank.Attributes.Add("onclick", "return confirm('是否提交空白卷?');");
   
            DM_Client = (TDM_Client)Session["DM_Client"];
            TBeaconClass = (TBeaconThreadClass)Session["TBeaconClass"];
            TUDPClass = (TUDPListenedClass)Session["TUDPClass"];
          //  GlobalV = (UnitGlobalV)Session["GlobalV"];
            if (Session["DM_Client"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                LabelName.Text = "评卷员" + new String(DM_Client.MyRecords.UserInfo.TrueName) + "已登陆";
                switch (DM_Client.MyRecords.UserInfo.Status)
                {
                    case UConstDefine.Trial:
                        LabelStatus.Text = "状态:试用";
                        break;
                    case UConstDefine.YangPing:
                        LabelStatus.Text = "状态:样评";
                        break;
                    case UConstDefine.CePing:
                        LabelStatus.Text = "状态:测评";
                        break;
                    case UConstDefine.ZhengPing:
                        LabelStatus.Text = "状态:正评";
                        break;
                }
                ButtonStart_Click(); //为了便于表单动态绑定 设定为开始直接点击阅卷
                //第一次启动该函数，会启动其他线程并保存状态

                FirstCheckData();  //初始化Check
                ShowPerQtyFirst();
                RegExcpData();  //注册后台异常字符串到js
            }
            Check = (UCheck)Session["Check"];
            TSaveScoreClass = (TSaveScoreThreadClass)Session["TSaveScoreClass"];
            TRcvPaperClass = (TRcvPaperThreadClass)Session["TRcvPaperClass"];
            TPreFetchClass = (TPreFetchThreadClass)Session["TPreFetchClass"];
            if (getPostBackControlName() == "ButtonSubmit")
            {
                SaveScoreString(); ////在Postback绑定之前，获取上次输入数据
            }
            if (getPostBackControlName() == "ButtonBlank")
            {
                SaveZeroString(); 
            }
            LoadScoreData();   //原读取评分标准函数,decodeinfo移位 ,
            LockUserData();

            ((TextBox)(GridViewScore.Rows[0].FindControl("txtScore"))).Focus();

        }
        public void RegExcpData()
        {
            ClientScriptManager cs = Page.ClientScript;
            string tmp;
            for (int i = 0; i < DM_Client.MyRecords.ExcpRsnList.Count; i++)
            {
                tmp = new string(DM_Client.MyRecords.ExcpRsnList[i].Reason);
                tmp = tmp.Substring(0, tmp.IndexOf('\0'));
                cs.RegisterArrayDeclaration("arrExcp", "'" + tmp + "'");
            }
        }
        public void bindchart()
        {
            DataTable speed = new DataTable();
            speed.Columns.Add("Date", typeof(DateTime));
            speed.Columns.Add("个人", typeof(Single));
            speed.Columns.Add("题组", typeof(Single));

            DataView speedview = new DataView(speed);
            // Data bind to the reader
            Chart1.Series["个人"].Points.DataBindXY(speedview, "Date", speedview, "个人");
            Chart1.Series["题组"].Points.DataBindXY(speedview, "Date", speedview, "题组");

            DataTable avg = new DataTable();
            avg.Columns.Add("Date", typeof(DateTime));
            avg.Columns.Add("个人", typeof(Single));
            avg.Columns.Add("题组", typeof(Single));

            DataView avgview = new DataView(avg);
            // Data bind to the reader
            Chart2.Series["个人"].Points.DataBindXY(avgview, "Date", avgview, "个人");
            Chart2.Series["题组"].Points.DataBindXY(avgview, "Date", avgview, "题组");
            Session["speedview"] = speedview;
            Session["avgview"] = avgview;
        }
        public void FirstCheckData()
        {
            DataTable tbl = new DataTable("CheckScore");
            tbl.Columns.Add("序号", typeof(string));
            tbl.Columns.Add("试卷号", typeof(string));
            tbl.Columns.Add("分数", typeof(string));
            for (int i = 1; i <= DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; i++)
            {
                tbl.Rows.Add(i, "", "");
            }
            GridViewCheck.DataSource = tbl;
            GridViewCheck.DataBind();
        }
        public void RefreshShow(int Operation)
        {
            UMyRecords.stMyPaper PTmpPaper;
            int i, j, k;
            UMyRecords.stQueryResult PTmpResult;
            string Content;

            switch (Operation)
            {
                case 1:
                    DataTable tbl = new DataTable("CheckScore");
                    tbl.Columns.Add("序号", typeof(string));
                    tbl.Columns.Add("试卷号", typeof(string));
                    tbl.Columns.Add("分数", typeof(string));

                    Content = "复查区被清空，准备进入临界区CsOldPaper";
                    UMyFuncs.WriteLogFile(DM_Client, Content);
                    lock (DM_Client.CsOldPaper)
                    {
                        j = 1;
                        Content = "RefreshShow进入临界区CsOldPaper成功";
                        UMyFuncs.WriteLogFile(DM_Client, Content);
                        if (DM_Client.OldPaperList.Count > 0)
                        {
                            for (i = 0; i < DM_Client.OldPaperList.Count; i++)
                            {
                                PTmpPaper = DM_Client.OldPaperList[i];
                                if (PTmpPaper.Status)
                                {
                                    tbl.Rows.Add(j.ToString(), PTmpPaper.PaperInfo.PaperNo.ToString(), PTmpPaper.TotalScore.ToString());
                                    Content = "找到了可以复查的试卷" + PTmpPaper.PaperInfo.PaperNo.ToString();
                                    UMyFuncs.WriteLogFile(DM_Client, Content);
                                    j++;
                                }
                            }
                        }
                        for (k = j - 1; k < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; k++)
                        {
                            tbl.Rows.Add(j.ToString(), "", "");
                            j++;
                        }
                    }
                    GridViewCheck.DataSource = tbl;
                    GridViewCheck.DataBind();
                    UpdatePanelCheck.Update();
                    break;
            }
        }
        /// <summary>
        /// 给锁定窗口的解锁用户名和密码赋值 
        /// </summary>
        protected void LockUserData()
        {
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            string username, userpwd;
            username = tmpstr1;
            userpwd = (string)Session["Password"];
            hidden.Value = username;
            hidden1.Value = userpwd;

        }
        //protected void lButtonStart_Click(object sender, EventArgs e)
        //{
        //    mvCompany.ActiveViewIndex = 0;
        //    Cell1.Attributes["class"] = "SelectedTopBorder";
        //    Cell2.Attributes["class"] = "TopBorder";
        //    Cell3.Attributes["class"] = "TopBorder";
        //}
        //protected void lButtonControl_Click(object sender, EventArgs e)
        //{
        //    mvCompany.ActiveViewIndex = 1;
        //    Cell1.Attributes["class"] = "TopBorder";
        //    Cell2.Attributes["class"] = "SelectedTopBorder";
        //    Cell3.Attributes["class"] = "TopBorder";
        //}
        //protected void lButtonAbout_Click(object sender, EventArgs e)
        //{
        //    mvCompany.ActiveViewIndex = 2;
        //    Cell1.Attributes["class"] = "TopBorder";
        //    Cell2.Attributes["class"] = "TopBorder";
        //    Cell3.Attributes["class"] = "SelectedTopBorder";
        //}
        protected void lButtonSpeed_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            Td1.Attributes["class"] = "SelectedTopBorder";
            Td2.Attributes["class"] = "TopBorder";
            BindSpeedTab();
        }
        protected void lButtonAvg_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            Td1.Attributes["class"] = "TopBorder";
            Td2.Attributes["class"] = "SelectedTopBorder";
            BindAvgTab();
        }
        protected void ButtonStart_Click()  //第一次才起作用，往后是buttonsubmit->checkscore->aftergivescore->save->showimage循环
        {
            int Len, i;
            string tmpstr, ErrorMsg;
            UMyRecords.stMyPaper PAddPaper;
            UMyRecords.stFullBlockInfo BlkInfo;
            uint ExitCode;
            ErrorMsg = "";

            CheckShow();
            if (!DM_Client.MyVars.BlockOK)
            {
                DM_Client.MyRecords.BlockInfoList.Clear();
                if (DM_Client.MyRecords.AssignList.Count > 0)
                {
                    for (i = 0; i < DM_Client.MyRecords.AssignList.Count; i++)
                    {
                        tmpstr = DM_Client.MyRecords.AssignList[i];
                        Len = UMyFuncs.GetBlockInfo(DM_Client, 1, 0, tmpstr, ErrorMsg);
                        if (Len >= 0)
                        {
                            if (Len > 0)
                            {
                                UMyFuncs.GetBlockInfo(DM_Client, 2, Len, tmpstr, ErrorMsg);
                            }
                            DM_Client.MyVars.BlockOK = true;
                        }
                    }

                    if (DM_Client.MyVars.CurVolName.Equals(""))
                    {
                        UMyFuncs.GetNextVol(DM_Client);
                    }
                    if (DM_Client.MyVars.BlockOK)
                    {
                        //add at 5.21
                        DM_Client.MyRecords.SampleInfoList = new List<UMyRecords.StExamRefScore>();
                        switch (DM_Client.MyRecords.UserInfo.Status)
                        {
                            case UConstDefine.Trial:
                                LabelStatus.Text = "状态:试用";
                                break;
                            case UConstDefine.YangPing:
                                DM_Client.MyVars.CurPaperNum=DM_Client.MyRecords.UserInfo.SmpPaperTackled;
                                LabelStatus.Text = "状态:样评";
                                break;
                            case UConstDefine.CePing:
                                DM_Client.MyVars.CurPaperNum=DM_Client.MyRecords.UserInfo.TstPaperTackled;
                                LabelStatus.Text ="状态:测评";
                                break;
                            case UConstDefine.ZhengPing:
                                DM_Client.MyVars.CurPaperNum= -1;
                                LabelStatus.Text ="状态:正评";
                                break;
                        }

                        TRcvPaperClass = new TRcvPaperThreadClass(DM_Client,Check);
                        TSaveScoreClass = new TSaveScoreThreadClass(DM_Client);
                        TPreFetchClass = new TPreFetchThreadClass(DM_Client);
                        TPreFetchClass.picTemp = Server.MapPath(".");
                        if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                        {
                            TRcvPaperClass.TRcvPaperThread.Start();
                            TSaveScoreClass.TSaveScoreThread.Start();
                        }
                    }
                }

            }
            else
            {
            }
            Thread.Sleep(1000);

            //   hidden2.Value = GlobalV.WindowTime.ToString();

            ShowImage(0);
            Check.FirstQtyFlag = true;
            Check.status_review = false;
            lbTips.Text = "个人已阅/题组已阅";
            BlkInfo = DM_Client.MyRecords.BlockInfoList[0];
            //如果是多评题是多评题才显示个人有效阅卷数
            if (BlkInfo.BlockInfo.nNEv > 1)
            {
                lbTips.Text = "个人有效/" + lbTips.Text;
            }

            Session["TRcvPaperClass"] = TRcvPaperClass;
            Session["TSaveScoreClass"] = TSaveScoreClass;
            Session["TPreFetchClass"] = TPreFetchClass;
            Session["DM_Client"] = DM_Client;
            Session["Check"] = Check;
        }
        protected void CheckShow()
        {
            int i;
            UMyRecords.stExcpMap PTmpRsn;

            if (DM_Client.MyRecords.CurBlockInfo.BlockInfo.VolName.Length != 0)           //极少时候这里会报空字符串，不知道与Session生存期有无关系，待查 
                                                                                          //可能是相关字符串的处理\0之后还有字符
            {
                DM_Client.MyVars.QuestionType = DM_Client.MyRecords.CurBlockInfo.BlockInfo.QuestionType;
                Review_RowCount = DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize;

                if ((DM_Client.MyRecords.UserInfo.Role > UConstDefine.PuTong) && (DM_Client.MyRecords.CurBlockInfo.BlockInfo.DispScore == 1))
                {
                    DM_Client.MyVars.AllowDisplay = true;
                }
                else
                {
                    DM_Client.MyVars.AllowDisplay = false;
                }

                DM_Client.MyVars.CurFullScore = -1;    //初始化
                //      UMyFuncs.DecodeInfo(DM_Client, new string(DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckRule), 2);   //解析评分标准


                //查找当前题块信息并保存到PMyBlock中
                if (DM_Client.MyRecords.BlockInfoList.Count > 0)
                {
                    for (i = 0; i < DM_Client.MyRecords.BlockInfoList.Count; i++)
                    {
                        Check.PMyBlock = DM_Client.MyRecords.BlockInfoList[i];

                        if (new string(Check.PMyBlock.BlockInfo.VolName).TrimEnd('\0').Equals(DM_Client.MyVars.CurVolName))
                        {
                            break;
                        }
                        else
                        {
                            //PMyBlock = null;
                        }
                    }
                }

                //获取套红框信息（需控件支持）
                //初始信息形如 5},(1,1,97,15),(1,18,97,30),(..),(..),(..)
                //StringRect:=strpas(PMyBlock^.BlockInfo.MarkModelList);
                //RectanglNum:=strtoint(copy(StringRect,1,pos('}',StringRect)-1));
                //StringRect:=copy(StringRect,pos(',',StringRect)+1,length(StringRect));  //形如(1,1,97,15),(1,18,97,30),(..),(..),(..)
                //tmpStringRect:=StringRect;

                switch (DM_Client.MyRecords.UserInfo.Status)
                {
                    case UConstDefine.YangPing:
                        Check.TestMin = DM_Client.MyRecords.CurBlockInfo.BlockInfo.SampleTestMin;
                        LabelStatus.Text = "状态:样评";
                        //目前不支持个人转换阅卷状态
                        if (DM_Client.MyVars.CurPaperNum >= Check.TestMin)   //若用户本次登录以来所改试卷数>=样评最小份数
                        {
                        }
                        else
                        {
                        }
                        if (DM_Client.MyRecords.UserInfo.SmpPaperTackled >= Check.TestMin)
                        {
                        }
                        break;
                    case UConstDefine.CePing:
                        Check.TestMin = DM_Client.MyRecords.CurBlockInfo.BlockInfo.TotalTestMin;
                        LabelStatus.Text = "状态:测评";
                        //目前不支持个人转换阅卷状态
                        if (DM_Client.MyVars.CurPaperNum >= Check.TestMin)   //若用户本次登录以来所改试卷数>=样评最小份数
                        {
                        }
                        else
                        {
                        }
                        if (DM_Client.MyRecords.UserInfo.TstPaperTackled >= Check.TestMin)
                        {
                        }
                        break;
                    case UConstDefine.Trial:
                        LabelStatus.Text = "状态:试用";
                        break;
                    case UConstDefine.ZhengPing:
                        LabelStatus.Text = "状态:正评";
                        break;
                }
             //   Check.tempstat3 = LabelStatus.Text;
            }
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            string tmpstr2 = new String(DM_Client.MyRecords.UserInfo.TrueName);
            tmpstr2 = tmpstr2.Substring(0, tmpstr2.IndexOf('\0'));
            LabelLoginName.Text = tmpstr1 + " " + tmpstr2;
            UpdatePanelLoginName.Update();
      //      Check.tempstat5 = LabelLoginName.Text;

            Check.YiJu = true;
            Check.LastPaper = false;
            Check.SampleOver = true;


            LabelName.Text = "正常阅卷状态";
            UpdatePanelStatus.Update();
            //Check.tempstat1 = LabelName.Text;
            //Check.tempstat2 = LabelPaperNo.Text;
            //Check.tempstat3 = LabelStatus.Text;
            //Check.tempstat4 = LabelPaperNum.Text;
            //Check.tempstat5 = LabelLoginName.Text;

            if (true) //不可自动判分
            {
                if (DM_Client.MyRecords.ExcpRsnList.Count > 0)
                {
                    for (i = 0; i <= 30; i++)
                        DM_Client.MyRecords.ArrRsnCode[i] = -1;     //赋予初值

                    for (i = 0; i < DM_Client.MyRecords.ExcpRsnList.Count; i++)
                    {
                        PTmpRsn = DM_Client.MyRecords.ExcpRsnList[i];
                        DM_Client.MyRecords.ArrRsnCode[i] = PTmpRsn.ReasonCode;
                    }

                    DM_Client.MyRecords.ArrRsnCode[i] = UConstDefine.SelfExcpCode;
                }
                else
                {
                    DM_Client.MyRecords.ArrRsnCode[0] = UConstDefine.SelfExcpCode;
                }
            }
        }
        protected void LoadScoreData()
        {
            String s, tmpStr, tmpDetail, Points;
            UMyRecords.stMyPaper PMyPaper;
            int i, j, k, code, row;
            Single TmpScore;
            string[] ScoreDetail;
            Boolean FirstFlag;
            DataTable tbl = new DataTable("InputScore");
            if (!IsPostBack) //第一次
            {
                s = new string(DM_Client.MyRecords.CurBlockInfo.BlockInfo.CheckRule);
                FirstFlag = true;
                DM_Client.MyVars.PositiveEndRow = -1;
                tmpStr = s;
                tmpStr = tmpStr.Substring(0, tmpStr.IndexOf('\0'));
                tmpStr = tmpStr.Substring(tmpStr.IndexOf('('));
                DM_Client.MyVars.CurFullScore = 0;
                switch (DM_Client.MyVars.QuestionType)
                {
                    case 1:
                    case 2:
                    case 3: //测试库都是3
                        tbl.Columns.Add("步骤", typeof(string));
                        tbl.Columns.Add("分数", typeof(string));
                        tbl.Columns.Add("满分", typeof(string));
                        tbl.Columns.Add("允许给半分", typeof(string));
                        j = 1;
                        
                        while (tmpStr.IndexOf('(') >= 0)
                        {
                            Points = "";
                            Points = tmpStr.Substring(tmpStr.IndexOf('(') + 1, tmpStr.IndexOf(')') - tmpStr.IndexOf('(') - 1);
                            ScoreDetail = Points.Split(',');
                            if (ScoreDetail.Length != 3)
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel_submit, this.UpdatePanel_submit.GetType(), "msg", "<script>alert('解析得分要点错误');</script>", false);

                            }
                            if (!UMyFuncs.IsNum(ScoreDetail[1]))
                            {
                                ScriptManager.RegisterStartupScript(this.UpdatePanel_submit, this.UpdatePanel_submit.GetType(), "msg", "<script>alert('得分要点非法字符');</script>", false);
                            }
                            TmpScore = (float)Microsoft.VisualBasic.Conversion.Val(ScoreDetail[1]);
                            if (TmpScore > 0) //正分给分
                            {
                                DM_Client.MyVars.CurFullScore = DM_Client.MyVars.CurFullScore + TmpScore;
                            }
                            else //负分给分点
                            {
                                if (FirstFlag) //第一次出现负分
                                {
                                    tbl.Rows.Add("负分给分区", "", "");
                                    FirstFlag = false;
                                    DM_Client.MyVars.PositiveEndRow = j - 1;
                                    j++;
                                }
                            }
                            tbl.Rows.Add(ScoreDetail[0], "", ScoreDetail[1], ScoreDetail[2]);
                            tmpStr = tmpStr.Substring(tmpStr.IndexOf(')') + 2);
                            j++;
                        }
                        if (tmpStr.Substring(0, 1).Equals("Y"))
                        {
                            DM_Client.MyVars.AllowMinus = true;
                        }
                        else
                        {
                            DM_Client.MyVars.AllowMinus = false;
                        }
                        if (DM_Client.MyVars.PositiveEndRow == -1)
                        {
                            DM_Client.MyVars.PositiveEndRow = j - 1;
                        }
                        DM_Client.tblScore = tbl;
                        DM_Client.stCheck = new DataModule.stCheck[DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize];
                        for (int n = 0; n < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; n++)
                        {
                            DM_Client.stCheck[n].PaperNo = -1;
                            DM_Client.stCheck[n].DetailScore = new string[DM_Client.tblScore.Rows.Count];  //初值
                        }
                        Check.stCheckTemp = new DataModule.stCheck[DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize];
                        for (int n = 0; n < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; n++)
                        {
                            Check.stCheckTemp[n].PaperNo = -1;
                            Check.stCheckTemp[n].DetailScore = new string[DM_Client.tblScore.Rows.Count];  //初值
                        }
                        Session["DM_Client"] = DM_Client;  //第一次，更新session
                        Session["Check"] = Check;
                        break;
                    case 4:
                    case 5:  //测试库无数据 思路，做两个gridview 显示和隐藏
                        //for(j=0;j<UConstDefine.LvlCol;j++)
                        //{
                        //    for(k = 0;k<UConstDefine.LvlRow;k++)
                        //    {
                        //        DM_Client.MyRecords.ArrScoreLevel[j,k] = -1;
                        //    }
                        //}
                        //tbl.Columns.Add("步骤", typeof(string));
                        //tbl.Columns.Add("等级", typeof(string));
                        //tbl.Columns.Add("得分", typeof(string));
                        //tbl.Columns.Add("分数区间", typeof(string));
                        break;

                }

            }
            else              //直接读取session中的datatable
            {
                //for (int m = 0; m < DM_Client.tblScore.Rows.Count; m++)
                //{
                //    for (int n = 0; n < DM_Client.tblScore.Columns.Count; n++)
                //    {
                //        string temp = DM_Client.tblScore.Rows[m][n].ToString();
                //        temp = temp;
                //    }
                //}
                tbl = DM_Client.tblScore;
            }
            GridViewScore.DataSource = tbl;
            GridViewScore.DataBind();
        }
        protected void ShowGist(object sender, EventArgs e)
        {
        }
        protected void ShowImage(int OperationType, int ReviewRow = -1) //可选参数
        {
            UMyRecords.stMyPaper PTmpPaper, PAddPaper, PNewPaper;
            int i;
            Boolean ToChange;
            double v;
            UMyRecords.stMyUser PTmpUser;
            int b, a;
            string FileName;

            UMyRecords.stPaperInfo tmp;
            MemoryStream tmpPaper;
            UMyRecords.StExamRefScore PExamRefScore;
            uint t1, t2, t3, t4;
            int RcvResult;
            string Content;

            Boolean flag = false;
            Boolean getFlag = false;
            ToChange = false;
            PTmpPaper = new UMyRecords.stMyPaper();
            switch (OperationType)
            {
                case 0: //显示新的试卷——正常试卷
                    //先查看OldPaperList中最后一个元素是否为未改试卷（对于试卷复查后）
                    lock (DM_Client.CsOldPaper)
                    {
                        b = DM_Client.OldPaperList.Count;
                        if (DM_Client.OldPaperList.Count > 0)
                        {
                            PTmpPaper = DM_Client.OldPaperList.Last();
                            if (PTmpPaper.Status == true)
                            {
                                PTmpPaper = new UMyRecords.stMyPaper();
                                flag = true;
                            }
                        }
                        else
                        {
                            PTmpPaper = new UMyRecords.stMyPaper();
                            flag = true;
                        }
                        //从NewPaperList中取出一个元素
                        //OldPaperList中无元素或者其中最后一个元素已给分

                        if (flag)
                        {
                            lock (DM_Client.CsNewPaper)
                            {
                                if (DM_Client.NewPaperList.Count > 0)
                                {
                                    for (i = 0; i < DM_Client.NewPaperList.Count; i++)
                                    {
                                        PTmpPaper = DM_Client.NewPaperList[i];
                                        if (PTmpPaper.Flag)
                                        {
                                            //当前为有效试卷（该结构中保存有试卷）
                                            PTmpPaper.Flag = false; //该试卷未复查
                                            PTmpPaper.Status = false;//该试卷未打分

                                            DM_Client.NewPaperList.RemoveAt(i);
                                            DM_Client.OldPaperList.Add(PTmpPaper);
                                            getFlag = true;
                                            a = DM_Client.OldPaperList.Count;

                                            if (a == b)
                                            {
                                                Thread.Sleep(50);
                                            }
                                            DM_Client.MyVars.CurPaperNum++;
                                            //显示已改试卷数
                                            switch (DM_Client.MyRecords.UserInfo.Status)
                                            {
                                                case UConstDefine.Trial:
                                                case UConstDefine.ZhengPing:
                                                    LabelPaperNum.Text = "本次阅卷数:" + DM_Client.MyVars.CurPaperNum.ToString();
                                                    UpdatePanelPaperNum.Update();
                                           //         Check.tempstat4 = LabelPaperNum.Text;
                                                    break;
                                                case UConstDefine.YangPing:
                                                    LabelPaperNum.Text = DM_Client.MyRecords.UserInfo.SmpPaperTackled.ToString() + "/" + DM_Client.MyVars.CurPaperNum.ToString();
                                                    UpdatePanelPaperNum.Update();
                                           //         Check.tempstat4 = LabelPaperNum.Text;
                                                    if (DM_Client.MyVars.CurPaperNum >= Check.TestMin)
                                                    {

                                                    }
                                                    break;
                                                case UConstDefine.CePing:
                                                    LabelPaperNum.Text = DM_Client.MyRecords.UserInfo.SmpPaperTackled.ToString() + "/" + DM_Client.MyVars.CurPaperNum.ToString();
                                                    UpdatePanelPaperNum.Update();
                                            //        Check.tempstat4 = LabelPaperNum.Text;
                                                    if (DM_Client.MyVars.CurPaperNum >= Check.TestMin)
                                                    {

                                                    }
                                                    break;
                                            }
                                            break;
                                        }
                                        else//当前试卷不是有效试卷（该结构中未保存有试题）
                                        {
                                            PTmpPaper = new UMyRecords.stMyPaper();
                                            if (TRcvPaperClass.RcvResult != UConstDefine.RM_RSP_OK && TRcvPaperClass.RcvResult != UConstDefine.RM_ERR_WAIT)
                                            {
                                                RcvResult = TRcvPaperClass.RcvResult;
                                                if (TRcvPaperClass.TRcvPaperThread.ThreadState != ThreadState.Suspended)
                                                {
                                                    TRcvPaperClass.TRcvPaperThread.Suspend();
                                                }
                                                TRcvPaperClass.TRcvPaperThread.Resume();
                                                SaveOne();
                                                getFlag = false;
                                                switch (RcvResult)
                                                {
                                                    case UConstDefine.RM_ERR_ROLE:
                                                        ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('当前没有需要评阅的试卷');</script>", false);
                                                        break;
                                                    case UConstDefine.RM_ERR_USER:
                                                    case UConstDefine.RM_ERR_ERR:
                                                    case UConstDefine.RM_ERR_PAPERTYPE:
                                                    case UConstDefine.RM_ERR_BLOCK:
                                                        ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('取题过程发生错误');</script>", false);
                                                        break;
                                                    case UConstDefine.RM_ERR_NOTEXIST:
                                                        SaveOne();
                                                        ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('获取共享图片失败');</script>", false);
                                                        break;
                                                    case UConstDefine.RM_ERR_NOBLK:
                                                    case UConstDefine.RM_ERR_NOPAPER:
                                                    case UConstDefine.RM_ERR_GETEXAMOVER:
                                                        switch (DM_Client.MyRecords.UserInfo.Status)
                                                        {
                                                            case UConstDefine.YangPing:
                                                                ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('当前没有可用的样评试题!');</script>", false);
                                                                Check.SampleOver = true;
                                                                break;
                                                            case UConstDefine.CePing:
                                                                ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('当前没有可用的测评试题!');</script>", false);
                                                                break;
                                                            default:
                                                                if (DM_Client.MyRecords.UserInfo.Role < UConstDefine.XiaoZuZhang)
                                                                {
                                                                    ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('本块试题已全部取完，请注销或退出系统!');</script>", false);
                                                                    if (TRcvPaperClass.TRcvPaperThread.ThreadState != ThreadState.Suspended)
                                                                    {
                                                                        TRcvPaperClass.TRcvPaperThread.Suspend();
                                                                    }
                                                                    TRcvPaperClass.TRcvPaperThread.Resume();
                                                                    break; //TODO:
                                                                }
                                                                else
                                                                {

                                                                    break; //TODO:
                                                                }

                                                        }
                                                        break;

                                                }
                                            }
                                            
                                        }
                                        if (i == DM_Client.NewPaperList.Count - 1 && Check.LastPaper == true)
                                        {
                                            Check.SampleOver = true;
                                            ScriptManager.RegisterStartupScript(this.ButtonSubmit, this.ButtonSubmit.GetType(), "msgAns", "<script>alert('所有样卷均已评阅完,请点击“样卷库浏览”按钮查看专家给分!');</script>", false);
                                            return;
                                        }

                                    }
                                }

                            }
                        }
                        break;
                    }
                case 1:  //check
                    lock (DM_Client.CsOldPaper)
                    {
                        if (ReviewRow + 1 <= DM_Client.OldPaperList.Count)
                        {
                            PTmpPaper = DM_Client.OldPaperList[ReviewRow];
                            if (PTmpPaper.Status)
                            {
                                getFlag = true;
                                PTmpPaper.Flag = true;
                            }
                        }
                    }
                    break;
            }
            if (getFlag)
            {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                //初始化并装载试卷图片
                PTmpPaper.PaperImage.Position = 0;
                switch (OperationType)
                {
                    case 0:
                    case 2:
                        lock (DM_Client.CsNextPaper)
<<<<<<< HEAD
=======
=======
                case 0:
                case 2:
                    lock (DM_Client.CsNextPaper)
                    {
                        string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                        tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                        string picTemp = Server.MapPath(".");
                        if (DM_Client.NextPaper == true && (PTmpPaper.PaperInfo.PaperNo == DM_Client.NextPaperNo))
                        {
                            images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
                        }
                        else
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
                        {
                            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                            string picTemp = Server.MapPath(".");
                            if (DM_Client.NextPaper == true && (PTmpPaper.PaperInfo.PaperNo == DM_Client.NextPaperNo))
                            {
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
                                UMyFuncs.LoadJP2(DM_Client, PTmpPaper, picTemp); //no matter the format is the FreeImageNET can deals with
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
                                images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
                            }
                            else
                            {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                                if (PTmpPaper.PaperInfo.ImageFormat == UConstDefine.JP2)
                                {
                                    UMyFuncs.LoadJP2(DM_Client, PTmpPaper, picTemp); //no matter the format is the FreeImageNET can deals with
                                    images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
                                }
                                else
                                {
                                    UMyFuncs.LoadJP2(DM_Client, PTmpPaper, picTemp);
                                    images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
                                }
<<<<<<< HEAD
=======
=======
                                UMyFuncs.LoadJP2(DM_Client, PTmpPaper, picTemp);
                                images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
                            }
                        }
                        break;
                    case 1: //check
                        lock (DM_Client.CsPreviewPaper)
                        {
                            Boolean exist = false;
                            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                            if (File.Exists(Server.MapPath("~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp")))
                            {
                                exist = true;
                            }
                            if ((PTmpPaper.PaperInfo.ImageFormat == UConstDefine.JP2) && exist)
                            {
                                images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
<<<<<<< HEAD
                                
=======
>>>>>>> origin/master
                            }
                        }
                        break;
                }

                //设置开始时间
                v = ((DateTime.Now - UnitGlobalV.javaTime).TotalDays + DM_Client.sysTimeDis) * 86400 - 8 * 60 * 60; //server
                if (OperationType == 1)
                {
                    PTmpPaper.TimeStamp = v + PTmpPaper.TimeStamp - PTmpPaper.StartTime;
                    PTmpPaper.StartTime = v;
                }
                else
                {
                    PTmpPaper.StartTime = v;              //这两个变量不能存到oldlist中 不解
                }
                PTmpPaper.RStartTime = GetTickCount();
                //样评处理
                if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                {
                    for (i = 0; i < DM_Client.MyRecords.SampleInfoList.Count; i++)
                    {
                        PExamRefScore = DM_Client.MyRecords.SampleInfoList[i];
                        if (PExamRefScore.PaperNo == PTmpPaper.PaperInfo.PaperNo)
                        {
<<<<<<< HEAD
                            DM_Client.MyRecords.SampleInfoList.RemoveAt(i);
                            break;
                        }
                    }
                }

                if ((DM_Client.MyRecords.UserInfo.Interceder > 0) && ((DM_Client.MyRecords.UserInfo.Status == UConstDefine.Trial) || (DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing)))
=======
<<<<<<< HEAD
                            DM_Client.MyRecords.SampleInfoList.RemoveAt(i);
                            break;
                        }
                    }
                }

                if ((DM_Client.MyRecords.UserInfo.Interceder > 0) && ((DM_Client.MyRecords.UserInfo.Status == UConstDefine.Trial) || (DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing)))
=======
                            images1.ImageUrl = "~/" + tmpstr1 + "/" + PTmpPaper.PaperInfo.PaperNo.ToString() + ".bmp";
                        }
                    }
                    break;
            }

            //设置开始时间
            v = ((DateTime.Now - UnitGlobalV.javaTime).TotalDays + DM_Client.sysTimeDis) * 86400 - 8 * 60 * 60; //server
            if (OperationType == 1)
            {
                PTmpPaper.TimeStamp = v + PTmpPaper.TimeStamp - PTmpPaper.StartTime;
                PTmpPaper.StartTime = v;
            }
            else
            {
                PTmpPaper.StartTime = v;              //这两个变量不能存到oldlist中 不解
            }
            PTmpPaper.RStartTime = GetTickCount();
            //样评处理
            if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
            {
                for (i = 0; i < DM_Client.MyRecords.SampleInfoList.Count; i++)
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
                {
                    //如果用户没有仲裁权且处于试用或者正评状态下，可以提交异常试卷
                }
                //显示当前试卷的试卷号
                DM_Client.MyVars.CurPaperID = PTmpPaper.PaperInfo.PaperNo;

                LabelPaperNo.Text = "试卷号:" + DM_Client.MyVars.CurPaperID.ToString();
                UpdatePanelPaperNo.Update();
           //     Check.tempstat2 = LabelPaperNo.Text;
                //对当前试卷，设置存分类型
                switch (PTmpPaper.PaperInfo.PaperType)
                {
                    case UConstDefine.PM_PAPERTYPE_CEPING:
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_CEPING;
<<<<<<< HEAD
                        break;
                    case UConstDefine.PM_PAPERTYPE_CONFLICT:
                    case UConstDefine.PM_PAPERTYPE_NORMAL: //仲裁试卷、正评试题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_NORMAL;
                        break;
                    case UConstDefine.PM_PAPERTYPE_FREESEL://抽调试卷
                        PTmpPaper.SaveType = UConstDefine.PM_SACESCORE_CHECKED; //已核查（零分或者满分）
                        break;
                    case UConstDefine.PM_PAPERTYPE_RESAMPLE: //样卷重评题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_RESAMPLE;
                        break;
                    case UConstDefine.PM_PAPERTYPE_SAMPLE://样卷试题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SAMPLE;
                        //说明样卷尚未改完
                        Check.SampleOver = false;
                        break;
                    case UConstDefine.PM_PAPERTYPE_SELF:
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SELF;
                        break;
                    case UConstDefine.PM_PAPERTYPE_TRIAL:
                        PTmpPaper.SaveType = -1;
                        break;
                }
                if (OperationType != 1)              //这里如果不加判断，会出现查卷时出现一张重复试卷的情况
=======
                        break;
                    case UConstDefine.PM_PAPERTYPE_CONFLICT:
                    case UConstDefine.PM_PAPERTYPE_NORMAL: //仲裁试卷、正评试题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_NORMAL;
                        break;
                    case UConstDefine.PM_PAPERTYPE_FREESEL://抽调试卷
                        PTmpPaper.SaveType = UConstDefine.PM_SACESCORE_CHECKED; //已核查（零分或者满分）
                        break;
                    case UConstDefine.PM_PAPERTYPE_RESAMPLE: //样卷重评题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_RESAMPLE;
                        break;
                    case UConstDefine.PM_PAPERTYPE_SAMPLE://样卷试题
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SAMPLE;
                        //说明样卷尚未改完
                        Check.SampleOver = false;
                        break;
                    case UConstDefine.PM_PAPERTYPE_SELF:
                        PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SELF;
                        break;
                    case UConstDefine.PM_PAPERTYPE_TRIAL:
                        PTmpPaper.SaveType = -1;
                        break;
                }
                if (OperationType != 1)
>>>>>>> origin/master
                {
                    DM_Client.OldPaperList[DM_Client.OldPaperList.Count - 1] = PTmpPaper; // 不是引用 重新复制
                }
                else
                {
                    DM_Client.OldPaperList[ReviewRow] = PTmpPaper;
                }
            }
            

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
            if ((DM_Client.MyRecords.UserInfo.Interceder > 0) && ((DM_Client.MyRecords.UserInfo.Status == UConstDefine.Trial) || (DM_Client.MyRecords.UserInfo.Status == UConstDefine.ZhengPing)))
            {
                //如果用户没有仲裁权且处于试用或者正评状态下，可以提交异常试卷
            }
            //显示当前试卷的试卷号
            DM_Client.MyVars.CurPaperID = PTmpPaper.PaperInfo.PaperNo;

            LabelPaperNo.Text = "试卷号:" + DM_Client.MyVars.CurPaperID.ToString();
            UpdatePanelPaperNo.Update();
            Check.tempstat2 = LabelPaperNo.Text;
            //对当前试卷，设置存分类型
            switch (PTmpPaper.PaperInfo.PaperType)
            {
                case UConstDefine.PM_PAPERTYPE_CEPING:
                    PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_CEPING;
                    break;
                case UConstDefine.PM_PAPERTYPE_CONFLICT:
                case UConstDefine.PM_PAPERTYPE_NORMAL: //仲裁试卷、正评试题
                    PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_NORMAL;
                    break;
                case UConstDefine.PM_PAPERTYPE_FREESEL://抽调试卷
                    PTmpPaper.SaveType = UConstDefine.PM_SACESCORE_CHECKED; //已核查（零分或者满分）
                    break;
                case UConstDefine.PM_PAPERTYPE_RESAMPLE: //样卷重评题
                    PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_RESAMPLE;
                    break;
                case UConstDefine.PM_PAPERTYPE_SAMPLE://样卷试题
                    PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SAMPLE;
                    //说明样卷尚未改完
                    Check.SampleOver = false;
                    break;
                case UConstDefine.PM_PAPERTYPE_SELF:
                    PTmpPaper.SaveType = UConstDefine.PM_SAVESCORE_SELF;
                    break;
                case UConstDefine.PM_PAPERTYPE_TRIAL:
                    PTmpPaper.SaveType = -1;
                    break;
            }
            if (OperationType != 1)
            {
                DM_Client.OldPaperList[DM_Client.OldPaperList.Count - 1] = PTmpPaper; // 不是引用 重新复制
            }
            else
            {
                DM_Client.OldPaperList[ReviewRow] = PTmpPaper;
            }

>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
            //已改试卷数超过了设定的缓冲值，将OldPaperList队首元素移出->SaveScoreList
            lock (DM_Client.CsOldPaper)
            {
                if (DM_Client.OldPaperList.Count - 1 > DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize)
                {
                    PAddPaper = DM_Client.OldPaperList.First();
                    if (PAddPaper.Status)//当前试卷已经打分
                    {
                        lock (DM_Client.CsSavePaper)
                        {
                            DM_Client.OldPaperList.RemoveAt(0);
                            DM_Client.SaveScoreList.Add(PAddPaper);
                        }
                    }
                    else//当前试卷尚未打分，直接放入JunkList中
                    {
                        lock (DM_Client.JunkList)
                        {
                            DM_Client.OldPaperList.RemoveAt(0);
                            DM_Client.JunkList.Add(PAddPaper);
                        }
                    }
                }
            }
            RefreshShow(1);

            if (!getFlag)
            {
                return;
            }

            if (PTmpPaper.PaperInfo.ImageFormat != UConstDefine.JP2)
            {
                return;
            }
           
            TPreFetchClass.FetchType = OperationType;
            TPreFetchClass.index = -1;
            lock (DM_Client.CsNewPaper)
            {
                if (OperationType == 0)
                {
                    for (i = 0; i < DM_Client.NewPaperList.Count; i++)
                    {
                        PNewPaper = DM_Client.NewPaperList[i];
                        if (PNewPaper.Flag)
                        {
                            TPreFetchClass.index = i;
                            break;
                        }
                    }
                }
            }
            if (TPreFetchClass.TPreFetchThread.ThreadState == ThreadState.Unstarted)
                TPreFetchClass.TPreFetchThread.Start();

            if (TPreFetchClass.TPreFetchThread.ThreadState == ThreadState.Suspended)
            {
                TPreFetchClass.TPreFetchThread.Resume();
            }
            Session["Check"] = Check;
            Session["DM_Client"] = DM_Client;
            Session["TPreFetchClass"] = TPreFetchClass;
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                Check.ScoreInputOK = CheckScore();
                if (Check.ScoreInputOK)
                {
                    AfterGiveScore(Check.gDetailScore, Check.gTotalScore);

                }

            }
            else
            {

            }
        }
        public void AfterGiveScore(string gDetailScore, float gTotalScore)
        {
            UMyRecords.stMyPaper PTmp = new UMyRecords.stMyPaper();
            int i = 0;
            double v, v1, v2;
            Boolean ImageFlag;
            int h1, h2;
            Boolean flag = false;
            lock (DM_Client.CsOldPaper)
            {
                if (DM_Client.OldPaperList.Count > 0)
                {
                    for (i = DM_Client.OldPaperList.Count - 1; i >= 0; i--)
                    {
                        PTmp = DM_Client.OldPaperList[i];
                        if (PTmp.PaperInfo.PaperNo == DM_Client.MyVars.CurPaperID)
                        {
                            flag = true;
                            break;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
                if (flag)  //在OldPaperList中找到了当前评阅试卷
                {
                    //填写给分时间，阅卷时长=给分时间-试卷显示时间
                    v = ((DateTime.Now - UnitGlobalV.javaTime).TotalDays + DM_Client.sysTimeDis) * 86400 - 8 * 60 * 60;
                    PTmp.TimeStamp = v;
                    PTmp.REndTime = GetTickCount();
                    DateTime s = new DateTime(1899,12,30);
                    s.AddSeconds(v);
                    UMyFuncs.WriteLogFile(DM_Client, s.ToString("yyyyMMdd-HH:mm:ss"));
                    v1 = PTmp.RStartTime;
                    v2 = PTmp.TimeStamp;

                    //根据个人质量信息判断是否给趋中分数和阅卷速度过快


                    //对于正在评阅的正常试卷，检查是否符合速度限制要求

                    //保存已有分数
                    PTmp.TotalScore = gTotalScore;
                    PTmp.DetailScore = gDetailScore;
                    PTmp.Status = true; //已给分 

                    DM_Client.OldPaperList[i] = PTmp; //saveback

                    if (PTmp.PaperInfo.ImageFormat == UConstDefine.JP2 && (!Check.status_review))
                    {
                        SavePicture(PTmp.PaperInfo.PaperNo);//保存当前试卷图片到已改试卷图片数组
                    }
                    if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                    {
                    }
                    else
                    {
                        RefreshShow(1);//显示更新复查试卷
                    }
                }

            }

            if (Check.status_review == true)
            {
                ExitReview();
            }
            if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                ShowImage(0);
            Session["DM_Client"] = DM_Client; //只需要保存这个
        }
        public void ExitReview()
        {
            LabelName.Text = "正常阅卷状态";
            UpdateLabelName.Update();
            Check.status_review = false;
            Session["Check"] = Check;
        }
        public void SavePicture(int PaperNo)  //保存已改试卷，处理多余试卷
        {
            int i;
            Boolean flag = false;
            lock (DM_Client.CsPreviewPaper)
            {
                for (i = 0; i < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; i++)
                {
                    if (Check.previewpaper[i] == -1)
                    {
                        Check.previewpaper[i] = PaperNo;
                        flag = true;
                        break;
                    }
                }
                if (flag == false)      //存满，替换最早一张
                {
                    string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
                    tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
                    if (File.Exists(Server.MapPath("~/" + tmpstr1 + "/" + Check.previewpaper[0].ToString() + ".bmp")))
                    {
                        File.Delete(Server.MapPath("~/" + tmpstr1 + "/" + Check.previewpaper[0].ToString() + ".bmp"));
                    }
                    for (i = 0; i < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize - 1; i++)
                    {
                        Check.previewpaper[i] = Check.previewpaper[i + 1];
                    }
                    Check.previewpaper[i] = PaperNo;
                }
            }
            Session["Check"] = Check;
        }
        public void SaveOne()
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
            Session["DM_Client"] = DM_Client;
        }
        public bool CheckScore()
        {
            //需要重新检查分数吗?

            //整个大题分数输入完后，根据已给分数为gDetailScore和gTotalScore赋值，并清空分数输入框
            int i, j, rate;
            Single Max, min, Score, CanGiveScore, ParamScore;
            Boolean ScoreIsVaild;
            int Code, MinusStart;
            int p;
            Boolean MinusFlag;
            string ScoreString, FullScoreString;
            Check.gDetailScore = "";
            Check.gTotalScore = 0;
            MinusStart = -1;
            p = 0;
            //给分普通情况，不考虑等级
            i = 1;
            foreach (GridViewRow gvrow in GridViewScore.Rows)
            {
                if (i == DM_Client.MyVars.PositiveEndRow + 1)
                {
                    i++;
                    continue;
                }
                ScoreString = Check.ScoreInput[i - 1];
                FullScoreString = DM_Client.tblScore.Rows[i - 1][2].ToString();
                Score = float.Parse(ScoreString);
                p = Check.gDetailScore.IndexOf('{');
                if (FullScoreString.IndexOf('-') >= 0)
                {
                    MinusFlag = true;
                }
                else
                {
                    MinusFlag = false;
                }
                if (!MinusFlag)
                {
                    Check.gDetailScore = Check.gDetailScore + ScoreString;
                    if (i < GridViewScore.Rows.Count)
                    {
                        Check.gDetailScore = Check.gDetailScore + ",";
                    }
                }
                else
                {
                    if (MinusStart < 0)
                    {
                        MinusStart = i;
                    }
                    if (p < 0) //如果还没有到负分给分点
                    {
                        Check.gDetailScore = Check.gDetailScore + "{";
                    }
                    Check.gDetailScore = Check.gDetailScore + ScoreString;
                    if (i < GridViewScore.Rows.Count)
                    {
                        Check.gDetailScore = Check.gDetailScore + "|";
                    }
                    else
                    {
                        Check.gDetailScore = Check.gDetailScore + "}";
                    }
                }
                Check.gTotalScore = Check.gTotalScore + Score;
                i++;
            }
            return true;
        }
        public void SaveScoreString()
        {
            int i, j, k, m, n;
            UMyRecords.stMyPaper PTmpPaper;
            Boolean flag = false;
            Check.ScoreInput = new string[DM_Client.tblScore.Rows.Count];
            j = 0;
            Check.stCheckTemp = (DataModule.stCheck[])UMsgDefine.Clone(DM_Client.stCheck);
            if (!Check.status_review) //正常阅卷
            {
                for (j = 0; j < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; j++)
                {
                    if (DM_Client.stCheck[j].PaperNo == -1)       //如果还有空位
                    {
                        flag = true;
                        DM_Client.stCheck[j].PaperNo = DM_Client.MyVars.CurPaperID; //记录当前试卷号
                        break;
                    }
                }
                if (!flag)                   //没有空位
                {
                    j = DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize - 1;    //复查列表没找到，替换掉最后一个数据
                    DM_Client.stCheck[j].PaperNo = DM_Client.MyVars.CurPaperID;
                    for (m = 0; m < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize - 1; m++)   //分数前移
                    {
                        for (n = 0; n < DM_Client.tblScore.Rows.Count; n++)
                        {
                            DM_Client.stCheck[m].DetailScore[n] = DM_Client.stCheck[m + 1].DetailScore[n];
                        }
                        DM_Client.stCheck[m].PaperNo = DM_Client.stCheck[m + 1].PaperNo;
                    }
                }

            }
            else                      //复查状态,肯定会找到对应试卷
            {
                k = 0;
                if (DM_Client.OldPaperList.Count > 0)
                {
                    for (i = 0; i < DM_Client.OldPaperList.Count; i++)
                    {
                        PTmpPaper = DM_Client.OldPaperList[i];
                        if (PTmpPaper.Status)                 //复查列表寻找
                        {
                            if (PTmpPaper.PaperInfo.PaperNo == DM_Client.MyVars.CurPaperID)  //找到试卷
                            {
                                j = k;       //赋值并退出循环
                                DM_Client.stCheck[j].PaperNo = PTmpPaper.PaperInfo.PaperNo;
                                break;
                            }
                            k++;             //试卷号不符合，复查列表下一个
                        }
                    }
                }
            }
            for (i = 0; i < DM_Client.tblScore.Rows.Count; i++)
            {
                if (i == DM_Client.MyVars.PositiveEndRow)
                {
                    continue;
                }
                Check.ScoreInput[i] = ((TextBox)(GridViewScore.Rows[i].FindControl("txtScore"))).Text;
                DM_Client.stCheck[j].DetailScore[i] = Check.ScoreInput[i];  //记录第m个试卷的每个题块分数
            }
            Session["Check"] = Check;
        }
        public void SaveZeroString()
        {
            int i, j, k, m, n;
            UMyRecords.stMyPaper PTmpPaper;
            Boolean flag = false;
            Check.ScoreInput = new string[DM_Client.tblScore.Rows.Count];
            j = 0;
            if (!Check.status_review) //正常阅卷
            {
                for (j = 0; j < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize; j++)
                {
                    if (DM_Client.stCheck[j].PaperNo == -1)       //如果还有空位
                    {
                        flag = true;
                        DM_Client.stCheck[j].PaperNo = DM_Client.MyVars.CurPaperID; //记录当前试卷号
                        break;
                    }
                }
                if (!flag)                   //没有空位
                {
                    j = DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize - 1;    //复查列表没找到，替换掉最后一个数据
                    DM_Client.stCheck[j].PaperNo = DM_Client.MyVars.CurPaperID;
                    for (m = 0; m < DM_Client.MyRecords.CurBlockInfo.BlockInfo.BufSize - 1; m++)   //分数前移
                    {
                        for (n = 0; n < DM_Client.tblScore.Rows.Count; n++)
                        {
                            DM_Client.stCheck[m].DetailScore[n] = DM_Client.stCheck[m + 1].DetailScore[n];
                        }
                        DM_Client.stCheck[m].PaperNo = DM_Client.stCheck[m + 1].PaperNo;
                    }
                }

            }
            else                      //复查状态,肯定会找到对应试卷
            {
                k = 0;
                if (DM_Client.OldPaperList.Count > 0)
                {
                    for (i = 0; i < DM_Client.OldPaperList.Count; i++)
                    {
                        PTmpPaper = DM_Client.OldPaperList[i];
                        if (PTmpPaper.Status)                 //复查列表寻找
                        {
                            if (PTmpPaper.PaperInfo.PaperNo == DM_Client.MyVars.CurPaperID)  //找到试卷
                            {
                                j = k;       //赋值并退出循环
                                DM_Client.stCheck[j].PaperNo = PTmpPaper.PaperInfo.PaperNo;
                                break;
                            }
                            k++;             //试卷号不符合，复查列表下一个
                        }
                    }
                }
            }
            for (i = 0; i < DM_Client.tblScore.Rows.Count; i++)
            {
                if (i == DM_Client.MyVars.PositiveEndRow)
                {
                    continue;
                }
                Check.ScoreInput[i] = "0";
                DM_Client.stCheck[j].DetailScore[i] = Check.ScoreInput[i];  //记录第m个试卷的每个题块分数
            }
            Session["Check"] = Check;

        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args) //没什么需要保存状态的
        {
            Single score;            // 记录当前老师所给分
            Boolean ScoreIsValid;   // 当前得分点分数输入是否有效
            int Rate;           //分等级或者分项分等级给分题中所给等级数
            int curRow;         //记录焦点所在行
            int curCol;         //记录焦点所在列
            Single stateFull;
            string point, tmps, pattern;
            Single CanGiveScore;
            Single Min;
            Single Max;
            int strinpos;
            int i;
            string fullscore;
            Boolean MinusFlag; //是否是给负分
            Boolean HalfFlag;//是否允许给0.5分
            Boolean match = false;
            int Code;
            if (getPostBackControlName() != "ButtonSubmit")
            {
                return;
            }
            MinusFlag = false;
            ScoreIsValid = false;
            Check.ScoreInputOK = false;
            //这个函数会对每个验证控件起作用
            CustomValidator cv = (CustomValidator)source;
            GridViewRow dvr = (GridViewRow)cv.Parent.Parent;
            i = dvr.RowIndex; //验证到第几行
            if (DM_Client.tblScore.Rows[i][3].ToString() == "Y")
                HalfFlag = true;
            else
                HalfFlag = false;
            if (DM_Client.tblScore.Rows[i][2].ToString().IndexOf('-') >= 0)
            {
                MinusFlag = true;
            }
            point = Check.ScoreInput[i]; //获取分数字符串
            fullscore = DM_Client.tblScore.Rows[i][2].ToString();
            switch (DM_Client.MyVars.QuestionType)
            {
                case 3: //测试库里只有任意给分题
                    //用正则表达式，输入只能为正负整数，或者正负一位小数
                    if (HalfFlag == false && MinusFlag == false)  //只能为正整数
                    {
                        pattern = @"^(0|[1-9]\d*)$"; //非负
                        match = Regex.IsMatch(point, pattern);
                    }
                    if (HalfFlag == true && MinusFlag == false) //正整数或一位小数
                    {
                        pattern = @"^(0(\.5)?|[1-9]\d*(\.5)?)$";
                        match = Regex.IsMatch(point, pattern);
                    }
                    if (HalfFlag == false && MinusFlag == true)
                    {
                        pattern = @"^(0|-?[1-9]\d*)$";  //整数
                        match = Regex.IsMatch(point, pattern);
                    }
                    if (HalfFlag == true && MinusFlag == true)
                    {
                        pattern = @"^(-?[1-9][0-9]*(\.[1-9])?|0(\.5)?)$"; //整数或一位小数
                        match = Regex.IsMatch(point, pattern);
                    }
                    if (!match)
                    {
                        DM_Client.stCheck = (DataModule.stCheck[])UMsgDefine.Clone(Check.stCheckTemp);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel_submit, this.UpdatePanel_submit.GetType(), "msg", "<script>alert('输入分数不合要求');</script>", false);
                        args.IsValid = false;
                    }
                    else //比较满分
                    {
                        if (Math.Abs(float.Parse(point)) > Math.Abs(float.Parse(fullscore)))
                        {
                            DM_Client.stCheck = (DataModule.stCheck[])UMsgDefine.Clone(Check.stCheckTemp);
                            ScriptManager.RegisterStartupScript(this.UpdatePanel_submit, this.UpdatePanel_submit.GetType(), "msg", "<script>alert('输入分数超过满分');</script>", false);
                            args.IsValid = false;
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 获取引发Postback的控件名
        /// </summary>
        /// <returns></returns>
        private string getPostBackControlName()
        {
            Control control = null;
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = Page.FindControl(ctrlname);
            }
            else
            {
                Control c;
                foreach (string ctl in Page.Request.Form)
                {
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        c = Page.FindControl(ctl.Substring(0, ctl.Length - 2));
                    }
                    else
                    {
                        c = Page.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            if (control != null)
                return control.ID;
            else
                return string.Empty;
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
        protected void LinkButtonSelect_Click(object sender, EventArgs e)
        {
            int i, j, k;
            UMyRecords.stMyPaper PMyPaper;
            DataTable tbl;
            LinkButton b = (LinkButton)sender;
            GridViewRow row = (GridViewRow)b.Parent.Parent;
            int index = row.RowIndex;    //确定点击的行
            
            //if (DM_Client.OldPaperList.Count > 0)       //找复查试卷对应哪个结构体
            //{
            //    k = 0;
            //    for (i = 0; i < DM_Client.OldPaperList.Count; i++)
            //    {
            //        PMyPaper = DM_Client.OldPaperList[i];
            //        if (PMyPaper.Status)                 //复查列表寻找
            //        {
            //            if (PMyPaper.PaperInfo.PaperNo == DM_Client.OldPaperList[index].PaperInfo.PaperNo)  //找到试卷
            //            {
            //                j = k;       //赋值并退出循环
            //                DM_Client.stCheck[j].PaperNo = PMyPaper.PaperInfo.PaperNo;
            //                break;
            //            }
            //            k++;             //试卷号不符合，复查列表下一个
            //        }
            //    }
            //}
            tbl = DM_Client.tblScore.Copy();
            for (i = 0; i < DM_Client.tblScore.Rows.Count; i++)            //把分数绑定
            {
                tbl.Rows[i][1] = DM_Client.stCheck[index].DetailScore[i];
            }
            GridViewScore.DataSource = tbl;
            GridViewScore.DataBind();
            ShowImage(1, index);
            Check.status_review = true;
            LabelName.Text = "试卷复查";
            UpdateLabelName.Update();
            Session["Check"] = Check;
        }
        public Boolean GetPerPty(UMsgDefine.FM_GETPERQTY_REQ GetPerQtyReq, ref UMsgDefine.FM_GETPERQTY_RSP GetPerQtyRsp)
        {
            uint StartTime, endTime;
            string content;

            GetPerQtyReq.MsgHead.MsgType = UConstDefine.TM_GETPERQTY_REQ;
            GetPerQtyReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_GETPERQTY_REQ));
            GetPerQtyReq.UserID = DM_Client.MyRecords.UserInfo.UserID;
            content = "个人质量信息获取CsSocket";
            UMyFuncs.WriteLogFile(DM_Client, content);
            lock (DM_Client.CsSocket)
            {
                content = "个人质量信息获取CsSocket成功,准备发送请求帧";
                UMyFuncs.WriteLogFile(DM_Client, content);
                byte[] buf = UMsgDefine.StructToBytes(GetPerQtyReq);
                DM_Client.TCPSocket.Send(buf, GetPerQtyReq.MsgHead.MsgLength, SocketFlags.None);
                content = "个人质量信息请求帧发送成功";
                UMyFuncs.WriteLogFile(DM_Client, content);

                content = "等待个人质量信息响应帧...";
                UMyFuncs.WriteLogFile(DM_Client, content);

                byte[] ReceiveMsg = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_GETPERQTY_RSP))];
                DM_Client.TCPSocket.Receive(ReceiveMsg, ReceiveMsg.Length, SocketFlags.None);
                GetPerQtyRsp = (UMsgDefine.FM_GETPERQTY_RSP)UMsgDefine.BytesToStruct(ReceiveMsg, typeof(UMsgDefine.FM_GETPERQTY_RSP));
                content = "收到个人质量信息响应帧";
                UMyFuncs.WriteLogFile(DM_Client, content);
                if (GetPerQtyRsp.MsgHead.MsgType == UConstDefine.TM_GETPERQTY_RSP)
                {
                    if (GetPerQtyRsp.RspCode == UConstDefine.RM_RSP_OK)
                    {
                        return true;
                    }
                    else
                    {
                        switch (GetPerQtyRsp.RspCode)
                        {
                            case UConstDefine.RM_ERR_ERR:
                                content = "获取个人质量信息时发生未知错误";
                                UMyFuncs.WriteLogFile(DM_Client, content);
                                break;
                            case UConstDefine.RM_ERR_USER:
                                content = "获取个人质量信息时发生用户错误";
                                UMyFuncs.WriteLogFile(DM_Client, content);
                                break;
                        }
                        return false;
                    }
                }
                else
                {
                    content = "获取个人质量信息时收到错误响应帧";
                    UMyFuncs.WriteLogFile(DM_Client, content);
                    return false;
                }
            }
        }
        public void ShowPerQty(object sender, EventArgs e)
        {
            UMsgDefine.FM_GETPERQTY_REQ GetPerQtyReq = new UMsgDefine.FM_GETPERQTY_REQ();
            UMsgDefine.FM_GETPERQTY_RSP GetPerQtyRsp = new UMsgDefine.FM_GETPERQTY_RSP();
            Single EffectiveRate, FinishedRate;
            int i;
            Single PerAvgScore, BlkAvgScore, PerSpeed, BlkSpeed;

            if (GetPerPty(GetPerQtyReq, ref GetPerQtyRsp))  //接受的总给分和不正确
            {
                EffectiveRate = 0;
                if (GetPerQtyRsp.PerQtyInfo.PChecked > 0)
                {
                    EffectiveRate = (Single)(GetPerQtyRsp.PerQtyInfo.PValid * 1.0/ GetPerQtyRsp.PerQtyInfo.PChecked);
                }
                FinishedRate = (Single)(GetPerQtyRsp.PerQtyInfo.GFinished *1.0 / GetPerQtyRsp.PerQtyInfo.GPaperCount);

                lbBlkOK.Style.Value = "width:" + (FinishedRate * 100).ToString() + "%;" + "float: left;display: inline";
                lbPerOK.Style.Value = "width:" + ((float)(GetPerQtyRsp.PerQtyInfo.PChecked * 100 / (float)GetPerQtyRsp.PerQtyInfo.GPaperCount)).ToString() + "%;" + "float: left;display: inline";

                lbCount.Text = GetPerQtyRsp.PerQtyInfo.PChecked.ToString() + "/" + GetPerQtyRsp.PerQtyInfo.GFinished.ToString();
                if (DM_Client.MyRecords.BlockInfoList[0].BlockInfo.nNEv > 1)
                {
                    lbEffective.Style.Value = "width:" + ((EffectiveRate) * ((float)(GetPerQtyRsp.PerQtyInfo.PChecked * 100 / (float)GetPerQtyRsp.PerQtyInfo.GPaperCount))).ToString() + "%;" + "float: left;display: inline";
                    lbCount.Text = GetPerQtyRsp.PerQtyInfo.PValid + "/" + lbCount.Text;
                }
                if (Check.FirstQtyFlag != true)
                {
                    PerSpeed = 0;
                    BlkSpeed = 0;
                    PerAvgScore = 0;
                    BlkAvgScore = 0;
                    //计算个人阅卷速度单位（份/分钟)
                    if (GetPerQtyRsp.PerQtyInfo.PSumTime != Check.LastQtyInfo.PSumTime)
                    {
                        PerSpeed = (float)(GetPerQtyRsp.PerQtyInfo.PChecked - Check.LastQtyInfo.PChecked) / (float)(GetPerQtyRsp.PerQtyInfo.PSumTime - Check.LastQtyInfo.PSumTime) * 60;
                    }
                    //计算题组阅卷速度
                    if (GetPerQtyRsp.PerQtyInfo.GSumTime != Check.LastQtyInfo.GSumTime)
                    {
                        BlkSpeed = (float)(GetPerQtyRsp.PerQtyInfo.GChecked - Check.LastQtyInfo.GChecked) / (float)(GetPerQtyRsp.PerQtyInfo.GSumTime - Check.LastQtyInfo.GSumTime) * 60;
                    }
                    //计算个人平均分
                    if (GetPerQtyRsp.PerQtyInfo.PChecked != Check.LastQtyInfo.PChecked)
                    {
                        PerAvgScore = (float)(GetPerQtyRsp.PerQtyInfo.PSumScore - Check.LastQtyInfo.PSumScore) / (float)(GetPerQtyRsp.PerQtyInfo.PChecked - Check.LastQtyInfo.PChecked);
                    }
                    //计算题组平均分
                    if (GetPerQtyRsp.PerQtyInfo.GChecked != Check.LastQtyInfo.GChecked)
                    {
                        BlkAvgScore = (float)(GetPerQtyRsp.PerQtyInfo.GSumScore - Check.LastQtyInfo.GSumScore) / (float)(GetPerQtyRsp.PerQtyInfo.GChecked - Check.LastQtyInfo.GChecked);
                    }
                    UpdatePerQtyArray(PerSpeed, PerAvgScore, BlkSpeed, BlkAvgScore);

                }
                Check.LastQtyInfo = GetPerQtyRsp.PerQtyInfo;
                Session["Check"] = Check;
                BindSpeed();
                BindAvg();
            }
        }
        public void ShowPerQtyFirst()
        {
            UMsgDefine.FM_GETPERQTY_REQ GetPerQtyReq = new UMsgDefine.FM_GETPERQTY_REQ();
            UMsgDefine.FM_GETPERQTY_RSP GetPerQtyRsp = new UMsgDefine.FM_GETPERQTY_RSP();
            Single EffectiveRate, FinishedRate;
            int i;
            Single PerAvgScore, BlkAvgScore, PerSpeed, BlkSpeed;

            if (GetPerPty(GetPerQtyReq, ref GetPerQtyRsp))
            {
                EffectiveRate = 0;
                if (GetPerQtyRsp.PerQtyInfo.PChecked > 0)
                {
                    EffectiveRate = (Single)(GetPerQtyRsp.PerQtyInfo.PValid * 1.0 / GetPerQtyRsp.PerQtyInfo.PChecked);
                }
                FinishedRate = (Single)(GetPerQtyRsp.PerQtyInfo.GFinished * 1.0 / GetPerQtyRsp.PerQtyInfo.GPaperCount);

                lbBlkOK.Style.Value = "width:" + (FinishedRate * 100).ToString() + "%;" + "float: left;display: inline";
                lbPerOK.Style.Value = "width:" + ((float)(GetPerQtyRsp.PerQtyInfo.PChecked * 100 / (float)GetPerQtyRsp.PerQtyInfo.GPaperCount)).ToString() + "%;" + "float: left;display: inline";

                lbCount.Text = GetPerQtyRsp.PerQtyInfo.PChecked.ToString() + "/" + GetPerQtyRsp.PerQtyInfo.GFinished.ToString();
                if (DM_Client.MyRecords.BlockInfoList[0].BlockInfo.nNEv > 1)
                {
                    lbEffective.Style.Value = "width:" + ((EffectiveRate) * ((float)(GetPerQtyRsp.PerQtyInfo.PChecked * 100 / (float)GetPerQtyRsp.PerQtyInfo.GPaperCount))).ToString() + "%;" + "float: left;display: inline";
                    lbCount.Text = GetPerQtyRsp.PerQtyInfo.PValid + "/"+ lbCount.Text;
                }
                Check.LastQtyInfo = GetPerQtyRsp.PerQtyInfo;
                bindchart();
                Check.FirstQtyFlag = false;
                Session["Check"] = Check;
            }
        }
        public void UpdatePerQtyArray(Single PerSpeed, Single PerAvgScore, Single BlkSpeed, Single BlkAvgScore)
        {
            int i;
            int h, m, s, ms;
            string strTime = "";
            string DebugInfo;

            for (i = 0; i < UnitGlobalV.WindowCount; i++)
            {
                if (Check.TimeArray[i].Equals(new DateTime(0001, 01, 01, 00, 00, 00)))
                {
                    Check.TimeArray[i] = DateTime.Now;
                    Check.PerAvgScoreArray[i] = PerAvgScore;
                    Check.PerSpeedArray[i] = PerSpeed;
                    Check.BlkAvgScoreArray[i] = BlkAvgScore;
                    Check.BlkSpeedArray[i] = BlkSpeed;
                    break;
                }
            }
            if (i == UnitGlobalV.WindowCount)
            {
                for (i = 0; i < UnitGlobalV.WindowCount - 1; i++)
                {
                    //for 2
                    Check.TimeArray[i] = Check.TimeArray[i + 1];
                    Check.PerAvgScoreArray[i] = Check.PerAvgScoreArray[i + 1];
                    Check.PerSpeedArray[i] = Check.PerSpeedArray[i + 1];
                    Check.BlkAvgScoreArray[i] = Check.BlkAvgScoreArray[i + 1];
                    Check.BlkSpeedArray[i] = Check.BlkSpeedArray[i + 1];
                } //end for 2
                Check.TimeArray[UnitGlobalV.WindowCount - 1] = DateTime.Now;
                Check.PerAvgScoreArray[UnitGlobalV.WindowCount - 1] = PerAvgScore;
                Check.PerSpeedArray[UnitGlobalV.WindowCount - 1] = PerSpeed;
                Check.BlkAvgScoreArray[UnitGlobalV.WindowCount - 1] = BlkAvgScore;
                Check.BlkSpeedArray[UnitGlobalV.WindowCount - 1] = BlkSpeed;
            }
        }
        public void BindSpeed()
        {
            DataTable speed = new DataTable();
            speed.Columns.Add("Date", typeof(DateTime));
            speed.Columns.Add("个人", typeof(Single));
            speed.Columns.Add("题组", typeof(Single));
            for (int i = 0; i < UnitGlobalV.WindowCount; i++)
            {
                if (!Check.TimeArray[i].Equals(new DateTime(0001, 01, 01, 00, 00, 00)))
                    speed.Rows.Add(Check.TimeArray[i], Check.PerSpeedArray[i], Check.BlkSpeedArray[i]);
            }
            DataView speedview = new DataView(speed);
            Session["speedview"] = speedview;
            // 绑定两条波形
            Chart1.Series["个人"].Points.DataBindXY(speedview, "Date", speedview, "个人");
            Chart1.Series["题组"].Points.DataBindXY(speedview, "Date", speedview, "题组");
            //设定坐标极值
            DateTime max = DateTime.FromOADate(Chart1.Series["个人"].Points[Chart1.Series["个人"].Points.Count - 1].XValue);
            DateTime min = DateTime.FromOADate(Chart1.Series["个人"].Points[0].XValue);
            Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = max.ToOADate();
            Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = min.ToOADate();
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{HH:mm}"; //设定只表示时分

            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;//仅不显示x轴方向的网格线
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;//仅不显示y轴方向的网格线
        }
        public void BindAvg()
        {
            DataTable avg = new DataTable();
            avg.Columns.Add("Date", typeof(DateTime));
            avg.Columns.Add("个人", typeof(Single));
            avg.Columns.Add("题组", typeof(Single));
            for (int i = 0; i < UnitGlobalV.WindowCount; i++)
            {
                if (!Check.TimeArray[i].Equals(new DateTime(0001, 01, 01, 00, 00, 00)))
                    avg.Rows.Add(Check.TimeArray[i], Check.PerAvgScoreArray[i], Check.BlkAvgScoreArray[i]);
            }
            DataView avgview = new DataView(avg);
            Session["avgview"] = avgview;
            // 绑定两条波形
            Chart2.Series["个人"].Points.DataBindXY(avgview, "Date", avgview, "个人");
            Chart2.Series["题组"].Points.DataBindXY(avgview, "Date", avgview, "题组");
            //设定坐标极值
            DateTime max = DateTime.FromOADate(Chart2.Series["个人"].Points[Chart2.Series["个人"].Points.Count - 1].XValue);
            DateTime min = DateTime.FromOADate(Chart2.Series["个人"].Points[0].XValue);
            Chart2.ChartAreas["ChartArea1"].AxisX.Maximum = max.ToOADate();
            Chart2.ChartAreas["ChartArea1"].AxisX.Minimum = min.ToOADate();
            Chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{HH:mm}"; //设定只表示时分

            Chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;//仅不显示x轴方向的网格线
            Chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;//仅不显示y轴方向的网格线

        }
        public void BindSpeedTab()
        {
            DataView speedview = (DataView)Session["speedview"];
            // 绑定两条波形
            Chart1.Series["个人"].Points.DataBindXY(speedview, "Date", speedview, "个人");
            Chart1.Series["题组"].Points.DataBindXY(speedview, "Date", speedview, "题组");
            //设定坐标极值
            if (speedview.Count != 0)
            {
                DateTime max = DateTime.FromOADate(Chart1.Series["个人"].Points[Chart1.Series["个人"].Points.Count - 1].XValue);
                DateTime min = DateTime.FromOADate(Chart1.Series["个人"].Points[0].XValue);
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = max.ToOADate();
                Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = min.ToOADate();
                Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{HH:mm}"; //设定只表示时分

                Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;//仅不显示x轴方向的网格线
                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;//仅不显示y轴方向的网格线
            }
        }
        public void BindAvgTab()
        {
            DataView avgview = (DataView)Session["avgview"];
            // 绑定两条波形
            Chart2.Series["个人"].Points.DataBindXY(avgview, "Date", avgview, "个人");
            Chart2.Series["题组"].Points.DataBindXY(avgview, "Date", avgview, "题组");
            //设定坐标极值
            if (avgview.Count != 0)
            {
                DateTime max = DateTime.FromOADate(Chart2.Series["个人"].Points[Chart2.Series["个人"].Points.Count - 1].XValue);
                DateTime min = DateTime.FromOADate(Chart2.Series["个人"].Points[0].XValue);
                Chart2.ChartAreas["ChartArea1"].AxisX.Maximum = max.ToOADate();
                Chart2.ChartAreas["ChartArea1"].AxisX.Minimum = min.ToOADate();
                Chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{HH:mm}"; //设定只表示时分
                
                Chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;//仅不显示x轴方向的网格线
                Chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;//仅不显示y轴方向的网格线
            }
        }

        protected void ButtonBlank_Click(object sender, EventArgs e)
        {
            //对于空白卷的处理

            int i, j, rate;
            Single Max, min, Score, CanGiveScore, ParamScore;
            Boolean ScoreIsVaild;
            int Code, MinusStart;
            int p;
            Boolean MinusFlag;
            string ScoreString, FullScoreString;
            Check.gDetailScore = "";
            Check.gTotalScore = 0;
            MinusStart = -1;
            p = 0;
            //给分普通情况，不考虑等级
            i = 1;

            Check.ScoreInput = new string[DM_Client.tblScore.Rows.Count];
            for (j = 0; j < DM_Client.tblScore.Rows.Count; j++)
            {
                if (j == DM_Client.MyVars.PositiveEndRow)
                {
                    continue;
                }
                MinusFlag = (DM_Client.tblScore.Rows[j][2].ToString().IndexOf('-') >= 0);
                if (MinusFlag)
                {
                    Check.ScoreInput[j] = "-0";
                }
                else
                {
                    Check.ScoreInput[j] = "0";
                }
            }


            for (i = 1; i < DM_Client.tblScore.Rows.Count + 1; i++)
            {
                if (i == DM_Client.MyVars.PositiveEndRow + 1)
                {
                    continue;
                }
                ScoreString = Check.ScoreInput[i - 1];
                FullScoreString = DM_Client.tblScore.Rows[i - 1][2].ToString();
                Score = float.Parse(ScoreString);
                p = Check.gDetailScore.IndexOf('{');
                if (FullScoreString.IndexOf('-') >= 0)
                {
                    MinusFlag = true;
                }
                else
                {
                    MinusFlag = false;
                }
                if (!MinusFlag)
                {
                    Check.gDetailScore = Check.gDetailScore + ScoreString;
                    if (i < DM_Client.tblScore.Rows.Count)
                    {
                        Check.gDetailScore = Check.gDetailScore + ",";
                    }
                }
                else
                {
                    if (MinusStart < 0)
                    {
                        MinusStart = i;
                    }
                    if (p < 0) //如果还没有到负分给分点
                    {
                        Check.gDetailScore = Check.gDetailScore + "{";
                    }
                    Check.gDetailScore = Check.gDetailScore + ScoreString;
                    if (i < DM_Client.tblScore.Rows.Count)
                    {
                        Check.gDetailScore = Check.gDetailScore + "|";
                    }
                    else
                    {
                        Check.gDetailScore = Check.gDetailScore + "}";
                    }
                }
                Check.gTotalScore = Check.gTotalScore + Score;
            }

            AfterGiveScore(Check.gDetailScore, Check.gTotalScore);
        }
        protected void Buttontmp_Click(object sender, EventArgs e)
        {
            UMsgDefine.FM_SaveExcp_Req fmExcpReq;   //异常提交帧——存分帧的一种
            UMsgDefine.FM_SaveScore_Rsp fmExcpRsp;   //异常提交响应帧
            Boolean Flag = false, RepOK = false;
            int i, j, RsnCode;
            UMyRecords.stMyPaper PTmpPaper;
            double d;
            string tmpString;
            int ExcpPaperNo = DM_Client.MyVars.CurPaperID;
            i = DM_Client.excp;
            if (i != DM_Client.MyRecords.ExcpRsnList.Count)
            {
                RsnCode = DM_Client.MyRecords.ArrRsnCode[i];
            }
            else
            {
                RsnCode = UConstDefine.SelfExcpCode;
            }
            lock (DM_Client.CsOldPaper)
            {
                if (DM_Client.OldPaperList.Count > 0)
                {
                    PTmpPaper = new UMyRecords.stMyPaper();
                    for (i = DM_Client.OldPaperList.Count - 1; i >= 0; i--)
                    {
                        PTmpPaper = DM_Client.OldPaperList[i];
                        if (PTmpPaper.PaperInfo.PaperNo == ExcpPaperNo)
                        {
                            Flag = true;
                            break;
                        }
                    }
                    if (Flag)
                    {
                        switch (DM_Client.MyRecords.UserInfo.Status)
                        {
                            case UConstDefine.Trial:
                                DM_Client.OldPaperList.RemoveAt(i);
                                lock (DM_Client.CsJunk)
                                {
                                    DM_Client.JunkList.Add(PTmpPaper);
                                }
                                DM_Client.MyVars.CurPaperID = 0;
                                RefreshShow(1);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('问题试卷提交成功');</script>", false);
                                if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                                {
                                    ShowImage(0);
                                }
                                break;
                            case UConstDefine.ZhengPing:
                                fmExcpReq = new UMsgDefine.FM_SaveExcp_Req();
                                fmExcpReq.MsgHead.MsgType = UConstDefine.TM_SAVESCORE_REQ;
                                fmExcpReq.MsgHead.MsgLength = Marshal.SizeOf(typeof(UMsgDefine.FM_SaveExcp_Req));
                                fmExcpReq.RecCount = 1;
                                fmExcpReq.Score.UserID = DM_Client.MyRecords.UserInfo.UserID;
                                fmExcpReq.Score.PaperNo = ExcpPaperNo;
                                fmExcpReq.Score.VolumeName = PTmpPaper.PaperInfo.VolumeName;
                                fmExcpReq.Score.ReasonType = RsnCode;
                                fmExcpReq.Score.DetailScore = "";
                                if (RsnCode == UConstDefine.SelfExcpCode)
                                {
                                    tmpString = DM_Client.reason;
                                    fmExcpReq.Score.DetailScore = tmpString;
                                }
                                fmExcpReq.Score.ScoreType = UConstDefine.PM_SAVESCORE_YCREPORT;
                                d = ((DateTime.Now - UnitGlobalV.javaTime).TotalDays + DM_Client.sysTimeDis) * 86400 - 8 * 60 * 60;
                                fmExcpReq.Score.TimeStamp = (uint)d;
                                fmExcpReq.Score.TimeCost = 0;


                                lock (DM_Client.CsSocket)
                                {
                                    byte[] buf = UMsgDefine.StructToBytes(fmExcpReq);
                                    DM_Client.TCPSocket.Send(buf, buf.Length, SocketFlags.None);

                                    byte[] RcvBuf = new byte[Marshal.SizeOf(typeof(UMsgDefine.FM_SaveScore_Rsp))];
                                    DM_Client.TCPSocket.Receive(RcvBuf, RcvBuf.Length, SocketFlags.None);
                                    fmExcpRsp = (UMsgDefine.FM_SaveScore_Rsp)UMsgDefine.BytesToStruct(RcvBuf, typeof(UMsgDefine.FM_SaveScore_Rsp));
                                    if (fmExcpRsp.MsgHead.MsgType == UConstDefine.TM_SAVESCORE_RSP)
                                    {
                                        if (fmExcpRsp.RspCode == UConstDefine.RM_RSP_OK)
                                        {
                                            DM_Client.OldPaperList.RemoveAt(i);
                                            lock (DM_Client.CsJunk)
                                            {
                                                DM_Client.JunkList.Add(PTmpPaper);
                                            }
                                            DM_Client.MyVars.CurPaperID = 0;
                                            RefreshShow(1);
                                            RepOK = true;
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('问题试卷提交成功');</script>", false);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('提交问题试卷时出现错误');</script>", false);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('收到错误的问题试卷提交响应帧');</script>", false);
                                    }
                                }
                                if (RepOK)
                                {
                                    if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
                                        ShowImage(0);
                                }
                                break;
                        }
                    }
                }
            }
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        //protected void Buttoninfo_Click(object sender, EventArgs e)
        //{
        //    string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
        //    tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
        //    string tmpstr2 = new String(DM_Client.MyRecords.UserInfo.TrueName);
        //    tmpstr2 = tmpstr2.Substring(0, tmpstr2.IndexOf('\0'));
        //    LabelLoginName.Text = tmpstr1 + " " + tmpstr2;
        //    UpdatePanelLoginName.Update();
        //}
<<<<<<< HEAD
=======
=======
        protected void Buttoninfo_Click(object sender, EventArgs e)
        {
            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            string tmpstr2 = new String(DM_Client.MyRecords.UserInfo.TrueName);
            tmpstr2 = tmpstr2.Substring(0, tmpstr2.IndexOf('\0'));
            LabelLoginName.Text = tmpstr1 + " " + tmpstr2;
            UpdatePanelLoginName.Update();
        }
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master

        protected void Buttonzhuxiao_Click(object sender, EventArgs e)
        {
            UMsgDefine.FM_UserLogout fmLogout;
            int i;
            UMyRecords.stMyPaper PTmpPaper;

            string tmpstr1 = new String(DM_Client.MyRecords.UserInfo.LoginName);
            tmpstr1 = tmpstr1.Substring(0, tmpstr1.IndexOf('\0'));
            if (tmpstr1.Length != 0)
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1, true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "DaAn"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "DaAn",true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "XiZe"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "XiZe",true);
            }
            if (System.IO.Directory.Exists(Server.MapPath(".") + "/" + tmpstr1 + "BiaoZhun"))
            {
                System.IO.Directory.Delete(Server.MapPath(".") + "/" + tmpstr1 + "BiaoZhun",true);
            }
            if (DM_Client.OldPaperList.Count > 0)
            {
                SaveOne();//将现有的分数存回服务器
            }
            while (DM_Client.SaveScoreList.Count != 0)
            {
                Thread.Sleep(3000);
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
            Response.Redirect("~/Login.aspx");
        }

        protected void Buttonyangjuan_Click(object sender, EventArgs e)
        {
            if(DM_Client.MyRecords.UserInfo.Role==UConstDefine.PuTong)
            {
                if(DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                {
                    if((Check.LastPaper == false) || (Check.LastPaper == true && Check.SampleOver == false))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('请将所有样卷评阅完毕后再点击“样卷库浏览”按钮查看专家给分!');</script>", false);
                        return;
                    }
                    if((DM_Client.MyRecords.UserInfo.Status == UConstDefine.Trial) ||(DM_Client.MyRecords.UserInfo.Status == UConstDefine.CePing))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgAns", "<script>alert('试用或测评状态下无法浏览样卷库!');</script>", false);
                        return;
                    }

                }
            }
            
        }
        [WebMethod(EnableSession = true)]
        public static string EnableSample()
        {
            TDM_Client DM_Client = (TDM_Client)(HttpContext.Current.Session["DM_Client"]);
            UCheck Check = (UCheck)(HttpContext.Current.Session["Check"]);
            if (DM_Client.MyRecords.UserInfo.Role == UConstDefine.PuTong)
            {
                if (DM_Client.MyRecords.UserInfo.Status == UConstDefine.YangPing)
                {
                    if ((Check.LastPaper == false) || (Check.LastPaper == true && Check.SampleOver == false))
                    {
                        return "请将所有样卷评阅完毕后再点击“样卷库浏览”按钮查看专家给分!";
                    }
                    if ((DM_Client.MyRecords.UserInfo.Status == UConstDefine.Trial) || (DM_Client.MyRecords.UserInfo.Status == UConstDefine.CePing))
                    {                     
                        return "试用或测评状态下无法浏览样卷库!";
                    }
                }
            }
            return "ok";
        }
    }
}