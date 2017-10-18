<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="ActivityEditView.aspx.cs" Inherits="Activities_ActivityEditView" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Activities/ProjectActivityEditViewControl.ascx" TagName="ProjectActivityEditView"
    TagPrefix="e4e" %>
<%@ Register Src="~/Activities/MiscActivityEditViewControl.ascx" TagName="MiscProjectActivityEditView"
    TagPrefix="e4e" %>
<%@ Register TagPrefix="e4e" TagName="UserSearchView" Src="~/SearchViews/UserSearchView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/AutoCompleteHelper.js"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
    <script type="text/javascript">

        function blkdt() {
           // alert(document.getElementById("ctl00_content_txtActivityDate").value);
           // alert(document.getElementById('<%= ddlActivityTypeList.ClientID %>').value);

            

//            if (document.getElementById('<%= ddlActivityTypeList.ClientID %>').value == 1) {
 //               if (document.getElementById("ctl00_content_projectActivityEditView_ddlLocationList").value == '1') {
 //                   if (document.getElementById('<%= txtActivityDate.ClientID %>').value > document.getElementById('<%= txtBlockDate.ClientID %>').value) {
//
//                    }
//                    else {
//                        alert("ACTIVITY ENTRY BLOCKED TILL THE DATE " + document.getElementById('<%= txtBlockDate.ClientID %>').value);
//                        return false;
//                    }
//                }
//            }
//            else {
//                if (document.getElementById("ctl00_content_miscActivityEditView_txtLocation").value == 'Bangalore') {

//                    if (document.getElementById('<%= txtActivityDate.ClientID %>').value > document.getElementById('<%= txtBlockDate.ClientID %>').value) {

  //                  }
//                    else {
//                        alert("ACTIVITY ENTRY BLOCKED TILL THE DATE " + document.getElementById('<%= txtBlockDate.ClientID %>').value);
  //                      return false;
//                    }
//                }
//            }
            
        }
        function validautocomplete(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was (' AND \)
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
    </script>
    <style type="text/css">
        .activity-user-info
        {
            border: 1px solid #CCCCCC;
            margin: 0px 3px 25px 3px;
        }
        .activity-user-info div:last-child
        {
            padding: 5px;
        }
        .activity-user-info-header
        {
            background-color: #F7F7F7;
            border-bottom: 1px solid #EFEFEF;
            line-height: 15pt;
            padding-left: 5px;
            vertical-align: middle;
            letter-spacing: 1px;
            font-weight: bold;
        }
        .activity-user-info span
        {
            display: block;
            margin-right: 3px;
        }
        .activity-user-info span:first-child
        {
            font-size: 10pt;
            color: #666666;
        }
        .activity-user-info span:last-child
        {
            font-size: 9pt;
            color: #999999;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <asp:Panel ID="Panel1" DefaultButton="btnUpdate" runat="server">
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="headerPanel">
                <h2><asp:Label runat="server" ID="lblAddActivity" Text="Add Activity"></asp:Label></h2>
            </div>
            <div class="hiddenFields">
                <input type="hidden" id="hdnActivityId" runat="server" value="0" />
                <input type="hidden" id="hdnClientId" runat="server" value="0" />
                <input type="hidden" id="hdnProjectId" runat="server" value="0" />
                <input type="hidden" id="hdnLocationId" runat="server" value="0" />
                <input type="hidden" id="hdnLangaugeId" runat="server" value="0" />
                <input type="hidden" id="hdnTimeZoneId" runat="server" value="0" />
                
                <input type="hidden" id="hdn1" runat="server"/>
            </div>
            <table cellpadding="0px" cellspacing="0" border="0" style="table-layout: auto; width: 100%">
                <tr>
                    <td style="width: 20%" valign="top">
                        <div style="padding-top: 10px;">
                            <div class="activity-user-info">
                                <div class="activity-user-info-header">
                                    <b><asp:Label ID="lblUser" runat="server" Text="User"></asp:Label> </b></div>
                                <div id="divUserInfo" runat="server">
                                </div>
                            </div>
                            <div class="activity-user-info">
                                <div class="activity-user-info-header">
                                    <b><asp:Label ID="lblSupervisor" runat="server" Text="Supervisor"></asp:Label> </b></div>
                                <div id="divSupervisorInfo" runat="server">
                                </div>
                            </div>
                            <div class="activity-user-info">
                                <div class="activity-user-info-header">
                                    <b><asp:Label ID="lblManager" runat="server" Text="Manager"></asp:Label> </b></div>
                                <div id="divManagerInfo" runat="server">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 80%; padding-left: 10px;" valign="top">
                        <div>
                            <div id="divErrorPanel" runat="server" class="warnMsg">
                                Status information here...
                            </div>
                            <div id="divInfoPanel" runat="server" class="warnMsg">
                                Success information here...
                            </div>
                            <div id="divWarnInfoPanel" runat="server" class="warnMsg">
                                Warning information here...
                            </div>
                        </div>
                        <div>
                        </div>
                        <div>
                            <div>
                                <div class="headerPanel3">
                                    <h4>
                                        <asp:Label runat="server" ID="lblActivityUserinfo" Text="Select Activity User Name, Date and Type"></asp:Label></h4>
                                </div>
                                <table cellspacing="0" class="data-entry-grid">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblUserName" Text="Select UserName"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <input id="txtuser" type="text" value="" runat="server" title="press F2 to search users"
                                                onfocus="return onRespUserFocus(this);" onblur="return onRespUserBlur(this);"
                                                readonly="readonly" />
                                            <input type="hidden" id="hdnuserid" runat="server" />
                                        </td>
                                        <td colspan="1">
                                            <asp:ImageButton ID="ibtSearchUser" ImageUrl="~/Images/user20.png" CssClass="img16"
                                                runat="server" OnClick="ibtSearchUser_Click" />
                                        </td>
                                        <td colspan="1">
                                        </td>
                                        <td colspan="7">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblActivityDate" Text="Activity Date:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                            
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtActivityDate" Enabled="false" runat="server" OnTextChanged="txtActivityDate_TextChanged"
                                                AutoPostBack="true" MaxLength="10" TabIndex="1"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calextValid" runat="server" TargetControlID="txtActivityDate"
                                                PopupButtonID="Imgfromdt" Format="MM/dd/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td colspan="1">
                                            <asp:ImageButton runat="Server" ID="Imgfromdt" ImageUrl="~/Images/btn_on_cal.gif"
                                                AlternateText="To display calendar." ImageAlign="AbsBottom" />
                                        </td>
                                        <td colspan="1">
                                        </td>
                                        <td colspan="7">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblActivityType" Text="Activity Type:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                            
                                        </td>
                                        <td colspan="4">
                                            <asp:DropDownList ID="ddlActivityTypeList" runat="server" TabIndex="2" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlActivityTypeList_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Project Activity"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Miscellaneous Activity" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="1">
                                        </td>
                                        <td colspan="7">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <e4e:ProjectActivityEditView ID="projectActivityEditView" runat="server" />
                            <e4e:MiscProjectActivityEditView ID="miscActivityEditView" runat="server" />
                            <div>
                                <div class="headerPanel3">
                                    <h4><asp:Label runat="server" ID="lblOthersinfo" Text="Others"></asp:Label></h4>
                                </div>
                                <table cellspacing="0" class="data-entry-grid">
                                    <tr>
                                        <td colspan="3" valign="top">
                                            <asp:Label runat="server" ID="lblComment" Text="Comment:"></asp:Label>
                                                
                                            
                                        </td>
                                        <td colspan="12">
                                            <textarea id="txtComment" runat="server" rows="5" cols="25" tabindex="14"></textarea>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <div class="headerPanel3">
                                    <h4><asp:Label runat="server" ID="lblCommentsHistoryinfo" Text="Comments History"></asp:Label></h4>
                                </div>
                                <div class="checkListBox" style="border: 1px solid #a8c5e1">
                                    <asp:Repeater ID="RepComments" runat="server">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCommentsBy" Text="Comment by :"></asp:Label>
                                            
                                            <%# Eval("CustomData[Username]")%>
                                            <br />
                                            <asp:Label runat="server" ID="lblCommentDate" Text="Comment Date :"></asp:Label>
                                            <%# DataBinder.Eval(Container.DataItem, "Createdate")%>
                                            <br />
                                            <div>
                                                Comment :
                                                <%# DataBinder.Eval(Container.DataItem, "Comment")%><br />
                                            </div>
                                            <hr>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <br>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div>
                                <br />
                                <asp:CheckBox ID="chkIsAutoapproval" runat="server" Text="" Visible="true"
                                    Checked="false" /><asp:Label ID="lblIsAutoapproval" runat="server" Text="IsAutoapproval"></asp:Label>
                            </div>
                            <div class="commandPanel" style="padding: 20px 0px 0px 0px">
                               <%-- <input type="button" id="btnUpdate" runat="server" class="primaryButton" value="Update"
                                    tabindex="15" onserverclick="btnUpdate_ServerClick" />--%>
                                      <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_ServerClick" CssClass="primaryButton" tabindex="15" OnClientClick="return blkdt();" />
                                <asp:Button ID="btnClose" runat="server" CssClass="secondaryButton" Text="Close" TabIndex="16" OnClick="btnClose_ServerClick" />
                                <input type="hidden" runat="server" id="txtBlockDate" value="07/31/2014" />
                                <asp:Label ID="lblValError" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="hdndt" Text="07/31/2014" Visible="false" runat="server" Width="96%" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
              </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="divUserSearchPanel" runat="server">
        <e4e:UserSearchView ID="UserSearchView" onDialogClose="closeUserSearchView" onSearchResultSelect="UserSearchResultSelected"
            runat="server" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[




        //user search dialog control

        var mUserSearchViewDialog = null;
        var mUserSearchViewDialogOptions = {
            'inputControlId': '<%=txtuser.ClientID%>',
            'searchButtonId': '<%=ibtSearchUser.ClientID%>',
            'valueControlId': '<%=hdnuserid.ClientID%>',
            'searchControlPanelId': '<%=divUserSearchPanel.ClientID %>',
            'title': 'User Search View'
        };

        function refreshUserSearchView() {
            if (mUserSearchViewDialog !== null)
                mUserSearchViewDialog.set_options(mUserSearchViewDialogOptions);
        }

        function showUserSearchView() {
            mUserSearchViewDialog.show();
        }

        function closeUserSearchView() {
            mUserSearchViewDialog.close();
            return false;
        }

        function UserSearchResultSelected(result) {
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

        $(document).ready(
            function () {
                mUserSearchViewDialog = new WebDialog(mUserSearchViewDialogOptions);
            }
        );

        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
    <script type="text/javascript">
        // <![CDATA[



        // ]]>
    </script>
</asp:Content>
