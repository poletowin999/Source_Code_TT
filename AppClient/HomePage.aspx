<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AppMaster.master" AutoEventWireup="true"
    CodeFile="HomePage.aspx.cs" Inherits="HomePage" Theme="Classical" %>

<%@ MasterType VirtualPath="~/MasterPages/AppMaster.master" %>
<%@ Register Src="Widgets/UserWorkDurationWidget.ascx" TagName="UserWorkDurationWidget"
    TagPrefix="e4e" %>
<%@ Register Src="Widgets/UserActivityStatusWidget.ascx" TagName="UserActivityStatusWidget"
    TagPrefix="e4e" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">


    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed">
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td>
                <e4e:UserWorkDurationWidget ID="UserWorkDurationWidget1" runat="server" />
            </td>
            <td>
                <e4e:UserActivityStatusWidget ID="UserActivityStatusWidget1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr style="background-color: #000099; border-style: solid" />
            </td>

        </tr>
        <tr>
            <td>
                <div id="dvbirthday" runat="server" visible="false">
                    <table id="tblbirthday" style="display: block;">
                        <tr>
                            <td>
                                <h4><asp:Label id="lblExamHeader" runat="server" Text="List of Exam"></asp:Label></h4>
                            </td>
                        </tr>
                        <tr>

                            <td>
                                <%--<marquee direction="up" onmouseover="this.stop()" onmouseout="this.start()" scrollamount="2" scrolldelay="1">--%>
                                <asp:Label runat="server" ID="lblwishes"> </asp:Label>
                                <asp:GridView ID="Grdlist" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno">
                                            <ItemStyle />
                                             <HeaderTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="Sno"></asp:Label>
                                                </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="TypeName" runat="server" Text='<%# Eval("CustomData[Row]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemStyle />
                                            <HeaderTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                                </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Name" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                                <%--</marquee>--%>
                            </td>
                        </tr>
                    </table>
                </div>


            </td>
            <td>
                <div id="dvalert" runat="server" visible="false">
                    <table id="tblalert" style="display: block;">
                        <tr>
                            <td>
                                <h4>Policies</h4>
                            </td>
                        </tr>
                        <tr>

                            <td>
                                <%--<marquee direction="up" onmouseover="this.stop()" onmouseout="this.start()" scrollamount="2" scrolldelay="1">--%>
                                <asp:Label runat="server" ID="Label1"> </asp:Label>
                                <asp:GridView ID="Gridalert" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno">
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="TypeName" runat="server" Text='<%# Eval("CustomData[Row]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="Name" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                                <%--</marquee>--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <div id="DivSurvey" runat="server" visible="false">
                    <table id="Table1" style="display: block;">
                        <tr>
                            <td>
                                <h4>Survey</h4>
                            </td>
                        </tr>
                        <tr>

                            <td>
                                <%--<marquee direction="up" onmouseover="this.stop()" onmouseout="this.start()" scrollamount="2" scrolldelay="1">--%>
                                <asp:Label runat="server" ID="Label2"> </asp:Label>
                                <asp:GridView ID="GridSurvey" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <Columns>

                                        <asp:TemplateField HeaderText="Name">
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:Label ID="Name" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                                <%--</marquee>--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="formEndContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="documentEndContent" runat="Server">
</asp:Content>
