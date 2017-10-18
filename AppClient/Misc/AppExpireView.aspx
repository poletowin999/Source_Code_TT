<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppExpireView.aspx.cs" Inherits="Misc_AppExpireView" %>

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
            <p style="font-weight: bold">
                Your current session has been expired due to no interaction in the application.</p>



            <p>
                You can choose any one of the following:</p>
            <ul>
                <li>
                    <p>
                        <asp:HyperLink ID="hlkDefaultPage" runat="server" NavigateUrl="~/Default.aspx">Login the application.</asp:HyperLink>
                    </p>
                </li>
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
