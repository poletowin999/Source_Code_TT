<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage104.aspx.cs" Inherits="Testing_TestPage104" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .textBox
        {
            line-height: 15pt;
            width: 90%;
            position: relative;
        }
        
        .img
        {
            width: 20px;
            height: 20px;
            vertical-align: bottom;
            position: absolute;
            right: 3px;
            float: right;
        }
        .span
        {
            display: inline-block;
            white-space: nowrap;
            border: 1px dashed #006699;
            padding: 1px;
            position: relative;
            width: 98%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:BulletedList ID="BulletedList1" runat="server" BulletStyle="NotSet" DisplayMode="Text">
            <asp:ListItem>Item 1</asp:ListItem>
            <asp:ListItem>Item 2</asp:ListItem>
            <asp:ListItem>Item 3</asp:ListItem>
        </asp:BulletedList>
        <table cellpadding="3px" border="1px" style="width: 100%; table-layout: fixed;">
            <tr>
                <td colspan="3">
                    <label>
                        Employee Name:
                    </label>
                </td>
                <td colspan="4">
                    <span class="span">
                        <input type="text" class="textBox" />
                        <img src="../Images/user20.png" alt="User" class="img" />
                    </span>
                </td>
                <td>
                </td>
                <td colspan="3">
                </td>
                <td colspan="4">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
