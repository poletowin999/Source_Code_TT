<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UnAttachedUserView.ascx.cs"
    Inherits="Users_UnAttachedUserView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="alertError" class="popupWarnMsg" runat="server" style="display: none">
</div>
<table border="0" cellpadding="0" cellspacing="0" class="formGrid">
    
    <tr>
        <td colspan="4">
            <asp:Label ID="lblUsername" runat="Server" Text="User Name"></asp:Label>
        </td>
        <td colspan="5">
            <asp:TextBox ID="txtUserName" runat="Server"></asp:TextBox>
        </td>
        <td>
        </td>
        <td colspan="4">
            <asp:Label ID="lblRole" runat="Server" Text="Role"></asp:Label>
        </td>
        <td colspan="5">
            <asp:TextBox ID="txtRole" runat="Server"></asp:TextBox>
        </td>
        <td>
        </td>
        <td colspan="11">
            <asp:Button ID="btnFilter" runat="Server" Text="Filter" OnClick="btnFilter_Click"
                CssClass="primaryButton"></asp:Button>
            <asp:Button ID="Clear" runat="Server" Text="Clear" OnClick="Clear_Click" CssClass="secondaryButton">
            </asp:Button>
        </td>
        <td></td>
    </tr>
    
    <tr>
        <td colspan="32">
            <div id="divGridHeader" runat="server" class="gridViewPanel" style="width:100%">
                <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                    <tr>
                        <th>
                            <asp:Label runat="server" ID="lblSelect" Text="Select"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblLastNameGrid" Text="Last Name"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblFirstNameGrid" Text="First Name"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblRoleGrid" Text="Role"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblEmailId" Text="EmailId"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <span id="spnMessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="gridViewHeader" style="overflow: auto" id="divmatchedusers" runat="server"
                visible="false">
            </div>
            <%-- <asp:Label ID="lblMatchedUser" runat="Server" Text="Matched users"></asp:Label>--%>
            <div class="gridViewPanel" id="divGridview" runat="server" style="height:250px; overflow:auto;">
                <asp:GridView ID="gvwunattachedUser" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                    DataKeyNames="Id" AutoGenerateEditButton="false" EmptyDataText="User not found..!">
                    <Columns>
                                   <asp:TemplateField>
            <HeaderTemplate>
            <label>Select</label><br />
            <asp:CheckBox ID="checkselectall" runat="server" AutoPostBack="true" OnCheckedChanged="checkselectall_Click"  />
            </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="Selecteduser" />
                </ItemTemplate>
                <ItemStyle Width="40px" />
            </asp:TemplateField>

                         <asp:TemplateField HeaderText="LastName">
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastNameGrid" runat="server" Text="LastName"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="LastName">
                                <HeaderTemplate>
                                    <asp:Label ID="lblFirstNameGrid" runat="server" Text="FirstName"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("FirstName") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                        <asp:TemplateField HeaderText="Role">
                            <HeaderTemplate>
                                    <asp:Label ID="lblRoleGrid" runat="server" Text="Role"></asp:Label>
                                </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:Label ID="Role" runat="server" Text='<%# Eval("CustomData[Role]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EmailId">
                            <HeaderTemplate>
                                    <asp:Label ID="lblEmailId" runat="server" Text="EmailId"></asp:Label>
                                </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:Label ID="EmailId" runat="server" Text='<%# Eval("CustomData[EmailId]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="UserId" DataField="ID" Visible="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="32">
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblAttachto" runat="Server" Text="Attach To"></asp:Label>
        </td>
        <td colspan="5">
            <asp:Label ID="lblSelectedUsername" runat="Server" Text="Selected user"></asp:Label>
        </td>
        <td></td>
        <td colspan="22">
        </td>
        <td></td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblAttachDate" runat="Server" Text="Attach Date:"></asp:Label>
            <span class="Mandetary">*</span>
        </td>
        <td colspan="4">
            <asp:TextBox ID="txtDate" runat="server" MaxLength="10"></asp:TextBox>
            <cc1:CalendarExtender ID="calextValid" runat="server" PopupButtonID="Imgfromdt" TargetControlID="txtDate" Format="MM/dd/yyyy">
            </cc1:CalendarExtender>
        </td>
        <td colspan="2">
        <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                            AlternateText="To display calendar." ImageAlign="AbsBottom" />
                            </td>
        <td colspan="21">
            <asp:Button ID="bntAttach" runat="server" Text="Attach" CssClass="primaryButton"
                OnClick="bntAttach_Click" CausesValidation="False" />
        </td>
        <td></td>
    </tr>
</table>
<asp:HiddenField ID="HidAttachUser" runat="server" Value="" />
