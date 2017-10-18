<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" CodeFile="wfrmAddEditAccessLevel.aspx.cs" Inherits="UsersProfile_wfrmAddEditAccessLevel" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .treeNode {
            color: #070707;
        }

        .rootNode {
            font-size: 14px;
            width: 100%;
            color: #070707;
            padding: 2px;
        }

        .leafNode {
            padding: 2px;
            color: #070707;
        }

       #divPermissions .leafNode {
            padding: 2px;
            color: #0066cc;
        }

        .selectNode {
            font-weight: bold;
        }

        .divTreeview {
            border: 1px solid seagreen;
            height: 450px;
            overflow-x: hidden;
            overflow-y: auto;
            width: 99%;
        }
    </style>
    <script type="text/javascript">

        function onSuccess() {
            setTimeout(okay, 3000);
        }

        function okay() {
            window.location = "List";
        }

        function cancel() {
            window.location = "List";
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="page-header-panel">
        <h2 runat="server" id="lblHeader">
            <label>User Permission</label></h2>
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
            <div id="divMessage" runat="server" class="warnMsg" style="margin-left: 0">
                Succeed message.
            </div>

            <table class="formGrid" style="width: 40%">
                <tr>
                    <td align="left" style="width: 20%">

                        <asp:Label runat="server" ID="lblAccessLevels" Text="Access Levels"></asp:Label><span class="Mandetary">*</span>

                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAccessLevel" runat="server">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td align="left" valign="top">

                        <asp:Label runat="server" ID="lblUserPermissions" Text="User Permissions"></asp:Label><span class="Mandetary">*</span>

                    </td>
                    <td align="left" valign="top">
                        <div class="divTreeview" id="divPermissions">
                            <asp:TreeView ID="tvPermissions" runat="server" TabIndex="2" ShowCheckBoxes="All" ExpandDepth="0" ShowLines="true" OnSelectedNodeChanged="tvPermissions_SelectedNodeChanged">
                                <LeafNodeStyle CssClass="leafNode" />
                                <NodeStyle CssClass="treeNode" />
                                <RootNodeStyle CssClass="rootNode" />
                                <SelectedNodeStyle CssClass="selectNode" />
                            </asp:TreeView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">

                        <div id="divActive" runat="server">
                            <table class="tableLayout">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:Label runat="server" ID="lblActive" Text="Active"></asp:Label>
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkActive" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" valign="top">
                                        <asp:Label runat="server" ID="lblreason" Text="Reason">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <textarea id="txtReason" runat="server" rows="5" cols="13" style="width: 50%"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnSubmit" runat="server" TabIndex="4"
                            OnClick="btnSubmit_Click" Text="Submit"></asp:Button>
                        <input id="btnCancel" value="Cancel" type="button" onclick="cancel();" class="secondaryButton" />

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialog">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="formGrid" style="width: 100%">
                    <tr>
                        <td colspan="6" align="left"><h2><asp:Label ID="lblPermissionName" runat="server" Text="Permission Name"></asp:Label></h2></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <h4>
                                <asp:Label ID="lblLocations" runat="server" Text="Locations"></asp:Label>
                            </h4>
                        </td>
                        <td colspan="2" align="left">
                            <h4>
                                <asp:Label ID="lblClients" runat="server" Text="Clients"></asp:Label></h4>
                        </td>
                        <td colspan="2" align="left">
                            <h4>
                                <label>
                                    <asp:Label ID="lblReports" runat="server" Text="Reports"></asp:Label></label></h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvLocations" runat="server" TabIndex="1" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true" ShowExpandCollapse="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvClients" runat="server" TabIndex="2" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvReports" runat="server" TabIndex="3" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(ScrollUpB);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ScrollUpE);

        var scrollPermission;

        function ScrollUpB() {
            scrollPermission = document.getElementById("divPermissions").scrollTop;
        }

        function ScrollUpE() {
            document.getElementById("divPermissions").scrollTop = scrollPermission;
        }

        function initializeEditPanelDialog() {
            // Display edit panel as dialog.
            $('#divDialog').dialog('destroy');
            $('#divDialog').dialog(
                {
                    autoOpen: false,
                    modal: true,
                    closeOnEscape: false,
                    show: 'fade',
                    hide: 'clip',
                    width: '800px',
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
                $('#divDialog').dialog('option', options);
            $('#divDialog').dialog('open');
        }

        function closeEditPanelDialog() {
            $('#divDialog').dialog('close');
            return false;
        }
        $(document).ready(function () {
            initializeEditPanelDialog();
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>


