<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ReportsView.aspx.cs" Inherits="Reports_ReportsView" EnableViewState="false"
    Theme="Classical" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        h4 
        {
            font-size: 10pt;
            font-weight: bold;
            margin: 0;
        }
        .headerPanel3 
        {
            padding: 7px 5px 3px 5px;
            border-bottom: 1px solid #336699;
        }
        .fixed-table
        {
            table-layout: fixed;
            width: 100%;
            border-collapse: collapse;
        }
        .fixed-table td
        {
            vertical-align: top;
        }
        div.report-item
        {
            margin: 3px 3px 7px 3px;
            padding: 5px;
            border: 0px solid #334455;
        }
        a, a:hover, a:active
        {
            text-decoration: none;
            color: #0066CC;
        }
        img.left-floater
        {
            float: left;
        }
        span.report-name
        {
            margin-left: 10px;
            display: block;
            padding: 3px 5px;
            font-size: 12pt;
            font-weight: bold;
        }
        span.report-desc
        {
            margin-left: 10px;
            display: block;
            padding: 3px 5px;
            color: Gray;
        }
        .clear
        {
            float: none;
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="headerPanel3">
        <h4>
            <asp:Label runat="server" ID="lblReportList" Text="Which report would you like to work with?"></asp:Label></h4>
    </div>
    
    <asp:DataList ID="dtlReportList" runat="server" RepeatColumns="2" RepeatLayout="Table"
        
        RepeatDirection="Horizontal" EnableViewState="false"
        CssClass="fixed-table">
        <ItemTemplate>
            <div class="report-item">
               <%-- <asp:Image ID="imgReportImage" runat="server" ImageUrl='<%#XPath("ImageUrl")%>' AlternateText="Report Image"
                    EnableViewState="false" CssClass="left-floater" />--%>                                
              <table id="tblReports" >
                  <tr>
                      <td>
                          <asp:Image runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ImageUrl")%>' />
                          </td><td>
                          <asp:HyperLink ID="hlkReportName"  runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Url")%>'
                    EnableViewState="false">
                        <span class="report-name">
                        <%# DataBinder.Eval(Container.DataItem, "Name")%></span>
                </asp:HyperLink>
                <span class="report-desc">
                    <%# DataBinder.Eval(Container.DataItem, "Description")%>
                </span>
                      </td>
                  </tr>
              </table>
                
                <div class="clear">
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
    <%--<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/Data/Reports.xml"
        XPath="//Reports/Report"></asp:XmlDataSource>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
