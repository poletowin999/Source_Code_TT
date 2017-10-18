<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserSwipeEditView.aspx.cs" Inherits="Activities_UserSwipeEditView"
    Theme="Classical" %>

<%@ Register Namespace="CalendarButton" TagPrefix="e4e" %>
<%@ Register TagPrefix="e4e" TagName="UserSwipeEditControl" Src="~/Activities/UserSwipeEditControl.ascx" %>
<%@ Register TagPrefix="e4e" TagName="UserSearchView" Src="~/SearchViews/UserSearchView.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
     <style type="text/css">
        .text-label
        {
            color: #cdcdcd;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <asp:UpdatePanel ID="UserSwipeEditPanel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                    <ProgressTemplate>
                        <div class="loading">
                            Loading...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div id="divMessage" runat="server" class="warnMsg" style="display: none;">
                </div>
                <div class="page-header-panel">
                    <h2><asp:Label runat="server" ID="lblEditTimeSheet" Text="Edit Time Sheet"></asp:Label></h2>
                </div>
                <div>
                    <table class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblEmployeeName" Text="EmployeeName"></asp:Label>
                            </td>
                            <td colspan="3">
                                <input id="txtuser" type="text" value="" runat="server" title=""  onfocus="return onRespUserFocus(this);"
                                onblur="return onRespUserBlur(this);" />
                                <input type="hidden" id="hdnuserid" runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtSearchUser" ImageUrl="~/Images/user20.png" CssClass="img16"
                                    runat="server"  OnClick="ibtSearchUser_Click" />
                            </td>
                            <td colspan="9">
                                <asp:Button ID="btnSearchUser" runat="server" Text="View" CssClass="primaryButton"
                                    OnClick="btnSearchUser_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="secondaryButton"
                                    OnClick="btnClear_Click1" />
                            </td>
                            <td colspan="8">
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="timeSheetContent">
                    <span id="spntag" runat="server" class="rgtflt"><b><asp:Label runat="server" ID="lblNotEnter" Text="NE: Not Enter"></asp:Label></b></span>
                    <div class="clear">
                    </div>
                    <asp:Calendar ID="CalendUserSwipe" DayHeaderStyle-BackColor="#def7ff" TodayDayStyle-BackColor="#DED6FF"
                        Font-Size="11px" Font-Names="verdana" ShowGridLines="true" Visible="false" SelectionMode="None"
                        Height="500px" Width="100%" PrevMonthText="Previous" BorderColor="#0066A6" BorderStyle="Solid"
                        BorderWidth="2" NextMonthText="Next" runat="server" OnDayRender="CalendUserSwipe_DayRender"
                        OnVisibleMonthChanged="CalendUserSwipe_VisibleMonthChanged">
                        <DayHeaderStyle BackColor="#DEF7FF" />
                        <NextPrevStyle CssClass="calTable" Font-Bold="True" ForeColor="#000066" />
                        <OtherMonthDayStyle Font-Bold="False" />
                        <TitleStyle CssClass="newTable" Font-Bold="True" />
                        <TodayDayStyle BackColor="#DED6FF" />
                    </asp:Calendar>
                    <e4e:CalendarLinkButton ID="CalendarLinkButton1" runat="server" CssClass="hide" OnCalendarClick="CalendarLinkButton1_CalendarClick"></e4e:CalendarLinkButton>
                    <br />
                </div>
                <div id="gridviewPanel" runat="server" class="gridViewPanel">
                    <table border="0" class="gridView">
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="lblUserSwipeInformation" Text="User Swipe Information"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td id="tdMessageNotFound" style="font-weight: bold;" runat="server">
                                <asp:Label runat="server" ID="lblChooseEmployeeName" Text="Please Choose the Employee Name"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divSwipeEditPanel">
        <asp:UpdatePanel ID="UserEditControlPanel" runat="server">
            <ContentTemplate>
                <e4e:UserSwipeEditControl ID="userSwipeEditControl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
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
            'valueControlId': '<%=hdnuserid.ClientID%>',
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



        function initializeEditPanelDialog() {
            // Display edit panel as dialog.
            $('#divSwipeEditPanel').dialog('destroy');
            $('#divSwipeEditPanel').dialog(
                {
                    autoOpen: false,
                    modal: true,
                    closeOnEscape: false,
                    show: 'fade',
                    hide: 'clip',
                    draggable: false,
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
                $('#divSwipeEditPanel').dialog('option', options);
            $('#divSwipeEditPanel').dialog('open');

        }

        function closeEditPanelDialog() {
            $('#divSwipeEditPanel').dialog('close');
            return false;
        }

        function FocusControl() {
            $('#' + '<%=txtuser.ClientID%>').bind('focus', function () {
                $('#' + '<%=txtuser.ClientID%>').val("");
            });
        }


        $(document).ready(
            function () {
                initializeEditPanelDialog(mUserSearchViewDialogOptions);
                FocusControl();

            }
        );
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

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
    <script type="text/javascript">
        // <!-- [CDATA[

        var mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);

        // ]]>
    </script>
</asp:Content>
