<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="PasswordChangeView.aspx.cs" Inherits="Users_PasswordChangeView" Theme="Classical" %>

<%@ Register Src="PasswordChangeViewControl.ascx" TagName="PasswordChangeViewControl"
    TagPrefix="e4e" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .center-panel
        {
            position: relative;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div style="top: 100px; width: 500px" class="center-panel">
        <asp:UpdatePanel ID="UpdateEditPanel" runat="server">
            <ContentTemplate>
                <e4e:PasswordChangeViewControl ID="PasswordChangeViewControl1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
