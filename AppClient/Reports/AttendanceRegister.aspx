<%@ Page Title="" Language="C#"  MasterPageFile="~/MasterPages/AppMaster.master" 
AutoEventWireup="true" CodeFile="AttendanceRegister.aspx.cs" Inherits="Reports_AttendanceRegister" Theme="Classical"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <div class="page-header-panel">
            <h2><asp:Label runat="server" ID="lblAttendanceRegister" Text="Attendance Register - Bangalore and Hyderabad"></asp:Label></h2>
              
        </div>
        <div id="statusBar" runat="server" class="warnMsg" style="display: none">
            Error Message Here.</div>
        <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
            <ContentTemplate>
                <div class="searchFields">
                    <table cellspacing="0" border="0" class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblFromDate" Text="Activity From Date:"></asp:Label>
                            </td>
                            <td colspan="4">
                                <span>
                                    <asp:TextBox ID="txtActivityFromDate" runat="server" Text="" EnableViewState="false"
                                        MaxLength="10"></asp:TextBox>
                                </span>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtActivityFromDate"
                                    PopupButtonID="imgFromDate" Animated="false" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="Calendar button" ImageAlign="AbsBottom" />
                            </td>
                            <td>
                            </td>
                            <td colspan="3">
                               <asp:Label runat="server" ID="lblToDate" Text="Activity To Date:"></asp:Label>
                            </td>
                            <td colspan="4">
                                <span>
                                    <asp:TextBox ID="txtActivityToDate" runat="server" Text="" EnableViewState="false"
                                        MaxLength="10"></asp:TextBox>
                                </span>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActivityToDate"
                                    PopupButtonID="imgToDate" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="Calendar button" ImageAlign="AbsBottom" />
                            </td>

                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblLocations" Text="Location(s)"></asp:Label>
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
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblUsers" Text="Users:"></asp:Label>
                            </td>
                            <td colspan="5">
                                <div class="checkListBox">
                                    <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkUsersAll_CheckedChanged" />
                                    <hr />
                                    <asp:CheckBoxList ID="chkUsers" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                             <td>
                            </td>
                               <td valign="top" colspan="7">
                                <br />
                                <asp:Label runat="server" ID="lblLegends" Text="Legends for Attendance Report"></asp:Label><br />
                            
                            <hr />
                            <table>
                                <tr>
                                    <td>
                                        P&nbsp;    <br />
                                        PN&nbsp;   <br />
                                        L&nbsp;    <br />
                                        LOP&nbsp;  <br />
                                        N&nbsp;I   <br />
                                        CO&nbsp;   <br />
                                        WO&nbsp;   <br />
                                        PH&nbsp;   <br />
                                        ATTR&nbsp; <br />
                                    </td>
                                    <td>
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />
                                       &nbsp; --> &nbsp;<br />

                                    </td>
                                    <td>
                                        &nbsp;<asp:Label ID="lblPresent" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblPresentNight" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblApproved" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblLossofPay" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblNotinformedLeave" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblCompoff" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblWeeklyOff" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblHoliday" runat="server"></asp:Label><br />
                                        &nbsp;<asp:Label ID="lblAttrition" runat="server"></asp:Label><br />
                                    </td>
                                </tr>
                            </table>
                            </td>
                            <td></td>
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
    <br /><br />
    </div>
    <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Visible="false">
    </rsweb:ReportViewer>
    <div id="divReport" runat="server">
    </div>

     <div>
        <asp:Label Visible="false" ID="lblSlno" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblLocation" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmpNo" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblDesignation" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblRemarks" runat="server"></asp:Label>
        

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>

