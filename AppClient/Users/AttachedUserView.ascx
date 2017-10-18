<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttachedUserView.ascx.cs"
    Inherits="Users_AttachedUserView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="alertError" class="popupWarnMsg" runat="server" style="display: none">
</div>
<div id="divGridHeader" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" class="gridView">
        <tr>
            <th>
                <asp:Label runat="server" ID="lblSelectGridd" Text="Select"></asp:Label>
            </th>
            <th>
                <asp:Label runat="server" ID="lblLastNameGrid" Text="Last Name"></asp:Label>
            </th>
            <th>
                <asp:Label runat="server" ID="lblFirstName" Text="First Name"></asp:Label>
            </th>
            <th>
                <asp:Label runat="server" ID="lblRoleGridd" Text="Role"></asp:Label>
            </th>
            <th>
                <asp:Label runat="server" ID="lblReportinguser" Text="Reporting user"></asp:Label>
            </th>
        </tr>

    </table>
</div>
<div class="gridViewPanel" id="divGridview" runat="server" style="border-bottom: 0px solid #ccc">
    <div style="overflow: auto; height: 250px; border-bottom: 1px solid #e8e8e8">
        <asp:GridView ID="gvwattachedUser" runat="server" AutoGenerateColumns="false" CssClass="gridView"
            DataKeyNames="Id" AutoGenerateEditButton="false" EmptyDataText="No User assigned..!">
            <EmptyDataRowStyle ForeColor="#990033" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblSelectGridd" runat="server" Text="Select"></asp:Label><br />
                        <asp:CheckBox ID="checkselectall" runat="server" AutoPostBack="true" OnCheckedChanged="checkselectall_Click" />
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

                <asp:TemplateField HeaderText="FirstName">
                    <HeaderTemplate>
                        <asp:Label ID="lblFirstName" runat="server" Text="FirstName"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("FirstName") %>
                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Role">
                    <HeaderTemplate>
                        <asp:Label ID="lblRoleGridd" runat="server" Text="FirstName"></asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Width="20%" />
                    <ItemTemplate>
                        <asp:Label ID="Role" runat="server" Text='<%# Eval("CustomData[Role]") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblReportinguser" runat="server" Text="FirstName"></asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Width="20%" />
                    <ItemTemplate>
                        <asp:Label ID="Reportinguser" runat="server" Text='<%# Eval("CustomData[Reporting user]") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="UserId" DataField="ID" Visible="false" />
            </Columns>
        </asp:GridView>
    </div>
    <tr>
        <td colspan="9">
            <span id="spnMessage" runat="server"></span>
        </td>
    </tr>
    <br />
    <table class="tableLayout" style="height: 40px">
        <tr>
            <td colspan="3">&nbsp;
            <asp:Label ID="lblDetachDate" runat="Server" Text="Detach Date:"></asp:Label>
                <span class="Mandetary">*</span>
            </td>
            <td colspan="3" style="width: 100px">
                <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="95px"></asp:TextBox>
                <cc1:CalendarExtender ID="calextValid" PopupButtonID="Imgfromdt" runat="server" TargetControlID="txtDate"
                    Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
            </td>
            <td colspan="2">
                <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                    AlternateText="To display calendar." ImageAlign="AbsBottom" />
            </td>
            <td colspan="14">
                <asp:Button ID="bntDetach" runat="server" Text="Detach" OnClick="bntDetach_Click"
                    CssClass="primaryButton" />
            </td>
            <td></td>
        </tr>
    </table>
    <br />
</div>

<asp:HiddenField ID="Hidparentid" Value="" runat="server" />
