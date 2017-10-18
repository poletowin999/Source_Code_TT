<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ActivityListView.aspx.cs" Inherits="Activities_ActivityListView" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/AutoCompleteHelper.js"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function blockk() {
            if (document.getElementById('<%= txtCopyActivityDate.ClientID %>').value == '') {
                alert("PLEASE ENTER A COPYY DATE");
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <div class="page-header-panel">
        <h2>
            <asp:Label runat="server" ID="lblActivityEntry" Text="Activity Entry"></asp:Label></h2>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div>
                <div id="divInfoPanel" class="warnMsg" runat="server">
                </div>
                <div id="divErrorPanel" runat="server" class="warnMsg">
                    Status information here...
                </div>
                <table cellpadding="0" cellspacing="0" border="0" class="tableLayout">
                    <tr>
                        <td colspan="4">
                            <asp:Label runat="server" ID="lblActivityDate" Text="Activity Date:"></asp:Label>
                            <span class="Mandetary">*</span>

                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtActivityDate" runat="server" Enabled="false" Width="96%" MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="calextValid" PopupButtonID="Imgfromdt" runat="server" TargetControlID="txtActivityDate"
                                Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td colspan="2">
                            <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                        </td>
                        <td colspan="7">
                            <asp:Button ID="btnViewActivityList" runat="server" Text="View" CssClass="primaryButton"
                                OnClick="btnViewActivityList_Click" />
                            <asp:Button ID="lbtClear" runat="server" CssClass="secondaryButton" OnClick="lbtClear_Click"
                                Text="Clear"></asp:Button>
                        </td>
                        <td></td>

                        <td colspan="7">
                            <asp:Label runat="server" ID="lblCopyActivity" Text="Copy all activities from Date:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtCopyActivityDate" Enabled="false" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImgCpoyActivity" runat="server" TargetControlID="txtCopyActivityDate"
                                Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                            <asp:TextBox ID="hdndt" Text="07/31/2014" Visible="false" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:ImageButton runat="Server" ID="ImgCpoyActivity" ImageUrl="~/Images/btn_on_cal.gif"
                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                        </td>
                        <td colspan="8">
                            <asp:Button ID="btnCopyActivitydate" Width="130px" runat="server" Text="Copy Activities"
                                CssClass="primaryButton" OnClick="btnCopyActivitydate_Click" />
                        </td>
                        <td></td>

                    </tr>
                </table>
            </div>


            <input type="hidden" runat="server" id="txtBlockDate" value="07/31/2014" />
            <input type="hidden" runat="server" id="Hidloc" />
            <div style="padding: 20px 0px 20px 0px">
            </div>
            <div id="hdrListHeader" runat="server" class="gridViewHeader">
            </div>
            <div class="lightToolbar" style="float: right;">
                <ul class="lightToolbarItems">
                    <li>
                        <%--<a href="ActivityEditView.aspx">Add New</a>--%>
                        <asp:Button ID="lbtAddNew" runat="server" EnableViewState="false" PostBackUrl="~/Activities/Edit" Text="Add New"></asp:Button>
                    </li>
                    <li>
                        <asp:Button ID="lbtRefreshActivity" runat="server" EnableViewState="false" OnClick="lbtRefreshActivity_Click" Text="Refresh"></asp:Button>

                    </li>
                </ul>
            </div>
            <div class="clearFloat">
            </div>
            <div class="gridViewContainer">
                <div runat="server" id="divgvwListactivityHeader" class="gridViewPanel">
                    <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="lblActivityType" Text="Activity Type"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblClientName" Text="Client Name"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblProjectName" Text="Project Name"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblLanguages" Text="Languages"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblLocation" Text="Location"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblPlatform" Text="Platform"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblTest" Text="Test"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblBillingType" Text="Billing Type"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblWorktype" Text="Work type"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblDuration" Text="Duration  (HH:MM)"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblComment" Text="Comment"></asp:Label>
                            </th>
                            <th>
                                <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                            </th>

                        </tr>
                        <tr>
                            <td colspan="14">
                                <span id="spnMessage" runat="server"></span>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="gridViewPanel" style="min-height: 300px;" id="divgvwListactivity" runat="server">
                    <asp:GridView ID="gvwListactivity" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false"
                        Width="2000px" CssClass="gridView" BorderColor="#a8c5e1" OnRowCommand="gvwListactivity_RowCommand"
                        OnRowEditing="gvwListactivity_RowEditing">
                        <Columns>
                            <asp:TemplateField HeaderText="" ShowHeader="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ActivityId" runat="server" CommandName="EditActivity" CommandArgument='<%# Eval("Id") %>'
                                        Text="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="DeleteActivity" runat="server" CommandName="DeleteActivity" CommandArgument='<%# Eval("Id") %>'
                                        Text="Delete" OnClientClick="return confirm('Are You Sure, Want to Delete? OK for delete , Cancel for Retain the Activity.')"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ViewActivity" runat="server" CommandName="ViewActivity" CommandArgument='<%# Eval("Id") %>'
                                        Text="View"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity Type">
                                <HeaderTemplate>
                                    <asp:Label ID="lblActivityType" runat="server" Text="ActivityType"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="TypeName" runat="server" Text='<%# Eval("CustomData[TypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Clientname">
                                <HeaderTemplate>
                                    <asp:Label ID="lblClientName" runat="server" Text="ClientName"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Clientname" runat="server" Text='<%# Eval("CustomData[ClientName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="project Name">

                                <HeaderTemplate>
                                    <asp:Label ID="lblProjectName" runat="server" Text="lProjectName"></asp:Label>
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
                                    <asp:Label ID="lblTest" runat="server" Text="lblTest"></asp:Label>
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
                                    <asp:Label ID="lblBillingType" runat="server" Text="Billing Type"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="BillingType" runat="server" Text='<%# Eval("CustomData[BillingTypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Work type">

                                <HeaderTemplate>
                                    <asp:Label ID="lblWorktype" runat="server" Text="Work type"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Worktype" runat="server" Text='<%# Eval("CustomData[WorkTypeName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:BoundField HeaderText="Start Date" DataField="StartDateTime" />
                            <asp:BoundField HeaderText="End Date" DataField="EndDateTime" />--%>
                            <asp:BoundField HeaderText="Duration" DataField="Duration" Visible="false" />

                            <asp:TemplateField HeaderText="Duration (HH:MM)">

                                <HeaderTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text="Duration"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Duration" runat="server" Text='<%# Eval("CustomData[FormattedDuration]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Comment">

                                <HeaderTemplate>
                                    <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="Comment" runat="server" Text='<%# Eval("CustomData[Comment]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">

                                <HeaderTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                </HeaderTemplate>

                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:Label ID="StatusName" runat="server" Text='<%# Eval("CustomData[StatusName]") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="HidActivity" runat="server" Value="0" />
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
