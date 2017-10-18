<%@ Page Title="" Language="C#"  MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" 
CodeFile="Ipconfig.aspx.cs" Inherits="Masters_Ipconfig" Theme="Classical" %>

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
                        Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div>
                <div class="page-header-panel">
                    <h2>
                        IP Config</h2>
                </div>
                <div id="divSuccess" class="successMsg" runat="server">
                    Success information here...
                </div>
                <div id="divSuccessMessage" runat="server" class="successMsg">
                </div>
                <table class="formGrid">
                    <tr>
                        <td colspan="3">
                            <label for="<%=txtIpAddress.ClientID%>">
                                IP Address:
                            </label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtIpAddress" runat="server" onkeypress="validIP(event);" />
                        </td>
                        <td>
                        </td>
                        <td colspan="3">
                            <label for="<%=ddlLink.ClientID%>">
                                Link Type:
                            </label>
                        </td>
                         <td colspan="5">
                            <select id="ddlLink" runat="server">
                                <option value="All" selected="selected">All</option>
                                <option value="Primary Link">Primary Link</option>
                                <option value="Secondary Link">Secondary Link</option>
                                <option value="Others">Others</option>
                            </select>
                        </td>
                        <td colspan="14">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label for="<%=txtLocation.ClientID%>">
                                Location:
                            </label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtLocation" runat="server" onkeypress="validautocomplete(event);"  />
                            <input type="hidden" id="hdnLocationId" runat="server" />
                        </td>
                        <td>
                        </td>
                        <td colspan="3">
                            <label>
                                Status:
                            </label>
                        </td>
                        <td colspan="5">
                            <select id="ddlStatus" runat="server">
                                <option value="All" selected="selected">All</option>
                                <option value="Active" >Active</option>
                                <option value="InActive">InActive</option>
                            </select>
                        </td>
                        <td>
                        </td>
                        <td colspan="13">
                            <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="primaryButton"
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="lbtClear" runat="server" Text="Clear" CssClass="secondaryButton"
                                OnClick="lbtClear_Click" />
                        </td>
                        <td>
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
                        <li>
                            <asp:LinkButton ID="lbtAddNewLocation" runat="server" OnClick="lbtAddNewLocation_Click">Add New</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lbtRefreshLocationList" runat="server" OnClick="lbtRefreshLocationList_Click">Refresh</asp:LinkButton></li>
                    </ul>
                </div>
                <div class="clearFloat">
                </div>
                <div class="gridViewPanel">
                    <div id="dvinitalvalue" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                            <tr>
                                
                                <th>
                                    Link Type
                                </th>
                                <th>
                                    IP Address
                                </th>
                                <th>
                                    Location
                                </th>
                                <th>
                                    Remarks
                                </th>
                                <th>
                                    Status
                                </th>
                                <th>
                                    Action
                                </th>
                            <tr>
                                <td colspan="8" id="divEmptyRow" runat="server">
                                    <span id="hideSpan" runat="server">Enter the criteria and click on Search button to
                                        view data.</span> <span id="InitalspnMessage" runat="server"></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="gvwIPList" runat="server" AutoGenerateColumns="False" CssClass="gridView"
                        EnableModelValidation="True" DataKeyNames="Ipaddress" OnRowCommand="gvwIPList_RowCommand"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvwIPList_PageIndexChanging"
                      >
                        <PagerStyle ForeColor="Blue" />
                        <Columns>
                            <asp:BoundField HeaderText="IPId" DataField="IPId" Visible="false"/>
                            <asp:BoundField HeaderText="Link Type" DataField="LinkType"/>
                            <asp:BoundField HeaderText="IP Address" DataField="Ipaddress"/>
                            <asp:BoundField HeaderText="Location" DataField="Location"/>
                             <asp:BoundField HeaderText="Status" DataField="Status"/>
                             <asp:ButtonField CommandName="TimeZoneEdit" HeaderText="Action" ButtonType="Link"
                                Text="Edit" />
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
                                <label>
                                    LinkType:<span class="Mandetary">*</span>
                                </label>
                            </td>
                            <td colspan="4">
                                 <select id="ddlEntitylink" runat="server">
                                <option value="Primary Link" selected="selected">Primary Link</option>
                                <option value="Secondary Link">Secondary Link</option>
                                <option value="Others">Others</option>
                            </select>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label>
                                    IP Address:<span class="Mandetary">*</span>
                                </label>
                            </td>
                            <td colspan="4">
                                <input type="text" id="txtEntityIPAddress" runat="server"  onkeypress="validIP(event);" />
                                  <input type="hidden" id="hiddenEntityId" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <label>
                                    Location:<span class="Mandetary">*</span>
                                </label>
                            </td>
                            <td colspan="4">
                                <input type="text" id="txtEntityLocation" runat="server"  onkeypress="validautocomplete(event);" />
                                <input type="hidden" id="hdnEntityLocationId" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <div id="divEditModePart" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                            <tr>
                                <td colspan="3">
                                    <label>
                                        Active:
                                    </label>
                                </td>
                                <td colspan="4">
                                    <input type="checkbox" id="chkEntityActive" runat="server" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <label>
                                        Reason:<span class="Mandetary">*</span>
                                    </label>
                                </td>
                                <td colspan="4">
                                    <textarea id="txtEntityReason" runat="server" rows="5" cols="23"></textarea>
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
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="primaryButton"
                                    OnClick="btnUpdate_Click" />
                                <input type="button" runat="server" id="btnCancel" value="Cancel" onclick="return closeEditPanelDialog();"
                                    class="secondaryButton" />
                            </td>
                            <td>
                            </td>
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

        function validIP(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key > 32 && key < 37) || (key >= 38 && key <= 41) || (key >= 43 && key < 46) || (key > 58 && key <= 64) 
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
                initializeLocationAutoComplete();
                initializeEntityLocationAutoComplete();
            });

            function initializeEntityLocationAutoComplete() {
                var LocationnameId = $('#' + '<%=txtEntityLocation.ClientID%>')
                var hdnLocationId = $('#' + '<%=hdnEntityLocationId.ClientID %>')
                $(LocationnameId).autocomplete('destroy');
                $(LocationnameId).autocomplete(
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
                            $(hdnLocationId).val(ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $(hdnLocationId).val("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.City + "(" + item.Country + ")</span></a>")
				                .appendTo(ul);
                    };
                }

                function initializeLocationAutoComplete() {
                    var LocationnameId = $('#' + '<%=txtLocation.ClientID%>')
                    var hdnLocationId = $('#' + '<%=hdnEntityLocationId.ClientID %>')
                    $(LocationnameId).autocomplete('destroy');
                    $(LocationnameId).autocomplete(
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
                            $(hdnLocationId).val(ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $(hdnLocationId).val("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.City + "(" + item.Country + ")</span></a>")
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
           // $('#<%=txtEntityLocation.ClientID%>').focus();
        }

        function closeEditPanelDialog() {
            $('#divEntityEditPanel').dialog('close');
            $('#' + '<%=divSuccess.ClientID %>').fadeOut(3000);
            return false;
        }
        $(document).ready(function () {
            initializeEditPanelDialog();
            initializeLocationAutoComplete();
            initializeEntityLocationAutoComplete();
        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
    