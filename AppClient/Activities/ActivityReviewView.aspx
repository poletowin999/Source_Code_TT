<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ActivityReviewView.aspx.cs" Inherits="Activities_ActivityReviewView"
    Theme="Classical" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
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
                    <asp:Label runat="server" ID="lblActivityReviewManagement" Text="Activity Review Management"></asp:Label></h2>

            </div>
            <div id="alertError" class="warnMsg" runat="server" style="display: none;">
            </div>
            <div class="treeDiv">
                <asp:TreeView ID="TreHierarchy" runat="server" SelectedNodeStyle-Font-Bold="true"
                    OnSelectedNodeChanged="TreHierarchy_SelectedNodeChanged" ShowLines="True">
                    <ParentNodeStyle Font-Bold="True" />
                    <SelectedNodeStyle Font-Bold="True" />
                </asp:TreeView>
            </div>

            <div runat="server" id="divgvwselectedactivityHeader" class="contentViewDiv" style="overflow: auto; height: 450px;">
                <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                    <tr>
                        <th>
                            <asp:Label runat="server" ID="lblUserNameGrid" Text="User Name"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblActivityDate" Text="Activity Date"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblDuration" Text="Duration (HH:MM)"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <span id="spnMsg" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="gridViewPanel" id="divgvwselectedactivity" runat="server" style="overflow: auto; height: 400px; border: 1px solid #a8c5e1;">
                <asp:GridView ID="gvwselectedactivity" runat="server" AutoGenerateColumns="false"
                    CssClass="gridView" DataKeyNames="userId" AutoGenerateEditButton="false" OnRowCommand="gvwselectedactivity_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Select All" ShowHeader="true">
                            <HeaderTemplate>
                                <asp:Label ID="lblSelect" Text="Select All" runat="server" Width="60px"></asp:Label>
                                <asp:CheckBox ID="checkselectall" runat="server" AutoPostBack="true" OnCheckedChanged="checkselectall_Click" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="SelectedActivity" AutoPostBack="true" OnCheckedChanged="SelectedActivity_CheckedChanged" />
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                        
                      <asp:BoundField DataField="username" HeaderText="User Name" />
                        <%--<asp:ButtonField DataTextField="username" Text="username" ButtonType="Link" HeaderText="User Name" />--%>

                        
                        <asp:TemplateField HeaderText="User Name">
                            <HeaderTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>                              

                                <asp:LinkButton ID="lnkbtn" runat="server" Text='<%# Eval("username") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Activitydate">
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityDateNew" runat="server" Text="Activitydate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("Activitydate") %>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <%--<asp:BoundField HeaderText="Activity Date" DataField="Activitydate" DataFormatString="{0:d}" />--%>

                        <asp:TemplateField HeaderText="ActivityCount">
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityCount" runat="server" Text="ActivityCount"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("ActivityCount") %>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <%--                        <asp:BoundField HeaderText="No.of Activites" DataField="ActivityCount" />--%>
                        <asp:TemplateField HeaderText="Duration (HH:MM)">
                            <HeaderTemplate>
                                <asp:Label ID="lblDurationGrid" runat="server" Text="Duration"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Duration" runat="server" Text='<%# Eval("CustomData[Duration]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="userId" DataField="userId" Visible="false" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="HidActivitydate" runat="server" Visible="false" Value='<%#Eval ("Activitydate") %>' />
                                <asp:HiddenField ID="HiduserId" runat="server" Visible="false" Value='<%#Eval ("userId") %>' />
                                <asp:HiddenField ID="Hidusername" runat="server" Visible="false" Value='<%#Eval ("username") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:CheckBox ID="chkapproval" runat="server" Text="Select All" AutoPostBack="true"
                OnCheckedChanged="chkapproval_CheckedChanged" Visible="FALSE" />
            <div class="clearFloat">
                <br />
                <br />
            </div>
            <div runat="server" id="divgvwListactivityHeader" class="gridViewPanel">
                <table cellpadding="0" cellspacing="0" border="0" class="gridView">
                    <tr>
                        <th>
                            <asp:Label runat="server" ID="lblStatus" Text="Status"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblActivityType" Text="Activity Type:"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblClientName" Text="Client :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblProjectName" Text="Project :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblLanguages" Text="Language :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblLocation" Text="Location :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblPlatform" Text="Platform :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblTest" Text="Test"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblBillingType" Text="Billing Type :"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblWorktype" Text="Work type :"></asp:Label>
                        </th>
                        <asp:Label runat="server" ID="hdnActivitydetailsfor" Visible="false"></asp:Label>
                        <%--<th>
                            Start Dt/Tm
                        </th>
                        <th>
                            End Dt/Tm
                        </th>--%>
                        <th>
                            <asp:Label runat="server" ID="lblTotDuration" Text="Tot Duration (HH:MM):"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" ID="lblCommentActivity" Text="Comment:"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="14">
                            <span id="spnMessage" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <span id="spnuserdtls" runat="server"></span>
            </div>
            <div class="gridViewPanel" style="min-height: 200px;" id="divgvwListactivity" runat="server">
                <asp:GridView ID="gvwListactivity" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="false"
                    AllowPaging="True" PageSize="10" CssClass="gridView" BorderColor="#a8c5e1" OnPageIndexChanging="gvwListactivity_PageIndexChanging"
                    Width="1600px" DataKeyNames="Id" OnRowCommand="gvwListactivity_RowCommand">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="Select" ShowHeader="true">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="SelectedActivity" />
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="ActivityId" DataField="Id" Visible="false" />
                        <%--  <asp:TemplateField HeaderText="ActivityId" ShowHeader="true" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ActivityId" runat="server" Text='<%# Eval("CustomData[Id]") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>--%>
                        <%--  <asp:TemplateField HeaderText="Date">
                            <ItemStyle Width="20%" />
                            <ItemTemplate>
                                <asp:Label ID="Activitydate" runat="server" Text='<%# Eval("CustomData[Activitydate]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="" ShowHeader="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="ActivityId" runat="server" CommandName="EditActivity" CommandArgument='<%# Eval("Id") %>'
                                    Text="Edit"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activity Type">
                            <ItemStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblActivityType" runat="server" Text="Name"></asp:Label>
                            </HeaderTemplate>
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
                            <ItemStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblProjectName" runat="server" Text="ProjectName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="projectname" runat="server" Text='<%# Eval("CustomData[ProjectName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                            </HeaderTemplate>

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
                            <ItemStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblPlatform" runat="server" Text="Platform"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Platform" runat="server" Text='<%# Eval("CustomData[PlatformName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Test">
                            <ItemStyle />
                                                        <HeaderTemplate>
                                <asp:Label ID="lblTest" runat="server" Text="Test"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Test" runat="server" Text='<%# Eval("CustomData[TestName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TimeZone Name">
                            <ItemStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblTimeZone" runat="server" Text="Platform"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="TimeZoneName" runat="server" Text='<%# Eval("CustomData[TimeZoneName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billing Type">
                            <ItemStyle />
                                       <HeaderTemplate>
                                <asp:Label ID="lblBillingType" runat="server" Text="BillingType"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="BillingType" runat="server" Text='<%# Eval("CustomData[BillingTypeName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Worktype">
                            <ItemStyle />

                                                                   <HeaderTemplate>
                                <asp:Label ID="lblWorktype" runat="server" Text="Worktype"></asp:Label>
                            </HeaderTemplate>


                            <ItemTemplate>
                                <asp:Label ID="Worktype" runat="server" Text='<%# Eval("CustomData[WorkTypeName]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- <asp:BoundField HeaderText="Start Date" DataField="StartDateTime" />
                        <asp:BoundField HeaderText="End Date" DataField="EndDateTime" />--%>
                        <asp:BoundField HeaderText="Duration" DataField="Duration" Visible="false" />
                        <asp:TemplateField HeaderText="Duration (HH:MM)">
                            <ItemStyle />

                            <HeaderTemplate>
                                <asp:Label ID="lblTotDuration" runat="server" Text="TotDuration"></asp:Label>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:Label ID="Duration" runat="server" Text='<%# Eval("CustomData[FormattedDuration]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment">

                            <HeaderTemplate>
                                <asp:Label ID="lblCommentActivity" runat="server" Text="CommentActivity"></asp:Label>
                            </HeaderTemplate>

                            <ItemStyle />
                            <ItemTemplate>
                                <asp:Label ID="Comment" runat="server" Text='<%# Eval("CustomData[Comment]") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table class="tableLayout" style="margin: 25px 0px 0px 0px;">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblComment" Text="Comment :" runat="server"></asp:Label>
                    </td>
                    <td colspan="6">
                        <asp:TextBox ID="txtcomment" runat="server" Rows="5" Columns="25" TextMode="MultiLine"
                            MaxLength="1000"></asp:TextBox>
                    </td>
                    <td></td>
                    <td colspan="7">
                        <asp:Button ID="btnApprove" CssClass="primaryButton" Text="Approve" runat="server"
                            OnClick="btnApprove_Click" />
                        <asp:Button ID="btnReject" CssClass="secondaryButton" Text="Reject" runat="server"
                            OnClick="btnReject_Click" />
                    </td>
                    <td colspan="15"></td>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
