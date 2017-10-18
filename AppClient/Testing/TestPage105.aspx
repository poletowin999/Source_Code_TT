<%@ Page Title="" Language="C#" MasterPageFile="~/Testing/TestMaster.master" AutoEventWireup="true" CodeFile="TestPage105.aspx.cs" Inherits="Testing_TestPage105" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Button ID="btnError" runat="server" Text="Error" 
            onclick="btnError_Click" />

    <rsweb:ReportViewer ID="ReportViewer1" runat="server">
    </rsweb:ReportViewer>
</asp:Content>

