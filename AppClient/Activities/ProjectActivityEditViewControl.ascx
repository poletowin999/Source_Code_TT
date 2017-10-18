<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectActivityEditViewControl.ascx.cs"
    Inherits="Activities_ProjectActivityEditViewControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <div class="hiddenFields">
        <input type="hidden" id="hdnClientId" runat="server" value="0" />
        <input type="hidden" id="hdnProjectId" runat="server" value="0" />
        <input type="hidden" id="hdnTimeZoneId" runat="server" value="0" />
        <input type="hidden" id="hdnLangaugeId" runat="server" value="0" />
    </div>
    <div class="headerPanel3">
        <h4><asp:Label runat="server" ID="lblProjectActivityinfo" Text="Project Activity"></asp:Label></h4>
    </div>
    <div>
        <table cellspacing="0" border="0" class="data-entry-grid">
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblClientName" Text="Client :"></asp:Label>
                        <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <input type="text" id="txtClient" runat="server" tabindex="3" onkeypress="validautocomplete(event);" />
                    <input type="button" id="btnSelectClient" runat="server" value="Select Client" class="hide"
                        onserverclick="btnSelectClient_OnServerClick" />
                </td>
                <td>
                </td>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblWorktype" Text="Work type :"></asp:Label>
                        <span class="Mandetary">*</span>
                    
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlWorkTypeList" TabIndex="9" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlWorkTypeList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblProjectName" Text="Project :"></asp:Label><span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <input type="text" id="txtProject" runat="server" tabindex="4" />
                    <input type="button" id="btnSelectProject" runat="server" value="Select Project"
                        class="hide" onserverclick="btnSelectProject_OnServerClick" />
                </td>
                <td>
                </td>
                <td colspan="3">
                    
                        <asp:Label runat="server" ID="lblBillingType" Text="Billing Type :"></asp:Label><span class="Mandetary">*</span>
                    
                </td>
                <td colspan="4">
                    <select id="ddlBillingTypeList" runat="server" tabindex="10">
                    </select>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblLocation" Text="Location :"></asp:Label>
                        <span class="Mandetary">*</span>
                    
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlLocationList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocationList_SelectedIndexChanged"
                        TabIndex="5">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td colspan="3">
                    
                        <asp:Label runat="server" ID="lblTotDuration" Text="Tot Duration (HH:MM):"></asp:Label>
                        <span class="Mandetary">*</span>
                    
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txthours" runat="server" Width="18px" MaxLength="2" TabIndex="11"></asp:TextBox>
                    :
                    <asp:TextBox ID="txtMinutes" runat="server" Width="18px" MaxLength="2" TabIndex="12"></asp:TextBox>
                    <asp:TextBox ID="txtActivityStartDateTime" runat="server" Text="" onkeypress="return false;"
                        Visible="False"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblTimeZone" Text="Time Zone:"></asp:Label>
                </td>
                <td colspan="4">
                    <span id="spnTimeZone" runat="server">time zone</span>
                </td>
                <td>
                </td>
                <td colspan="3">
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtActivityEndDateTime" runat="server" onkeypress="return false;"
                        OnTextChanged="txtActivityEndDateTime_TextChanged" AutoPostBack="true" Visible="False"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPlatform" Text="Platform :"></asp:Label>
                    <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPlatformList" TabIndex="6" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPlatformList_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td colspan="7">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblTest" Text="Test"></asp:Label>
                    <span class="Mandetary">*</span>
                </td>
                <td colspan="4">
                    <select id="ddlTestList" runat="server" tabindex="7">
                    </select>
                </td>
                <td>
                </td>
                <td colspan="7">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblLanguages" Text="Language :"></asp:Label>
                        <span class="Mandetary">*</span>
                    
                </td>
                <td colspan="4">
                    <input type="text" id="txtLanguage" runat="server" tabindex="8" onkeypress="validautocomplete(event);" />
                </td>
                <td>
                </td>
                <td colspan="7">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="SHOULDNOTBEEMPTY" runat="server" Visible="false"></asp:Label>
</div>
