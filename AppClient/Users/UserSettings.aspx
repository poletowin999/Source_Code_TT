<%@ Page Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserSettings.aspx.cs" Inherits="Users_UserSettings" Theme="Classical"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <div class="page-header-panel">
            <h2>
                Users Settings</h2>
        </div>
        <asp:UpdatePanel ID="UpdateSearchPanel" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                    <ProgressTemplate>
                        <div class="loading">
                            Loading...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div id="divSuccessMessage" runat="server" class="warnMsg" style="display: none">
                    Succeed message.
                </div>
                <table class="formGrid">
                    <tr>
                        <td colspan="3" align="left">
                            <label for="<%=txtName.ClientID %>">
                                Name:</label>
                        </td>
                        <td colspan="4">
                            <input type="text" id="txtName" maxlength="100" runat="server" />
                        </td>
                        <td>
                        </td>
                        <td colspan="3">
                            <label for="<%=txtRoleName.ClientID %>">
                                Role Name:</label>
                        </td>
                        <td colspan="4">
                            <input type="text" id="txtRoleName" runat="server" />
                            <input type="hidden" id="hdnRoleId" runat="server" />
                        </td>
                        <td>
                        </td>
                        <td colspan="4">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label for="<%=ddlLocation.ClientID %>">
                                Location:</label>
                        </td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlLocation" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td colspan="7">
                            <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Text="Filter" />
                            <asp:Button ID="btnclear0" runat="server" Text="Clear" OnClick="btnclear_Click" CssClass="secondaryButton" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <div style="padding: 20px 0px 10px 0px">
                </div>
                <div class="gridViewContainer">
                    <div id="LstofRecord" runat="server" class="gridViewHeader" style="float: left;">
                    </div>
                    <div class="clearFloat">
                    </div>
                    <div class="gridViewPanel">
                        <asp:GridView ID="GvwUserList" runat="server" AutoGenerateColumns="false" CssClass="gridView"
                            AllowPaging="true" OnRowDataBound="GvwUserList_RowDataBound" DataKeyNames="Id"
                            OnRowCommand="GvwUserList_RowCommand" OnPageIndexChanging="GvwUserList_PageIndexChanging"
                            PageSize="10">
                            <Columns>
                                <asp:BoundField HeaderText="Last Name" DataField="LastName" />
                                <asp:BoundField HeaderText="First Name" DataField="FirstName" />
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <span id="spnRoleName" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <span id="spnLocation" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Language">
                                    <ItemTemplate>
                                        <span id="spnLanguage" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shift">
                                    <ItemTemplate>
                                        <span id="spnShift" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="From Date">
                                    <ItemTemplate>
                                        <span id="spnFromDate" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date">
                                    <ItemTemplate>
                                        <span id="spnToDate" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:BoundField HeaderText="From Date" DataField="FromDate" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="To Date" DataField="ToDate" DataFormatString="{0:d}" />--%>
                                <asp:ButtonField Text="Edit" HeaderText="Action" CommandName="UserEdit" ButtonType="Link" />
                            </Columns>
                        </asp:GridView>
                        <div class="gridViewPanel">
                            <div id="divGridHeader" runat="server">
                                <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                                    <tr>
                                        <th>
                                            Last Name
                                        </th>
                                        <th>
                                            First Name
                                        </th>
                                        <th>
                                            Role Name
                                        </th>
                                        <th>
                                            Location
                                        </th>
                                        <th>
                                            Language
                                        </th>
                                        <th>
                                            Shift
                                        </th>
                                        <%--<th>
                                            From Date
                                        </th>
                                        <th>
                                            To Date
                                        </th>--%>
                                        <th>
                                            Action
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5" id="divEmptyRow" runat="server">
                                            Enter the criteria and click on Search button to view data.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divEntityEditPanel">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divMessage" runat="server" class="popupErrMsg" style="display: none">
                </div>
                <table class="tableLayout">
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <input type="hidden" value="0" id="hdnuserid" runat="server" />
                            <label>
                                First Name:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="5">
                            <input type="text" maxlength="100" id="txtFirstname" runat="server" readonly="readonly" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label>
                                Last Name:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="5">
                            <input type="text" maxlength="100" id="txtLastname" runat="server" readonly="readonly" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label>
                                Location:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtLocation" runat="server" />
                            <input type="hidden" id="hdnLocationId" runat="server" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label>
                                Language:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="txtLanguage" runat="server" />
                            <input type="hidden" id="hdnLanguageId" runat="server" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label>
                                Shift:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="5">
                            <asp:DropDownList ID="drpShift" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label id="lblDateOfJoining" runat="server">
                                From Date:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDateOfJoining" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImgDateOfJoining" runat="server"
                                TargetControlID="txtDateOfJoining" Format="MM/dd/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td colspan="4">
                            <asp:ImageButton runat="Server" ID="ImgDateOfJoining" ImageUrl="~/Images/btn_on_cal.gif"
                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td colspan="4">
                            <label id="lblDateOfRelieving" runat="server">
                                To Date:<span class="Mandetary">*</span></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDateOfRelieving" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" PopupButtonID="ImgDateOfRelieving" runat="server"
                                TargetControlID="txtDateOfRelieving" Format="MM/dd/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td colspan="4">
                            <asp:ImageButton runat="Server" ID="ImgDateOfRelieving" ImageUrl="~/Images/btn_on_cal.gif"
                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
                <table>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                        CssClass="primaryButton" />
                    <asp:Button ID="btnCancel" CssClass="secondaryButton" runat="server" Text="Cancel"
                        OnClick="btnCancel_Click" />
                </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {
            // ValidateData();
            initializeLocationAutoComplete();
            initializeLanguageAutoComplete();
            initializeEditPanelDialog();

        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            function (sender, args) {
                initializeLocationAutoComplete();
                initializeLanguageAutoComplete();
                //                initializeEditPanelDialog();
                // ValidateData();
            });

        function initializeLocationAutoComplete() {
            var LocationnameId = $('#' + '<%=txtLocation.ClientID%>')
            var hdnLocationId = $('#' + '<%=hdnLocationId.ClientID %>')
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

        function initializeLanguageAutoComplete() {
            var LanguagenameId = $('#' + '<%=txtLanguage.ClientID%>')
            var hdnLanguageId = $('#' + '<%=hdnLanguageId.ClientID %>')
            $(LanguagenameId).autocomplete('destroy');
            $(LanguagenameId).autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetLanguagesByName',
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
                            $(hdnLanguageId).val(ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $(hdnLanguageId).val("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.Name + "<br> </span></a>")
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
            $('#<%=txtFirstname.ClientID%>').focus();
        }

        function closeEditPanelDialog() {
            $('#divEntityEditPanel').dialog('close');
            var errmsg = $('#' + '<%=divSuccessMessage.ClientID %>').text();
            if (errmsg != "")
                $('#' + '<%=divSuccessMessage.ClientID %>').fadeOut(3000);


            return false;
        }


                
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
