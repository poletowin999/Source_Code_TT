<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserListView.aspx.cs" Inherits="Users_UserListView" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="page-header-panel">
        <h2 runat="server" id="LblHeader">
            <asp:Label runat="server" ID="lblUsersManagement" Text="Users Management"></asp:Label></h2>
    </div>
    <asp:UpdatePanel ID="UpdateSearchPanel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="divMessage" runat="server" class="warnMsg">
                Succeed message.
            </div>


            <table class="formGrid">
                <tr>
                    <td colspan="3" align="left">
                        <asp:Label runat="server" ID="lbltxtName" Text="Name:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <input type="text" id="txtName" maxlength="100" runat="server" />
                    </td>
                    <td></td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblRoleName" Text="Role Name:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <input type="text" id="txtRoleName" runat="server" />
                        <input type="hidden" id="hdnRoleId" runat="server" />
                    </td>
                    <td></td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblEmail" Text="Email ID:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <input type="text" id="txtEmail" runat="server" />
                    </td>
                    <td></td>
                    <td colspan="3"></td>
                    <td colspan="4"></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblLocation" Text="Location:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlLocation" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblStatus" Text="Status:"></asp:Label>

                    </td>
                    <td colspan="4">
                        <select id="drpstatus" runat="server" name="D1">
                            <option value="0">All</option>
                            <option selected="selected" value="1">Active</option>
                            <option value="2">InActive</option>
                        </select>
                    </td>
                    <td></td>
                    <td colspan="3"></td>
                    <td colspan="4"></td>
                    <td></td>
                    <td colspan="7">
                        <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click"
                            Text="Filter" />
                        <asp:Button ID="btnclear0" runat="server" Text="Clear" OnClick="btnclear_Click"
                            CssClass="secondaryButton" />
                    </td>
                    <td></td>
                </tr>
            </table>


            <div style="padding: 20px 0px 10px 0px"></div>

            <div class="gridViewContainer">
                <div id="LstofRecord" runat="server" class="gridViewHeader" style="float: left;">
                </div>
                <div class="lightToolbar" style="float: right;">
                    <ul class="lightToolbarItems">
                        <li><a href="Add">
                            <asp:Label runat="server" ID="lblAddNew" Text="AddNew"></asp:Label></a> </li>
                        <li>
                            <asp:LinkButton ID="LnkRefersh" runat="server" OnClick="LnkRefersh_Click">
                                <asp:Label runat="server" ID="lblRefresh" Text="Refresh"></asp:Label>
                            </asp:LinkButton></li>
                    </ul>
                </div>
                <div class="clearFloat">
                </div>
                <div class="gridViewPanel">
                    <div id="InitalBind" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                            <tr>

                                <th>
                                    <asp:Label runat="server" ID="lblLastName" Text="Last Name"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblFirstName" Text="First Name"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblGender" Text="Gender"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblEmailId" Text="Email ID"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblRoleNameGrid" Text="Role Name"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblLoginName" Text="Login Name"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblLocationGrid" Text="Location"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblJoinDate" Text="Join Date"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblStatusGrid" Text="Status"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                                </th>
                                <%--<th>
                                    Last Update User
                                </th>
                                <th>
                                    Last Update Date
                                </th>--%>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <span id="spnMessage" runat="server"></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="GvwUserList" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                        AllowPaging="true" OnRowDataBound="GvwUserList_RowDataBound" OnPageIndexChanging="GvwUserList_PageIndexChanging"
                        PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="Last Name">
                                <HeaderTemplate>
                                    <asp:Label ID="lblLastName" runat="server" Text="Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="First Name">
                                <HeaderTemplate>
                                    <asp:Label ID="lblFirstName" runat="server" Text="Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("FirstName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender">
                                <HeaderTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Gender") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EmailID">
                                <HeaderTemplate>
                                    <asp:Label ID="lblEmailId" runat="server" Text="EmailID"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("EmailID") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Role Name">
                                <HeaderTemplate>
                                    <asp:Label ID="lblRoleNameGrid" runat="server" Text="Role Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span id="spndesignation" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Location">
                                <HeaderTemplate>
                                    <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <span id="spnLocation" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="JoinDate">
                                <HeaderTemplate>
                                    <asp:Label ID="lblJoinDate" runat="server" Text="JoinDate"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <%# Eval("JoinDate") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusGrid" runat="server" Text="Status"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span id="spnIsActive" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%-- <asp:TemplateField HeaderText="Last Update User">
                                <ItemTemplate>
                                    <span id="spnUsername" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Update Date">
                                <ItemTemplate>
                                    <span id="spnLastUpdateDate" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Action">
                                <HeaderTemplate>
                                    <asp:Label ID="lblAction" runat="server" Text="Action"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a href="031-<%#DataBinder.Eval(Container.DataItem,"Id") %>-User-Edit">Edit</a>
                                    <%--<a href="UserEditView.aspx?id=<%#DataBinder.Eval(Container.DataItem,"Id") %>">Edit</a>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            function (sender, args) {
                //                initializeRoleAutoComplete();

            });
        function initializeRoleAutoComplete() {
            var RolenameId = $('#' + '<%=txtRoleName.ClientID%>')

            $(RolenameId).autocomplete('destroy');
            $(RolenameId).autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetRolesByName',
                                    type: 'POST',
                                    contentType: 'application/json; charset=utf-8',
                                    data: "{ 'name': '" + request.term + "'}",
                                    dataType: "json",
                                    success: function (data, textStatus, req) {
                                        var dataArray = $.parseJSON(data.d);
                                        if (dataArray === null) return;
                                        response(dataArray);
                                    },
                                    error: function (req, textStatus, error) {
                                        alert(error);
                                    }
                                });
                        },

                        focus: function (event, ui) {
                            $(this).val(ui.item.Name);
                            return false;
                        },

                        select: function (event, ui) {
                            $(this).val(ui.item.Name);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.Name + "<br> </span></a>")
				                .appendTo(ul);
                    };
        }
        $(document).ready(function () {
            //           initializeRoleAutoComplete();
            $('#' + '<%=divMessage.ClientID %>').fadeOut(8000);

        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
