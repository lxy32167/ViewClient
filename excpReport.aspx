<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="excpReport.aspx.cs" Inherits="Client.excpReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self">
    <script type="text/javascript" src="jquery-1.9.1.js"></script>
    <script type="text/javascript" src="jquery.blockUI.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ExcpDiv" style="cursor: default">
        <h3 style="text-align: center">
            请提交异常试卷
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </h3>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 300px; text-align: center">
                    <tr>
                        <td>
                            异常原因
                        </td>
                        <td>
                            <asp:DropDownList ID="excp" runat="server" OnSelectedIndexChanged="excp_SelectedIndexChanged"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            自定义异常原因
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="reason" runat="server" Enabled="false" Text="请输入自定义异常原因" Height="30px"
                                Width="163px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="excpsubmit" runat="server" Text="提交" OnClick="excpsubmit_Click" Style="margin-right: 40px" />
                            <asp:Button ID="excpcancel" runat="server" Text="取消" OnClick="excpcancel_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="excpcancel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
