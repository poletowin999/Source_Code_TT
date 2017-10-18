<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Authorization.aspx.cs" Inherits="Misc_Authorization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
    <script type="text/javascript">
        // <!--[CDATA[

        function closeWindow() {
            window.opener = self;
            window.close();
            return false;
        }

        // ]]>
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <div class="loginPanel" style="width: 80%; margin-left: 10%; margin-top: 15%; float: left;">
        <img src="../Images/unauthorise.png" alt="Unauthorized" title="Unauthorized" />
            <p style="font-weight: bold">
                You don't have access to the site. </p>
                <p>please contact the site administrator for access...</p>
            <ul>
                <li>
                    <p>
                        <a href="javascript:void(0);" onclick="javascript:closeWindow();">Close the application.</a></p>
                </li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
