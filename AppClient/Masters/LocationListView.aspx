<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="LocationListView.aspx.cs" Inherits="Masters_LocationListView" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <!-- Search Entities -->
    <asp:UpdatePanel ID="updatePanel" runat="server">
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
                        <asp:Label runat="server" ID="lblLocationManagement" Text="Location Management"></asp:Label></h2>
                </div>
                <div id="divSuccess" class="successMsg" runat="server">
                    Success information here...
                </div>
                <div id="divSuccessMessage" runat="server" class="successMsg">
                </div>
                <table class="formGrid">
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblCity" Text="City:"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtCity" runat="server" onkeypress="validUser(event);" />
                        </td>
                        <td></td>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblState" Text="State:"></asp:Label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtState" runat="server" onkeypress="validUser(event);" />
                        </td>
                        <td colspan="14"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>

                            </label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtCountry" runat="server" onkeypress="validUser(event);" />
                        </td>
                        <td></td>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblStatus" Text="Status:"></asp:Label>
                        </td>
                        <td colspan="5">
                            <select id="ddlStatus" runat="server">
                                <option value="All">All</option>
                                <option value="Active" selected="selected">Active</option>
                                <option value="InActive">InActive</option>
                            </select>
                        </td>
                        <td></td>
                        <td colspan="13">
                            <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="primaryButton"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="lbtClear" runat="server" Text="Clear" CssClass="secondaryButton"
                                OnClick="lbtClear_Click" />
                        </td>
                        <td></td>
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
                        <li>
                            <asp:Button ID="lbtAddNewLocation" runat="server" OnClick="lbtAddNewLocation_Click" Text="Add New"></asp:Button></li>
                        <li>
                            <asp:Button ID="lbtRefreshLocationList" runat="server" OnClick="lbtRefreshLocationList_Click" Text="Refresh"></asp:Button></li>
                    </ul>
                </div>
                <div class="clearFloat">
                </div>
                <div class="gridViewPanel">
                    <div id="dvinitalvalue" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                            <tr>

                                <th>
                                    <asp:Label runat="server" ID="lblCityGrid" Text="City"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblStateGrid" Text="State"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblCountryGrid" Text="Country"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblTimeZone" Text="Time Zone"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblStatusidGrid" Text="Status"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                                </th>
                                <%--<th>
                                    Last Update User
                                </th>
                                <th>
                                    Last Update Date
                                </th>
                            </tr>--%>
                                <%-- <tr>
                                    <td colspan="9">
                                        <span id="spnMessage" runat="server"></span>
                                    </td>
                                </tr>--%>
                            </tr>
                            <tr>
                                <td colspan="8" id="divEmptyRow" runat="server">
                                    <span id="hideSpan" runat="server"></span><span id="InitalspnMessage" runat="server"></span>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <asp:GridView ID="gvwLocationList" runat="server" AutoGenerateColumns="False" CssClass="gridView"
                        EnableModelValidation="True" DataKeyNames="Id" OnRowCommand="gvwLocationList_RowCommand"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvwLocationList_PageIndexChanging"
                        OnRowDataBound="gvwLocationList_RowDataBound">
                        <PagerStyle ForeColor="Blue" />
                        <Columns>
                            <asp:TemplateField HeaderText="City">
                                <HeaderTemplate>
                                    <asp:Label ID="lblCityGrid" runat="server" Text="City"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("City") %>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="State">
                                <HeaderTemplate>
                                    <asp:Label ID="lblStateGrid" runat="server" Text="State"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("State") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Country">
                                <HeaderTemplate>
                                    <asp:Label ID="lblCountryGrid" runat="server" Text="Country"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("Country") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="TimeZoneName">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTimeZone" runat="server" Text="TimeZoneName"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span id="spntimeZonename" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusidGrid" runat="server" Text="Status"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span id="spnIsActive" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:ButtonField CommandName="TimeZoneEdit" HeaderText="" ButtonType="Link"
                                Text="Edit" />
                            <%-- <asp:TemplateField HeaderText="Last Update User">
                                <ItemTemplate>
                                    <span id="spnLastUpdateUser" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="LastUpdateDate" DataField="LastUpdateDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss tt}" />--%>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialog">
        <div id="divEntityEditPanel">
            <!-- Add and Edit Entity -->
            <asp:UpdatePanel ID="updatePanelAddNew" runat="server">
                <ContentTemplate>
                    <div id="divStatusInfo" runat="server" class="popupWarnMsg">
                        Status information here...
                    </div>
                    <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblCityAdd" Text="City"></asp:Label>
                                <span class="Mandetary">*</span>

                            </td>
                            <td colspan="4">
                                <input type="text" id="txtEntityCity" runat="server" onkeypress="validUser(event);" />
                                <input type="hidden" id="hiddenEntityId" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblStateAdd" Text="State:"></asp:Label>
                                <span class="Mandetary">*</span>
                            </td>
                            <td colspan="4">
                                <input type="text" id="txtEntityState" runat="server" onkeypress="validUser(event);" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblCountryAdd" Text="Country:"></asp:Label>
                                <span class="Mandetary">*</span>

                            </td>
                            <td colspan="4">
                                <input type="text" id="txtEntityCountry" runat="server" onkeypress="validUser(event);" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblTimeZoneAdd" Text="Time Zone:"></asp:Label>
                                <span class="Mandetary">*</span>
                                </label>
                            </td>
                            <td colspan="4">
                                <input type="text" id="txtTimeZone" runat="server" onkeypress="validautocomplete(event);" />
                                <input type="hidden" id="hdnTimeZoneId" runat="server" />
                                <span id="spnTimeZoneId" runat="server"></span>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <div id="divEditModePart" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblActive" Text="Active:"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <input type="checkbox" id="chkEntityActive" runat="server" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblReason" Text="Reason:"></asp:Label>
                                    <span class="Mandetary">*</span>
                                </td>
                                <td colspan="4">
                                    <textarea id="txtEntityReason" runat="server" rows="5" cols="23"></textarea>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                    <table class="tableLayout">
                        <tr>
                            <td colspan="8"></td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td colspan="4">
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="primaryButton"
                                    OnClick="btnUpdate_Click" />
                                <%--<input type="button" runat="server" id="btnCancel" value="Cancel" onclick="return closeEditPanelDialog();" class="secondaryButton" />--%>
                                <asp:Button runat="server" ID="btnCancel" OnClientClick="closeEditPanelDialog()" Text="Cancel" CssClass="secondaryButton" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[


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

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            function (sender, args) {
                initializeRoleAutoComplete();
                if (args.get_error() !== null) {
                    // Display error.
                    alert(args.get_error().message.replace(args.get_error().name, '').replace(':', '').trim());
                }
            });


        function initializeRoleAutoComplete() {
            var timeZoneName = $('#' + '<%=txtTimeZone.ClientID%>');
            var timeZoneId = $('#' + '<%=hdnTimeZoneId.ClientID%>');
            timeZoneName.autocomplete('destroy');
            timeZoneName.autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetTimeZonesByName',
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
                            $(this).val(ui.item.Name);
                            return false;
                        },

                        select: function (event, ui) {
                            $(this).val(ui.item.Name);
                            timeZoneId.val(ui.item.Id);
                            //$('#TimeZone').html('Id: ' + ui.item.Id)
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                timeZoneId.text("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.Name + "<br>(" + item.ShortName + ")</span></a>")
				                .appendTo(ul);
                    };
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
            $('#<%=txtEntityCity.ClientID%>').focus();
        }

        function closeEditPanelDialog() {
            $('#divEntityEditPanel').dialog('close');
            $('#' + '<%=divSuccess.ClientID %>').fadeOut(3000);
            return false;
        }
        $(document).ready(function () {
            initializeRoleAutoComplete();
            initializeEditPanelDialog();
        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
