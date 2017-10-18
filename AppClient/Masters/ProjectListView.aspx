<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ProjectListView.aspx.cs" Inherits="Masters_ProjectListView" Theme="Classical" %>

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
                        <asp:Label runat="server" ID="lblProjectManagement" Text="Project Management"></asp:Label></h2>
                </div>
                <div id="divMessage" runat="server" class="errMsg" style="display: none">
                </div>
                <div id="divmsg" runat="server" class="successMsg">
                    Successfully message ...
                </div>
                <table class="formGrid">
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblProject" Text="Project"><%--<label for="<%=txtName.ClientID %>">--%>
                            </asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtName" maxlength="100" runat="server" />
                        </td>
                        <td></td>
                        <td colspan="2">
                            <%--<label for="<%=txtClient.ClientID %>">--%>
                            <asp:Label runat="server" ID="lblClient" Text="Client">
                            </asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtClient" runat="server" />
                        </td>
                        <td></td>
                        <td colspan="2">
                            <%--<label for="<%=txtLocation.ClientID %>">--%>
                            <asp:Label runat="server" ID="lblLocation" Text="Location"></asp:Label>

                        </td>
                        <td colspan="5">
                            <input type="text" id="txtLocation" runat="server" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Label runat="server" ID="lblCategory" Text="Category"></asp:Label></td>
                        <td></td>
                        <td colspan="5">
                            <select id="ddCategory" runat="server">
                                <option value="0" selected="selected">Others</option>
                                <option value="1">Mobile/ Social (clubbed together)</option>
                                <option value="2">MMO – Massive multimedia online</option>
                                <option value="3">Console</option>
                            </select>
                        </td>


                        <td></td>
                        <td colspan="4"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblplatform" Text="Platform"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtPlatform" runat="server" />
                        </td>
                        <td></td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblTest" Text="Test"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtTest" runat="server" />
                        </td>
                        <td></td>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                        </td>
                        <td colspan="5">
                            <select id="ddlStatus" runat="server">
                                <option value="All">All</option>
                                <option value="Active" selected="selected">Active</option>
                                <option value="InActive">InActive</option>
                            </select>
                        </td>
                        <td></td>
                        <td></td>

                        <td colspan="7">
                            <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Filter"
                                CssClass="primaryButton" />
                            <asp:Button ID="btnclear" runat="server" Text="Clear" OnClick="btnclear_Click1" CssClass="secondaryButton" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div id="div1" runat="server" style="padding: 40px 0px 0px 0px;">
            </div>
            <div class="gridViewContainer">
                <div id="hdrGridHeader" runat="server" class="resultHeader">
                </div>
                <div class="lightToolbar" style="float: right;">
                    <ul class="lightToolbarItems">
                        <li><a href="032-0-Project-Edit">
                            <asp:Label runat="server" ID="lblAddNew" Text="Add New"></asp:Label></a></li>
                        <%--ProjectEditView.aspx?ProjectId=0--%>
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
                                    <asp:Label runat="server" ID="lblProjectid" Text="Project"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblClientid" Text="Client"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblDescription" Text="Description"></asp:Label>
                                </th>
                                <%-- <th>
                                    Manager
                                </th>--%>
                                <th>
                                    <asp:Label runat="server" ID="lblStatusid" Text="Status"></asp:Label>
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

                        </table>
                    </div>
                    <div>
                        <asp:GridView ID="GvwUserList" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                            AllowPaging="true" PageSize="10" OnRowDataBound="GvwUserList_RowDataBound1" OnPageIndexChanging="GvwUserList_PageIndexChanging">
                            <Columns>

                                <asp:TemplateField HeaderText="Project">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblProjectid" runat="server" Text="Project"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Name") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Client">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblClientid" runat="server" Text="Project"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnClient" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Description") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="Manager">
                                    <ItemTemplate>
                                        <span id="spnManager" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Status">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStatusid" runat="server" Text="Project"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnIsActive" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAction" runat="server" Text="Project"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="032-<%#DataBinder.Eval(Container.DataItem,"Id") %>-Project-Edit">Edit</a>
                                        <%--<a href="ProjectEditView.aspx?ProjectId=<%#DataBinder.Eval(Container.DataItem,"Id") %>">
                                            Edit</a>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Last Update User">
                                    <ItemTemplate>
                                        <span id="spnUsername" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Last Update Date" DataField="LastUpdateDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss tt}" />--%>
                            </Columns>
                        </asp:GridView>
                        <tr>
                            <td colspan="8">
                                <span id="spnMessage" runat="server"></span>
                            </td>
                        </tr>
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
                $('#' + '<%=divmsg.ClientID %>').fadeOut(8000);
            }
        );



        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
