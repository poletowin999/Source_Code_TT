<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Userrestriction.aspx.cs" Inherits="Misc_Userrestriction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
  
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <div class="loginPanel" style="width: 80%; margin-left: 10%; margin-top: 15%; float: left;">
         <img src="../Images/authroize.png" alt="Unauthorized User" title="Unauthorized User" />
            <p style="font-weight: bold">
                 You are not authorized to view the page. </p>
                <p>please contact your superior for access...</p>
            <ul>
                <li>
                    <p>
                        <a href="../HomePage.aspx" >Home.</a>
                        </p>
                </li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>