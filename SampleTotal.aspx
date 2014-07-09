<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleTotal.aspx.cs" Inherits="Client.SampleTotal" EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="jquery-1.9.1.js"></script>
    <script type="text/javascript" src="jquery.blockUI.js"></script>
    <link href="layout.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    
    .mainTitle
    {
        font-size: 12pt;
        font-weight: bold;
        font-family: 宋体;
    }

    .commonText
    {
        font-size: 11pt;
        font-family: 宋体;
    }

    .littleMainTitle
    {
        font-size: 10pt;
        font-weight: bold;
        font-family: 宋体;
    }

    .TopTitle
    {
        border: 0px;
        font-size: 10pt;
        font-weight: bold;
        text-decoration: none;
        color: Black;
        display: inline-block;
        width: 100%;
    }

    .SelectedTopTitle
    {
        border: 0px;
        font-size: 10pt;
        text-decoration: none;
        color: Black;
        display: inline-block;
        width: 100%;
        background-color: White;
    }

    .ContentView
    {
        border: 0px;
        padding: 3px 3px 3px 3px;
        background-color: White;
        display: inline-block;
        width: 390px;
    }

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

    .Button
    {
        position: relative;
    }

    .Title
    {
        text-align: center;
    }

    .FootName
    {
        position: relative;
        left: 100px;
    }

    .FootDate
    {
        float: right;
    }

    .hidden
    {
        display: none;
    }

    
    #content
    {
            position: relative;
            overflow: auto;
            cursor: move;
    }

        .dragAble
        {
            position:absolute;
            cursor: move;
            top:  0px;
            left: 0px;
        }
</style>
<script type="text/javascript">
    //图片放大和缩小（兼容IE和火狐，谷歌）
    var divId;
    var v_left;
    var v_top;

    window.onload = function () {
        var images1 = document.getElementById("imagesSample");
        divId = document.getElementById("sampletotal");
        var height1 = images1.height; //图片的高度
        var width1 = images1.width; //图片的宽度
        v_left = (document.body.clientWidth - width1) / 2;
        v_top = (document.body.clientHeight - height1) / 2;
        divId.style.left = v_left;
        divId.style.top = v_top;
  
    }
    drag = 0;
    move = 0;
    // 拖拽对象
    var ie = document.all;
    var nn6 = document.getElementById && !document.all;
    var isdrag = false;
    var y, x;
    var oDragObj;

    function moveMouse(e) {
        divId = document.getElementById("sampletotal");
        var clientWidth = divId.clientWidth;
        var clientHeight = divId.clientHeight;
        var zoom = parseInt(oDragObj.style.zoom, 10) || 100;
        var iLeft;
        var iTop;
        if (isdrag && (oDragObj.offsetWidth * zoom / 100 > clientWidth || oDragObj.offsetHeight * zoom / 100 > clientHeight)) {
            if (oDragObj.offsetWidth * zoom / 100 > clientWidth) {
                iLeft = Math.max(Math.min(nn6 ? nTX + e.clientX - x : nTX + event.clientX - x, 0), (divId.clientWidth - oDragObj.offsetWidth * zoom / 100));
                oDragObj.style.left = iLeft + "px";
            }
            if (oDragObj.offsetHeight * zoom / 100 > clientHeight) {
                iTop = Math.max(Math.min(nn6 ? nTY + e.clientY - y : nTY + event.clientY - y, 0), (divId.clientHeight - oDragObj.offsetHeight * zoom / 100));
                oDragObj.style.top = iTop + "px";
            }
        }



        return false;
    }
    // 拖拽方法
    function initDrag(e) {
        var oDragHandle = nn6 ? e.target : event.srcElement;
        var topElement = "HTML";
        while (oDragHandle.tagName != topElement && oDragHandle.className != "dragAble") {
            oDragHandle = nn6 ? oDragHandle.parentNode : oDragHandle.parentElement;
        }
        if (oDragHandle.className == "dragAble") {
            isdrag = true;
            oDragObj = oDragHandle;
            nTY = parseInt(oDragObj.style.top + 0);
            y = nn6 ? e.clientY : event.clientY;
            nTX = parseInt(oDragObj.style.left + 0);
            x = nn6 ? e.clientX : event.clientX;
            document.onmousemove = moveMouse;
            //document.onmouseup=MUp;// 事件会在鼠标按键被松开时发生
            return false;
        }
    }
    document.onmousedown = initDrag;
    document.onmouseup = new Function("isdrag=false");
    //上下左右移动
    function clickMove(s) {
        if (s == "up") {
            dragObj.style.top = parseInt(dragObj.style.top) + 100;
        } else {
            if (s == "down") {
                dragObj.style.top = parseInt(dragObj.style.top) - 100;
            } else {
                if (s == "left") {
                    dragObj.style.left = parseInt(dragObj.style.left) + 100;
                } else {
                    if (s == "right") {
                        dragObj.style.left = parseInt(dragObj.style.left) - 100;
                    }
                }
            }
        }
    }
    //缩小倍数
    function smallit() {
        //将图片缩小，失去热点
        height1 = images1.height;
        width1 = images1.width;
        images1.height = height1 / 1.1;
        images1.width = width1 / 1.1;
    }
    //放大倍数
    function bigit() {

        //将图片放大，失去热点
        height1 = images1.height;
        width1 = images1.width;
        images1.height = height1 * 1.1;
        images1.width = width1 * 1.1;
    }
    //还原
    function realsize() {
        images1.style.zoom = 100 + "%";
        images1.height = images2.height;
        images1.width = images2.width;
        divId.style.left = v_left;
        divId.style.top = v_top;
    }
    function featsize() {
        var width1 = images2.width;
        var height1 = images2.height;
        var width2 = 360;
        var height2 = 200;
        var h = height1 / height2;
        var w = width1 / width2;
        if (height1 < height2 && width1 < width2) {
            images1.height = height1;
            images1.width = width1;
        } else {
            if (h > w) {
                images1.height = height2;
                images1.width = width1 * height2 / height1;
            } else {
                images1.width = width2;
                images1.height = height1 * width2 / width1;
            }
        }
        block1.style.left = 0;
        block1.style.top = 0;
    }
    //鼠标滚轮放大缩小
    function bbimg(o) {
        var zoom = parseInt(o.style.zoom, 10) || 100;
        zoom += event.wheelDelta / 12;
        if (zoom > 0) {
            o.style.zoom = zoom + "%";

        }
        return false;
    }
    //处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外
    function banBackSpace(e) {
        var ev = e || window.event; //获取event对象
        var obj = ev.target || ev.srcElement; //获取事件源
        var t = obj.type || obj.getAttribute('type'); //获取事件源类型
        //获取作为判断条件的事件类型
        var vReadOnly = obj.readOnly;
        var vDisabled = obj.disabled;
        //处理undefined值情况
        vReadOnly = (vReadOnly == undefined) ? false : vReadOnly;
        vDisabled = (vDisabled == undefined) ? true : vDisabled;
        //当敲Backspace键时，事件源类型为密码或单行、多行文本的，
        //并且readOnly属性为true或disabled属性为true的，则退格键失效
        var flag1 = ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea") && (vReadOnly == true || vDisabled == true);
        //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效
        var flag2 = ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea";
        //判断
        if (flag2 || flag1) return false;
    }
    //禁止退格键 作用于Firefox、Opera
    document.onkeypress = banBackSpace;
    //禁止退格键 作用于IE、Chrome
    document.onkeydown = banBackSpace;
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="mainContent">
             <asp:UpdatePanel ID="UpdateImage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="content" style="margin-left: auto; margin-right: auto;background-color:White;height:420px">
                        <div id="sampletotal" style="overflow:auto;height:420px;" >
                            <asp:Image ID="imagesSample" runat="server" CssClass="dragAble" onmousewheel="return bbimg(this)"/>
                        </div>  
                    </div>
                    
                </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
              <%--  <asp:AsyncPostBackTrigger ControlID="DropDownSampleGroup" />--%>
                <asp:AsyncPostBackTrigger ControlID="GridViewCheckSample" />
                </Triggers>
               </asp:UpdatePanel>
            
                <div id="sidebar" style="text-align: center;">
              
                  <div style="border-style:groove">
                    <div style="text-align:left;margin-top:10px;">
                        <asp:Label ID="LabelBlkName" runat="server" Text="分卷名:"></asp:Label>
                        <asp:DropDownList ID="DropDownList1" runat="server" Enabled="false" AutoPostBack="true"></asp:DropDownList>
                        <%--<asp:Label ID="Label1" runat="server"></asp:Label>--%>
                        <asp:Button ID="ButtonRandomGetPaper" runat="server" Text="随机取卷" Style="margin-left:5px" Enabled="false" />
                        
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelGroup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <div style="margin-top:20px;text-align:left;">
                        <asp:Label ID="LabelSampleGroup" runat="server" Text="样卷分组:"></asp:Label>
                        
                        <asp:DropDownList ID="DropDownSampleGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownSampleGroup_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    
                    <div style="margin-top:20px;text-align:left;">
                        <asp:Label ID="SampleAim" runat="server" Text="培训目标:"></asp:Label>
                        <br />
                        <asp:TextBox ID="TextBoxAim" runat="server" ReadOnly="true" Height="100px" Width="100%" TextMode="MultiLine" ></asp:TextBox>
                    </div>
                     </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
                    <%--<asp:AsyncPostBackTrigger EventName=""/>--%>
                    <asp:AsyncPostBackTrigger ControlID="GridViewCheckSample" />
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
               
                    <div id="checkname" class="Title" style="margin-top:10px">
                        <asp:Label ID="LabelCheck" runat="server" Text="按试卷号取卷" ForeColor="#FFFFFF" Font-Bold="True"></asp:Label>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelCheck" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="sidebardown" style="overflow:auto; ">
                                <asp:GridView ID="GridViewCheckSample" runat="server" EnableModelValidation="True" OnRowDataBound="GridViewCheck_RowDataBound"
                                    ForeColor="#333333" AutoGenerateColumns="False" DataKeyNames="试卷号"
                                    Width="100%">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="试卷号" InsertVisible="False" SortExpression="试卷号">
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Wrap="false" />
                                            <ItemTemplate>
                                                <asp:Label ID="GridPaperNo" runat="server" Text='<%# Bind("试卷号") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="状态" DataField="状态" InsertVisible="False" ReadOnly="True">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButtonSelect" OnClick="LinkButtonSelect_Click" runat="server"
                                                    CausesValidation="False" CommandName="Select" Text="选择"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
                     <%--   <asp:AsyncPostBackTrigger ControlID="DropDownSampleGroup" />--%>
                        <asp:AsyncPostBackTrigger ControlID="GridViewCheckSample" />
                        </Triggers>
                    </asp:UpdatePanel>
                
          
                </div>
                <asp:UpdatePanel ID="UpdateImage1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <div id="content1" style="margin-left: auto; margin-right: auto;background-color:White;width:79.5%;height:180px;border: groove 1.5px #fff;border-collapse: collapse;float:left">  
                        <div id="sampleScore" style="overflow:auto;">
                            <asp:GridView ID="GridViewSampleScore" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                    Width="100%" GridLines="None" >
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                              
                                    <Columns>
                                        <asp:BoundField DataField="步骤" HeaderText="步骤" ReadOnly="True" />
                                        <asp:BoundField DataField="档次" HeaderText="档次" ReadOnly="True" />
                                        <asp:BoundField DataField="理由" HeaderText="理由" ReadOnly="True" />
                                    </Columns>
                              
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </div>   
                        </div>
                        </ContentTemplate>
                         <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDownList1" />
                    <%--  <asp:AsyncPostBackTrigger ControlID="DropDownSampleGroup" />--%>
                    <asp:AsyncPostBackTrigger ControlID="GridViewCheckSample" />
                    </Triggers>
                        </asp:UpdatePanel>
        </div>
        <div id="div4" style="clear: both; height: 4px;">
        </div>
    
    </div>
    </form>
</body>
</html>
