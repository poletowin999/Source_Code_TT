<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserWorkDurationWidget.ascx.cs"
    Inherits="Widgets_UserWorkDurationWidget" %>
<div>
    <h4><asp:Label runat="server" ID="lblUSERDURATION" Text="Work Duration"></asp:Label> </h4>
    <div>
        <asp:Chart ID="Chart1" runat="server" Height="200px" Width="500px" BackColor="Transparent"
            AntiAliasing="Graphics">
            <Series>
                <asp:Series Name="Default" ToolTip="Hours : #VALY">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                    <AxisY Interval="4" LineColor="Gray" Maximum="20" Minimum="0" Title="Duration (in Hours)"
                        TitleForeColor="Gray">
                        <MajorGrid LineColor="Silver" />
                        <MajorTickMark LineColor="Silver" />
                        <LabelStyle ForeColor="Gray" />
                    </AxisY>
                    <AxisX Interval="1" IntervalType="Days" IsLabelAutoFit="False" LineColor="Gray" Title="Work Date"
                        TitleForeColor="Gray">
                        <MajorGrid Enabled="False" />
                        <MajorTickMark LineColor="Silver" />
                        <LabelStyle ForeColor="Gray" Format="dd MMM" Angle="-90" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>   
</div>
