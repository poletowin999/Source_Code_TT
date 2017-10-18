<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ClientListView.aspx.cs" Inherits="Masters_ClientListView" Theme="Classical" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchViews/UserSearchView.ascx" TagName="UserSearchView" TagPrefix="tks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
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
                        <h2><asp:Label runat="server" ID="lblClientManagement" Text="Client Management"></asp:Label>
                            </h2>
                    </div>
                    <div id="divSuccessMessage" runat="server" class="errMsg">
                    </div>
                    <table class="formGrid">
                        <tr>
                            <td colspan="2">
                            </td>
                            <td colspan="5">
                                <asp:Label runat="server" ID="lblClient" Text="Client :"></asp:Label>
                            </td>
                            <td colspan="7">
                                <input type="text" id="txtSearchName" runat="server" onkeypress="validUser(event);" />
                            </td>
                            <td colspan="2">
                            </td>
                            <td colspan="5">
                                <asp:Label runat="server" ID="lblStatus" Text="Status:"></asp:Label>
                            </td>
                            <td colspan="7">
                                <select id="ddlStatus" runat="server">
                                    <option value="All">All</option>
                                    <option value="Active" selected="selected">Active</option>
                                    <option value="InActive">InActive</option>
                                </select>
                            </td>
                            <td colspan="2">
                            </td>
                            <td colspan="16">
                                <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="primaryButton"
                                    OnClick="btnSearch_Click" UseSubmitBehavior="False"  ClientIDMode="Static" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="secondaryButton"
                                    OnClick="btnClear_Click" UseSubmitBehavior="False"  ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblUserSearchview" Text="User Search view" Visible="false" ></asp:Label>
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
                    </div>
                    <div class="lightToolbar" style="float: right;">
                        <ul class="lightToolbarItems">
                            <li>
                                <asp:Button ID="lnkadd" Text="AddNew" runat="server" OnClick="LnkAdd_Click"></asp:Button></li>
                            <li>
                                <asp:Button ID="lnkrefresh" Text="Refresh" runat="server" OnClick="LnkRefersh_Click"></asp:Button></li>
                        </ul>
                    </div>
                    <div class="clearFloat">
                    </div>
                     <div id="dvinitalvalue" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" class="gridView">
                                <tr>
                                    <th>
                                        <asp:Label runat="server" ID="lblName" Text="Name"></asp:Label>
                                    </th>
                                    <th style="text-align:left;">
                                        <asp:Label runat="server" ID="lblDescription" Text="Description"></asp:Label>
                                    </th>
                                    <th style="text-align:left;">
                                        <asp:Label runat="server" ID="lblManager" Text="Manager"></asp:Label>
                                    </th>
                                    <th style="text-align:left;">
                                        <asp:Label runat="server" ID="lblStatusGrid" Text="Status"></asp:Label>
                                    </th>
                                    <th>
                                        <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                                    </th>
                                    <%-- <th>
                                        Last Update User
                                    </th>
                                    <th>
                                        Last Update Date
                                    </th>--%>
                                </tr>
                                <tr>
                                    <td colspan="7" id="divEmptyRow" runat="server">
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                    <div class="gridViewPanel">
                        <asp:GridView ID="gvwEntityList" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeader="true" 
                            AutoGenerateEditButton="false" DataKeyNames="ID" OnRowCommand="gvwEntityList_RowCommand"
                            OnRowDataBound="gvwEntityList_RowDataBound" AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="gvwEntityList_PageIndexChanging">
                            <Columns>
                                <%--<asp:BoundField HeaderText="Name" DataField="Name" />
                                    <asp:BoundField HeaderText="Description" DataField="Description" />
                                    --%>
                                <asp:TemplateField HeaderText="Name">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Name") %>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Description") %>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Manager">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblManager" runat="server" Text="Name"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnResponsibleUserName" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text="Name"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span id="spnIsActive" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField Text="Edit" HeaderText="" CommandName="ClientEdit" ButtonType="Link" />
                                <%--<asp:TemplateField HeaderText="Last Update User">
                                    <ItemTemplate>
                                        <span id="spnLastUpdateUser" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText=" Last Update Date" DataField="LastUpdateDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}" />--%>
                                <%-- <asp:BoundField HeaderText="IsActive" DataField="IsActive" />
           <asp:BoundField HeaderText="LastUpdateUserId" DataField ="LastUpdateUserId" />
           <asp:BoundField HeaderText ="LastUpdateDate" DataField="LastUpdateDate" />--%>
                            </Columns>
                        </asp:GridView>
                       
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divEntityEditPanel">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMessage" runat="server" class="popupErrMsg" style="display: none">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table class="tableLayout">
                    <tr>
                        <td colspan="3">
                            <input type="hidden" id="hdnClientId" value="0" runat="server" />
                             <asp:Label runat="server" ID="lblClientname" Text="Client"></asp:Label>
                                <span class="Mandetary">*</span>
                            </label>
                        </td>
                        <td colspan="4">
                            <input type="text" id="txtName" runat="server" style="width: 180px" onkeypress="validUser(event);" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblDescriptiontext" Text="Description :"></asp:Label>
                                
                        </td>
                        <td colspan="4">
                            <textarea id="txtDescription" cols="22" rows="4" runat="server"></textarea>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblManagername" Text="Manager :"></asp:Label>
                                <span class="Mandetary">*</span>
                            </label>
                        </td>
                        <td colspan="4">
                            <input type="text" id="txtResponsibleUser" style="width: 150px" readonly="readonly"
                                runat="server" onfocus="return onRespUserFocus(this);"
                                onblur="return onRespUserBlur(this);" />
                            <input type="hidden" id="hdnResponsibleUserId" runat="server" />
                            
                            <asp:ImageButton ID="ibtSearchUsers" runat="server" CssClass="img16" ImageUrl="~/Images/user20.png"
                                OnClick="ibtSearchUsers_Click" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div id="divEditControl" runat="server">
                    <table class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblActive" Text="Active"></asp:Label>
                                    
                            </td>
                            <td colspan="4">
                                <input type="checkbox" id="chkIsActive" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblReason" Text="Reason"></asp:Label>
                                    <span class="Mandetary">*</span>
                            </td>
                            <td colspan="4">
                                <textarea id="txtReason" cols="22" rows="4" runat="server"></textarea>
                                <%--<label for="<%=txtLastUpdateUserId %>"> LastUpdateUserId :</label>
            <input type ="text" id="txtLastUpdateUserId" runat="server" /><br />
            <label for="<%=txtLastUpdateDate%>">LastUpdateDate:</label>
            <input tye="text" id="txtLastUpdateDate" runat="server" /><br />--%>
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
                            <asp:Button ID="btnUpdate" runat="server" CssClass="primaryButton" OnClick="btnUpdate_Click"
                                Width="88px" UseSubmitBehavior="False"  ClientIDMode="Static" />
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="secondaryButton"
                                OnClick="btnCancel_Click" Width="78px" UseSubmitBehavior="False"  ClientIDMode="Static" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divUsersSearchPanel">
        <tks:UserSearchView ID="UserSearchView1" onSearchResultSelect="userSearchResultSelected"
            runat="server" onDialogClose="closeUserSearchView" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        var mUserSearchViewDialog = null;
        var mUserSearchViewDialogOptions = {
            'inputControlId': '<%=txtResponsibleUser.ClientID %>',
            'searchButtonId': '<%=ibtSearchUsers.ClientID%>',
            'valueControlId': '<%=hdnResponsibleUserId.ClientID%>',
            'searchControlPanelId': 'divUsersSearchPanel',
            'title': '<%=lblUserSearchview.Text%>'
        }; 
        //var mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);

        function refreshUserSearchView() {
            if (mUserSearchViewDialog != null)

                mUserSearchViewDialog.set_options(mUserSearchViewDialogOptions);
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
            var userName = resultObject.FirstName + ',' + resultObject.LastName;
            mUserSearchViewDialog.set_displayText(resultObject.LastName + ',' + resultObject.FirstName);
            mUserSearchViewDialog.set_valueText(resultObject.Id);
            mUserSearchViewDialog.close();
        }



        function initializeEditPanelDialog() {
            // Display edit panel as dialog.
            $('#divEntityEditPanel').dialog('destroy');
            $('#divEntityEditPanel').dialog(
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
                $('#divEntityEditPanel').dialog('option', options);
            $('#divEntityEditPanel').dialog('open');
            $('#<%=txtName.ClientID%>').focus();
           
        }

        function closeEditPanelDialog() {
            $('#divEntityEditPanel').dialog('close');

            $('#' + '<%=divSuccessMessage.ClientID %>').fadeOut(5000);

            return false;
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

        $(document).ready(function () {
            initializeEditPanelDialog(mUserSearchViewDialogOptions);
           
        });

        function validUser(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key > 32 && key <= 39) || (key >= 42 && key <= 44 ) || (key == 46) ||(key >= 58 && key <= 64) 
            || (key >= 91 && key <= 96) || (key >= 123 && key <= 126) )) {
                if (evt.preventDefault)
                    evt.preventDefault();
                else
                    evt.returnValue = false;
            }
        }

        //        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
        //            function (sender, args) {
        //                initializeUserAutoComplete();
        //                if (args.get_error() !== null) {
        //                     Display error.
        //                    alert(args.get_error().message.replace(args.get_error().name, '').replace(':', '').trim());
        //                }
        //            });

        //        function initializeUserAutoComplete() {
        //            var ClientName = $('#' + '<%=txtSearchName.ClientID%>');
        //           
        //            ClientName.autocomplete('destroy');
        //            ClientName.autocomplete(
        //                    {
        //                        minLength: 0,
        //                        source: function (request, response) {
        //                            $.ajax(
        //                                {
        //                                    url: '../WebServices/MasterService.asmx/GetClientsByName',
        //                                    type: 'POST',
        //                                    contentType: 'application/json; charset=utf-8',
        //                                    data: "{ 'name': '" + request.term + "'}",
        //                                    dataType: "json",
        //                                    success: function (data, textStatus, req) {
        //                                        var dataArray = $.parseJSON(data.d);
        //                                        if (dataArray === null) return;
        //                                        response(dataArray);
        //                                    },
        //                                    error: function (req, textStatus, error) {
        //                                        alert(error);
        //                                    }
        //                                });
        //                        },

        //                        focus: function (event, ui) {
        //                            $(this).val(ui.item.Name);
        //                            return false;
        //                        },

        //                        select: function (event, ui) {
        //                            $(this).val(ui.item.Name);
        //                          
        //                            $('#TimeZone').html('Id: ' + ui.item.Id)
        //                            return false;
        //                        },

        //                        change: function (event, ui) {
        //                            if (ui.item === null) {
        //                                $(this).val("");
        //                              
        //                            }
        //                            return false;
        //                        }
        //                    })
        //                    .data("autocomplete")._renderItem = function (ul, item) {
        //                        return $("<li></li>")
        //				                .data("item.autocomplete", item)
        //                                .append("<a><span>" + item.Name + "<br>(" + item.Description + ")</span></a>")
        //				                .appendTo(ul);
        //                    };
        //        }
        //        $(document).ready(function () {
        //            initializeUserAutoComplete();
        //        });


    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
    <script type="text/javascript">
        var mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);
    </script>
</asp:Content>
