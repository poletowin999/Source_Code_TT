<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserActivityStatusWidget.ascx.cs"
    Inherits="Widgets_UserActivityStatusWidget" %>
<div>
    <h4>
        <asp:Label runat="server" ID="lblActivityStatus" Text="Activity Status"></asp:Label></h4>
    <div>
        <asp:Chart ID="Chart1" runat="server" Height="200px" Width="500px" BackColor="Transparent"
            AntiAliasing="Graphics">
            <Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                    <AxisY Interval="5" LineColor="Gray" Maximum="15" Minimum="0" Title="Activity Count"
                        TitleForeColor="Gray">
                        <MajorGrid LineColor="Silver" />
                        <MajorTickMark LineColor="Silver" />
                        <LabelStyle ForeColor="Gray" />
                    </AxisY>
                    <AxisX Interval="1" IntervalType="Days" IsLabelAutoFit="False" LineColor="Gray" Title="Activity Date"
                        TitleForeColor="Gray">
                        <MajorGrid Enabled="False" />
                        <MajorTickMark LineColor="Silver" />
                        <LabelStyle ForeColor="Gray" Format="dd MMM" Angle="-90" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Top" ForeColor="DimGray"
                    Name="Legend1">
                </asp:Legend>
            </Legends>
        </asp:Chart>
    </div>
</div>
