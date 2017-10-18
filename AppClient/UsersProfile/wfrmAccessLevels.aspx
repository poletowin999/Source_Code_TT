<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" CodeFile="wfrmAccessLevels.aspx.cs" Inherits="UsersProfile_wfrmAccessLevels" Theme="Classical" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdateSearchPanel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div>
                <div class="page-header-panel">
                    <h2>
                        <asp:Label runat="server" ID="lblAccessLevels" Text="Access Levels"></asp:Label></h2>
                </div>
                <div id="divMessage" runat="server" class="errMsg" style="display: none">
                </div>               
                <table class="formGrid">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblAccessLevelName" Text="Access Level Name.">
                            </asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtAccessLevelName" maxlength="10" runat="server" />
                        </td>
                        <td></td>
                        <td >
                            <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                        </td>
                        <td>
                            <select id="ddlStatus" runat="server">
                                <option value="All">All</option>
                                <option value="Active" selected="selected">Active</option>
                                <option value="InActive">InActive</option>
                            </select>
                        </td>                        
                        <td></td>

                        <td>
                            <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Filter"
                                CssClass="primaryButton" />
                            <asp:Button ID="btnclear" runat="server" Text="Clear" OnClick="btnclear_Click" CssClass="secondaryButton" />
                        </td>                     
                       
                    </tr>                  
                </table>
            </div>
            <div id="div1" runat="server" style="padding: 40px 0px 0px 0px;">
            </div>
            <div class="gridViewContainer">
                <div id="hdrGridHeader" runat="server" class="gridViewHeader" style="float: left;">
                </div>
                <div class="lightToolbar" style="float: right;">
                    <ul class="lightToolbarItems">
                        <li><a href="Add">
                            <asp:Label runat="server" ID="lblAddNew" Text="Add New"></asp:Label></a></li>                        
                        <li>
                            <asp:LinkButton ID="LnkRefersh" runat="server" OnClick="LnkRefersh_Click">
                                <asp:Label runat="server" ID="lblRefresh" Text="Refresh"></asp:Label>
                            </asp:LinkButton></li>
                    </ul>
                </div>
                <div class="clearFloat">
                </div>
                <div class="gridViewPanel"> 
                     <div id="InitalBind" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                            <tr>
                                <th>
                                    <label><% = lblAccessLevelName.Text %></label>
                                </th>  
                                 <th>
                                    <asp:Label ID="lblAccessLevelDescription" runat="server" Text="Access Level Description"></asp:Label>
                                </th> 
                                <th>
                                   <asp:Label ID="lblPermissions" runat="server" Text="Permissions"></asp:Label>
                                </th>                               
                                <th>
                                    <asp:Label runat="server" ID="lblStatusGrid" Text="Status"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                                </th>
                                <%--<th>
                                    Last Update User
                                </th>
                                <th>
                                    Last Update Date
                                </th>--%>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <span id="spnMessage" runat="server"></span>
                                </td>
                            </tr>
                        </table>
                    </div>                  
                    <div>
                        <asp:GridView ID="GvAccessLevel" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                            AllowPaging="true" PageSize="10" OnRowDataBound="GvAccessLevel_RowDataBound" OnPageIndexChanging="GvAccessLevel_PageIndexChanging">
                            <Columns>

                                <asp:TemplateField HeaderText="Access Level Name">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAccessLevelName" runat="server" Text="Access Level Name"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("AccessLevelName") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Access Level Description">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAccessLevelDescription" runat="server" Text="Access Level Description"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("AccessLevelName") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Permissions">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPermissions" runat="server" Text="Permissions"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnPermissions" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                                  
                                <asp:TemplateField HeaderText="Status">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStatusid" runat="server" Text="Status"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnIsActive" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAction" runat="server" Text="Action"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="<%#DataBinder.Eval(Container.DataItem,"Id") %>-AccessLevel-EDIT">Edit</a>                                       
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                            </Columns>
                        </asp:GridView>                       
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[

        $(document).ready(
            function () {
                $('#' + '<%=divMessage.ClientID %>').fadeOut(8000);
            }
        );
        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>


