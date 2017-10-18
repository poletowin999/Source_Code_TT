<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ProjectEditView.aspx.cs" Inherits="Masters_ProjectEditView" Theme="Classical" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register Src="~/SearchViews/UserSearchView.ascx" TagName="UserSearchView" TagPrefix="tks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" src="../Scripts/RowSelectionControl.js"></script>--%>
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
    <style type="text/css">
        .text-label
        {
            color: #cdcdcd;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <%--<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server" />--%>
    <div id="divProjectInfoPanel">
        <asp:UpdatePanel ID="ProjectInfoPanel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                    <ProgressTemplate>
                        <div class="loading">
                            Loading...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="page-header-panel">
                    <h2 id="HeaderTitle" runat="server"></h2>
                </div>
                <div id="divmsg" runat="server" class="successMsg" style="display: none">
                    Successfully message ...
                </div>
                <div id="divinf" runat="server" class="warnMsg" style="display: none">
                    Error message here...
                </div>
                <table cellpadding="0" cellspacing="0" border="0" style="table-layout: fixed; width: 100%">
                    <tr>
                        <td colspan="35%">
                            <div class="headerPanel3">
                                <h4>
                                    <asp:Label runat="server" ID="lblProjectInformation" Text="Project Information"></asp:Label></h4>
                            </div>
                            <table class="tableLayout" border="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblName" Text="Name :"></asp:Label>
                                        <span class="Mandetary">*</span>

                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="textBox"></asp:TextBox>
                                        <input type="hidden" id="hdnProjectId" runat="server" value="0" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblClient" Text="Client :"></asp:Label>
                                        <span class="Mandetary">*</span>
                                    </td>
                                    <td colspan="4">
                                        <%--<asp:TextBox ID="txtClient" runat="server" CssClass="textBox"></asp:TextBox>--%>
                                        <input type="text" id="txtClient" runat="server" onkeypress="validautocomplete(event);" />
                                        <input type="hidden" id="hdnClientId" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblProjectManager" Text="Project Manager :"></asp:Label><span class="Mandetary">*</span>
                                    </td>
                                    <td colspan="4">
                                        <input type="text" id="txtResponsibleUser" runat="server" title="" onfocus="return onRespUserFocus(this);"
                                            onblur="return onRespUserBlur(this);" />
                                        <input type="hidden"
                                            id="hdnUserId" runat="server" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ibtSearchUsers" ImageAlign="AbsBottom" runat="server" CssClass="img16"
                                            ImageUrl="~/Images/user20.png" OnClick="ibtSearchUsers_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" valign="top">
                                        <asp:Label runat="server" ID="lblDescription" Text="Description :"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5"
                                            CssClass="textBox"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="60%" valign="top" style="padding-left: 5px">
                            <div>
                                <div class="headerPanel3">
                                    <h4>
                                        <asp:Label runat="server" ID="lblProjectLocations" Text="Project Locations"></asp:Label></h4>
                                </div>
                                <table class="tableLayout" border="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblLocation" Text="Location :"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="4">
                                            <input type="text" id="txtLocation" runat="server" onkeypress="validautocomplete(event);" />
                                        </td>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblLocationManager" Text="Location Manager :"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="4">
                                            <input type="text" id="txtLocationUser" runat="server" onkeypress="validautocomplete(event);" />
                                            <input type="hidden" id="hdnLocationUser" runat="server" />
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnAddLocation" runat="server"
                                                OnClick="btnAddLocation_Click" Text="Add"></asp:Button>
                                            <input type="hidden" id="hdnLocationId" runat="server" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblCategory" Text="Category :"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="4">
                                            <asp:DropDownList ID="ddcategory" runat="server" AutoPostBack="True">
                                                <asp:ListItem Value="0" Text="Others"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mobile/ Social (clubbed together)"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="MMO – Massive multimedia online"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Console"></asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                </table>
                                <div id="dvinitalvalue" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" class="gridView">
                                        <tr>
                                            <th>
                                                <asp:Label runat="server" ID="lblActionGRD" Text="Action"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblCityGRD" Text="City"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblStateGRD" Text="State"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblCountryGRD" Text="Country"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblManagerGRD" Text="Manager"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblStatusGRD" Text="Status"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblLastUPDGRD" Text="Last Update Date"></asp:Label>
                                            </th>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 100%; height: 150px; overflow: auto;">
                                    <asp:GridView ID="gvwLocation" runat="server" GridLines="Horizontal" BorderColor="#A8C5E1"
                                        CssClass="gridView" AutoGenerateColumns="False" DataKeyNames="Id" Width="100%"
                                        OnRowCommand="gvwLocation_RowCommand">
                                        <Columns>
                                            <asp:ButtonField Text="Delete" HeaderText="" CommandName="LocationEdit" ButtonType="Link"
                                                ImageUrl="~/Images/icocross.gif" ItemStyle-Width="50px" />

                                            <asp:TemplateField HeaderText="City">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCityGRD" runat="server" Text="City"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("City") %>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="State">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblStateGRD" runat="server" Text="State"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("State") %>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Country">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCountryGRD" runat="server" Text="Country"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Country") %>
                                                </ItemTemplate>

                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Manager" ItemStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblManagerGRD" runat="server" Text="Manager"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnLocationManagerId" runat="server" Value='<% #Eval("CustomData[LocationManagerId]")%>' />
                                                    <asp:Label ID="lblLocationManager" runat="server" Text='<%# Eval("CustomData[LocationManager]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="50px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblStatusGRD" runat="server" Text="Status"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnIsActive" runat="server" Value='<% #Eval("CustomData[IsActive]")%>' />
                                                    <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("CustomData[CustomActive]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Country">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblLastUPDGRD" runat="server" Text="Country"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("LastUpdateDate") %>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <%--      <asp:BoundField DataField="LastUpdateDate" DataFormatString="{0:MM/dd/yyyy HH:mm}"
                                                HeaderText="Last Update Date" ItemStyle-Width="150px" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                        <td colspan="5%"></td>
                    </tr>
                </table>
                <div class="headerPanel3">
                    <h4>
                        <asp:Label runat="server" ID="lblPlatformandTest" Text="Platform and Test"></asp:Label></h4>
                </div>
                <div class="treeDivToolBar">
                    <asp:LinkButton ID="lnkPlatformEdit" runat="server" Text="Edit" OnClick="lnkPlatformEdit_Click"></asp:LinkButton>
                    <%--&nbsp;
                        <asp:LinkButton ID="lnkPlatfromDelete" runat="server" Text="Delete" OnClick="lnkPlatfromDelete_Click"></asp:LinkButton>--%>
                </div>
                <div class="lightToolbar" style="float: right;">
                    <ul class="lightToolbarItems">
                        <li>
                            <asp:Button ID="btnAddPlatform" runat="server" Text="Add New" OnClick="btnAddPlatform_Click"></asp:Button></li>
                        <li style="border-left: 1px solid #ffffff;">
                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click"></asp:Button></li>
                    </ul>
                </div>
                <div class="clearFloat">
                </div>
                <div class="treeDiv" style="height: 300px; overflow: auto">
                    <asp:TreeView ID="TreHierarchy" runat="server" SelectedNodeStyle-Font-Bold="true"
                        OnSelectedNodeChanged="TreHierarchy_SelectedNodeChanged">
                    </asp:TreeView>
                </div>
                <div class="contentViewDiv" style="height: 300px">
                    <table id="Tablelabel" class="gridView" border="0" runat="server" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <th>&nbsp;
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblNameGrid" Text="Name"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblDescriptionGrid" Text="Description"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblplatform" Text="Platform"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                            </th>
                        </tr>

                    </table>

                    <asp:GridView ID="gvwPlatformTests" runat="server" GridLines="Horizontal" AutoGenerateColumns="false" 
                        CssClass="gridView" AllowPaging="true" PageSize="5" OnPageIndexChanging="gvwPlatformTests_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnTestsId" runat="server" Value='<% #Eval("Id")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name1">
                                <HeaderTemplate>
                                    <asp:Label ID="lblNameGrid" runat="server" Text="Name2"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Description">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDescriptionGrid" runat="server" Text="Description"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Description") %>
                                </ItemTemplate>

                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Platform">
                                <HeaderTemplate>
                                    <asp:Label ID="lblplatform" runat="server" Text="Platform"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlatformName" runat="server" Text='<%# Eval("CustomData[PlatformName]") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">

                                <HeaderTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# Eval("CustomData[CustomActive]") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>


                        <SelectedRowStyle Font-Size="12px" />
                        <HeaderStyle />
                        <RowStyle />
                        <AlternatingRowStyle />
                        <PagerStyle />
                    </asp:GridView>
                    <tr>
                        <td align="center" colspan="5">
                            <span id="testnotavail" class="noRows">
                                <asp:Label runat="server" Text="-- Test not available --" ID="lblTestnotavailable"></asp:Label>
                            </span>
                        </td>
                    </tr>
                </div>
                <div class="clear">
                </div>
                <div id="divActive" runat="server">
                    <div class="headerPanel3">
                         <h4>
                        <asp:Label runat="server" ID="lblOthers" Text="Others"></asp:Label></h4>
                    </div>
                    <table class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblActive" Text="Active:"></asp:Label>
                            </td>
                            <td colspan="4">
                                <input type="checkbox" id="chkActive" runat="server" />
                            </td>
                            <td colspan="7"></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3" valign="top">
                                <asp:Label runat="server" ID="lblreason" Text="Reason:">
                                </asp:Label>
                            </td>
                            <td colspan="4">
                                <textarea id="txtReason" runat="server" rows="5" cols="13" style="width: 95%"></textarea>
                            </td>
                            <td colspan="7"></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="commandPanel">
            <asp:Button runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click"
                CssClass="primaryButton" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                CssClass="secondaryButton" />
        </div>
    </div>
    <div id="divAddPlatformTest">
        <div runat="server" id="divUpdatePanelTest">
            <asp:UpdatePanel ID="updatePanelPlatformTest" runat="server">
                <ContentTemplate>
                    <div id="divSuccess" runat="server" class="popupWarnMsg" style="display: none">
                        Successfully message ...
                    </div>
                    <div id="divErrorMessage" runat="server" class="popupWarnMsg" style="display: none">
                        Error message here...
                    </div>
                    <div>
                        <asp:Label runat="server" ID="lblPlatformTest" Text="Platform and Tests Selection:"></asp:Label>
                    </div>
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="width: 250px">
                                <asp:DropDownList ID="drpPlatform" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px">
                                <div>
                                    <table border="0" cellpadding="0" cellspacing="0" class="gridView">
                                        <tr>
                                            <th>
                                                <asp:Label runat="server" ID="lblActionPOP" Text="Action"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblNamePOP" Text="Name"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label runat="server" ID="lblDescriptionPOP" Text="Description"></asp:Label>
                                            </th>
                                        </tr>
                                    </table>
                                    <%--                <div id="divtestsListHeader" runat="server" class="header3">
                                        Available Tests (n found):</div>--%>
                                    <div class="gridViewPanel" style="width: 500px; height:700px">
                                        <asp:GridView ID="gvwTests" runat="server" GridLines="Horizontal" AutoGenerateColumns="false" ShowHeader="false"
                                            CssClass="gridView" Width="500" AllowPaging="false" OnRowDataBound="gvwTests_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkTestsId" runat="server" />
                                                        <asp:HiddenField ID="hdnTestsId" runat="server" Value='<% #Eval("Id")%>' />
                                                        <asp:HiddenField ID="hdnName" runat="server" Value='<% #Eval("Name")%>' />
                                                        <asp:HiddenField ID="hdnDescription" runat="server" Value='<% #Eval("Description")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" HorizontalAlign="Left" />
                                                    <HeaderTemplate>
                                                        <%--                      <img id="imgRowSelectionButton" src="../Images/multiselect_checked.png" class="img16"
                                                            alt="Select" title="Select/Unselect the rows" />--%>
                                                        <div>
                                                            <asp:Label runat="server" ID="lblSelect" Text="Select"></asp:Label>
                                                        </div>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="300px" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="300px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <table class="resultGrid" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr class="resultGridRowHeader">
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblNamePT" Text="Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblDescriptionPT" Text="Description"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="11">
                                                            <span class="noRows">-- Files not found --</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle Font-Size="12px" />
                                            <HeaderStyle Height="30px" />
                                            <RowStyle />
                                            <AlternatingRowStyle />
                                            <PagerStyle Height="30px" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsActive" runat="server" /><asp:Label runat="server" ID="lblActivePT" Text="Active:"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="primaryButton" OnClick="btnOK_Click" />
                                &nbsp;
                                <asp:Button ID="btnRemove" runat="server" Text="Cancel" OnClientClick="return closeEditPanelDialog();" OnClick="btnRemove_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:Label runat="server" ID="lblUserSearchview" Text="User Search view" Visible="false"></asp:Label>
    <div id="divUsersSearchPanel">
        <tks:UserSearchView ID="UserSearchView1" onSearchResultSelect="userSearchResultSelected"
            runat="server" onDialogClose="closeUserSearchView" />
    </div>
    <asp:Label ID="lblTestValue" runat="server" Text="Test" Visible="false"></asp:Label>
    <asp:Label ID="lblPlatformValue" runat="server" Text="Platform" Visible="false"></asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[

        $(document).ready(
            function () {

                initializeClientAutoComplete();
                initializeLocationUserAutoComplete();
                initializeLocationAutoComplete();
                initializeEditPanelDialog(mUserSearchViewDialogOptions);

                $('#' + '<%=txtResponsibleUser.ClientID%>').bind('focus', function () {
                    $('#' + '<%=txtResponsibleUser.ClientID%>').val("");
                });

            }
        );


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
                function (sender, args) {
                    initializeClientAutoComplete();
                    initializeLocationUserAutoComplete();
                    initializeLocationAutoComplete();
                    if (args.get_error() !== null) {
                        // Display error.
                        alert(args.get_error().message.replace(args.get_error().name, '').replace(':', '').trim());
                    }
                });


            function initializeClientAutoComplete() {
                var ClientName = $('#' + '<%=txtClient.ClientID%>');
                var ClientId = $('#' + '<%=hdnClientId.ClientID%>');
                ClientName.autocomplete('destroy');
                ClientName.autocomplete(
                        {
                            minLength: 0,
                            source: function (request, response) {
                                $.ajax(
                                    {
                                        url: '../WebServices/MasterService.asmx/GetClientsByName',
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8',
                                        data: "{ 'name': '" + request.term + "','userid': '0'}",
                                        dataType: "json",
                                        success: function (data, textStatus, req) {
                                            var dataArray = $.parseJSON(data.d);
                                            if (dataArray === null) return;
                                            response(dataArray);
                                        },
                                        error: function (req, textStatus, error) {
                                            alert(error);
                                        }
                                    });
                            },

                            focus: function (event, ui) {
                                $(this).val(ui.item.Name);
                                return false;
                            },

                            select: function (event, ui) {
                                $(this).val(ui.item.Name);
                                ClientId.val(ui.item.Id);
                                //$('#TimeZone').html('Id: ' + ui.item.Id)
                                return false;
                            },

                            change: function (event, ui) {
                                if (ui.item === null) {
                                    $(this).val("");
                                    ClientId.val("");
                                }
                                return false;
                            }
                        })
                        .data("autocomplete")._renderItem = function (ul, item) {
                            return $("<li></li>")
                                    .data("item.autocomplete", item)
                                    .append("<a><span>" + item.Name + "</span></a>")
                                    .appendTo(ul);
                        };
            }

            function initializeLocationUserAutoComplete() {
                var ClientName = $('#' + '<%=txtLocationUser.ClientID%>');
                var ClientId = $('#' + '<%=hdnLocationUser.ClientID%>');
                ClientName.autocomplete('destroy');
                ClientName.autocomplete(
                        {
                            minLength: 0,
                            source: function (request, response) {
                                $.ajax(
                                    {
                                        url: '../WebServices/MasterService.asmx/GetUsersByLocation',
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8',
                                        data: "{ 'name': '" + request.term + "'}",
                                        dataType: "json",
                                        success: function (data, textStatus, req) {
                                            var dataArray = $.parseJSON(data.d);
                                            if (dataArray === null) return;
                                            response(dataArray);
                                        },
                                        error: function (req, textStatus, error) {
                                            alert(error);
                                        }
                                    });
                            },

                            focus: function (event, ui) {
                                $(this).val(ui.item.Name + " " + ui.item.ShortName);
                                return false;
                            },

                            select: function (event, ui) {
                                $(this).val(ui.item.Name + " " + ui.item.ShortName);
                                ClientId.val(ui.item.Id);
                                //$('#TimeZone').html('Id: ' + ui.item.Id)
                                return false;
                            },

                            change: function (event, ui) {
                                if (ui.item === null) {
                                    $(this).val("");
                                    ClientId.val("");
                                }
                                return false;
                            }
                        })
                        .data("autocomplete")._renderItem = function (ul, item) {
                            return $("<li></li>")
                                    .data("item.autocomplete", item)
                                    .append("<a><span>" + item.Name + "  " + item.ShortName + "</span></a>")
                                    .appendTo(ul);
                        };
            }


            function initializeLocationAutoComplete() {
                var LocationName = $('#' + '<%=txtLocation.ClientID%>');
                var LocationtId = $('#' + '<%=hdnLocationId.ClientID%>');
                LocationName.autocomplete('destroy');
                LocationName.autocomplete(
                        {
                            minLength: 0,
                            source: function (request, response) {
                                $.ajax(
                                    {
                                        url: '../WebServices/MasterService.asmx/GetLocationsByCity',
                                        type: 'POST',
                                        contentType: 'application/json; charset=utf-8',
                                        data: "{ 'cityName': '" + request.term + "','userid': '0'}",
                                        dataType: "json",
                                        success: function (data, textStatus, req) {
                                            var dataArray = $.parseJSON(data.d);
                                            if (dataArray === null) return;
                                            response(dataArray);
                                        },
                                        error: function (req, textStatus, error) {
                                            alert(error);
                                        }
                                    });
                            },

                            focus: function (event, ui) {
                                $(this).val(ui.item.City);
                                return false;
                            },

                            select: function (event, ui) {
                                $(this).val(ui.item.City);
                                LocationtId.val(ui.item.Id);
                                return false;
                            },

                            change: function (event, ui) {
                                if (ui.item === null) {
                                    $(this).val("");
                                    LocationtId.val("");
                                }
                                return false;
                            }
                        })
                        .data("autocomplete")._renderItem = function (ul, item) {
                            return $("<li></li>")
                                    .data("item.autocomplete", item)
                                     .append("<a><span>" + item.City + "<br>(" + item.Country + ")</span></a>")
                                    .appendTo(ul);
                        };
            }

            function initializeEditPanelDialog() {
                // Display edit panel as dialog.
                $('#divAddPlatformTest').dialog('destroy');
                $('#divAddPlatformTest').dialog(
                    {
                        autoOpen: false,
                        modal: true,
                        closeOnEscape: false,
                        show: 'fade',
                        hide: 'clip',
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
                    $('#divAddPlatformTest').dialog('option', options);
                $('#divAddPlatformTest').dialog('open');
                $('#<%=drpPlatform.ClientID%>').focus();
            }

            function closeEditPanelDialog() {
                $('#divAddPlatformTest').dialog('close');
                $('#' + '<%=divSuccess.ClientID %>').fadeOut(1500);
                return false;
            }


            var mUserSearchViewDialog = null;
            var mUserSearchViewDialogOptions = {
                'inputControlId': '<%=txtResponsibleUser.ClientID%>',
                'searchButtonId': '<%=ibtSearchUsers.ClientID%>',
                'valueControlId': '<%=hdnUserId.ClientID%>',
                'searchControlPanelId': 'divUsersSearchPanel',
                'title': '<%=lblUserSearchview.Text%>'
            };

            function refreshUserSearchView() {
                if (mUserSearchViewDialog !== null) {
                    mUserSearchViewDialog.set_options(mUserSearchViewDialogOptions);
                }
            }

            function showUserSearchView() {
                mUserSearchViewDialog.show();
            }

            function closeUserSearchView() {
                mUserSearchViewDialog.close();
                return false;
            }

            function userSearchResultSelected(result) {
                var resultObject = Sys.Serialization.JavaScriptSerializer.deserialize(result);
                mUserSearchViewDialog.set_displayText(resultObject.LastName + ',' + resultObject.FirstName);
                mUserSearchViewDialog.set_valueText(resultObject.Id);
                mUserSearchViewDialog.close();
            }




            function onRespUserFocus(ctrl) {
                if (ctrl.value == ctrl.title) {
                    ctrl.value = '';
                    ctrl.className = '';
                }
            }

            function onRespUserBlur(ctrl) {
                if (ctrl.value == '') {
                    ctrl.value = ctrl.title;
                    ctrl.className = 'WatermarkText';
                }
            }

            function validautocomplete(evt) {
                //var usernme=document.frmAddUser.userName;alert(usernme.value);
                // Verify if the key entered was a numeric character (0-9) or a decimal (.)
                var key;
                if (evt.keyCode) { key = evt.keyCode; }
                else if (evt.which) { key = evt.which; }

                if (((key == 39) ||
                     (key == 92))) {
                    if (evt.preventDefault)
                        evt.preventDefault();
                    else
                        evt.returnValue = false;
                }
            }


            $(document).ready(
                function () {
                    mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);
                }
            );

            // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
