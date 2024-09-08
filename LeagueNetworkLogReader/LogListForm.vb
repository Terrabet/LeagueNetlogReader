Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports LeagueNetworkLogReader.MainForm

Friend Class LogListForm

    Private logFiles As String()

    ' Constructor to pass in the list of log files
    Public Sub New(logFiles As String())
        InitializeComponent()
        Me.logFiles = logFiles

        ' Populate the ListBox with the log file names
        For Each logFile In logFiles
            ListBoxLogs.Items.Add(Path.GetFileName(logFile))
        Next

    End Sub


    ' Event handler when the "Analyze" button is clicked
    Private Sub ListBoxLogs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxLogs.SelectedIndexChanged
        ' Get the selected file from the ListBox
        If ListBoxLogs.SelectedIndex >= 0 Then
            Dim selectedLogFile As String = logFiles(ListBoxLogs.SelectedIndex)

            ' Load and analyze the selected log file
            MainForm.LoadLogFile(selectedLogFile)

        Else
            MessageBox.Show("Please select a log file to analyze.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub LogListForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If MainForm.glueWindows = True Then
            Dim screenBounds As Rectangle = Screen.FromControl(MainForm).WorkingArea
            Dim logListFormWidth As Integer = 300

            ' Check if the MainForm is maximized
            If MainForm.WindowState = FormWindowState.Maximized Then
                MainForm.WindowState = FormWindowState.Normal
            End If

            Dim availableWidth As Integer = screenBounds.Width

            ' Calculate the combined width of both forms (MainForm + LogListForm)
            Dim combinedWidth As Integer = MainForm.Width + logListFormWidth + 5 ' Add 5 pixels for spacing

            If combinedWidth > availableWidth Then
                ' Resize the MainForm to fit the LogListForm beside it
                MainForm.Width = availableWidth - logListFormWidth - 5 ' Subtract the width of the LogListForm and spacing
            End If

            Me.Left = MainForm.Right + 5
            Me.Top = MainForm.Top

            ' Set the height of the LogListForm to match the MainForm's height
            Me.Size = New Drawing.Size(logListFormWidth, MainForm.Height)
        End If
    End Sub

    Friend Sub LogListForm_RegionChanged(sender As Object, e As EventArgs) Handles MyBase.RegionChanged
        If MainForm.glueWindows = True Then
            Call LogListForm_Resize(sender, e)
        End If
    End Sub

    Private Sub AnalyseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnalyseAllToolStripMenuItem.Click
        Dim allMetrics As New List(Of MainForm.LogFileMetrics) ' To store the metrics for each log

        Me.Text = "Analyzing..."
        If MainForm.showOverviewAnalysis Then MsgBox("This will turn off showing Analysis for each file, renable by right clicking in the grid view", vbOKOnly, "Metrics")
        MainForm.showOverviewAnalysis = False
        MainForm.ShowOverviewAnalysisToolStripMenuItem.Checked = False

        For Each logFile As String In logFiles
            Dim metrics As MainForm.LogFileMetrics = MainForm.LoadLogFile(logFile)
            If metrics IsNot Nothing Then
                allMetrics.Add(metrics)
            End If
        Next

        Me.Text = "List of Recent Logs"

        ShowMetricsGraph(allMetrics)

    End Sub

    Private Sub ShowMetricsGraph(metricsList As List(Of LogFileMetrics))
        ' Create a new form for the chart
        Dim graphForm As New Form()
        graphForm.Icon = Me.Icon
        graphForm.Text = "Log File Metrics Over Time"
        graphForm.Size = New Drawing.Size(1200, 800)
        graphForm.AutoScroll = True

        ' Define the chart types and create charts
        Dim chartTypes As New Dictionary(Of String, DataVisualization.Charting.SeriesChartType) From {
        {"Highest Ping", DataVisualization.Charting.SeriesChartType.Line},
        {"Highest Jitter", DataVisualization.Charting.SeriesChartType.Line},
        {"Packet Loss", DataVisualization.Charting.SeriesChartType.Line},
        {"Reconnects", DataVisualization.Charting.SeriesChartType.Line},
        {"Max Incoming Change", DataVisualization.Charting.SeriesChartType.Line},
        {"Max Outgoing Change", DataVisualization.Charting.SeriesChartType.Line}
    }

        Dim charts As New List(Of DataVisualization.Charting.Chart)

        For Each chartType In chartTypes
            Dim metricsChart As New DataVisualization.Charting.Chart()
            metricsChart.Dock = DockStyle.Top
            metricsChart.Height = 250

            ' Create the chart area
            Dim chartArea As New DataVisualization.Charting.ChartArea("MetricsArea")
            metricsChart.ChartAreas.Add(chartArea)

            ' Create and configure series
            Dim series As New DataVisualization.Charting.Series(chartType.Key)
            series.ChartType = chartType.Value
            series.XValueType = DataVisualization.Charting.ChartValueType.DateTime
            metricsChart.Series.Add(series)

            ' Add data to the series
            For Each logFileMetrics In metricsList
                series.Points.AddXY(logFileMetrics.LogDate,
                                GetMetricValue(chartType.Key, logFileMetrics))
            Next

            ' Configure X-axis to ensure all items are visible
            With metricsChart.ChartAreas(0).AxisX
                .Interval = metricsList.Count / 2
                .LabelStyle.Angle = -45
                .LabelStyle.Format = "yyyy-MM-dd HH:mm:ss" ' Format to display both date and time
                .IntervalType = DataVisualization.Charting.DateTimeIntervalType.Hours ' Set appropriate interval
                .IsMarginVisible = True
            End With

            With metricsChart.ChartAreas(0).AxisY
                .LabelStyle.Format = "0.00" ' Example format
                .LabelStyle.Angle = 0
                .IsLabelAutoFit = True
                .LabelAutoFitMaxFontSize = 8
                .LabelAutoFitMinFontSize = 8
                .IsMarginVisible = True
                .Title = "Metric Value" ' Set a common title if needed
                .TitleAlignment = StringAlignment.Far
            End With

            'Add legend
            Dim legend As New DataVisualization.Charting.Legend()
            metricsChart.Legends.Add(legend)

            ' Add chart to the form
            charts.Add(metricsChart)
            graphForm.Controls.Add(metricsChart)
        Next

        ' Display the form with the charts
        graphForm.Show()

        Dim saveResults As MsgBoxResult = MsgBox("Do you wish to save results as an image file?", vbYesNo, "Save Results?")
        If saveResults = vbYes Then
            ' Create a SaveFileDialog to get the file path from the user
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp"
            saveFileDialog.Title = "Save Form as Image"
            saveFileDialog.DefaultExt = "png"

            ' Show the dialog and check if the user clicked OK
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' Save the form (or panel with scrollable content) as an image
                Dim filePath As String = saveFileDialog.FileName
                SaveScrollableControlAsImage(graphForm, filePath)
                MessageBox.Show("Form saved as image successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If


    End Sub

    Private Sub SaveScrollableControlAsImage(ctrl As ScrollableControl, filePath As String)
        ' Save the original scroll position of the control
        Dim originalScrollPos As Point = ctrl.AutoScrollPosition

        ' Calculate the full size of the client area (excluding borders and title)
        Dim scrollableSize As Size = ctrl.DisplayRectangle.Size

        ' Create a bitmap with the full size of the client area
        Dim bmp As New Bitmap(scrollableSize.Width, scrollableSize.Height)

        ' Create a Graphics object to draw on the large bitmap
        Using g As Graphics = Graphics.FromImage(bmp)
            ' Iterate over the scrollable area in sections
            Dim sectionHeight As Integer = ctrl.ClientSize.Height
            Dim sectionWidth As Integer = ctrl.ClientSize.Width

            For yOffset As Integer = 0 To scrollableSize.Height - 1 Step sectionHeight
                For xOffset As Integer = 0 To scrollableSize.Width - 1 Step sectionWidth
                    ' Scroll to the appropriate position
                    ctrl.AutoScrollPosition = New Point(xOffset, yOffset)

                    ' Create a temporary bitmap for the visible section
                    Dim tempBmp As New Bitmap(sectionWidth, sectionHeight)

                    ' Draw the visible section of the control into the temporary bitmap
                    ctrl.DrawToBitmap(tempBmp, New Rectangle(0, 0, sectionWidth, sectionHeight))

                    ' Copy the temporary bitmap into the appropriate position in the large bitmap
                    g.DrawImage(tempBmp, xOffset, yOffset)

                    ' Dispose of the temporary bitmap
                    tempBmp.Dispose()
                Next
            Next
        End Using

        ' Save the large bitmap to the specified file path
        bmp.Save(filePath, Imaging.ImageFormat.Png) ' Change to appropriate format based on file extension

        ' Restore the original scroll position
        ctrl.AutoScrollPosition = New Point(Math.Abs(originalScrollPos.X), Math.Abs(originalScrollPos.Y))

        ' Clean up
        bmp.Dispose()
    End Sub


    ' Function to get the metric value based on the series name
    Private Function GetMetricValue(metricName As String, metrics As LogFileMetrics) As Double
        Select Case metricName
            Case "Highest Ping"
                Return metrics.HighestPing
            Case "Highest Jitter"
                Return metrics.HighestJitter
            Case "Packet Loss"
                Return metrics.HighestPacketLoss
            Case "Reconnects"
                Return metrics.Reconnects
            Case "Max Incoming Change"
                Return metrics.PacketBytesMaxIncomingChange
            Case "Max Outgoing Change"
                Return metrics.PacketBytesMaxOutgoingChange
            Case Else
                Return 0
        End Select
    End Function


End Class
