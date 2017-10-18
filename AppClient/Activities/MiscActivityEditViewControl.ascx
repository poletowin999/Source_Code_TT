<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MiscActivityEditViewControl.ascx.cs"
    Inherits="Activities_MiscActivityEditViewControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="divMiscActivityPanel" runat="server">
    <div class="headerPanel3">
        <h4><asp:Label runat="server" ID="lblMiscellaneousActivity" Text="Miscellaneous Activity"></asp:Label></h4>
    </div>
    <table class="data-entry-grid">
        <tr>
            <td colspan="3">
                <asp:Label runat="server" ID="lblTimeZoneMisc" Text="Time Zone :"></asp:Label>
                    <span class="Mandetary">*</span>
                
            </td>
            <td colspan="4">
                <select id="ddlMiscActivityTimeZoneList" runat="server" tabindex="3">
                </select>
            </td>
            <td>
            </td>
            <td colspan="3">
                <asp:Label runat="server" ID="lblTotDurationMISC" Text="Tot Duration (HH:MM):"></asp:Label>
                    <span class="Mandetary">*</span>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txthours" runat="server" Width="15px" MaxLength="2" TabIndex="6"></asp:TextBox>
                :
                <asp:TextBox ID="txtMinutes" runat="server" Width="15px" MaxLength="2" TabIndex="7"></asp:TextBox>
                <asp:TextBox ID="txtMiscActivityStartDateTime" runat="server" onkeypress="return false;"
                    Visible="false"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label runat="server" ID="lblWorktypeMISC" Text="Work type :"></asp:Label>
                <span class="Mandetary">*</span>
                
            </td>
            <td colspan="4">
                <asp:DropDownList ID="ddlMiscActivityWorkTypeList" TabIndex="4" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMiscActivityWorkTypeList_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td colspan="3">
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtMiscActivityEndDateTime" runat="server" onkeypress="return false;"
                    OnTextChanged="txtMiscActivityEndDateTime_TextChanged" AutoPostBack="true" Visible="false"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3">
               <asp:Label runat="server" ID="lblLocationMISC" Text="Location :"></asp:Label>
                <span class="Mandetary">*</span>
            </td>
            <td colspan="4">
                <input type="text" id="txtLocation" runat="server" tabindex="5" onkeypress="validautocomplete(event);"/>
                <input type="hidden" id="hdnLocationId" runat="server" />
                <asp:Label ID="SHOULDNOTBEEMPTYMISC" runat="server" Visible="false"></asp:Label>
            </td>
            <td></td>
        </tr>
    </table>
</div>
