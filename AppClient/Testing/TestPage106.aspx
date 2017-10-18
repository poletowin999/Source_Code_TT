<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SiteMaster.master"
    AutoEventWireup="true" CodeFile="TestPage106.aspx.cs" Inherits="Testing_TestPage106"
    Theme="Classical" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="SampleSearchControl.ascx" TagName="SampleSearchControl" TagPrefix="e4e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Scripts/jQuery/jquery-1.6.3.min.js"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <label id="lblTime" runat="server">
            </label>
            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <e4e:SampleSearchControl ID="SampleSearchControl1" runat="server" />
            <e4e:SampleSearchControl ID="SampleSearchControl2" runat="server" />
            <br />
            <label id="Label1" runat="server">
            </label>
            <asp:Button ID="Button1" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server">
    </rsweb:ReportViewer>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
