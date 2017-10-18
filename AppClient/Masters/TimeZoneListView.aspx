<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="TimeZoneListView.aspx.cs" Inherits="Masters_TimeZoneListView"
    Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divSuccessMessage" runat="server" class="warnMsg" style="display: none">
                </div>
                <div>
                    <div class="headerTitle">
                        TimeZone Management<br />
                        <br />
                    </div>
                    <table class="formGrid">
                        <tr>
                            <td colspan="3">
                            </td>
                            <td colspan="7">
                                <label for="<%=txtSearchName.ClientID%>">
                                    Name:
                                </label>
                            </td>
                            <td>
                            </td>
                            <td colspan="7">
                                <label for="<%=txtSearchShortName.ClientID%>">
                                    Short Name:
                                </label>
                            </td>
                            <td colspan="14">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
                            <td colspan="7">
                                <input type="text" id="txtSearchName" runat="server" />
                            </td>
                            <td>
                            </td>
                            <td colspan="7">
                                <input type="text" id="txtSearchShortName" runat="server" />
                            </td>
                            <td colspan="4">
                            </td>
                            <td colspan="10">
                                <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="primaryButton"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnclear" runat="server" Text="Clear" CssClass="secondaryButton"
                                    OnClick="btnclear_Click" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                    </table>
                    <div class="gridViewContainer">
                        <div id="hdrGridHeader" runat="server" class="gridViewHeader" style="float: left">
                        </div>
                        <div class="lightToolbar" style="float: right;">
                            <ul class="lightToolbarItems">
                                <li>
                                    <asp:LinkButton ID="LnkAdd" Text="AddNew" runat="server" OnClick="LnkAdd_Click"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="LnkRefersh" Text="Refresh" runat="server" OnClick="LnkRefersh_Click"></asp:LinkButton></li></ul>
                        </div>
                        <div class="clearFloat">
                        </div>
                        <div class="gridViewPanel">
                            <div id="divGridHeader" runat="server">
                                <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                                    <tr>
                                        <th>
                                            Name
                                        </th>
                                        <th>
                                            Short Name
                                        </th>
                                        <th>
                                            Active
                                        </th>
                                        <th>
                                            Last Update User
                                        </th>
                                        <th>
                                            Last Update Date
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5" id="divEmptyRow" runat="server">
                                            Enter the criteria and click on Search button to view data.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvwTimeZoneList" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false"
                                DataKeyNames="Id" CssClass="gridView" OnRowCommand="gvwTimeZoneList_RowCommand"
                                OnRowDataBound="gvwTimeZoneList_RowDataBound" AllowPaging="true" OnPageIndexChanging="gvwTimeZoneList_PageIndexChanging"
                                PageSize="10">
                                <Columns>
                                    <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="TimeZoneEdit" ButtonType="Link" />
                                    <asp:BoundField HeaderText="Name" DataField="Name" />
                                    <asp:BoundField HeaderText="Short Name" DataField="ShortName" />
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <span id="spnIsActive" runat="server"></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Update User">
                                        <ItemTemplate>
                                            <span id="spnLastUpdateUser" runat="server"></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Last Update Date" DataField="LastUpdatedate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divEntityEditPanel">
        <asp:UpdatePanel ID="UpdateEditPanel" runat="server">
            <ContentTemplate>
                <div id="divMessage" runat="server" class="popupWarnMsg">
                </div>
                <table class="tableLayout">
                    <tr>
                        <td colspan="3">
                            <input type="hidden" id="hdnTimeZoneId" value="0" runat="server" />
                            <label for="<%=txtName.ClientID %>">
                                Name:</label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label for="<%=txtShortName.ClientID %>">
                                Short Name:</label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtShortName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label for="<%=txtDescription.ClientID %>">
                                Description:</label>
                        </td>
                        <td colspan="5">
                            <textarea id="txtDescription" rows="4" cols="29" runat="server"></textarea>
                        </td>
                    </tr>
                </table>
                <div id="divEditControl" runat="server">
                    <table class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <label for="<%=chkIsActive.ClientID %>">
                                    Is Active</label>
                            </td>
                            <td colspan="5">
                                <input type="checkbox" id="chkIsActive" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label for="<%=txtReason.ClientID %>">
                                    Reason:</label>
                            </td>
                            <td colspan="5">
                                <textarea id="txtReason" rows="4" cols="29" runat="server"></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
                <table class="tableLayout">
                    <tr>
                        <td colspan="3">
                        </td>
                        <td colspan="5">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                                CssClass="primaryButton" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                CssClass="secondaryButton" UseSubmitBehavior="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        initializeEditPanelDialog();

        function initializeEditPanelDialog() {
            // Display edit panel as dialog.
            $('#divEntityEditPanel').dialog('destroy');
            $('#divEntityEditPanel').dialog(
                {
                    autoOpen: false,
                    modal: true,
                    closeOnEscape: false,
                    show: 'fade',
                    hide: 'clip',
                    draggable: true,
                    resizable: false,
                    title: "Title here...",
                    open: function (event, ui) {
                        $(this).parent().appendTo("form:first");
                        return false;
                    },
                    close: function (event, ui) {
                        // Set the focus on input control.
                        return false;
                    }
                });
        }

        function showEditPanelDialog(options) {
            if (options !== null)
                $('#divEntityEditPanel').dialog('option', options);
            $('#divEntityEditPanel').dialog('open');
            $('#<%=txtName.ClientID%>').focus();
        }

        function closeEditPanelDialog() {
            $('#divEntityEditPanel').dialog('close');
            var errmsg = $('#' + '<%=divSuccessMessage.ClientID %>').text();
            if (errmsg != "")
                $('#' + '<%=divSuccessMessage.ClientID %>').fadeOut(3000);


            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
