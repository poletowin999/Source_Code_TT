<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="BillingInvoiceReportView.aspx.cs" Inherits="Reports_BillingInvoiceReportView"
    Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
        <ContentTemplate>
            <div class="page-header-panel">
                <h2><asp:Label runat="server" ID="lblPayrollReport" Text="Payroll Report"></asp:Label></h2>
            </div>
            <div>
            </div>
            <div id="statusBar" runat="server" class="warnMsg" style="display: none">
                Error Message Here.</div>
            <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblFromDate" Text="Activity From Date:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtFromDate" MaxLength="10" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" runat="server"
                            PopupButtonID="Imgfromdt" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:ImageButton runat="Server" ID="Imgfromdt" ImageAlign="AbsBottom" ImageUrl="~/Images/btn_on_cal.gif"
                            AlternateText="To display calendar." />
                    </td>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblToDate" Text="Activity To Date:"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtToDate" MaxLength="10" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" runat="server"
                            PopupButtonID="Imgtodt" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:ImageButton runat="Server" ID="Imgtodt" ImageAlign="AbsBottom" ImageUrl="~/Images/btn_on_cal.gif"
                            AlternateText="To display calendar." />
                    </td>
                    <td>
                    </td>
                    <td colspan="15">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
                <tr>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblLocations" Text="Location(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblUsers" Text="User(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblProjects" Text="Project(s)"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:Label runat="server" ID="lblLanguage" Text="Language(s):"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkLocationAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkLocations_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkUsersAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkUsers" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkProjectAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkProjectAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkProjects" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkLanguageAll" Text="All" runat="server" AutoPostBack="True" OnCheckedChanged="chkLanguageAll_CheckedChanged" />
                            <hr />
                            <asp:CheckBoxList ID="chkLanguages" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="commandPanel">
        <asp:Button ID="btnView" Text="View" runat="server" OnClick="btnView_Click" />
        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="secondaryButton"
            OnClick="btnCancel_Click" />
    </div>
  
    <div id="divData" runat="server" style="display: none; text-align: center; padding: 10px 0px 10px 0px;
        font-weight: bold;" class="gridViewPanel">
        Data's not found
    </div>
    <div id="divReport" runat="server">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false">
        </rsweb:ReportViewer>
    </div>

    <div>
        <asp:Label Visible="false" ID="lblActivityType" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblclients" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblCategoryName" runat="server"></asp:Label>
        <%--<asp:Label Visible="false" ID="lblProjects" runat="server"></asp:Label>--%>
        <asp:Label Visible="false" ID="lblPlatformName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblWorktype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblBillingtype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblHours" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblMinutes" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblTotalhours" runat="server"></asp:Label>

        <asp:Label Visible="false" ID="lblEmpNo" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblEmployeeName" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblRolename" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblContracttype" runat="server"></asp:Label>
        <asp:Label Visible="false" ID="lblContractDays" runat="server"></asp:Label>
        
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
