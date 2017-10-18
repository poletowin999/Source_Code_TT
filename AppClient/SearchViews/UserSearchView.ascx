<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSearchView.ascx.cs"
    Inherits="SearchViews_UserSearchView" %>


<asp:UpdatePanel ID="UpdateUserSearch" runat="server">
    <ContentTemplate>
        <asp:PlaceHolder ID="PlcUser" runat="server">
            <div>
                <div id="divMessage" runat="server" class="popupWarnMsg" style="display: none">
                </div>
                <table class="formGrid">
                    <tr>
                        <td colspan="14">
                            <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label>

                        </td>
                        <td></td>
                        <td colspan="14">
                            <asp:Label ID="lblNamePopup" runat="server" Text="Name:"></asp:Label>
                        </td>
                        <td></td>
                        <td colspan="14">
                            <asp:Label ID="lblRole" runat="server" Text="Role:"></asp:Label>
                        </td>
                        <td></td>
                        <td colspan="14">
                            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>

                        </td>
                        <td></td>
                        <td colspan="18"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="14">
                            <asp:DropDownList ID="ddlLocation" runat="server" onkeypress="searchKeyPress(event);">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                        <td colspan="14">
                            <input type="text" id="txtName" runat="server" onkeypress="searchKeyPress(event);" />
                        </td>
                        <td></td>
                        <td colspan="14">
                            <input type="text" id="txtRole" runat="server" onkeypress="searchKeyPress(event);" />
                        </td>
                        <td></td>
                        <td colspan="14">
                            <input type="text" id="txtEmail" runat="server" onkeypress="searchKeyPress(event);" />
                        </td>
                        <td colspan="17">
                            <span style="padding: 0px 2px 0px 0px">
                                <asp:Button ID="btnSearchPopup" runat="server" Width="70px" Text="Filter" UseSubmitBehavior="true"
                                    CssClass="primaryButton" OnClick="btnSearch_Click" ClientIDMode="Static" /></span> <span style="padding: 0px 0px 0px 0px">
                                        <asp:Button ID="btnClearPopup" runat="server" Width="70px" Text="Clear" UseSubmitBehavior="true"
                                            CssClass="secondaryButton" OnClick="btnClear_Click" ClientIDMode="Static" /></span>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="rgtflt">
                <asp:Button ID="LknRefersh" Text="Refresh" runat="server" OnClick="LnkRefersh_Click" />
            </div>
            <div class="clearFloat">
            </div>
            <div class="gridViewPanel" style="height: 250px; overflow: auto;">
                <div id="dvinitalvalue" runat="server" class="gridViewPanel">
                    <table border="0" cellpadding="0" cellspacing="0" class="gridView">
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblFirstName" Text="First Name"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblLastName" Text="Last Name"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblEmailId" Text="Email Id"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblGender" Text="Gender"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblRoleName" Text="Role"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <span id="InitalspnMessage" runat="server" style="text-align: center;"></span>
                            </td>
                        </tr>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <asp:GridView ID="GvwUserList" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                    OnRowDataBound="GvwUserList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <a style="color: #0053A6" href="javascript:void(0);" id="lnkFirstname" runat="server">Select</a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblFirstName" runat="server" Text="FirstNam"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("FirstName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--                        <asp:BoundField HeaderText="First Name" DataField="FirstName" />--%>
                        <asp:TemplateField HeaderText="LastName">
                            <HeaderTemplate>
                                <asp:Label ID="lblLastName" runat="server" Text="LastName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("LastName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Last Name" DataField="LastName" />--%>
                        <asp:TemplateField HeaderText="Email">
                            <HeaderTemplate>
                                <asp:Label ID="lblEmailId" runat="server" Text="Email"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("Emailid") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Email" DataField="Emailid" />--%>
                        <asp:TemplateField HeaderText="Gender">
                            <HeaderTemplate>
                                <asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("Gender") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Gender" DataField="Gender" />--%>

                        <asp:TemplateField HeaderText="LoginName">
                            <HeaderTemplate>
                                <asp:Label ID="lblLoginName" runat="server" Text="LoginName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("LoginName") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:BoundField HeaderText="Login Name" DataField="LoginName" />--%>
                        <asp:TemplateField HeaderText="Role Name">
                            <HeaderTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text="RoleName"></asp:Label>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <span id="spnRoleName" runat="server"></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <HeaderTemplate>
                                <asp:Label ID="lblLocationGrid" runat="server" Text="Location"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span id="spnLocation" runat="server"></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>

            <br />
            <asp:Button type="button" ID="btnClose" class="secondaryButton" runat="server" Text="Close" />
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function searchKeyPress(e) {
        // look for window.event in case event isn't passed in 
        if (typeof e == 'undefined' && window.event) { e = window.event; }
        if (e.keyCode == 13) {
            document.getElementById('btnSearchPopup').click();
        }
    }
</script>
