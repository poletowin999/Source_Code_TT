<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordChangeLogin.aspx.cs" Inherits="Users_PasswordChangeLogin" Theme="Classical" %>

<%@ Register Src="PasswordChangeViewControl.ascx" TagName="PasswordChangeViewControl" TagPrefix="e4e" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Password</title>
    <style type="text/css">
        .center-panel
        {
            position: relative;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/jQuery/jquery-1.6.3.js" />
                <asp:ScriptReference Path="~/Scripts/jQuery/jquery-ui-1.8.16.custom.js" />
            </Scripts>
        </cc1:ToolkitScriptManager>
        <div style="top: 100px; width: 500px" class="center-panel">
            <asp:UpdatePanel ID="UpdateEditPanel" runat="server">
                <ContentTemplate>
                    <e4e:PasswordChangeViewControl ID="PasswordChangeViewControl1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
