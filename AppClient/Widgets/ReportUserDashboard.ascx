<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportUserDashboard.ascx.cs" Inherits="Widgets_ReportUserDashboard" %>
  <link rel="stylesheet" type="text/css" media="all" href="<%= this.ResolveUrl("~/Css/DateRangePicker/daterangepicker.css")%>" />
    <link rel="stylesheet" type="text/css" media="all" href="<%= this.ResolveUrl("~/Css/w3_tab.css")%>" />
    <script type="text/javascript" src="<%= this.ResolveUrl("~/Scripts/jQuery/jquery-1.11.3.min.js") %>"></script>
    <script type="text/javascript" src="<%= this.ResolveUrl("~/Scripts/DateRangePicker/moment.js") %>"></script>
    <script type="text/javascript" src="<%= this.ResolveUrl("~/Scripts/DateRangePicker/daterangepicker.js") %>"></script>
  <script type="text/javascript">
      var begin, end;
      $(document).ready(function () {
          begin = moment().subtract(15, 'days');
          end = moment();
          DateRangePicker();
      });

      function DateRangePicker() {

          $("[id*=txtDateRange]").daterangepicker({
              firstDay: 1,
              dateLimit: {
                  days: 30
              },
              startDate: begin,
              endDate: end,
              ranges: {
                  'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                  'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                  'This Month': [moment().startOf('month'), moment().endOf('month')],
                  'Last Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],
                  'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
              }
          }, cb);

          cb(begin, end);
      }

      function cb(startDate, endDate) {
          begin = startDate;
          end = endDate;
          document.getElementById('<%=hfStartDate.ClientID%>').value = startDate.format('YYYY-MM-DD');
            document.getElementById('<%=hfEndDate.ClientID%>').value = endDate.format('YYYY-MM-DD');
        }



        function openChart(evt, cityName) {
            // Declare all variables
            var i, tabcontent, tablinks;

            // Get all elements with class="tabcontent" and hide them
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }

            // Get all elements with class="tablinks" and remove the class "active"
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }

            // Show the current tab, and add an "active" class to the button that opened the tab
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed">
                <tr>
                    <td colspan="2">

                        <div class="col-md-4">
                            <h4><asp:Label runat="server" ID="lblDateRange" Text="Date Range"></asp:Label> </h4>
                            <input type="text" id="txtDateRange" runat="server" class="form-control" enableviewstate="true" />
                            <%--<i class="glyphicon glyphicon-calendar fa fa-calendar"></i>--%>

                            <input id="hfStartDate" runat="server" type="hidden" />
                            <input id="hfEndDate" runat="server" type="hidden" />

                        </div>
                        <div class="col-md-4" style="padding-top: 15px;">

                            <asp:Button ID="btnSearch" runat="server" CssClass="primaryButton" Text="Search" OnClick="btnSearch_Click" />
                            &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="secondaryButton" Text="Clear" OnClientClick="DateRangePicker();" />

                        </div>


                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>

                            <div class="tab">                              
                                 <a class="tablinks active" id="lblWorkStat" runat="server" onclick="openChart(event,'workduration')">Work Duration</a>
                                <a class="tablinks" id="lblActivityStat" runat="server" onclick="openChart(event,'activity')">Activity Status</a>
                            </div>

                            <div id="workduration" class="tabcontent" style="display: block;">


                                <h4><asp:Label runat="server" ID="hdrWorkDuration" Text="Work Duration"></asp:Label></h4>
                                <div>
                                    <asp:Chart ID="chtWrkDuration" runat="server" Height="300px" Width="800px" BackColor="Transparent"
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

                            <div id="activity" class="tabcontent" style="display: none;">
                                <div>
                                    <h4><asp:Label runat="server" ID="HDRActivityStatus" Text="Activity Status"></asp:Label></h4>
                                    <div>
                                        <asp:Chart ID="chtActStatus" runat="server" Height="300px" Width="800px" BackColor="Transparent"
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
                            </div>
                    </td>
                </tr>


            </table>

        </ContentTemplate>
    </asp:UpdatePanel>