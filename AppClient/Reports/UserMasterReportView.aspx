<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserMasterReportView.aspx.cs" Inherits="Reports_UserMasterReportView"
    Theme="Classical" ValidateRequest="false" EnableEventValidation="false"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UserMasterData" runat="server">
        <ContentTemplate>
            <div class="page-header-panel">
                <h2>
                    UserMasterDetails</h2>
            </div>
            <div id="statusBar" runat="server" class="warnMsg" style="display: none">
                Error Message Here.</div>
            <div>
            
                <table border="0" cellpadding="0" cellspacing="0" class="tableLayout" >
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblFromDate" Text="From Date:"></asp:Label>
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
                            <asp:Label runat="server" ID="lblToDate" Text="To Date:"></asp:Label>
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
                        <td ></td>
                        <td colspan="14">
                         <div>
                                <asp:Button ID="btnView" Text="View" runat="server" Font-Bold="true" OnClick="btnView_Click1" EnableViewState="true"/>
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="secondaryButton"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" valign="top">
                            <asp:Label runat="server" ID="lblLocations" Text="Location(s)"></asp:Label><br /><br />
                            <div class="checkListBox">
                                <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkLocationAll_CheckedChanged"/>
                                <hr />
                                <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                        </td>
                        <td colspan="7" valign="top">
                            <asp:Panel ID="panel1" runat="server" GroupingText="DateFilter">
                                <asp:RadioButton ID="RdnAlldate" runat="server" Text="All" GroupName="group1" Checked="True" />
                                <br />
                                <asp:RadioButton ID="RdnJoinDate" runat="server" Text="JoinDate" GroupName="group1" />
                                <br />
                                <asp:RadioButton ID="RdnRelieveDate" runat="server" Text="RelieveDate" GroupName="group1" />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="panel2" runat="server" GroupingText="UserStatus">
                                <asp:RadioButton ID="RdnAll" runat="server" Text="All" GroupName="group2" />
                                <br />
                                <asp:RadioButton ID="RdnActive" runat="server" Text="Active" GroupName="group2" />
                                <br />
                                <asp:RadioButton ID="RdnInActive" runat="server" Text="InActive" GroupName="group2" />
                            </asp:Panel>
                        </td>
                        <td></td>
                        <td colspan="9" valign="top">
                        <br />
                            
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="23">
                        <br />
                           
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
       
    </asp:UpdatePanel>
    <div class="clear">
    </div>
    <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
    <div id="divReport" runat="server" visible="true">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server">
        </rsweb:ReportViewer>
    </div>
    <div>
        <asp:Label Visible="false" ID="lblEmployeeMasterReport" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmpNo" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmployeeName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblRolename" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblWorktype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblGender" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblEmailId" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblSupervisor" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblHomePhone" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblOfficePhone" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblExtension" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblIsAdminuser" runat="server"></asp:Label>   
        <asp:Label Visible="false" ID="lblIsAutoapproval" runat="server"></asp:Label>   
        <asp:Label Visible="false" ID="lblJoinDate" runat="server"></asp:Label>   
        <asp:Label Visible="false" ID="lblRelieveDate" runat="server"></asp:Label>  
        <asp:Label Visible="false" ID="lblTenority" runat="server"></asp:Label>   
        <asp:Label Visible="false" ID="lblStatus" runat="server"></asp:Label>   
        <asp:Label Visible="false" ID="lblreason" runat="server"></asp:Label> 
        <asp:Label Visible="false" ID="lblContractDays" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
