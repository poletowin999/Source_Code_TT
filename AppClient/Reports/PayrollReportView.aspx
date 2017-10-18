<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="PayrollReportView.aspx.cs" Inherits="Reports_PayrollReportView" Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <div class="page-header-panel">
            <h2>
                Payroll Summary Report</h2>
        </div>
        <div id="statusBar" runat="server" class="warnMsg" style="display: none">
            Error Message Here.</div>
        <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
            <ContentTemplate>
                <div class="searchFields">
                    <table cellspacing="0" border="0" class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <label>
                                    From Date:
                                </label>
                            </td>
                            <td colspan="4">
                                <span>
                                    <asp:TextBox ID="txtActivityFromDate" runat="server" Text="" EnableViewState="false"></asp:TextBox>
                                </span>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtActivityFromDate"
                                    PopupButtonID="imgFromDate" Animated="false">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="Calendar button" ImageAlign="AbsBottom" />
                            </td>
                            <td>
                            </td>
                            <td colspan="3">
                                <label>
                                    To Date:
                                </label>
                            </td>
                            <td colspan="4">
                                <span>
                                    <asp:TextBox ID="txtActivityToDate" runat="server" Text="" EnableViewState="false"></asp:TextBox>
                                </span>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActivityToDate"
                                    PopupButtonID="imgToDate">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="Calendar button" ImageAlign="AbsBottom" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="3">
                                Locations:
                            </td>
                            <td colspan="5">
                                <div class="checkListBox">
                                    <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkLocationAll_CheckedChanged" />
                                    <hr />
                                    <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkLocations_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                            </td>
                            <td valign="top" colspan="3">
                                Users:
                            </td>
                            <td colspan="5">
                                <div class="checkListBox">
                                    <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkUsersAll_CheckedChanged" />
                                    <hr />
                                    <asp:CheckBoxList ID="chkUsers" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="searchButtons">
            <asp:Button ID="btnViewReport" Text="View" runat="server" Font-Bold="true"
                OnClick="btnViewReport_Click" />
            <asp:Button ID="btnCancel" Text="Cancel" CssClass="secondaryButton" runat="server"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="clear">
    </div>
      <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
    <div id="divReport" runat="server">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
