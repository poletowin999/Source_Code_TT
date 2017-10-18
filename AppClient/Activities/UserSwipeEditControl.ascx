<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSwipeEditControl.ascx.cs"
    Inherits="Activities_UserSwipeEditControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="divUserSwipeDetails" runat="server">
    <div id="divmessage" runat="server" class="popupWarnMsg" style="display: none;">
    </div>
    <div id="SwipePanel" runat="server">
        <table id="tblswipedetail" runat="server" class="tableLayout" style="empty-cells: hide">
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblTimeZone" Text="Time Zone:"></asp:Label>
                        
                </td>
                <td colspan="4">
                    <select id="ddlTimeZoneList" runat="server">
                    </select>
                    <span id="LabTimeZone" runat="server"></span>
                    <input type="hidden" id="hdnSLNo" value="0" runat="server" />
                    <input type="hidden" id="hdnUserId" runat="server" value="0" />
                    <input type="hidden" id="hdnCreateUserId" runat="server" value="0" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblDate" Text="Date:"></asp:Label>
                        
                </td>
                <td colspan="4">
                    <span id="spnSwipeDate" runat="server"></span>
                    <asp:TextBox ID="txtSwipeDate" runat="server"></asp:TextBox>
                    <%--<input type="text" id="txtSwipeDate" value="MM/dd/yyy/" runat="server" />--%>
                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtSwipeDate"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblShift" Text="Shift:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlShift" runat="server">
                    </asp:DropDownList>
                </td>
                </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPurpose" Text="Purpose:"></asp:Label>
                        
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddWFH" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label id="LblSwipeTime" runat="server" Text="Time:">
                        </asp:Label>
                </td>
                <td colspan="4">
                    <%--   <input type="text" id="txtSwipeTime" runat="server" />--%>
                    <asp:TextBox ID="txtSwipeTime" runat="server" Width="120px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtSwipeTime"
                        Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                        MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" UserTimeFormat="TwentyFourHour" />
                    <asp:DropDownList ID="ddlTimeSpan" runat="server" Width="50px">
                        <asp:ListItem Value="AM" Text="AM">
                        </asp:ListItem>
                        <asp:ListItem Value="PM" Text="PM"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <div id="reasonGridView" runat="server">
            <table class="tableLayout">
                <tr>
                    <td colspan="3">
                        <asp:Label id="lblreason" runat="server" Text="Reason:">
                            </asp:Label>
                    </td>
                    <td colspan="4">
                        <textarea id="tbxreason" rows="5" cols="22" runat="server">
        </textarea>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                <td colspan="8">
                  <asp:CheckBox ID="chkRemove" 
                    Text="Delete  Check In/Out Details." 
                    runat="server" oncheckedchanged="chkRemove_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                </tr>
            </table>
        </div>
        <table class="tableLayout">
            <tr>
                <td colspan="3">
                </td>
                <td colspan="4">
                <asp:Button ID="btnDelete" CssClass="primaryButton" runat="server" Text="Delete" 
                        onclick="btnDelete_Click" />
                    <asp:Button ID="btnOk" CssClass="primaryButton" runat="server" Text="Ok" OnClick="btnOk_Click" />
                    <asp:Button ID="btnCancel" CssClass="secondaryButton" runat="server" OnClientClick="closeEditPanelDialog()"
                        Text="Cancel" OnClick="btnCancel_Click" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: center" id="popupClickOk" runat="server">
        <br />
        <span id="MessagePanel" runat="server"></span>
        <br />
        <br />
        <br />
        <asp:Button ID="btndisplay" runat="server" Text="ok" OnClick="btndisplay_Click" />
    </div>
</div>
