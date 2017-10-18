<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ActivitySummaryReportView.aspx.cs" Inherits="Reports_ActivitySummaryReportView"
    Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<add name="Reserved-ReportViewerWebControl-axd" path="Reserved.ReportViewerWebControl.axd"
verb="*" type="Microsoft.Reporting.WebForms.HttpHandler" resourcetype="Unspecified"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
        <ContentTemplate>
            <div class="page-header-panel">
                <h2><asp:Label runat="server" ID="lblActivityEntryReport" Text="Activity Entry Report"></asp:Label></h2>
                    
            </div>
            <div id="statusBar" runat="server" class="warnMsg" style="display: none">
                Error Message Here.
            </div>
            <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblFromDate" Text="Activity From Date:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFromDate" MaxLength="10" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" runat="server"
                            PopupButtonID="Imgfromdt" Animated="true" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                            AlternateText="To display calendar." ImageAlign="AbsBottom" />
                    </td>
                    <td >
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblToDate" Text="Activity To Date:"></asp:Label>
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
                    <td colspan="8">
                        <asp:CheckBox ID="chkMiscellaneous" runat="server" Checked="True" Text="" /><asp:Label runat="server" ID="lblIncludeMiscellaneousActivity" Text="Include Miscellaneous Activity"></asp:Label>
                    </td>
                    <td colspan="7"></td>
                    <td>
                    </td>
                </tr>
                </table>
                 <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
                
                 <tr>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblclients" Text="client(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblProjects" Text="Project(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblLocations" Text="Location(s)"></asp:Label>
                    </td>
                      <td colspan="8">
                        <asp:Label runat="server" ID="lblSupervisors" Text="Supervisors(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblUsers" Text="User(s)"></asp:Label>
                    </td>
                    <!--td colspan="8">
                        Work Type(s):
                    </td-->
                </tr>
                <tr>
                   <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkClientAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkClientAll_CheckedChanged" />
                            <hr />
                            <%--        <asp:CheckBoxList ID="chkClient" runat="server" Width="100%" 
                                BorderWidth="0" OnSelectedIndexChanged="chkClient_SelectedIndexChanged">--%>
                            <asp:CheckBoxList ID="chkClient" runat="server" Width="100%" BorderWidth="0" AutoPostBack="true"
                                OnSelectedIndexChanged="chkClient_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkProjectAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkProjectAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkProject" runat="server" Width="200px" BorderWidth="0">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td  colspan="7">
                      <div class="checkListBox">
                            <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkLocationAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkLocations_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                        </td>
                        <td></td>


                     <td colspan="7">
                         <div class="checkListBox">
                                    <asp:CheckBox ID="CheckSupervisorAll" Text="All" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="CheckBox2_CheckedChanged" TabIndex="3"  
                                         />
                                    <hr />
                                    <asp:CheckBoxList ID="CheckSupervisor" AutoPostBack="true" OnSelectedIndexChanged="CheckSupervisor_SelectedIndexChanged" runat="server" TabIndex="4">
                                    </asp:CheckBoxList>
                                </div>
                        </div>
                    </td>
                    <td></td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkUsersAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkUsers" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td></td>
                   
                </tr>
           
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="commandPanel">
        <asp:Button ID="btnViewAS" Width="160px" Text="View Activity Summary" runat="server"
            Font-Bold="true" OnClick="btnView_Click" />
        <asp:Button ID="btnViewDetailsAD" Text="View Activity Details" runat="server" Width="150px"
            Font-Bold="true" OnClick="btnViewDetails_Click" />
        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="secondaryButton"
            OnClick="btnCancel_Click" />
    </div>
     <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
    <div id="divReport" runat="server" >
      <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false">
    </rsweb:ReportViewer>
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Visible="false">
        </rsweb:ReportViewer>
    </div>
   
    <div>
        <asp:Label Visible="false" ID="lblActivityDate" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmpNo" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblUserType" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblUserRole" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblActivityType" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblClientName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblCategoryName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblProjectName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblPlatformName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblLocation" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblWorktype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblBillingtype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblHours" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblMinutes" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblNSA" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblStatus" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblManager" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblContractDays" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblJoinDate" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblCity" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblState" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblCountry" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblTimeZone" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblTest" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblLanguage" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblCommentsHistory" runat="server"></asp:Label>

        
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
