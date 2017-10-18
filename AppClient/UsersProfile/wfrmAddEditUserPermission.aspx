<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true" CodeFile="wfrmAddEditUserPermission.aspx.cs" Inherits="UsersProfile_wfrmAddEditUserPermission" Theme="Classical"%>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .treeNode {
            color: #070707;
        }

        .rootNode {
            font-size: 14px;
            width: 100%;
            color: #070707;
            padding: 2px;
        }

        .leafNode {
            padding: 2px;
            color: #070707;
        }

        .selectNode {
            font-weight: bold;
        }

        .divTreeview {
            border: 1px solid seagreen;
            height: 450px;
            overflow-x: hidden;
            overflow-y: auto;
            width: 99%;
        }
    </style>  
    <script type="text/javascript">

        function onSuccess() {
            setTimeout(okay, 3000);
        }

        function okay() {
            window.location = "List";
        }

        function cancel() {
            window.location = "List";
        }


    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="page-header-panel">
        <h2 runat="server" id="lblHeader">
            <label>User Permission</label></h2>
    </div>
    <asp:UpdatePanel ID="UpdateSearchPanel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                <ProgressTemplate>
                    <div class="loading">
                        Loading...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="divMessage" runat="server" class="warnMsg" style="margin-left:0">
                Succeed message.
            </div>

            <table class="formGrid" style="width: 70%">
                <tr>
                    <td colspan="2" align="left">
                        <h4>
                            <asp:Label ID="lblLocations" runat ="server" Text="Locations"></asp:Label><span class="Mandetary">*</span>
                            </h4>
                    </td>
                    <td colspan="2" align="left">
                        <h4>
                            <asp:Label ID="lblClients" runat ="server" Text="Clients"></asp:Label><span class="Mandetary">*</span></h4>
                    </td>
                    <td colspan="2" align="left">
                        <h4>
                            <label><asp:Label ID="lblReports" runat ="server" Text="Reports"></asp:Label></label><span class="Mandetary">*</span></h4>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top">
                        <div class="divTreeview" id="divLocation">
                            <asp:TreeView ID="tvLocations" runat="server" TabIndex="1" ShowCheckBoxes="All" ShowLines="true" ShowExpandCollapse="true" OnTreeNodeCheckChanged="tvLocations_CheckChanged">
                                <LeafNodeStyle CssClass="leafNode" />
                                <NodeStyle CssClass="treeNode" />
                                <RootNodeStyle CssClass="rootNode" />
                                <SelectedNodeStyle CssClass="selectNode" />
                            </asp:TreeView>
                        </div>
                    </td>
                    <td colspan="2" align="left" valign="top">
                        <div class="divTreeview" id="divClient">
                            <asp:TreeView ID="tvClients" runat="server" TabIndex="2" ShowCheckBoxes="All" ExpandDepth="0" ShowLines="true" OnTreeNodeCheckChanged="tvClients_CheckChanged">
                                <LeafNodeStyle CssClass="leafNode" />
                                <NodeStyle CssClass="treeNode" />
                                <RootNodeStyle CssClass="rootNode" />
                                <SelectedNodeStyle CssClass="selectNode" />
                            </asp:TreeView>
                        </div>
                    </td>
                    <td colspan="2" align="left" valign="top">
                        <div class="divTreeview" id="divReport">
                            <asp:TreeView ID="tvReports" runat="server" TabIndex="3" ShowCheckBoxes="All" ExpandDepth="0" ShowLines="true" OnTreeNodeCheckChanged="tvReports_CheckChanged">
                                <LeafNodeStyle CssClass="leafNode" />
                                <NodeStyle CssClass="treeNode" />
                                <RootNodeStyle CssClass="rootNode" />
                                <SelectedNodeStyle CssClass="selectNode" />
                            </asp:TreeView>
                        </div>
                    </td>
                </tr>
                <tr style="display:none">
                    <td colspan="6" align="left" valign="top">                      
                        <asp:Literal ID="lblPermissionName" runat="server" Visible="false"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="left" valign="top">

                        <div id="divActive" runat="server">                    
                    <table class="tableLayout">
                        <tr>
                            <td style="width:10%">
                                <asp:Label runat="server" ID="lblActive" Text="Active"></asp:Label>
                            </td>
                            <td>
                                <input type="checkbox" id="chkActive" runat="server" />
                            </td>                            
                        </tr>
                        <tr>
                            <td style="width:10%" valign="top">
                                <asp:Label runat="server" ID="lblreason" Text="Reason">
                                </asp:Label>
                            </td>
                            <td>
                                <textarea id="txtReason" runat="server" rows="5" cols="13" style="width: 50%"></textarea>
                            </td>                            
                        </tr>
                    </table>
                </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="left">
                        <asp:Button ID="btnSubmit" runat="server" TabIndex="4"
                            OnClick="btnSubmit_Click" Text="Submit"></asp:Button>
                         <input id="btnCancel" value="Cancel" type="button" onclick="cancel();" class="secondaryButton" />
                       
                    </td>
                </tr>
            </table>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">
        function locationsCheckUncheck() {
            $("[id*=tvLocations] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {


                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");

                    if ($("input[type=checkbox]:checked", parentDIV).length == 0) {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                    //if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    //    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    //} else {
                    //    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    //}
                }
            });
        }


        function clientsCheckUncheck() {
            $("[id*=tvClients] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {


                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");

                    if ($("input[type=checkbox]:checked", parentDIV).length == 0) {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                    //if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    //    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    //} else {
                    //    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    //}
                }
            });
        }


        function reportsCheckUncheck() {
            $("[id*=tvReports] input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    //Is Parent CheckBox
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                } else {


                    //Is Child CheckBox
                    var parentDIV = $(this).closest("DIV");
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");

                    if ($("input[type=checkbox]:checked", parentDIV).length == 0) {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                    //if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    //    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    //} else {
                    //    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    //}
                }
            });
        }


        function postBackByObject(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(ScrollUpB);
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ScrollUpE);
                Sys.WebForms.PageRequestManager.getInstance().beginAsyncPostBack(['<%=UpdateSearchPanel.ClientID %>'], '<%=UpdateSearchPanel.ClientID %>', null);
               
                //__doPostBack("", "");
            }
        }


        var scrollLocation, scrollClient, scrollReport;

        function ScrollUpB() {            
            scrollLocation = document.getElementById("divLocation").scrollTop;
            scrollClient = document.getElementById("divClient").scrollTop;
            scrollReport = document.getElementById("divReport").scrollTop;
        }

        function ScrollUpE() {            
            document.getElementById("divLocation").scrollTop = scrollLocation;
            document.getElementById("divClient").scrollTop = scrollClient;
            document.getElementById("divReport").scrollTop = scrollReport;
        }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>

