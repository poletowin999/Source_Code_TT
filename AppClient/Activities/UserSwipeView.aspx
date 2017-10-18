<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserSwipeView.aspx.cs" Inherits="Activities_UserSwipeView" Theme="Classical" %>

<%@ Register Namespace="CalendarButton" TagPrefix="e4e" %>
<%@ Register TagPrefix="e4e" TagName="UserSwipeEditControl" Src="~/Activities/UserSwipeEditControl.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="page-header-panel">
        <h2><asp:Label runat="server" ID="lblTimeSheet" Text="Time Sheet"></asp:Label></h2>
    </div>
    <div class="timeSheetContent">
        <span class="rgtflt"><b><asp:Label runat="server" ID="lblNotEnter" Text="NE: Not Enter"></asp:Label> </b></span>
        <div class="clear">
        </div>
        <asp:UpdatePanel ID="UpdateChekIn" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                    <ProgressTemplate>
                        <div class="loading">
                            Loading...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="gridViewPanel">
                    <asp:Calendar ID="CalUserSwipe" runat="server" DayHeaderStyle-BackColor="#def7ff"
                        NextPrevStyle-CssClass="calTable" OtherMonthDayStyle-CssClass="calTable" TitleStyle-CssClass="calTable"
                        TodayDayStyle-BackColor="#DED6FF" Font-Size="11px" Font-Names="verdana" Height="500px"
                        SelectionMode="None" OnDayRender="CalUserSwipe_DayRender" BorderColor="#0066A6"
                        BorderWidth="2px" ShowGridLines="True" Width="100%" NextMonthText="Next " PrevMonthText="Previous "
                        OnVisibleMonthChanged="CalUserSwipe_VisibleMonthChanged"></asp:Calendar>
                    <e4e:CalendarLinkButton ID="CalendarLinkButton1" runat="server" CssClass="hide" OnCalendarClick="CalendarLinkButton1_CalendarClick"></e4e:CalendarLinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divSwipeEditPanel">
            <asp:UpdatePanel ID="UpdateDialog" runat="server">
                <ContentTemplate>
                    <e4e:UserSwipeEditControl ID="userSwipeEditControl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">



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
        $(document).ready(function () {
            initializeEditPanelDialog();
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
