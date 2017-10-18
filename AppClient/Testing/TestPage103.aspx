<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage103.aspx.cs" Inherits="Testing_TestPage103" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" 
            onclick="btnSendMail_Click" />
            <asp:Button ID="btnError" runat="server" Text="Error" 
            onclick="btnError_Click" />
    </div>
    </form>
</body>
</html>
