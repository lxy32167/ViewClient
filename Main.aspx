<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Client.Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<<<<<<< HEAD
=======

>>>>>>> origin/master
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>基于大数据中心的网上阅卷系统客户端</title>
    <script type="text/javascript" src="jquery-1.9.1.js"></script>
    <script type="text/javascript" src="jquery.blockUI.js"></script>
    <link href="layout.css" rel="stylesheet" type="text/css" />
    <link href="css/gh-buttons.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
<<<<<<< HEAD
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
            position: absolute;
            cursor: move;
        }
    </style>
=======
    
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
        }
</style>
>>>>>>> origin/master
    <script type="text/javascript">
        //图片放大和缩小（兼容IE和火狐，谷歌）
        var divId;
        var v_left;
        var v_top;

        window.onload = function () {
            var images1 = document.getElementById("images1");
            divId = document.getElementById("content");
            var height1 = images1.height; //图片的高度
            var width1 = images1.width; //图片的宽度
            v_left = (document.body.clientWidth - width1) / 2;
            v_top = (document.body.clientHeight - height1) / 2;
            divId.style.left = v_left;
            divId.style.top = v_top;
<<<<<<< HEAD

        }
        window.onunload = function () {
            if (window.XMLHttpRequest) {
                xmlHttp = new XMLHttpRequest();
            }
            else {
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            if (xmlHttp) {
                xmlHttp.open("GET", "Leave.aspx", true);
                xmlHttp.onreadystatechange = getLeavedata;
                xmlHttp.send();
            }

        }
        if ($.browser.msie) {
            /*
            window.onbeforeunload = function(e){
            var tabClick = false;
            $('#tt').tabs({
            onSelect:function(title,index){
            tabClick = true;
            return true;
            }
            });
            if( !tabClick ){
            return "Leaving this page may cause loss of your code!";
            }
            };
            */
        } else {
            window.onbeforeunload = function () {

                return "请最好不要直接关闭浏览器而是先注销后再关闭!";
            };
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
            divId = document.getElementById("content");
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
                if (oDragObj.offsetHeight * zoom / 100 > divId.clientHeight) {
                    iTop = Math.max(Math.min(nn6 ? nTY + e.clientY - y : nTY + event.clientY - y, 0), (divId.clientHeight - oDragObj.offsetHeight * zoom / 100));
                    oDragObj.style.top = iTop + "px";
                }
            }

=======

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
            divId = document.getElementById("content");
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
                if (oDragObj.offsetHeight * zoom / 100 > divId.clientHeight) {
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
        $.blockUI.defaults.fadeIn = 500;
        $.blockUI.defaults.fadeOut = 500;

<<<<<<< HEAD
=======
    .nav
    {
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
        }
</style>
    <script type="text/javascript">
        //图片放大和缩小（兼容IE和火狐，谷歌）
        var divId;
        var v_left;
        var v_top;

        window.onload = function () {
            var images1 = document.getElementById("images1");
            divId = document.getElementById("content");
            var height1 = images1.height; //图片的高度
            var width1 = images1.width; //图片的宽度
            v_left = (document.body.clientWidth - width1) / 2;
            v_top = (document.body.clientHeight - height1) / 2;
            divId.style.left = v_left;
            divId.style.top = v_top;
>>>>>>> origin/master

        }
        drag = 0;
        move = 0;
        // 拖拽对象
        var ie = document.all;
        var nn6 = document.getElementById && !document.all;
        var isdrag = false;
        var y, x;
        var oDragObj;

<<<<<<< HEAD
=======
        function moveMouse(e) {
            divId = document.getElementById("content");
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
                if (oDragObj.offsetHeight * zoom / 100 > divId.clientHeight) {
                    iTop = Math.max(Math.min(nn6 ? nTY + e.clientY - y : nTY + event.clientY - y, 0), (divId.clientHeight - oDragObj.offsetHeight * zoom / 100));
                    oDragObj.style.top = iTop + "px";
                }
            }
           
          
          
>>>>>>> origin/master
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
        $.blockUI.defaults.fadeIn = 500;
        $.blockUI.defaults.fadeOut = 500;

<<<<<<< HEAD
=======
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
        function userLogin() {
            $.blockUI({ message: $('#loginDiv'), css: { width: '275px'} });
        }
        function login() {
            var i = document.getElementById('<%=hidden.ClientID %>').value;
            var j = document.getElementById('<%=hidden1.ClientID %>').value;
            var uName = $("#username").val();
            var uPwd = $("#userpwd").val();

            if (i == uName && j == uPwd) {
                $.blockUI({ message: "<h1>登录中...</h1>" });
                $.unblockUI();
                document.getElementById('username').value = '';
                document.getElementById('userpwd').value = '';
            }

            else {
                $.blockUI({ message: "<h1>密码错误...</h1>" });
                $.unblockUI();
                document.getElementById('username').value = '';
                document.getElementById('userpwd').value = '';
<<<<<<< HEAD
                setTimeout(userLogin, 1000);
=======
                setTimeout(userLogin,1000);
>>>>>>> origin/master
            }
        }
        function userChgPwd() {
            $.blockUI({ message: $('#ChgPwdDiv'), css: { width: '275px'} });
        }
        function chgpwd() {
            debugger;
            var oldpwd = $("#OldPwd").val();
            var newpwd = $("#NewPwd").val();
            var newpwd2 = $("#NewPwd2").val();

            if (oldpwd.length == 0) {
                $.blockUI({ message: "<h1>原密码未输入!</h1>" });
                $.unblockUI();
                return false;
            }
            else if (newpwd.length == 0 || newpwd2.length == 0) {
                $.blockUI({ message: "<h1>请输入新密码!</h1>" });
                $.unblockUI();
                return false;
            }
            else if (newpwd != newpwd2) {
                $.blockUI({ message: "<h1>密码不一致!</h1>" });
                $.unblockUI();
                return false;
            }
            else if (newpwd.length < 4) {
                $.blockUI({ message: "<h1>密码至少需要4位!</h1>" });
                $.unblockUI();
                return false;
            }
            else if (newpwd.length > 16) {
                $.blockUI({ message: "<h1>密码不能超过16位!</h1>" });
                $.unblockUI();
                return false;
            }
            else if (oldpwd != null && newpwd != null) {
                $.unblockUI();
                if (window.XMLHttpRequest) {
                    xmlHttp = new XMLHttpRequest();
                }
                else {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                if (xmlHttp) {
                    xmlHttp.open("GET", "validatePwd.aspx?oldpwd=" + oldpwd + "&newpwd=" + newpwd, true);
                    xmlHttp.onreadystatechange = getdata;
                    xmlHttp.send();
                }
            }
        }
        function getdata() {
            debugger;
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                alert(xmlHttp.responseText);
            }
        }
        function showInfo() {
            $.blockUI({ message: $('#DivInfo'), css: { width: '275px'} });
        }
        function submitInfo() {
            var truename = $("#TrueName").val();
            var workplace = $("#WorkPlace").val();
            truename = escape(truename);
            workplace = escape(workplace);
            if (truename.length == 0) {
                alert("请输入真实姓名!");
                $.unblockUI();
                return false;
            }
            if (workplace.length == 0) {
                alert("请输入工作单位!");
                $.unblockUI();
                return false;
            }
            else if (truename.length != 0 && workplace.length != 0) {
                $.unblockUI();
                if (window.XMLHttpRequest) {
                    xmlHttp = new XMLHttpRequest();
                }
                else {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                if (xmlHttp) {
                    xmlHttp.open("GET", "info.aspx?truename=" + truename + "&workplace=" + workplace, true);
                    xmlHttp.onreadystatechange = getdata;
                    xmlHttp.send();
                }
            }

        }
        function getdata() {
            debugger;
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                alert(xmlHttp.responseText);
                document.getElementById("LabelLoginName").innerText = xmlHttp.responseText;
            }
        }
        Date.prototype.DatePart = function (interval) {
            var myDate = new Date();
            var partStr = '';
            var Week = ['日', '一', '二', '三', '四', '五', '六'];
            switch (interval) {
                case 'y': partStr = myDate.getFullYear(); break;
                case 'm': partStr = myDate.getMonth() + 1; break;
                case 'd': partStr = myDate.getDate(); break;
                case 'w': partStr = Week[myDate.getDay()]; break;
                case 'ww': partStr = myDate.WeekNumOfYear(); break;
                case 'h': partStr = checkTime(myDate.getHours()); break;
                case 'n': partStr = checkTime(myDate.getMinutes()); break;
                case 's': partStr = checkTime(myDate.getSeconds()); break;
            }
            return partStr;
        }
        function checkTime(i) {
            if (i < 10) {
                i = "0" + i;
            }
            return i;
        }
        function getCurrentTime() {
            var cur_time = "";
            cur_time += Date.prototype.DatePart("y") + "年";
            cur_time += Date.prototype.DatePart("m") + "月";
            cur_time += Date.prototype.DatePart("d") + "日 ";
            cur_time += "星期" + Date.prototype.DatePart("w") + " ";
            cur_time += Date.prototype.DatePart("h") + ":";
            cur_time += Date.prototype.DatePart("n") + ":";
            cur_time += Date.prototype.DatePart("s");
            document.getElementById("b_current_time").innerHTML = cur_time;
            setTimeout(getCurrentTime, 1000);
        }

        function DaAn() {
            window.open("Answer.aspx", "answer", "status=no,menubar=no,toolbar=no,Location=no,Direction=no,scrollbars=yes");
        }
        function yichang() {
            window.open("excpReport.aspx", "window", "status=no,menubar=no,toolbar=no,Scrollbars=no,Location=no,Direction=no,resizable=no,Width=320px,Height=200px");
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        function YangJuan() {
            $.ajax({
                type: "Post",
                url: "Main.aspx/EnableSample",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //返回的数据用data.d获取内容     
                    if (data.d == "ok") {
                        window.open("SampleTotal.aspx", "window", "status=no,menubar=no,toolbar=no,Scrollbars=no,Location=no,Direction=no,Width=1200px,Height=800px");
                    }
                    else {
                        alert(data.d);
                    }
                },
                error: function (err) {
                    alert(err);
                }
<<<<<<< HEAD
            });
            //            window.open("SampleTotal.aspx", "window", "status=no,menubar=no,toolbar=no,Scrollbars=no,Location=no,Direction=no,Width=1200px,Height=800px");
=======
            });     
//            window.open("SampleTotal.aspx", "window", "status=no,menubar=no,toolbar=no,Scrollbars=no,Location=no,Direction=no,Width=1200px,Height=800px");
=======
        function info() {
            window.open("PInfo.aspx", "window", "status=no,menubar=no,toolbar=no,Scrollbars=no,Location=no,Direction=no,resizable=no,Width=320px,Height=200px");
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
        }
        function checkLeave() {
            if (confirm("确认要注销吗?")) {
                Leave();
                window.location.href = "Login.aspx"; 
            }
        }
        function Leave() {
            if (window.XMLHttpRequest) {
                xmlHttp = new XMLHttpRequest();
            }
            else {
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            if (xmlHttp) {
                xmlHttp.open("GET", "Leave.aspx", true);
                xmlHttp.onreadystatechange = getLeavedata;
                xmlHttp.send();
            }
        }
        function getLeavedata() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            }
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

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        var isIe = (document.all) ? true : false;
        function keydown(e) {
            var currentForm = document.getElementById('form1');
            var table = document.getElementById('GridViewScore');

            var e = e || event;
            var currKey = e.keyCode || e.which || e.charCode;
            if (currKey == 13) {
                var el = e.srcElement || e.target;
                if (el.tagName.toLowerCase() == "input" && el.type != "submit") {
                    if (isIe) {
                        currKey = 9;
                    }
                    else {
                        if (el.parentNode.parentNode.rowIndex == table.rows.length - 1) {

                        }
                        else {
                            nextCtl(el).focus();
                            e.preventDefault();
                        }
                    }
                }
            }
        }
        function nextCtl(ctl) {
            var form = ctl.form;
            for (var i = 0; i < form.elements.length - 1; i++) {
<<<<<<< HEAD
                if (ctl == form.elements[i]) {
                    return form.elements[i + 1];
                }
=======
                if(ctl == form.elements[i]){
                    return form.elements[i+1];
                } 
>>>>>>> origin/master
            }
            return ctl;
        }

<<<<<<< HEAD
=======
=======
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
      
    </script>
</head>
<body onload="getCurrentTime();">
    <form id="form1" runat="server">
    <div id="loginDiv" style="display: none; cursor: default">
        <h3>
            请输入密码解锁！</h3>
        <table style="width: 300px; text-align: center">
            <tr>
                <td>
                    用户名
                </td>
                <td>
                    <input type="text" id="username" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td>
                    密码
                </td>
                <td>
                    <input type="password" id="userpwd" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" id="yes" onclick="login()" value="登录" />
                </td>
            </tr>
        </table>
    </div>
    <div id="ChgPwdDiv" style="display: none; cursor: default">
        <h3>
            请输入原密码和新密码!</h3>
        <table style="width: 300px; text-align: center">
            <tr>
                <td>
                    原密码
                </td>
                <td>
                    <input type="password" id="OldPwd" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td>
                    新密码
                </td>
                <td>
                    <input type="password" id="NewPwd" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td>
                    确认密码
                </td>
                <td>
                    <input type="password" id="NewPwd2" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input id="Button2" onclick="chgpwd()" type="button" value="确认" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DivInfo" style="display: none; cursor: default">
        <h3>
            请输入个人信息！</h3>
        <table style="width: 300px; text-align: center">
            <tr>
                <td>
                    真实姓名
                </td>
                <td>
                    <input type="text" id="TrueName" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td>
                    工作单位
                </td>
                <td>
                    <input type="text" id="WorkPlace" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" id="ButtonInfo" onclick="submitInfo()" value="提交" />
                </td>
            </tr>
        </table>
    </div>
    <div id="container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <label id="lblMsg" runat="server">
        </label>
        <input type="hidden" name="hidden" id="hidden" runat="server" />
        <input type="hidden" name="hidden1" id="hidden1" runat="server" />
        <div id="header_two">
            <fieldset>
                <legend>系统菜单</legend>
                <asp:UpdatePanel ID="UpdateSystemMenu" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
<<<<<<< HEAD
                        <table cellpadding="0" cellspacing="0" width="100%" border="0">
=======
                        <table cellpadding="0" cellspacing="0" width="100%" border="0">    
>>>>>>> origin/master
                            <tr>
                                <td>
                                    <table class="ContentBorder" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:MultiView ID="mvCompany" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="View1" runat="server">
<<<<<<< HEAD
                                                        <div class="button-group">
                                                            <%-- <asp:Button ID="ButtonStart" runat="server" Text="开始阅卷" Style="margin-right: 10px" />--%>
                                                            <button id="Buttonyichang" class="button icon arrowup" onclick="yichang()">提交异常卷</button>
                                                            <button id="Buttonpingfen" type="button" class="button icon log" onclick="DaAn()">查看评分依据</button>
                                                            <button id="Buttonyangjuan" type="button" onclick="YangJuan()" class="button icon favorite" >样卷库浏览</button> 
                                                            <!--               <asp:Button ID="Buttonhongkuang" CssClass="Button" runat="server" Text="去除套红框" Style="margin-right: 10px" /> -->
                                                            <button id="Buttonxinxi" type="button" class="button icon user" onclick="showInfo()" >用户信息登记</button> 
                                                            <button id="Buttonshuaxin" type="button" class="button icon loop" >设置刷新时间</button> 
                                                            <button id="Buttonmima" type="button"  class="button icon key" onclick="userChgPwd();" >修改用户密码</button>
                                                            <button id="Buttonsuoding" type="button" class="button icon lock" onclick="userLogin()" >锁定</button> 
                                                            <button ID="Buttonzhuxiao"  class="button icon home" onclick="checkLeave()" >用户注销</button>
                                                        </div>
=======
                                                        <%-- <asp:Button ID="ButtonStart" runat="server" Text="开始阅卷" Style="margin-right: 10px" />--%>
                                                        <input id="Buttonyichang" type="button" value="提交异常卷" onclick="yichang()" style="margin-right: 10px" />
                                                        <input id="Buttonpingfen" type="button" value="查看评分依据" onclick="DaAn()" style="margin-right: 10px" />
<<<<<<< HEAD
                                                        <input id="Buttonyangjuan" type="button" value="样卷库浏览" onclick="YangJuan()" style="margin-right: 10px" />
=======
                                                        <asp:Button ID="Buttonyangjuan" CssClass="Button" runat="server" Text="样卷库浏览" Style="margin-right: 10px" />
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
                                                        <!--               <asp:Button ID="Buttonhongkuang" CssClass="Button" runat="server" Text="去除套红框" Style="margin-right: 10px" /> -->
                                                        <input id="Buttonxinxi" type="button" value="用户信息登记" style="margin-right: 10px" onclick="showInfo()" />
                                                        <input id="Buttonshuaxin" type="button" value="设置刷新时间" style="margin-right: 10px" />
                                                        <input id="Buttonmima" type="button" value="修改用户密码" style="margin-right: 10px" onclick="userChgPwd();" />
                                                        <input id="Buttonsuoding" type="button" value="锁定" style="margin-right: 10px" onclick="userLogin()" />
<<<<<<< HEAD
                                                        <asp:Button ID="Buttonzhuxiao" runat="server" Text="用户注销" Style="margin-right: 10px"
                                                            OnClick="Buttonzhuxiao_Click" />
=======
                                                        <asp:Button ID="Buttonzhuxiao" CssClass="Button" runat="server" Text="用户注销" Style="margin-right: 10px"
                                                            OnClick="Buttonzhuxiao_Click" />
                                                    </asp:View>
                                                    <asp:View ID="View2" runat="server">
                                                    </asp:View>
                                                    <asp:View ID="View3" runat="server">
                                                        该网上阅卷系统基于ASP.NET开发,和原delphi开发的C/S版客户端互相辅助,完成阅卷。
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
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
                </asp:UpdatePanel>
            </fieldset>
<<<<<<< HEAD
            <%-- <menu type="toolbar" class="main">
=======
           <%-- <menu type="toolbar" class="main">
>>>>>>> origin/master
                <ul class="main_ul">
                    <li>
                        <bzb:Zooming ID="Zooming1" runat="server" />
                    </li>
                    <li>
                        <bpb:BasicProcessing ID="BasicProcessing1" runat="server" />
                    </li>
                </ul>
            </menu>--%>
        </div>
        <div id="mainContent">
            <asp:UpdatePanel ID="UpdateImage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
<<<<<<< HEAD
                    <div id="content" style="margin-left: auto; margin-right: auto; background-color: White">
                        <asp:Image ID="images1" runat="server" CssClass="dragAble" onmousewheel="return bbimg(this)" />
=======
                    <div id="content" style="margin-left: auto; margin-right: auto;background-color:White">
                        <asp:Image ID="images1" runat="server" CssClass="dragAble" onmousewheel="return bbimg(this)"/>
>>>>>>> origin/master
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonSubmit" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonBlank" />
                    <asp:AsyncPostBackTrigger ControlID="GridViewCheck" />
                    <asp:AsyncPostBackTrigger ControlID="Buttontmp" />
                </Triggers>
            </asp:UpdatePanel>
            <div id="sidebar" style="text-align: center;">
                <div id="givescore" class="Title">
                    <asp:Label ID="LabelScore" runat="server" ForeColor="#FFFFFF" Text="试卷给分区" Font-Bold="True"></asp:Label>
                </div>
                <asp:UpdatePanel ID="UpdateScore" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
<<<<<<< HEAD
                        <div id="sidebarup" style="overflow: auto; width: 100%">
=======
                        <div id="sidebarup" style="overflow: auto; width:100% ">
>>>>>>> origin/master
                            <asp:GridView ID="GridViewScore" runat="server" EnableModelValidation="True" AllowPaging="True"
                                AutoGenerateColumns="False" DataKeyNames="步骤" CellPadding="4" ForeColor="#333333"
                                Width="100%" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="False" />
                                    <asp:BoundField DataField="步骤" HeaderText="步骤" InsertVisible="False" ReadOnly="True"
                                        SortExpression="步骤" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="分数" SortExpression="分数">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtScore" runat="server" Text='<%# Eval("分数") %>' Height="16px"
                                                Width="30px" AutoCompleteType="Disabled" onkeydown="keydown()"></asp:TextBox>
                                            <%--<cc1:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtScore" Display="None" ValidateEmptyText="true" OnServerValidate="CustomValidator1_ServerValidate"></cc1:CustomValidator>--%>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtScore"
                                                Display="None" ValidateEmptyText="True" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="满分" HeaderText="满分" InsertVisible="False" ReadOnly="True"
                                        SortExpression="满分" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="允许给半分" HeaderText="允许给半分" InsertVisible="False" Visible="false"
                                        ReadOnly="True" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonSubmit" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonBlank" />
                        <asp:AsyncPostBackTrigger ControlID="Buttontmp" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewCheck" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel_submit" runat="server">
                    <ContentTemplate>
<<<<<<< HEAD
                        <div>
                            <div id="submitbutton">
                                <ul class="button-group" style="margin: 5px">
                                    <li>
                                        <asp:Button ID="ButtonSubmit" runat="server" CssClass="button pill" Text="分数提交" OnClick="ButtonSubmit_Click" /></li>
                                    <li>
                                        <asp:Button ID="ButtonBlank" runat="server" CssClass="button pill" Text="空白卷" OnClick="ButtonBlank_Click" /></li>
                                    <li>
                                        <input id="ButtonError" class="button pill" type="button" value="提交异常卷" onclick="yichang()" /></li>
                                </ul>
                            </div>
                            <div id="yichangbutton" style="text-align: center">
                                <asp:Button ID="Buttontmp" runat="server" Text="Button" Style="display: none" OnClick="Buttontmp_Click" />
                                <%--  <asp:Button ID="Buttoninfo" runat="server" Text="Button" Style="display: none" OnClick="Buttoninfo_Click" />--%>
                            </div>
=======
                      <div>
                        <div id="submitbutton">
<<<<<<< HEAD
                            <ul class="button-group" style="margin:5px">
                                <li><asp:Button ID="ButtonSubmit" runat="server" CssClass="button pill"
                                Text="分数提交" OnClick="ButtonSubmit_Click" /></li>
                                <li><asp:Button ID="ButtonBlank" runat="server" CssClass="button pill"
                                Text="空白卷" OnClick="ButtonBlank_Click" /></li>
                                <li><input id="ButtonError" class="button pill" type="button"
                                value="提交异常卷" onclick="yichang()" /></li>
                            </ul>
                            
                            
                        </div>
                        <div id="yichangbutton" style="text-align: center">
                            
                            <asp:Button ID="Buttontmp" runat="server" Text="Button" Style="display: none" OnClick="Buttontmp_Click" />
                          <%--  <asp:Button ID="Buttoninfo" runat="server" Text="Button" Style="display: none" OnClick="Buttoninfo_Click" />--%>
=======
                            <asp:Button ID="ButtonSubmit" runat="server" Style="margin-right: 10px; margin-top: 3px"
                                Text="分数提交" OnClick="ButtonSubmit_Click" />
                            <asp:Button ID="ButtonBlank" runat="server" Style="margin-left: 10px; margin-top: 3px"
                                Text="空白卷" OnClick="ButtonBlank_Click" />
                        </div>
                        <div id="yichangbutton" style="text-align: center">
                            <input id="ButtonError" style="margin-top: 10px; margin-bottom: 5px" type="button"
                                value="提交异常卷" onclick="yichang()" />
                            <asp:Button ID="Buttontmp" runat="server" Text="Button" Style="display: none" OnClick="Buttontmp_Click" />
                            <asp:Button ID="Buttoninfo" runat="server" Text="Button" Style="display: none" OnClick="Buttoninfo_Click" />
>>>>>>> b079695ce7b9036f23551636c3c43de9cdf2fac1
>>>>>>> origin/master
                        </div>
                      </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="checkname" class="Title">
                    <asp:Label ID="LabelCheck" runat="server" Text="试卷重查区" ForeColor="#FFFFFF" Font-Bold="True"></asp:Label>
                </div>
                <asp:UpdatePanel ID="UpdatePanelCheck" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="sidebardown" style="overflow: auto;">
                            <asp:GridView ID="GridViewCheck" runat="server" EnableModelValidation="True" OnRowDataBound="GridViewCheck_RowDataBound"
                                ForeColor="#333333" AutoGenerateColumns="False" DataKeyNames="试卷号" AllowPaging="True"
                                PageSize="3" Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField HeaderText="序号" DataField="序号" InsertVisible="False" ReadOnly="True">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="试卷号" InsertVisible="False" SortExpression="试卷号">
                                        <HeaderStyle Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                        <ItemTemplate>
                                            <asp:Label ID="GridPaperNo" runat="server" Text='<%# Bind("试卷号") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="分数" DataField="分数" InsertVisible="False" ReadOnly="True">
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
<<<<<<< HEAD
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
=======
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center"/>
>>>>>>> origin/master
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonSubmit" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonBlank" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewCheck" />
                    </Triggers>
                </asp:UpdatePanel>
                <div id="qty" class="Title">
                    <asp:Label ID="LabelQty" runat="server" Text="质量信息" ForeColor="#FFFFFF" Font-Bold="True"></asp:Label>
                </div>
                <div id="qtylabel" class="Title">
                    <asp:Label ID="lbTips" runat="server" Font-Bold="true" ForeColor="#000000"></asp:Label>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanellbCount" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lbCount" runat="server" Font-Bold="true" ForeColor="#000000"></asp:Label>
                            <div id="lbTotal" class="Bar" runat="server">
                                <div id="lbBlkOK" style="float: left; display: inline;" runat="server">
                                    <div id="lbPerOK" style="float: left; display: inline;" runat="server">
                                        <div id="lbEffective" style="float: left; display: inline;" runat="server">
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both">
                                </div>
                            </div>
                            <div id="Div1" class="Title" style="overflow: auto">
                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <table id="Table2" runat="server" cellpadding="0" cellspacing="0" width="100%" border="0">
                                                <tr style="height: 22px">
                                                    <td class="SelectedTopBorder" id="Td1" align="center">
                                                        <asp:LinkButton ID="LinkButtonSpeed" runat="server" OnClick="lButtonSpeed_Click">阅卷速度(份/分钟)</asp:LinkButton>
                                                    </td>
                                                    <td class="SepBorder" style="width: 2px; height: 22px;">
                                                    </td>
                                                    <td class="TopBorder" id="Td2" align="center">
                                                        <asp:LinkButton ID="LinkButtonAvg" runat="server" OnClick="lButtonAvg_Click">平均分</asp:LinkButton>
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
                                                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                                            <asp:View ID="View4" runat="server">
                                                                <asp:Chart ID="Chart1" runat="server" Height="200px" Width="250px" Palette="BrightPastel"
                                                                    BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                                                                    BorderWidth="2" BackColor="#D3DFF0" BorderColor="26, 59, 105">
                                                                    <Legends>
                                                                        <asp:Legend LegendStyle="Row" IsTextAutoFit="False" Docking="Bottom" Name="Default"
                                                                            BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" Alignment="Far">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Series>
                                                                        <asp:Series MarkerSize="1" Name="个人" ChartType="Spline" MarkerStyle="Circle" BorderColor="180, 26, 59, 105"
                                                                            ShadowOffset="1">
                                                                        </asp:Series>
                                                                        <asp:Series MarkerSize="1" Name="题组" ChartType="Spline" MarkerStyle="Circle" BorderColor="180, 26, 59, 105"
                                                                            ShadowOffset="1">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                                                            BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                                                                            BackGradientStyle="TopBottom">
                                                                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                            <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False" IsStartedFromZero="False">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>
                                                            </asp:View>
                                                            <asp:View ID="View5" runat="server">
                                                                <asp:Chart ID="Chart2" runat="server" Height="200px" Width="250px" Palette="BrightPastel"
                                                                    BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                                                                    BorderWidth="2" BackColor="#D3DFF0" BorderColor="26, 59, 105">
                                                                    <Legends>
                                                                        <asp:Legend LegendStyle="Row" IsTextAutoFit="False" Docking="Bottom" Name="Default"
                                                                            BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" Alignment="Far">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Series>
                                                                        <asp:Series MarkerSize="1" Name="个人" ChartType="Spline" MarkerStyle="Circle" BorderColor="180, 26, 59, 105"
                                                                            ShadowOffset="1">
                                                                        </asp:Series>
                                                                        <asp:Series MarkerSize="1" Name="题组" ChartType="Spline" MarkerStyle="Circle" BorderColor="180, 26, 59, 105"
                                                                            ShadowOffset="1">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                                                            BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                                                                            BackGradientStyle="TopBottom">
                                                                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                                                WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                            <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False" IsStartedFromZero="False">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>
                                                            </asp:View>
                                                        </asp:MultiView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Timer ID="TimerlbCount" runat="server" Interval="60000" OnTick="ShowPerQty">
                            </asp:Timer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div id="div4" style="clear: both; height: 4px;">
        </div>
        <div id="footer">
            <b id="b_current_time" style="color: white" class="FootDate"></b>
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdateLabelName" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="LabelName" runat="server" ForeColor="#FFFFFF" Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelPaperNo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="LabelPaperNo" Style="margin-left: 10px" runat="server" ForeColor="#FFFFFF"
                                    Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelStatus" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="LabelStatus" Style="margin-left: 10px" runat="server" ForeColor="#FFFFFF"
                                    Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelPaperNum" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="LabelPaperNum" Style="margin-left: 10px" runat="server" ForeColor="#FFFFFF"
                                    Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelLoginName" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="LabelLoginName" Style="margin-left: 40px" runat="server" ForeColor="#FFFFFF"
                                    Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
