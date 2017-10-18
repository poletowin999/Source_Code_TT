<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SiteMaster.master"
    AutoEventWireup="true" CodeFile="ResetPasswordView.aspx.cs" Inherits="Users_ResetPasswordView"
    Theme="Classical" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div style="border: 1px solid #0066cc; padding: 20px; top: 100px; width: 500px" class="center-panel">
                <div class="page-header-panel" style="text-align: center">
                    <h2>
                        Reset Password
                    </h2>
                </div>
                <div id="divInfoPanel" runat="server" class="popupSuccessMsg">
                    Sucess Message
                </div>
                <div id="divErrorPanel" runat="server" class="popupErrMsg">
                    Error Message
                </div>
                <div id="divResetIdErrorPanel" runat="server">
                    <span><b>For this Link Request Password Already Changed.</b></span>
                </div>
                <div id="divEditPanel" runat="server">
                    <table class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <input type="hidden" id="hdnResetId" runat="server" />
                                <label for="<%=txtEmailId.ClientID %>">
                                    EmailId :<span class="Mandetary">*</span></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtEmailId" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="EMailRegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtEmailId" Display="Dynamic" ErrorMessage="Emailid Should be in Valid Format"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label for="<%=txtNewPassword.ClientID %>">
                                    New Password :<span class="Mandetary">*</span></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label for="<%=txtConfirmPassword.ClientID %>">
                                    Confirm Password :<span class="Mandetary">*</span></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:CompareValidator ID="CompareConfirmPassword" runat="server" ControlToCompare="txtNewPassword"
                                    ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="Must Match With New Password"></asp:CompareValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
                            <td colspan="4">
                                <asp:Button ID="BtnUpdate" Text="Update" runat="server" OnClick="BtnUpdate_Click" />
                                <asp:Button ID="BtnClear" Text="Clear" runat="server" OnClick="BtnClear_Click" CssClass="secondaryButton"
                                    CausesValidation="False" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
