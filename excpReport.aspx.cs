using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Client.App_Code;
namespace Client
{
    public partial class excpReport : System.Web.UI.Page
    {
        TDM_Client DM_Client;
        protected void Page_Load(object sender, EventArgs e)
        {
            int i;
            string tmp;
            DM_Client = (TDM_Client)Session["DM_Client"];
            if (!IsPostBack)
            {
                for (i = 0; i < DM_Client.MyRecords.ExcpRsnList.Count; i++)
                {
                    tmp = new string(DM_Client.MyRecords.ExcpRsnList[i].Reason);
                    tmp = tmp.Substring(0, tmp.IndexOf('\0'));
                    excp.Items.Add(new ListItem(tmp, i.ToString()));
                }
                excp.Items.Add(new ListItem("自定义异常原因", i.ToString()));
            }
        }
  

        protected void excpcancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'> window.close();</script>");
        }

        protected void excpsubmit_Click(object sender, EventArgs e)
        {
            DM_Client.excp = excp.SelectedIndex;
            DM_Client.reason = reason.Text;
            Session["DM_Client"] = DM_Client;
            string js = "window.opener.document.getElementById('Buttontmp').click();window.close();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ParentButtonClick", js, true);
        }

        protected void excp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (excp.SelectedIndex == DM_Client.MyRecords.ExcpRsnList.Count)
            {
                reason.Enabled = true;
                reason.Text = "";
            }
            else
            {
                reason.Enabled = false;
                reason.Text = "请输入自定义异常原因";
            }
        }
    }
}