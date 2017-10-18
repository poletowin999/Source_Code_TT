<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="CommonMastersListView.aspx.cs" Inherits="Masters_CommonMastersListView" 
    Theme="Classical" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <!-- Search Entities -->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
             <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                <div>
                    <div class="page-header-panel">
                        <h2>
                            <span id="spnTitle" runat="server"></span>&nbsp;
                        </h2>
                    </div>
                    <div id="divSuccess" class="successMsg" runat="server">
                        Success information here...
                    </div>
                    <div id="divSuccessMessage" runat="server" class="errMsg">
                    </div>
                    <table class="formGrid">
                        <tr>
                            <td colspan="2">
                            </td>
                            <td colspan="6">
                                <label>
                                <span id="spnName" runat="server">Name:</span>
                                    
                                </label>
                            </td>
                            <td colspan="12">
                                <input type="text" id="txtName" runat="server"  onkeypress="validUser(event);" />
                            </td>
                            <td colspan="2">
                            </td>
                            <td colspan="6">
                                <asp:Label id="lblStatusGrid" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td colspan="12">
                                <select id="ddlStatus" runat="server">
                                    <option value="All">All</option>
                                    <option value="Active" selected="selected">Active</option>
                                    <option value="InActive">InActive</option>
                                </select>
                            </td>
                            <td colspan="2">
                            </td>
                            <td colspan="45">
                                <asp:Button ID="btnSearch" runat="server" UseSubmitBehavior="true" Text="Filter"
                                    CssClass="primaryButton" OnClick="btnSearch_ServerClick" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" UseSubmitBehavior="false"
                                    CssClass="secondaryButton" OnClick="btnClear_ServerClick" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div1" runat="server" style="padding: 40px 0px 0px 0px;">
                </div>
                <div class="gridViewContainer">
                    <div id="hdrGridHeader" runat="server" class="gridViewHeader" style="float: left">
                        Entity List : (n found)</div>
                    <div class="lightToolbar" style="float: right;">
                        <ul class="lightToolbarItems">
                            <li>
                                <asp:Button ID="lbtAddNew" runat="server" Text="Add" OnClick="lbtAddNew_Click"></asp:Button></li>
                            <li>
                                <asp:Button ID="lbtRefresh" Text="Refresh" runat="server" OnClick="lbtRefreshList_Click"></asp:Button></li>
                        </ul>
                    </div>
                    <div class="clearFloat">
                    </div>
                    <div class="gridViewPanel">
                        <div id="dvinitalvalue" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" class="gridView">
                                <tr>
                                    
                                    <th>
                                        <asp:Label runat="server" ID="lblGridName" Text="Name"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" ID="lblgridDescription" Text="Description"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" ID="lblGridStatus" Text="Status"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" ID="lblGridAction" Text="Action"></asp:Label>
                                    </th>
                                    <%--<th>
                                        Last Update User
                                    </th>
                                    <th>
                                        Last Update Date
                                    </th>--%>
                                </tr>
                                <tr>
                                    <td colspan="6" id="divEmptyRow" runat="server">
                                        <span id="hideSpan" runat="server">Enter the criteria and click on Search button to
                                            view data.</span> <span id="InitalspnMessage" runat="server"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="gvwEntityList" runat="server" CssClass="gridView" AutoGenerateColumns="False" 
                            EnableModelValidation="True" DataKeyNames="Id" OnRowCommand="gvwEntityList_RowCommand"
                            OnRowDataBound="gvwEntityList_RowDataBound" OnPageIndexChanging="gvwEntityList_PageIndexChanging"
                            AllowPaging="true" PageSize="10">
                            <PagerStyle ForeColor="Blue" />
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                <HeaderTemplate>
                                    <asp:Label ID="lblGridName" runat="server" Text="Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Description">
                                <HeaderTemplate>
                                    <asp:Label ID="lblgridDescription" runat="server" Text="Description"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Description") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                
                                <%-- <asp:BoundField DataField="IsActive" HeaderText="Status" />--%>
                                <asp:TemplateField HeaderText="Status">
                                     <HeaderTemplate>
                                    <asp:Label ID="lblGridStatus" runat="server" Text="Status"></asp:Label>
                                </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnIsActive" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Action">
                                     <HeaderTemplate>
                                        <asp:Label ID="lblGridAction" runat="server" Text="Action"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtn" runat="server" Text="Edit" OnClick="lnkBtn_Click" HeaderText="Action" CommandName="ClientEdit"/>

                                        <asp:ButtonField Text="Edit" HeaderText="Action" CommandName="ClientEdit" ButtonType="Link"/>
                                    </ItemTemplate>                                   
                                </asp:TemplateField>--%>
                                <asp:ButtonField Text="Edit" CommandName="ClientEdit" ButtonType="Link"/>
                                <%--<asp:TemplateField HeaderText="Last Update User">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastUpdateUser" runat="server" Text='<%# Eval("CustomData[LastUpdateUserName]") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LastUpdateDate" HeaderText="Last Update Date" DataFormatString="{0:MM/dd/yyyy HH:mm:ss tt}" />--%>
                               
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <%--  <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="ServerClick" />
            </Triggers>--%>
        </asp:UpdatePanel>
        <div id="divDialog">
            <div id="divEntityEditPanel" runat="server">
                <!-- Add and Edit Entity -->
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div id="divStatusInfo" runat="server" class="popupWarnMsg">
                            Status information here...
                        </div>
                        <div>
                            <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label id="lblName" runat="server" Text="Name:"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        
                                    </td>
                                    <td colspan="4">
                                        <input type="text" id="txtEntityName" runat="server"  onkeypress="validUser(event);" />
                                        <input type="hidden" id="hiddenEntityId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label id="lblDescription" runat="server" Text="Description:"></asp:Label>
                                            
                                        
                                    </td>
                                    <td colspan="4">
                                        <textarea id="txtEntityDescription" runat="server" rows="5" class="txtArea"></textarea>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divEditModePart" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label id="lblActive" runat="server" Text="Active:"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <input type="checkbox" id="chkEntityActive" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label id="lblReason" runat="server" Text="Reason:"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        
                                    </td>
                                    <td colspan="4">
                                        <textarea id="txtEntityReason" runat="server" rows="5" class="txtArea"></textarea>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table class="tableLayout">
                            <tr>
                                <td colspan="8">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                </td>
                                <td colspan="4">
                                    <asp:Button id="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_ServerClick" />
                                    <asp:Button id="btnCancel" runat="server" Text="Cancel" CssClass="secondaryButton" OnClick="btnCancel_ServerClick" OnClientClick="javascript: return closeEditPanelDialog();" />
                                    

                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <%-- <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="ServerClick" />
                        </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">


        function validUser(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key > 32 && key <= 39) || (key >= 42 && key <= 44) || (key == 46) || (key >= 58 && key <= 64)
            || (key >= 91 && key <= 96) || (key >= 123 && key <= 126))) {
                if (evt.preventDefault)
                    evt.preventDefault();
                else
                    evt.returnValue = false;
            }
        }

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
                    width: '400px',
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
            $('#<%=txtEntityName.ClientID%>').focus();
        }

        function closeEditPanelDialog() {
            $('#divDialog').dialog('close');
            $('#' + '<%=divSuccess.ClientID %>').fadeOut(3000);
            return false;
        }
        $(document).ready(function () {
            initializeEditPanelDialog();
        });
        function btnUpdate_onclick() {

        }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
