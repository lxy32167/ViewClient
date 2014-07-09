<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Answer.aspx.cs" Inherits="Client.Answer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .SepBorder
    {
        border-top-width: 0px;
        border-left-width: 0px;
        font-size: 1px;
        border-bottom: Gray 1px solid;
        border-right-width: 0px;
    }

    .TopBorder
    {
        border-right: Gray 1px solid;
        border-top: Gray 1px solid;
        background: #DCDCDC;
        border-left: Gray 1px solid;
        color: black;
        border-bottom: Gray 1px solid;
    }

    .ContentBorder
    {
        border-right: Gray 1px solid;
        border-top: Gray 0px solid;
        border-left: Gray 1px solid;
        border-bottom: Gray 1px solid;
        height: 100%;
        width: 100%;
    }

    .SelectedTopBorder
    {
        border-right: Gray 1px solid;
        border-top: Gray 1px solid;
        background: none transparent scroll repeat 0% 0%;
        border-left: Gray 1px solid;
        color: black;
        border-bottom: Gray 0px solid;
    }

    </style>
    <script type="text/javascript">
        function winresize(width, height) {
            window.resizeTo(width, height);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <fieldset>
        <legend>系统菜单</legend>
                <asp:UpdatePanel ID="UpdateSystemAnswer" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table id="Table1" runat="server" cellpadding="0" cellspacing="0" width="30%" border="0">
                                        <tr style="height: 22px">
                                            <td class="SelectedTopBorder" id="Cell1" align="center">
                                                <asp:LinkButton ID="lButtonBiaoZhun" runat="server" OnClick="lButtonBiaoZhun_Click">评分标准</asp:LinkButton>
                                            </td>
                                            <td class="SepBorder" style="width: 2px; height: 22px;">
                                            </td>
                                            <td class="TopBorder" id="Cell2" align="center">
                                                <asp:LinkButton ID="lButtonXiZe" runat="server" OnClick="lButtonXiZe_Click">评分细则</asp:LinkButton>
                                            </td>
                                            <td class="SepBorder" style="width: 2px; height: 22px;">
                                            </td>
                                            <td class="TopBorder" id="Cell3" align="center">
                                                <asp:LinkButton ID="lButtonDaAn" runat="server" OnClick="lButtonDaAn_Click">参考答案</asp:LinkButton>
                                            </td>
                                            <td class="SepBorder" style="width: 2px; height: 22px;">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="ContentBorder" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:MultiView ID="mvCompany" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="View1" runat="server">
                                                        <asp:Image ID="biaozhunimage"  runat="server"/>
                                                        <br/>
                                                        <asp:Button ID="biaozhunbefore" Text="上一张" runat="server" 
                                                            onclick="biaozhunbefore_Click" />
                                                        <asp:Button ID="biaozhunnext" Text="下一张" runat="server" 
                                                            onclick="biaozhunnext_Click" />
                                                    </asp:View>
                                                    <asp:View ID="View2" runat="server">
                                                        <asp:Image ID="xizeimage" runat="server"/>
                                                        <br/>
                                                        <asp:Button ID="xizebefore" Text="上一张" runat="server" 
                                                            onclick="xizebefore_Click" />
                                                        <asp:Button ID="xizenext" Text="下一张"  runat="server" onclick="xizenext_Click" />
                                                    </asp:View>
                                                    <asp:View ID="View3" runat="server">
                                                        <asp:Image ID="daanimage" runat="server" />
                                                        <br/>
                                                        <asp:Button ID="daanbefore" Text="上一张" runat="server" 
                                                            onclick="daanbefore_Click" />
                                                        <asp:Button ID="daannext" Text="下一张" runat="server" onclick="daannext_Click" />
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lButtonBiaoZhun" />
                        <asp:AsyncPostBackTrigger ControlID="lButtonXiZe" />
                        <asp:AsyncPostBackTrigger ControlID="lButtonDaAn" />
                    </Triggers>
                </asp:UpdatePanel>
            </fieldset>
    
    </div>
    </form>
</body>
</html>
