<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" CodeFile="wfrmUserPermission.aspx.cs" Inherits="UsersProfile_wfrmUserPermission" Theme="Classical" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .divTreeview {
            border: 1px solid seagreen;
            height: 450px;
            overflow-x: hidden;
            overflow-y: auto;
            width: 99%;
        }
    </style>
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
                        <asp:Label runat="server" ID="lblUserPermissions" Text="User Permissions"></asp:Label></h2>
                </div>
                <div id="divMessage" runat="server" class="errMsg" style="display: none">
                </div>               
                <table class="formGrid">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblPermissionId" Text="Permission Id">
                            </asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtPermissionNo" maxlength="10" runat="server" />
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
                                    <label><% = lblPermissionId.Text %></label>
                                </th>   
                                <th>
                                    <asp:Label runat="server" ID="lblAccessLocations" Text="Access Locations"></asp:Label>
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
                        <asp:GridView ID="GvUserPermission" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                            AllowPaging="true" PageSize="10" OnRowDataBound="GvUserPermission_RowDataBound" OnPageIndexChanging="GvUserPermission_PageIndexChanging" OnRowCommand="GvUserPermission_RowCommand">
                            <Columns>

                                <asp:TemplateField HeaderText="Permission Id">
                                    <HeaderTemplate>
                                       <asp:Label runat="server" ID="lblPermissionId" Text="Permission Id"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Eval("PermissionNo") %>' CommandArgument='<%#Eval("PermissionId") %>' CommandName='<%# Eval("PermissionNo") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Locations">
                                    <HeaderTemplate>
                                       <asp:Label runat="server" ID="lblAccessLocations" Text="Access Locations"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <span id="spnLocations" runat="server"></span>
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
                                        <a href="<%#DataBinder.Eval(Container.DataItem,"PermissionId") %>-Permission-EDIT">Edit</a>                                       
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                            </Columns>
                        </asp:GridView>                       
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialog">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="formGrid" style="width: 100%">
                    <tr>
                        <td colspan="6" align="left"><h2><asp:Label ID="lblPermissionName" runat="server" Text="Permission Name"></asp:Label></h2></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <h4>
                                <asp:Label ID="lblLocations" runat="server" Text="Locations"></asp:Label>
                            </h4>
                        </td>
                        <td colspan="2" align="left">
                            <h4>
                                <asp:Label ID="lblClients" runat="server" Text="Clients"></asp:Label></h4>
                        </td>
                        <td colspan="2" align="left">
                            <h4>
                                <label>
                                    <asp:Label ID="lblReports" runat="server" Text="Reports"></asp:Label></label></h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvLocations" runat="server" TabIndex="1" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true" ShowExpandCollapse="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvClients" runat="server" TabIndex="2" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                        <td colspan="2" align="left" valign="top">
                            <div class="divTreeview">
                                <asp:TreeView ID="tvReports" runat="server" TabIndex="3" ShowCheckBoxes="None" ExpandDepth="0" ShowLines="true">
                                    <LeafNodeStyle CssClass="leafNode" />
                                    <NodeStyle CssClass="treeNode" />
                                    <RootNodeStyle CssClass="rootNode" />
                                    <SelectedNodeStyle CssClass="selectNode" />
                                </asp:TreeView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[

        $(document).ready(

            function () {
                initializeEditPanelDialog();
                $('#' + '<%=divMessage.ClientID %>').fadeOut(8000);
            }
        ); 
       

        function initializeEditPanelDialog() {
            // Display edit panel as dialog.
            $('#divDialog').dialog('destroy');
            $('#divDialog').dialog(
                {
                    autoOpen: false,
                    modal: true,
                    closeOnEscape: false,
                    show: 'fade',
                    hide: 'clip',
                    width: '800px',
                    draggable: true,
                    resizable: false,
                    title: "Title here...",
                    open: function (event, ui) {
                        $(this).parent().appendTo("form:first");
                        return false;
                    },
                    close: function (event, ui) {
                        // Set the focus on input control.
                        return false;
                    }
                });
        }

        function showEditPanelDialog(options) {
            if (options !== null)
                $('#divDialog').dialog('option', options);
            $('#divDialog').dialog('open');
        }

        function closeEditPanelDialog() {
            $('#divDialog').dialog('close');
            return false;
        }
        $(document).ready(function () {
            
        });

        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>


