<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ActivityResetView.aspx.cs" Inherits="Activities_ActivityResetView"
    Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                    <ProgressTemplate>
                        <div class="loading">
                            Loading...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="page-header-panel">
                    <h2>
                        <asp:Label runat="server" ID="lblActivityResetView" Text="Activity Reset View"></asp:Label></h2>
                </div>
                <div id="divMessage" runat="server" class="warnMsg" style="display: none">
                </div>
                <br />
                <div>
                    <table class="formGrid">
                        <tr>
                            <td colspan="8">
                                <asp:Label runat="server" ID="lblActivityEnteredUser" Text="Activity Entered User:"></asp:Label>

                                <span class="Mandetary">*</span></label>
                            </td>
                            <td colspan="8">
                                <input type="text" id="txtCreateUser" runat="server" onkeypress="validautocomplete(event);" />
                                <input type="hidden" id="hdnCreateUserId" runat="server" value="0" />
                            </td>
                            <td></td>
                            <td colspan="9">
                                <asp:Label runat="server" ID="lblActivityEnteredFromDate" Text="Activity Entered FromDate :"></asp:Label>
                                <span class="Mandetary">*</span>
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtFromDate" Enabled="false" runat="server" MaxLength="10" TabIndex="4"></asp:TextBox>
                                <cc1:CalendarExtender PopupButtonID="Imgfromdt" ID="calextValid" runat="server" TargetControlID="txtFromDate"
                                    Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td colspan="3">
                                <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                                    AlternateText="To display calendar." ImageAlign="AbsBottom" />
                                <td colspan="8">
                                    <asp:Label runat="server" ID="lblActivityEnteredToDate" Text="Activity Entered ToDate :"></asp:Label>
                                    <span class="Mandetary">*</span></label>
                                </td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtToDate" Enabled="false" runat="server" MaxLength="10" TabIndex="4"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="MM/dd/yyyy" PopupButtonID="ImageButton1">
                                    </cc1:CalendarExtender>
                                </td>
                                <td colspan="3">
                                    <asp:ImageButton runat="Server" ID="ImageButton1" ImageUrl="~/Images/btn_on_cal.gif"
                                        AlternateText="To display calendar." ImageAlign="AbsBottom" />
                                </td>
                                <td colspan="11">
                                    <asp:Button ID="BtnSearch" Text="Search" runat="server" OnClick="BtnSearch_Click"
                                        CssClass="primaryButton" />
                                    <asp:Button ID="btnClear" Text="Clear" runat="server" CssClass="secondaryButton"
                                        OnClick="BtnClear_Click" />
                                </td>
                                <td></td>
                        </tr>
                    </table>
                </div>
                <br />
                <div id="hdrGridHeader" runat="server" class="gridViewHeader" style="float: left">
                </div>
                <br />
                <div runat="server" id="divGridHeader" class="contentViewDiv" style="width: 99%">
                    <table cellpadding="0" cellspacing="0" border="0" class="gridView" width="100%" style="table-layout: fixed">
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="lblSelect" Text="Select"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblView" Text="View"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblActivityDate" Text="Activity Date"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblUserName" Text="UserName"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblActivityCount" Text="UserName"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div id="divEmptyRow" runat="server">
                                    <b></b>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="clear"></div>

                <br />

                <asp:GridView ID="GridEntity" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridEntity_RowDataBound"
                    CssClass="gridView" OnRowCommand="GridEntity_RowCommand" BorderColor="#a8c5e1">
                    <Columns>
                        <asp:TemplateField HeaderText="UserId" Visible="false">
                            <ItemTemplate>
                                <input type="hidden" id="hdnUserId" runat="server" value="0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" Enabled="true" AutoPostBack="false" OnCheckedChanged="chkSelect_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField Text="View" HeaderText="View" CommandName="ResetActivity" ButtonType="Link" />
                        <asp:TemplateField HeaderText="Activity Date">
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityDate" runat="server" Text="ActivityDate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span id="spnActivityDate" runat="server"></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UserName">
                            <HeaderTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text="ActivityDate"></asp:Label>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <span id="spnUserName" runat="server"></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Activity Count">
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityCount" runat="server" Text="ActivityCount"></asp:Label>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <span id="spnActivityCount" runat="server"></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblAlertMsg" runat="server" Text="Label" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
                <asp:Label ID="lblResetsuccessfully" runat="server" Visible="false"></asp:Label>

                <br />

                <div style="overflow: auto;">
                    <asp:GridView ID="GridResetView" runat="server" AutoGenerateColumns="false"
                        CssClass="gridView" Width="1800px" BorderColor="#a8c5e1" Height="150px" AllowPaging="true" PageSize="10"
                        OnPageIndexChanging="GridResetView_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Activity Type">
                                <HeaderTemplate>
                                    <asp:Label ID="lblActivityType" runat="server" Text="Activity Type"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="TypeName" runat="server" Text='<%# Eval("CustomData[TypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client Name">
                                <ItemStyle />
                                <HeaderTemplate>
                                    <asp:Label ID="lblClientName" runat="server" Text="ClientName"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="Clientname" runat="server" Text='<%# Eval("CustomData[ClientName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Project Name">

                                <HeaderTemplate>
                                    <asp:Label ID="lblProjectName" runat="server" Text="ProjectName"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="projectname" runat="server" Text='<%# Eval("CustomData[ProjectName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">

                                <HeaderTemplate>
                                    <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                </HeaderTemplate>


                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Location" runat="server" Text='<%# Eval("CustomData[City]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Languages">

                                <HeaderTemplate>
                                    <asp:Label ID="lblLanguages" runat="server" Text="Languages"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Languages" runat="server" Text='<%# Eval("CustomData[LanguageName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Platform">

                                                                <HeaderTemplate>
                                    <asp:Label ID="lblPlatform" runat="server" Text="Platform"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Platform" runat="server" Text='<%# Eval("CustomData[PlatformName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTest" runat="server" Text="Test"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Test" runat="server" Text='<%# Eval("CustomData[TestName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TimeZone Name">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTimeZone" runat="server" Text="TimeZone"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="TimeZoneName" runat="server" Text='<%# Eval("CustomData[TimeZoneName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Billing Type">
                                <HeaderTemplate>
                                    <asp:Label ID="lblBillingType" runat="server" Text="BillingType"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="BillingType" runat="server" Text='<%# Eval("CustomData[BillingTypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Worktype">
                                <HeaderTemplate>
                                    <asp:Label ID="lblWorktype" runat="server" Text="Worktype"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Worktype" runat="server" Text='<%# Eval("CustomData[WorkTypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start Date">
                            <HeaderTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("StartDateTime") %>
                            </ItemTemplate>

                        </asp:TemplateField>

                            <%--<asp:BoundField HeaderText="Start Date" DataField="StartDateTime" />--%>
                            <asp:BoundField HeaderText="End Date" DataField="EndDateTime" />

                        <%--    <asp:TemplateField HeaderText="Duration">
                            <HeaderTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text="Duration"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("Duration") %>
                            </ItemTemplate>

                        </asp:TemplateField>--%>

                            <asp:BoundField HeaderText="Duration" DataField="Duration" Visible="false" />
                            <asp:TemplateField HeaderText="Duration (HH:MM)">
                                 <HeaderTemplate>
                                <asp:Label ID="lblTotDuration" runat="server" Text="Duration"></asp:Label>
                            </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Duration" runat="server" Text='<%# Eval("CustomData[FormattedDuration]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment">

                                <HeaderTemplate>
                                <asp:Label ID="lblComment" runat="server" Text="Duration"></asp:Label>
                            </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="lblComment" runat="server" Text='<%# Eval("CustomData[Comment]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <br />
                <br />
                <div id="divEditControl" runat="server">
                    <table class="formGrid">
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblComment" Text="Comment :"></asp:Label>
                                <span class="Mandetary">*</span></label>
                            </td>
                            <td colspan="3">
                                <textarea id="txtComment" cols="25" rows="4" runat="server"></textarea>
                            </td>
                            <td colspan="6">
                                <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="primaryButton" OnClick="btnReSet_Click" />
                                <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="secondaryButton"
                                    OnClick="btnClose_Click" />
                                <br />
                                <br />
                            </td>
                            <td colspan="4">
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

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
                initializeUserAutoComplete();
                if (args.get_error() !== null) {
                    // Display error.
                    alert(args.get_error().message.replace(args.get_error().name, '').replace(':', '').trim());
                }
            });

        function initializeUserAutoComplete() {
            var CreateUser = $('#' + '<%=txtCreateUser.ClientID%>');
            var CreateUserId = $('#' + '<%=hdnCreateUserId.ClientID%>');
            CreateUser.autocomplete('destroy');
            CreateUser.autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetUsersByName',
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
                            CreateUserId.val(ui.item.Id);
                            //$('#TimeZone').html('Id: ' + ui.item.Id)
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                CreateUserId.text("");
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
        $(document).ready(function () {
            initializeUserAutoComplete();
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
