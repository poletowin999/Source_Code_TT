<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SiteMaster.master"
    AutoEventWireup="true" CodeFile="Default.aspx.cs" Theme="Classical" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function displayError(text) {

            var errorPanel = document.getElementById("<%=divMessagePanel.ClientID%>");
            if (errorPanel == null) return;
            errorPanel.innerHTML = text;
            showErrorPanel();


        }
        function showErrorPanel() {
            var errorPanel = document.getElementById("<%=divMessagePanel.ClientID%>");
            if (errorPanel == null) return;

            errorPanel.style.visibility = "visible";
            errorPanel.style.display = "block";

        }
        function hideErrorPanel() {
            var errorPanel = document.getElementById("<%=divMessagePanel.ClientID%>");
            if (errorPanel == null) return;

            errorPanel.style.visibility = "hidden";
            errorPanel.style.display = "none";

        }

        function Validatecontrols_onclick() {
            var strUserName = document.getElementById("<%=txtUserName.ClientID%>").value;
            var strPassword = document.getElementById("<%=txtPassword.ClientID%>").value;
            var errorPanel = document.getElementById("<%=divMessagePanel.ClientID%>");
            var userName = document.getElementById("<%=txtUserName.ClientID%>");
            var Password = document.getElementById("<%=txtPassword.ClientID%>");

            if (!/\S/.test(strUserName)) {
                displayError("Login name should not be blank.");
                userName.focus();
                return false;
            }
            else if (strPassword.length == 0) {
                displayError("Password should not be blank");
                Password.focus();
                return false;
            }
            hideErrorPanel();
        }

        function validUser(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key >= 32 && key < 46) || (key > 57 && key < 65)
                || (key > 90 && key < 93))) {
                if (evt.preventDefault)
                    evt.preventDefault();
                else
                    evt.returnValue = false;
            }
        }

        function encryptfunc() {
            var x = document.getElementById("<%= txtPassword.ClientID %>").value;
            var encpassword = "";
            for (var i = 0; i < x.length; i++) {
                //alert(x.charCodeAt(i));
                var byte = 1000 + parseInt(x.charCodeAt(i));
                encpassword += byte.toString();
            }
            //alert(encpassword);
            document.getElementById("<%= txtPassword.ClientID %>").value = encpassword;
            return true;
        }

        function blurName1(theControl, evt) {
            var checkOK = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890._";
            var checkStr = theControl.value;
            var allValid = true;
            var validValue = "";
            var ErrorValue = "";
            for (i = 0; i < checkStr.length; i++) {
                ch = checkStr.charAt(i);
                for (j = 0; j < checkOK.length; j++) {
                    allValid = false;
                    if (ch == checkOK.charAt(j)) {
                        validValue = validValue + ch;
                        allValid = true;
                        break;
                    }
                }
                if (!allValid) {
                    ErrorValue = ErrorValue + ch;
                }
            }
            theControl.value = validValue;
            if (ErrorValue.length > 0) {
                if (evt.target) {
                    evt.target.focus();
                    evt.target.select();
                }
                else {
                    evt.srcElement.focus();
                    evt.srcElement.select();
                }
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!--div style="position:absolute; right:10px; padding:5px 0px 0px 0px;"><a target="_blank" href="HelpPages/Help_LoginPage.html"><img border="0" src="Images/help.png" alt="Help Icon" title="help Icon" /></a></div-->
    <div class="center-panel" style="width: 370px; top: 100px;">
        <div class="loginPanel">
            <div class="loginHeader">
                <span class="signIn">Sign In</span>
            </div>
            <div id="divMessagePanel" runat="server" class="login-error">
            </div>
            <div id="divSuccessPanel" runat="server" class="login-succeed">
            </div>
            <table class="tableLayout" cellspacing="0" border="0px">
                <tr>
                    <td valign="bottom">
                        <label class="login-label">
                            User Name
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="login-textbox" onkeypress="validUser(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <label class="login-label">
                            Password
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--  <asp:TextBox ID="txtPassword" runat="server" CssClass="login-textbox" TextMode="Password"
                            onkeypress="validUser(event);" onchange="return blurName1(this,event);" OnPreRender="txtPassword_PreRender"></asp:TextBox>--%>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="login-textbox" TextMode="Password"
                            OnPreRender="txtPassword_PreRender"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="bottom">
                        <label class="login-label">
                            Language
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                            <asp:DropDownList ID="drpLanguageId" runat="server" Height="30px">
                                <asp:ListItem Value="0" Text="--- Select a language ---"></asp:ListItem>
                                <asp:ListItem Value="1" Text="English"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Korean"></asp:ListItem>
                                <%--<asp:ListItem Value="4" Text="Japanese"></asp:ListItem>--%>
                                <asp:ListItem Value="5" Text="Chinese"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Treditional Chinese"></asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <span class="forgotPassword">
                <asp:LinkButton ID="lnkForgetPassword" Text="Forgot Password" runat="server" OnClick="lnkForgetPassword_Click"></asp:LinkButton>
            </span>
            <div class="clear">
            </div>
            <div class="commandPanel">
                <input id="btnLogin" type="submit" runat="server" value="Login" class="primaryButton" onclick="return encryptfunc()"
                    onserverclick="btnLogin_Click" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>

