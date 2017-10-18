<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserHierarchyView.aspx.cs" Inherits="Users_UserHierarchyView" Theme="Classical" %>

<%@ Register TagPrefix="e4e" TagName="UserSearchView" Src="~/SearchViews/UserSearchView.ascx" %>
<%@ Register Src="~/Users/AttachedUserView.ascx" TagName="Attached" TagPrefix="UserHierarchy" %>
<%@ Register Src="~/Users/UnAttachedUserView.ascx" TagName="UnAttached" TagPrefix="UserHierarchy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="page-header-panel">
                <h2><asp:Label runat="server" ID="lblReportingHierarchy" Text="Reporting Hierarchy"></asp:Label> </h2>
            </div>
            <div id="alertError" class="warnMsg" runat="server" style="display: none">
            </div>
            <div class="lightToolbar" style="float: right; padding: 0px 10px 0px 0px;">
                <ul class="lightToolbarItems">
                    <li>
                        <asp:Button ID="btnAddnew" runat="Server" Text="AddNew" OnClick="btnAddnew_Click" />
                    </li>
                    <li>
                        <asp:Button ID="lbtRefresh" runat="Server" Text="Refresh" OnClick="btnReferesh_Click" />
                    </li>
                </ul>
            </div>
            <div class="clearFloat">
            </div>
            <div class="treeDiv" style="height: 450px; overflow: auto">
                <asp:TreeView ID="TreHierarchy" runat="server" OnSelectedNodeChanged="TreHierarchy_SelectedNodeChanged"
                    SelectedNodeStyle-Font-Bold="true" SelectedNodeStyle-ForeColor="DarkSlateBlue"
                    ExpandDepth="0" ImageSet="Arrows" ShowLines="True">
                    <ParentNodeStyle Font-Bold="True" />
                    <RootNodeStyle Font-Bold="True" />
                    <SelectedNodeStyle Font-Bold="True" ForeColor="DarkSlateBlue" />
                </asp:TreeView>
            </div>
            <div class="contentViewDiv" style="height: 450px;">
                <UserHierarchy:UnAttached runat="Server" ID="UnAttachedUsers" Visible="False" />
                <UserHierarchy:Attached runat="Server" ID="AttachedUsers" Visible="False" />
                <asp:Panel ID="pnlmanager" GroupingText="Change Manager" runat="server" Visible="false">
                    <table class="tableLayout">
                        <tr>
                            <td colspan="2">
                                <label>
                                    New Manager</label>
                                <span class="Mandetary">*</span>
                            </td>
                            <td colspan="3">
                                <input id="txtuser" type="text" value="" runat="server" title="press F2 to search users"
                                    onfocus="return onRespUserFocus(this);" onblur="return onRespUserBlur(this);"
                                    readonly="readonly" />
                                <input type="hidden" id="hdnManagerId" runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtSearchUser" ImageUrl="~/Images/user20.png" CssClass="img16"
                                    runat="server" OnClick="ibtSearchUser_Click" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                                <asp:Label ID="lblchangedate" runat="Server" Text="Change Date:"></asp:Label>
                                <span class="Mandetary">*</span>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtmanagerdate" runat="server" Width="105px" MaxLength="10" ></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="Imgmandt" runat="server"
                                    TargetControlID="txtmanagerdate" Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>
                                </td>
                                <td>
                                <asp:ImageButton runat="Server" ID="Imgmandt" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="To display calendar." ImageAlign="AbsBottom" />
                            </td>
                            <td colspan="3">
                                <asp:Button ID="btnChange" runat="server" Text="Change Manager" Width="130px" OnClick="btnChange_Click" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="showUserConrtol" runat="server">
                <asp:HiddenField ID="Hidhierarchy" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="divUserSearchPanel" runat="server">
        <e4e:UserSearchView ID="UserSearchView" onDialogClose="closeUserSearchView" onSearchResultSelect="UserSearchResultSelected"
            runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        //user search dialog control


        var mUserSearchViewDialog = null;
        var mUserSearchViewDialogOptions = {
            'inputControlId': '<%=txtuser.ClientID%>',
            'searchButtonId': '<%=ibtSearchUser.ClientID%>',
            'valueControlId': '<%=hdnManagerId.ClientID%>',
            'searchControlPanelId': '<%=divUserSearchPanel.ClientID %>',
            'title': 'User Search View'
        };

        function refreshUserSearchView() {
            if (mUserSearchViewDialog !== null)
                mUserSearchViewDialog.set_options(mUserSearchViewDialogOptions);
        }

        function showUserSearchView() {
            mUserSearchViewDialog.show();
        }

        function closeUserSearchView() {
            mUserSearchViewDialog.close();
            return false;
        }

        function UserSearchResultSelected(result) {
            var resultObject = Sys.Serialization.JavaScriptSerializer.deserialize(result);
            mUserSearchViewDialog.set_displayText(resultObject.LastName + ',' + resultObject.FirstName);
            mUserSearchViewDialog.set_valueText(resultObject.Id);
            mUserSearchViewDialog.close();
        }

        function onRespUserFocus(ctrl) {
            if (ctrl.value == ctrl.title) {
                ctrl.value = '';
                ctrl.className = '';
            }
        }

        function onRespUserBlur(ctrl) {
            if (ctrl.value == '') {
                ctrl.value = ctrl.title;
                ctrl.className = 'WatermarkText';
            }
        }

        $(document).ready(
            function () {

                mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);
            }
        );

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
    <%--<script type="text/javascript">
     mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);
</script>--%>
</asp:Content>
