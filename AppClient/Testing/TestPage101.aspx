<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SiteMaster.master"
    AutoEventWireup="true" CodeFile="TestPage101.aspx.cs" Inherits="Testing_TestPage101"
    Theme="Classical" %>

<%@ Register Src="../SearchViews/ClientSearchView.ascx" TagName="ClientSearchView"
    TagPrefix="tks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ui-autocomplete
        {
            position: absolute;
            cursor: default;
            height: 300px;
            overflow: auto;
        }
         .ui-autocomplete-loading 
         {
             background: white url('../images/loading60.gif') right center no-repeat;
         }
    </style>
    <script src="../Scripts/jQuery/jquery-1.6.3.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="../Scripts/TksScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <h4>
                    Testing</h4>
                <table cellpadding="0" cellspacing="0" border="0px" style="table-layout: fixed; width: 500px">
                    <tr>
                        <td colspan="3">
                            <label>
                                Ci<label>t</label>y</label>
                        </td>
                        <td colspan="5">
                            <input type="text" id="Text1" style="width: 98%" />
                            <input type="text" id="txtCityName" style="width: 98%" runat="server" />
                            <span id="spnSelectedCity"></span>
                        </td>
                        <td colspan="3">
                            Search Client:
                        </td>
                        <td colspan="4">
                            <div style="white-space: nowrap">
                                <input type="text" id="txtClient" runat="server" value="Press F2 to search" readonly="readonly"
                                    style="float: left;" />
                                <input type="hidden" id="hdnClientId" runat="server" value="0" />
                                <asp:ImageButton ID="ibtSearchClient" runat="server" Height="16px" ImageUrl="~/Images/user20.png"
                                    OnClick="ibtSearchClient_Click" Width="16px" />
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Add New</asp:LinkButton>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <input type="button" value="Show" onclick="showClientSearchView();" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divClientSearchPanel">
        <tks:ClientSearchView ID="ClientSearchView1" runat="server" onSearchResultSelect="clientSearchResultSelected"
            onDialogClose="closeClientSearchView" />
    </div>
    <div id="divSampleDialogPanel">
        <h3>
            I am inside dialog box.</h3>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
    <script type="text/javascript">

        initializeRoleAutoComplete();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
            function (sender, args) {
                initializeRoleAutoComplete();
            });


            $(document).ready(
            function () {
                // initializeRoleAutoComplete();
                $("#divSampleDialogPanel").dialog(
                    {
                        modal: true,
                        draggable: false,
                        resizable: false,
                        title: 'Title here...'
                    });
            });




        function initializeRoleAutoComplete() {
            $('#txtCityName').autocomplete('destroy');
            $('#' + '<%=txtCityName.ClientID%>').autocomplete(
                    {
                        minLength: 3,
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
                                        setTimeout(
                                            function () {
                                                response(dataArray);
                                            }, 5000);
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
                            $("#spnSelectedCity").text("Id: " + ui.item.Id);
                            return false;
                        },

                        change: function (event, ui) {
                            if (ui.item === null) {
                                $(this).val("");
                                $("#spnSelectedCity").text("");
                            }
                            return false;
                        }
                    })
                    .data("autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
				                .data("item.autocomplete", item)
                                .append("<a><span>" + item.City + "<br>" + item.Country + "</span></a>")
				                .appendTo(ul);
                    };
        }



        var mClientSearchViewDialogOptions = {
            'inputControlId': '<%=txtClient.ClientID%>',
            'searchButtonId': '<%=ibtSearchClient.ClientID%>',
            'valueControlId': '<%=hdnClientId.ClientID%>',
            'searchControlPanelId': 'divClientSearchPanel',
            'title': 'Client Search View - Testing'
        };
        var mClientSearchViewDialog = new WebDialog(mClientSearchViewDialogOptions);

        function refreshClientSearchView() {
            mClientSearchViewDialog.set_options(mClientSearchViewDialogOptions);
        }

        function showClientSearchView() {
            mClientSearchViewDialog.show();
        }

        function closeClientSearchView() {
            mClientSearchViewDialog.close();
            return false;
        }

        function clientSearchResultSelected(result) {
            var resultObject = Sys.Serialization.JavaScriptSerializer.deserialize(result);
            mClientSearchViewDialog.set_displayText(resultObject.Name);
            mClientSearchViewDialog.set_valueText(resultObject.Id);
            mClientSearchViewDialog.close();
        }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
