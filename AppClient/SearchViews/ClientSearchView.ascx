<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientSearchView.ascx.cs"
    Inherits="SearchViews_ClientSearchView" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:PlaceHolder ID="phdContent" runat="server" Visible="false">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <asp:GridView ID="gvwClientList" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                OnRowDataBound="gvwClientList_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:void(0);" id="lnkName" runat="server"></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%#(((Tks.Entities.Client)DataBinder.GetDataItem(Container)).IsActive)? "Yes":"No"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <input type="button" id="btnClose" runat="server" value="Close" />
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
