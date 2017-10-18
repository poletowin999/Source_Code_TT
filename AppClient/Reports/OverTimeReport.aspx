<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" 
CodeFile="OverTimeReport.aspx.cs" Inherits="Reports_OverTimeReport" Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdateActivitySummary" runat="server">
        <ContentTemplate>
            <div class="page-header-panel">
                <h2>
                    US - Overtime Report
                </h2>
            </div>
            <div>
            </div>
            <div id="statusBar" runat="server" class="warnMsg" style="display: none">
                Error Message Here.</div>
            <table border="0" cellpadding="0" cellspacing="0" class="tableLayout">
                <tr>
                    <td colspan="2">
                        Pay Year:
                    </td>
                    <td colspan="4">
                      <asp:DropDownList ID="ddlPayyear" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddlPayyear_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td colspan="2">
                       Pay Period:
                    </td>
                    <td colspan="4">
                      <asp:DropDownList ID="ddlPayperiod" runat="server"></asp:DropDownList>
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
                        Location(s)
                    </td>
                    <td colspan="8">
                        User(s):
                    </td>
                    <td colspan="8">
                        Project(s):
                    </td>
                    <td colspan="8">
                        Language(s):
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkLocationAll" Text="All" runat="server" AutoPostBack="True" 
                                oncheckedchanged="chkLocationAll_CheckedChanged"/>
                            <hr />
                            <asp:CheckBoxList ID="chkLocations" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="chkLocations_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td colspan="7">
                        <div class="checkListBox">
                            <asp:CheckBox ID="chkUsersAll" Text="All" runat="server" AutoPostBack="True" 
                                oncheckedchanged="chkUsersAll_CheckedChanged" />
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
        <asp:Button ID="btnView" Text="View" runat="server" onclick="btnView_Click" />
        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="secondaryButton"/>
    </div>
    <%--<div id="divReport" runat="server">--%>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false">
    </rsweb:ReportViewer>
    <%--</div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
