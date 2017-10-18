<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="UserEditView.aspx.cs" Inherits="Users_UserEditView" Theme="Classical"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="e4e" TagName="UserSearchView" Src="~/SearchViews/UserSearchView.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
    <script type="text/javascript">
        function encryptfunc() {

            /*Password*/

            if ($('#' + '<%=chkIsactive.ClientID %>').prop('checked') == false) {
                var ActvityCnt = $('#' + '<%=HdnNoofActivities.ClientID %>').val();

                //alert(ActvityCnt);
                if (ActvityCnt !== "0") {
                    return confirm(ActvityCnt + " activities have not been approved for the user. Do you still want to deactivate?");
                }
            }
            
            var pass = document.getElementById("<%= txtpasswd.ClientID %>").value;
            var encpassword = "";
            

            for (var i = 0; i < pass.length; i++) {
                
                var byte = 1000 + parseInt(pass.charCodeAt(i));
                //alert(byte);
                encpassword += byte.toString();
            }
            //alert(encpassword);
            document.getElementById("<%= txtpasswd.ClientID %>").value = encpassword;

            /*Confirm Password*/
            var cpass = document.getElementById("<%= txtComfirmPassword.ClientID %>").value;
            encpassword = "";
            for (var i = 0; i < cpass.length; i++) {
                var byte = 1000 + parseInt(cpass.charCodeAt(i));
                encpassword += byte.toString();
            }           
            document.getElementById("<%= txtComfirmPassword.ClientID %>").value = encpassword;
            return true;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:Panel ID="Panel1" DefaultButton="btnUpdate" runat="server">
        <div>
            <asp:UpdatePanel ID="UpdatePanelUserInfo" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                        <ProgressTemplate>
                            <div class="loading">
                                Loading...
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div class="page-header-panel">
                        <h2 id="heading" runat="server"></h2>
                    </div>
                    <div id="divMessage" runat="server" class="warnMsg" style="display: none;">
                    </div>
                    <table class="tableLayout">
                        <tr>
                            <td style="padding: 0px 0px 0px 40px">
                                <div class="headerPanel3">
                                    <h4><asp:Label ID="lblUserInformation" runat="server" Text="User Information"></asp:Label> </h4>
                                </div>
                            </td>
                            <td style="padding: 0px 0px 0px 40px">
                                <div class="headerPanel3">
                                    <h4><asp:Label ID="lblLoginInformation" runat="server" Text="Login Information"></asp:Label> 
                                    </h4>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table class="tableLayout">
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <input type="hidden" value="0" id="hdnuserid" runat="server" />
                                            <asp:Label runat="server" ID="lblLastName" Text="Last Name:">
                                                </asp:Label><span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" maxlength="100" id="txtlastname" runat="server" onkeypress="validUser(event);" />
                                        </td>
                                        <%-- onclick="return txtlastname_onclick()" --%>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblFirstName" Text="First Name:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" maxlength="100" id="txtfirstname" runat="server" onkeypress="validUser(event);" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblInitial" Text="Initial:"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" maxlength="5" style="width: 30px;" id="txtInital" runat="server"
                                                onkeypress="validUser(event);" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblGender" Text="Gender:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <select id="drpdnGender" runat="server">
                                                <option value="0">Select</option>
                                                <option value="M">Male</option>
                                                <option value="F">Female</option>
                                            </select>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr> 
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                           <asp:Label runat="server" ID="lblHomePhone" Text="Home Phone:"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtHomePhone" runat="server"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeHomePhone" runat="server" TargetControlID="txtHomePhone"
                                                MaskType="Number" Mask="(999)-999-9999">
                                            </asp:MaskedEditExtender>
                                            <%--  <input type="text" id="txtHomePhone" maxlength="12" runat="server" />--%>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="13">
                                            <div class="headerPanel3" style="margin: 0px 0px 0px 40px;">
                                                <h4><asp:Label ID="lblOfficialInformation" runat="server" Text="Official Information"></asp:Label> 
                                    </h4>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblEmpID" Text="Emp ID:"></asp:Label>
                                                
                                        </td>
                                        <td colspan="5">
                                            <input type="text" id="txtEmpID" maxlength="10" runat="server" onkeypress="validEmpId(event);" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblUserType" Text="User Type:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <%-- <select id="drpusertype" runat="server">
                                            <option value="0">Select</option>
                                            <option value="1">Permanent</option>
                                             <option value="2">Contract</option>
                                        </select>--%>
                                            <asp:DropDownList ID="drpusertype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpusertype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4" align="left">

                                            <asp:Label runat="server" ID="lblUserProfile" Text="User Profile"></asp:Label><span class="Mandetary">*</span>

                                        </td>
                                        <td colspan="5" align="left">
                                            <asp:DropDownList ID="ddlUserProfile" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblRoleName" Text="Role1:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="hidden" id="hdnRoleId" runat="server" />
                                            <input type="text" id="txtRoleName" runat="server" onkeypress="validautocomplete(event);" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblOfficePhone" Text="Office Phone:"></asp:Label>
                                                
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtofficephone" runat="server"></asp:TextBox>
                                            <asp:MaskedEditExtender ID="meeofficephone" runat="server" TargetControlID="txtofficephone"
                                                MaskType="Number" Mask="(999)-999-9999">
                                            </asp:MaskedEditExtender>
                                            <%--<input type="text" id="txtofficephone" maxlength="12" runat="server" />--%>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblExtension" Text="Extension:"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtExtension" runat="server" MaxLength="5"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fteExtension" runat="server" TargetControlID="txtExtension"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <%--  <input type="text" id="txtExtension" maxlength="10" runat="server" />--%>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblLocation" Text="Location:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" id="txtLocation" runat="server" onkeypress="validautocomplete(event);" />
                                            <input type="hidden" id="hdnLocationId" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <label>
                                                <asp:Label runat="server" ID="lblDepartment" Text="Department:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" id="txtDepartment" runat="server" onkeypress="validautocomplete(event);" />
                                            <input type="hidden" id="hdnDepartmentId" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblIsAdminuser" Text="Is Admin user:"></asp:Label>
                                                
                                        </td>
                                        <td colspan="5">
                                            <input type="checkbox" id="chkadmin" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblIsLandDAdmin" Text="Is LandD Admin:"></asp:Label>
                                                
                                        </td>
                                        <td colspan="5">
                                            <input type="checkbox" id="landd" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblActivitiesAutoApproval" Text="Activities Auto Approval:"></asp:Label>
                                                
                                        </td>
                                        <td colspan="5">
                                            <input type="checkbox" id="chkAutoapporal" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblAllowUserFromAnyLocation" Text="Allow User From AnyLocation:"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <input type="checkbox" id="chkAnylocation" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                         <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblContractMonths" Text="Contract Months:"></asp:Label>
                                            <span class="Mandetary">*</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList runat="server" Enabled="false" OnSelectedIndexChanged="ddMonths_SelectedIndexChanged" AutoPostBack="true" ID="ddMonths">
                                                    <asp:ListItem Value="0" Selected="True" Text="SELECT"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="One"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Two"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Three"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Four"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Five"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="Six"></asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblDateOfJoining" Text="Join Date"></asp:Label>
                                            <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtDateOfJoining" Enabled="false" runat="server" Width="20%" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImgDateOfJoining" runat="server"
                                                TargetControlID="txtDateOfJoining" Format="MM/dd/yyyy">
                                            </asp:CalendarExtender>
                                            <%--</td>
                                        <td colspan="6">--%>
                                            <asp:ImageButton runat="Server" ID="ImgDateOfJoining" ImageUrl="~/Images/btn_on_cal.gif"
                                                AlternateText="To display calendar." ImageAlign="AbsBottom" />

                                            <span>
                                                <asp:Label runat="server" ID="lblContractendDate" Text="Contract end Date:"></asp:Label>
                                                    

                                                <asp:TextBox ID="txtConEnDt" runat="server" Width="20%" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" PopupButtonID="ImageButton1" runat="server"
                                                    TargetControlID="txtConEnDt" Format="MM/dd/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:ImageButton runat="Server" ID="ImageButton1" Enabled="false" ImageUrl="~/Images/btn_on_cal.gif"
                                                    AlternateText="To display calendar." ImageAlign="AbsBottom" />
                                                
                                            </span>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblDateOfRelieving" Text="Relieve Date:"></asp:Label>
                                            
                                                
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
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>

                                        <td colspan="2"></td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="tableLayout">
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                           <asp:Label runat="server" ID="lblEmailId" Text="Email ID:"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" id="txtEmailId" maxlength="150" runat="server" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblLoginName" Text="Login Name:"></asp:Label>
                                                <span class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <input type="text" id="txtLoginName" maxlength="100" runat="server" onkeypress="validUser(event);" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblPassword" Text="Password:"></asp:Label>
                                                <span id="spnpwd" runat="server" class="Mandetary">*</span>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtpasswd" TextMode="Password" runat="server" OnPreRender="txtpasswd_PreRender"
                                                MaxLength="50" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblConfirmPassword" Text="Confirm Password:"></asp:Label>
                                                <span id="spncpwd" runat="server" class="Mandetary">*</span>
                                            
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtComfirmPassword" TextMode="Password" runat="server" OnPreRender="txtComfirmPassword_PreRender"
                                                MaxLength="50" />
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4"></td>
                                        <td colspan="5"></td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="15">
                                            <div id="div1" runat="server" class="headerPanel4" style="margin: 0px 0px 0px 40px;">
                                                <h4><asp:Label ID="lblManagerInformation" runat="server" Text="Manager Information"></asp:Label> </h4>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="4">
                                            <asp:Label runat="server" ID="lblSelectManager" Text="Select Manager"></asp:Label>
                                                
                                        </td>
                                        <td colspan="3">
                                            <%--title="press F2 to search users"--%>
                                            <input id="txtuser" type="text" value="" runat="server" 
                                                onfocus="return onRespUserFocus(this);" onblur="return onRespUserBlur(this);"
                                                readonly="readonly" />
                                            <input type="hidden" id="hdnManagerId" runat="server" />
                                        </td>
                                        <td colspan="1">
                                            <asp:ImageButton ID="ibtSearchUser" ImageUrl="~/Images/user20.png" CssClass="img16"
                                                runat="server" OnClick="ibtSearchUser_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--  <td colspan="2">
                                        </td>
                                        <td colspan="3">
                                            <label>
                                                Language :<span class="Mandetary">*</span>
                                            </label>
                                        </td>
                                        <td colspan="4">
                                            <input type="hidden" id="hdnLangaugeId" runat="server" value="0" />
                                            <input type="text" id="txtLanguage" runat="server" tabindex="0" onkeypress="validautocomplete(event);"/>
                                        </td>
                                        <td>
                                        </td>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                            <td colspan="3">
                                                <label>
                                                Shift</label>
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="ddlShift" runat="server">
                                                </asp:DropDownList>
                                                <input type="hidden" id="hdnShiftId" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="15">
                                                <div id="divOtherinfo" runat="server" class="headerPanel3"
                                                    style="margin: 0px 0px 0px 40px;">
                                                    <h4><asp:Label runat="server" ID="lblOtherInformation" Text="Other Information"></asp:Label></h4>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                            <td colspan="4">
                                                
                                                <asp:Label id="lblStatus" visible="false" runat="server" Text="Active:">
                                                    </asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <input type="checkbox" id="chkIsactive" visible="false" runat="server" />
                                                &nbsp;
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                            <td colspan="4" valign="top">
                                                <asp:Label Text="Reason:" id="lblreason" runat="server" visible="false">
                                                    <span class="Mandetary">*</span></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <textarea rows="5" style="width: 97%" id="tbxReason" visible="false" runat="server"></textarea>
                                            </td>
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                            <td colspan="4"></td>
                                            <td colspan="5"></td>
                                            <td colspan="2"></td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div class="commandPanel" style="margin-left:50px;">
                        <input type="hidden" id="HdnNoofActivities" value="0" runat="server" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Add" OnClientClick="return encryptfunc()" OnClick="btnUpdate_Click" CssClass="primaryButton" />
                        <asp:Button ID="btnCancel" CssClass="secondaryButton" runat="server" Text="Cancel"
                            OnClick="btnCancel_Click" />
                    </div>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="divUserSearchPanel" runat="server">
                <e4e:UserSearchView ID="UserSearchView" onDialogClose="closeUserSearchView" onSearchResultSelect="UserSearchResultSelected"
                    runat="server" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {
            // ValidateData();
            initializeRoleAutoComplete();
            initializeLocationAutoComplete();
            initializeDepartmentAutoComplete();
            //  initializeLanguageAutoComplete();

        });

        function SubmitConfirmation() {
            //            alert($('#' + '<%=chkIsactive.ClientID %>').prop('checked'));
            if ($('#' + '<%=chkIsactive.ClientID %>').prop('checked') == false) {
                var ActvityCnt = $('#' + '<%=HdnNoofActivities.ClientID %>').val();

                //alert(ActvityCnt);
                if (ActvityCnt !== "0") {
                    return confirm(ActvityCnt + " activities have not been approved for the user. Do you still want to deactivate?");
                }
            }
            return true;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            function (sender, args) {
                initializeRoleAutoComplete();
                initializeLocationAutoComplete();
                initializeDepartmentAutoComplete();
                //  initializeLanguageAutoComplete();
                // ValidateData();
            });

        function ValidateData() {
            $('#' + '<%=txtHomePhone.ClientID %>').keydown(function (event) {         // Allow only backspace and delete     
                if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) // 0-9 or numpad 0-9 
                {
                    // check textbox value now and tab over if necessary 
                }
                else if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39) // not esc, del, left or right 
                {
                    event.preventDefault();
                }
                // else the key should be handled normally 
            });
            $('#' + '<%=txtofficephone.ClientID %>').keydown(function (event) {
                if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) // 0-9 or numpad 0-9 
                {
                    // check textbox value now and tab over if necessary 
                }
                else if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39) // not esc, del, left or right 
                {
                    event.preventDefault();
                }
                // else the key should be handled normally 
            });


            $('#' + '<%=txtExtension.ClientID %>').keydown(function (event) {
                // Allow only backspace and delete     
                if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) // 0-9 or numpad 0-9 
                {
                    // check textbox value now and tab over if necessary 
                }
                else if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39) // not esc, del, left or right 
                {
                    event.preventDefault();
                }
                // else the key should be handled normally 
            });

        }

        //        function validUser(evt) {
        //            //var usernme=document.frmAddUser.userName;alert(usernme.value);
        //            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
        //            var key;
        //            if (evt.keyCode) { key = evt.keyCode; }
        //            else if (evt.which) { key = evt.which; }

        //            if (((key >= 65 && key < 90) || (key >= 97 && key < 122)
        //                || (key = 32))) {
        //                if (evt.preventDefault)
        //                    evt.preventDefault();
        //                else
        //                    evt.returnValue = false;
        //            }
        //        }


        function validUser(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key > 32 && key <= 45) || (key > 46 && key <= 64) || (key >= 91 && key <= 96)
                || (key >= 123 && key <= 126))) {
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
        function validEmpId(evt) {
            //var usernme=document.frmAddUser.userName;alert(usernme.value);
            // Verify if the key entered was a numeric character (0-9) or a decimal (.)
            var key;
            if (evt.keyCode) { key = evt.keyCode; }
            else if (evt.which) { key = evt.which; }

            if (((key > 32 && key <= 47) || (key >= 58 && key <= 64)
            || (key >= 91 && key <= 96) || (key >= 123 && key <= 126))) {
                if (evt.preventDefault)
                    evt.preventDefault();
                else
                    evt.returnValue = false;
            }
        }

        function initializeRoleAutoComplete() {
            var RolenameId = $('#' + '<%=txtRoleName.ClientID%>')
            var hdnRoleId = $('#' + '<%=hdnRoleId.ClientID %>')
            $(RolenameId).autocomplete('destroy');
            $(RolenameId).autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetRolesByName',
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
                            $(hdnRoleId).val(ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $(hdnRoleId).val("");
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

        function initializeDepartmentAutoComplete() {
            var DepartmentnameId = $('#' + '<%=txtDepartment.ClientID%>')
            var hdnDepartmentId = $('#' + '<%=hdnDepartmentId.ClientID %>')
            $(DepartmentnameId).autocomplete('destroy');
            $(DepartmentnameId).autocomplete(
                    {
                        minLength: 0,
                        source: function (request, response) {
                            $.ajax(
                                {
                                    url: '../WebServices/MasterService.asmx/GetDepartments',
                                    type: 'POST',
                                    contentType: 'application/json; charset=utf-8',
                                    data: "{ 'DeptartmentName': '" + request.term + "'}",
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
                            $(hdnDepartmentId).val(ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $(hdnDepartmentId).val("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a><span>" + item.City + "</span></a>")
                                .appendTo(ul);
                    };
        }

        //        function initializeLanguageAutoComplete() {
        //            var LanguagenameId = $('#' + '=txtLanguage.ClientID%>')
        //            var hdnLangaugeId = $('#' + '=hdnLangaugeId.ClientID %>')
        //            $(LanguagenameId).autocomplete('destroy');
        //            $(LanguagenameId).autocomplete(
        //                    {
        //                        minLength: 0,
        //                        source: function (request, response) {
        //                            $.ajax(
        //                                {
        //                                    url: '../WebServices/MasterService.asmx/GetLanguagesByName',
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
        //                            $(hdnLangaugeId).val(ui.item.Id);
        //                            return false;
        //                        },

        //                        change: function (event, ui) {
        //                            if (ui.item === null) {
        //                                $(this).val("");
        //                                $(hdnLangaugeId).val("");
        //                            }
        //                            return false;
        //                        }
        //                    })
        //                    .data("autocomplete")._renderItem = function (ul, item) {
        //                        return $("<li></li>")
        //				                .data("item.autocomplete", item)
        //                                .append("<a><span>" + item.Name + "<br> </span></a>")
        //				                .appendTo(ul);
        //                    };
        //        }

        //user search dialog control

        var mUserSearchViewDialog = null;
        var mUserSearchViewDialogOptions = {
            'inputControlId': '<%=txtuser.ClientID%>',
            'searchButtonId': '<%=ibtSearchUser.ClientID%>',
            'valueControlId': '<%=hdnManagerId.ClientID%>',
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


    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
