<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PasswordChangeViewControl.ascx.cs"
    Inherits="Users_PasswordChangeViewControl" ClassName="PasswordChangeViewControl" %>
<script type="text/javascript">
    function encryptfunc() {

        /*Old Password*/
        var oldpass = document.getElementById("<%= txtOldPassword.ClientID %>").value;
        var encoldpassword = "";
        for (var i = 0; i < oldpass.length; i++) {
            var byte = 1000 + parseInt(oldpass.charCodeAt(i));
            encoldpassword += byte.toString();
        }
        document.getElementById("<%= txtOldPassword.ClientID %>").value = encoldpassword;

        /*Password*/
        var pass = document.getElementById("<%= txtNewPassWord.ClientID %>").value;
        var encpassword = "";
        for (var i = 0; i < pass.length; i++) {
            var byte = 1000 + parseInt(pass.charCodeAt(i));
            encpassword += byte.toString();
        }        
        document.getElementById("<%= txtNewPassWord.ClientID %>").value = encpassword;

        /*Confirm Password*/
        var confirmpass = document.getElementById("<%= txtConfirmPassword.ClientID %>").value;
        var enconfirmpassword = "";
        for (var i = 0; i < confirmpass.length; i++) {
            var byte = 1000 + parseInt(confirmpass.charCodeAt(i)); 
            enconfirmpassword += byte.toString();
        }
        document.getElementById("<%= txtConfirmPassword.ClientID %>").value = enconfirmpassword;


            return true;
        }
</script>
<div style="border:1px solid #0066cc; padding:20px;">
<div class="page-header-panel" style="text-align:center">
<h2><asp:Label id="lblChangePasswordHeader" runat="server" Text="Change Password"></asp:Label></h2>
</div>
<br />
    <div id="divInfoPanel" runat="server" class="popupSuccessMsg">
        Successfully message ...
    </div>
    <div id="divErrorPanel" runat="server" class="popupErrMsg">
        Error message here...
    </div>
    <div>
        <table class="tableLayout">
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblOldPassWord" runat="server" Text="Old Password:"></asp:Label>
                    <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblNewPassWord" runat="server" Text="New Password:"></asp:Label>
                    <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtNewPassWord" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                    <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
                <td colspan="4">
                    <asp:Button id="btnUpdate" Text="Update" runat="server" OnClientClick="encryptfunc();" OnClick="btnUpdate_ServerClick"
                        class="primaryButton" />
                    <input type="button" id="btnCancel" value="ReLogin" visible="false" runat="server" onserverclick="btnCancel_ServerClick"
                        class="secondaryButton" />
                </td>
            </tr>
        </table>
    </div>
</div>
