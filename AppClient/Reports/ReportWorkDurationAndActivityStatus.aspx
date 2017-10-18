<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" CodeFile="ReportWorkDurationAndActivityStatus.aspx.cs" Inherits="Reports_ReportWorkDurationAndActivityStatus" Theme="Classical" %>
<%@ Register Src="~/Widgets/ReportUserDashboard.ascx" TagName="ReportUserDashboardWidget"
    TagPrefix="e4e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
  <e4e:ReportUserDashboardWidget ID="ReportUserDashboardWidget1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>

