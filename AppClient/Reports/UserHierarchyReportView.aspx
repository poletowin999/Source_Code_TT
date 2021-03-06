﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" 
CodeFile="UserHierarchyReportView.aspx.cs" Inherits="Reports_UserHierarchyReportView" Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
        <ContentTemplate>
            <div class="page-header-panel">
                <h2><asp:Label runat="server" ID="lblUserHierarchyReport" Text="User Hierarchy Report"></asp:Label></h2>
                    
            </div>
            <div id="statusBar" runat="server" class="warnMsg" style="display: none">
                Error Message Here.</div>
            <div class="searchFields">
                <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
            <%--        <tr>
                        <td colspan="3">
                            Activity From Date:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtFromDate" MaxLength="10" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" runat="server"
                                PopupButtonID="Imgfromdt" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                        </td>
                        <td>
                        </td>
                        <td colspan="3">
                            Activity To Date:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtToDate" MaxLength="10" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" runat="server"
                                PopupButtonID="Imgtodt" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:ImageButton runat="Server" ID="Imgtodt" ImageUrl="~/Images/btn_on_cal.gif" AlternateText="To display calendar."
                                ImageAlign="AbsBottom" />
                        </td>
                        <td>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblLocations" Text="Location(s)"></asp:Label>
                        </td>
                        <td colspan="4">
                            <div class="checkListBox">
                                <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="chkLocationAll_CheckedChanged" TabIndex="1" 
                                     />
                                <hr />
                                <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="chkLocations_SelectedIndexChanged" TabIndex="2" 
                                    >
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblSupervisor" Text="Supervisor:"></asp:Label>
                            </td>
                            <td colspan="4">
                                <div class="checkListBox">
                                    <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="chkUsersAll_CheckedChanged" TabIndex="3"  
                                         />
                                    <hr />
                                    <asp:CheckBoxList ID="chkUsers" runat="server" TabIndex="4">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                            </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="searchButtons">
        <asp:Button ID="btnView" Text="View" runat="server" Font-Bold="true" 
            onclick="btnView_Click1" TabIndex="5" 
             />
        <asp:Button ID="btnCancel" Text="Cancel" runat="server" 
            CssClass="secondaryButton" onclick="btnCancel_Click" TabIndex="6" 
             />
    </div>
    <div class="clear">
    </div>
    <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
  
    
     <div id="divReport" runat="server" visible="true">
      <rsweb:ReportViewer ID="ReportViewer1" runat="server" TabIndex="7" Visible="false">
     
        </rsweb:ReportViewer>
    </div>
      
   <div>
        <asp:Label Visible="false" ID="lblEmpNo" runat="server"></asp:Label>
       <asp:Label Visible="false" ID="lblManager" runat="server"></asp:Label> 
       <asp:Label Visible="false" ID="lblName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblUserRole" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmailId" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblLocation" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblUserType" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblContractDays" runat="server"></asp:Label>
        
        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>


