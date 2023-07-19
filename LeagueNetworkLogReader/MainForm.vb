Imports System.IO
Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting

Public Class MainForm
    Private Descriptions As New Dictionary(Of String, String)
    Private table As DataTable

    Private Sub MainForm_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        ' Check if the dragged data contains file(s)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            dropcap.Visible = True
            e.Effect = DragDropEffects.Copy ' Set the drag effect to Copy
        End If
    End Sub

    Private Sub MainForm_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        ' Get the array of file paths from the dropped data
        Dim filePaths As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())

        ' Process each dropped file
        For Each filePath As String In filePaths
            dropcap.Visible = False
            ' Load and process the file here
            LoadLogFile(filePath)
        Next

    End Sub

    Private Sub LoadLogFile(filePath As String)
        Dim logLines As String() = File.ReadAllLines(filePath)
        Dim hasDescriptions As Boolean = False
        Descriptions.Clear()

        ' Find the line with column descriptions
        Dim columnDescLines As String() = logLines.Where(Function(line) line.Contains("=")).ToArray
        For Each description As String In columnDescLines
            Descriptions.Add(description.Split(":")(1).Split("="c)(0).Trim().ToLower, description.Split(":")(1).Split("="c)(1).Trim())
        Next
        If columnDescLines Is Nothing Then
            MessageBox.Show("Failed to find column descriptions in the log file. I will carry on without descriptions.")
            hasDescriptions = False
        Else
            hasDescriptions = True
        End If

        ' Find the line with column headings
        Dim columnNamesLine As String = logLines.LastOrDefault(Function(line) line.Contains("[") AndAlso line.Contains("]"))
        If columnNamesLine Is Nothing Then
            MessageBox.Show("Failed to find column headings in the log file.")
            Return
        End If

        ' Extract column names from the line
        Dim startBracketIndex As Integer = columnNamesLine.IndexOf("[")
        Dim endBracketIndex As Integer = columnNamesLine.LastIndexOf("]")
        Dim columnNamesString As String = columnNamesLine.Substring(startBracketIndex, endBracketIndex - startBracketIndex + 1)
        Dim columnNames As String() = columnNamesString.Split(","c)

        ' Create DataTable
        table = New DataTable()
        For Each columnName As String In columnNames
            columnName = columnName.Replace("[", "").Replace("]", "").Trim()
            'correct a spelling mistake
            If columnName = "Latency Maxin Window" Then columnName = "Latency Max in Window"
            table.Columns.Add(columnName)
        Next

        Dim isHeaderLineRead As Boolean = False
        ' Change Time Data to be more user readable
        For Each line As String In logLines
            Dim parts As String() = line.Split(","c)
            Dim partTime As TimeSpan

            If parts.Length = table.Columns.Count Then
                If isHeaderLineRead Then
                    partTime = TimeSpan.FromMilliseconds(parts(0))
                    parts(0) = "m: " & partTime.Minutes.ToString & " s: " & partTime.Seconds.ToString
                    table.Rows.Add(parts)
                Else
                    isHeaderLineRead = True
                End If
            End If
        Next

        ' Extract data rows
        'For Each line As String In logLines
        ' Dim parts As String() = line.Split(","c)
        ' If parts.Length = table.Columns.Count Then
        ' table.Rows.Add(parts)
        ' End If
        ' Next

        'table.Rows(0).Delete()

        ' Display data in DataGridView
        DataGridView1.DataSource = table

        For Each col As DataGridViewColumn In DataGridView1.Columns
            col.ToolTipText = Descriptions(col.Name.ToLower)
        Next

        AnalyzeDataAndHighlightIssues()
    End Sub

    Private Sub AnalyzeDataAndHighlightIssues()

        ' Calculate average values for each column
        Dim pingAverage As Double = 0
        If table.Columns.Contains("ping") Then
            Dim pingValues As List(Of Integer) = table.AsEnumerable().Select(Function(row) Convert.ToInt32(row("ping"))).ToList()
            pingAverage = pingValues.Average()
        End If

        ' Calculate average difference between each row for incoming and outgoing columns
        Dim incomingAverage As Double = 0
        If table.Columns.Contains("incoming") Then
            Dim incomingValues As List(Of Integer) = table.AsEnumerable().Select(Function(row) Convert.ToInt32(row("incoming"))).ToList()
            Dim incomingDifferences As List(Of Integer) = incomingValues.Zip(incomingValues.Skip(1), Function(a, b) b - a).ToList()
            incomingAverage = incomingDifferences.Average()
        End If

        Dim outgoingAverage As Double = 0
        If table.Columns.Contains("outgoing") Then
            Dim outgoingValues As List(Of Integer) = table.AsEnumerable().Select(Function(row) Convert.ToInt32(row("outgoing"))).ToList()
            Dim outgoingDifferences As List(Of Integer) = outgoingValues.Zip(outgoingValues.Skip(1), Function(a, b) b - a).ToList()
            outgoingAverage = outgoingDifferences.Average()
        End If

        Dim jitterAverage As Double = 0
        If table.Columns.Contains("Jitter in Window") Then
            Dim jitterValues As List(Of Integer) = table.AsEnumerable().Select(Function(row) Convert.ToInt32(row("Jitter in Window"))).ToList()
            jitterAverage = jitterValues.Average()
        End If

        Dim packetLossAverageac As Double = 0
        If table.Columns.Contains("loss") Then
            Dim packetLossacValues As List(Of Double) = table.AsEnumerable().Select(Function(row) Convert.ToDouble(row("loss"))).ToList()
            packetLossAverageac = packetLossacValues.Average()
        End If

        Dim packetLossAverage As Double = 0
        If table.Columns.Contains("Packet Loss percentage in Window") Then
            Dim packetLossValues As List(Of Double) = table.AsEnumerable().Select(Function(row) Convert.ToDouble(row("Packet Loss percentage in Window"))).ToList()
            packetLossAverage = packetLossValues.Average()
        End If

        Dim reconnectAverage As Double = 0
        If table.Columns.Contains("reconnect") Then
            Dim reconnectValues As List(Of Integer) = table.AsEnumerable().Select(Function(row) Convert.ToInt32(row("reconnect"))).ToList()
            reconnectAverage = reconnectValues.Average()
        End If

        ' Calculate thresholds relative to average values
        Dim maximumPingThreshold As Double = pingAverage * 1.2
        Dim highestPing = table.AsEnumerable().Max(Function(row) Convert.ToInt32(row("ping")))
        Dim incomingChangeThreshold As Double = incomingAverage * 1.5
        Dim outgoingChangeThreshold As Double = outgoingAverage * 1.5
        Dim maximumJitterThreshold As Double = jitterAverage * 2
        Dim packetLossThreshold As Double = 0 'we actually don't want any loss.
        Dim packetLossacThreshold As Double = 0 'we actually don't want any loss.
        Dim maximumReconnectThreshold As Double = 0 'we never want a reconnect

        Dim previousIncoming As Integer = 0
        Dim previousOutgoing As Integer = 0


        Dim maxIncomingChange As Integer = 0
        Dim minIncomingChange As Integer = Integer.MaxValue

        Dim maxOutgoingChange As Integer = 0
        Dim minOutgoingChange As Integer = Integer.MaxValue

        ' Perform analysis and highlight cells if necessary
        For Each row As DataGridViewRow In DataGridView1.Rows

            For Each cell As DataGridViewCell In row.Cells
                ' Get the cell values for the relevant columns
                Dim columnName As String = DataGridView1.Columns(cell.ColumnIndex).Name
                Dim cellValue As Object = cell.Value

                ' Perform analysis based on column name
                Select Case columnName
                    Case "ping"
                        Dim ping As Integer
                        If Integer.TryParse(cellValue?.ToString(), ping) Then
                            If ping > maximumPingThreshold Then
                                cell.Style.BackColor = Color.LightPink
                                cell.ToolTipText = "Over your average ping by threshold: " & maximumPingThreshold & " possible spike."
                            End If
                            Debug.Print(ping & highestPing)
                            If ping = highestPing Then
                                cell.Style.ForeColor = Color.Red
                                cell.ToolTipText = "This was your highest ping " & highestPing & ", the higher the more latency between you and riot."
                            End If
                        End If
                    Case "incoming"
                        Dim incoming As Integer
                        If Integer.TryParse(cellValue?.ToString(), incoming) Then
                            If incoming <> 0 AndAlso previousIncoming <> 0 Then
                                Dim difference As Integer = Math.Abs(incoming - previousIncoming)
                                If difference > incomingChangeThreshold Then
                                    cell.Style.BackColor = Color.Coral
                                    cell.ToolTipText = "Packets on their way to you from increased meaning there could of been a delay in the connection to you."
                                End If

                                ' Check if it's the maximum or minimum change
                                If difference > maxIncomingChange Then
                                    maxIncomingChange = difference
                                End If
                                If difference < minIncomingChange Then
                                    minIncomingChange = difference
                                End If
                            End If
                            previousIncoming = incoming
                        End If
                    Case "outgoing"
                        Dim outgoing As Integer
                        If Integer.TryParse(cellValue?.ToString(), outgoing) Then
                            If outgoing <> 0 AndAlso previousOutgoing <> 0 Then
                                Dim difference As Integer = Math.Abs(outgoing - previousOutgoing)
                                If difference > outgoingChangeThreshold Then
                                    cell.Style.BackColor = Color.Coral
                                    cell.ToolTipText = "Packets on their way to riot increased meaning there could of been a delay in your connection to riot."
                                End If

                                ' Check if it's the maximum or minimum change
                                If difference > maxOutgoingChange Then
                                    maxOutgoingChange = difference
                                End If
                                If difference < minOutgoingChange Then
                                    minOutgoingChange = difference
                                End If
                            End If
                            previousOutgoing = outgoing
                        End If
                    Case "Jitter in Window"
                        Dim jitter As Integer
                        If Integer.TryParse(cellValue?.ToString(), jitter) Then
                            If jitter > maximumJitterThreshold Then
                                cell.Style.BackColor = Color.LightGoldenrodYellow
                                cell.ToolTipText = "High jitter points to unstable connection"
                            End If
                        End If
                    Case "Packet Loss percentage in Window"
                        Dim packetLossPercentage As Double
                        If Double.TryParse(cellValue?.ToString(), packetLossPercentage) Then
                            If packetLossPercentage > packetLossThreshold Then
                                cell.Style.BackColor = Color.Coral
                                cell.ToolTipText = "Packets were lost this is usually due to a network issue. Your PC or a node between you and riot lost packets. Too many and the game will be unplayable."
                            End If
                        End If
                    Case "loss"
                        Dim packetLossac As Double
                        If Double.TryParse(cellValue?.ToString(), packetLossac) Then
                            If packetLossac > packetLossacThreshold Then
                                cell.Style.BackColor = Color.Orange
                                cell.ToolTipText = "Packet Loss Detected, this can cause data to be lost or slow."
                            End If
                        End If
                    Case "reconnect"
                        Dim reconnect As Integer
                        If Integer.TryParse(cellValue?.ToString(), reconnect) Then
                            If reconnect > maximumReconnectThreshold Then
                                cell.Style.BackColor = Color.DarkRed
                                cell.Style.ForeColor = Color.White
                                cell.ToolTipText = "Your client reconnected at this point. Which means it was disconnected prior this point."
                            End If
                        End If
                End Select
            Next
        Next

        ' Calculate the highest value of packet loss
        Dim highestPacketLoss As Integer = 0

        If table.Columns.Contains("loss") Then
            highestPacketLoss = table.AsEnumerable().Max(Function(row) Convert.ToInt32(row("loss")))
        End If

        Dim reconnectCount As Integer = 0
        If table.Columns.Contains("reconnect") Then
            reconnectCount = table.AsEnumerable().Count(Function(row) Convert.ToInt32(row("reconnect")))
        End If

        ' Generate analysis report
        Dim analysisReport As New StringBuilder()

        analysisReport.AppendLine("Analysis Based On File")
        analysisReport.AppendLine("-----------------------")

        analysisReport.AppendLine("Ping:")
        analysisReport.AppendLine($"- Ping Average: {pingAverage}")
        analysisReport.AppendLine($"- Ping High: {highestPing}")
        analysisReport.AppendLine()

        analysisReport.AppendLine("Incoming:")
        analysisReport.AppendLine($"- Incoming Change Threshold: {incomingChangeThreshold}")
        analysisReport.AppendLine($"- Max Incoming Change: {maxIncomingChange}")
        analysisReport.AppendLine($"- Min Incoming Change: {minIncomingChange}")
        analysisReport.AppendLine()

        analysisReport.AppendLine("Outgoing:")
        analysisReport.AppendLine($"- Outgoing Change Threshold: {outgoingChangeThreshold}")
        analysisReport.AppendLine($"- Max Outgoing Change: {maxOutgoingChange}")
        analysisReport.AppendLine($"- Min Outgoing Change: {minOutgoingChange}")
        analysisReport.AppendLine()

        analysisReport.AppendLine("Jitter:")
        analysisReport.AppendLine($"- Maximum Jitter Threshold: {maximumJitterThreshold}")
        analysisReport.AppendLine()

        analysisReport.AppendLine("Packet Loss:")
        analysisReport.AppendLine($"- Packet Loss Threshold: {packetLossThreshold}")
        analysisReport.AppendLine($"- Packet Loss Amount: {highestPacketLoss}")
        analysisReport.AppendLine()

        analysisReport.AppendLine("Reconnect:")
        analysisReport.AppendLine($"- Reconnect Count: {reconnectCount}")
        analysisReport.AppendLine()

        ' Explanation of network issues
        analysisReport.AppendLine("Explanation of Network Issues")
        analysisReport.AppendLine("-----------------------")
        analysisReport.AppendLine("1. Ping: The ping value represents the round-trip time it takes for a network packet to travel from your computer to the game server and back. Higher ping values indicate longer delays in communication and can lead to increased lag in online games.")

        analysisReport.AppendLine("2. Incoming/Outgoing Changes: These values represent the difference in incoming and outgoing packets between consecutive rows. Significant changes in these values can indicate variations in network traffic, which may result in delays or disruptions in your connection to the game server.")

        analysisReport.AppendLine("3. Jitter: Jitter refers to the variation in delay between packets arriving at their destination. Higher jitter values indicate an inconsistent network connection, which can lead to unstable gameplay, increased lag, and disruptions in real-time applications.")

        analysisReport.AppendLine("4. Packet Loss: Packet loss occurs when network packets fail to reach their destination. High packet loss can result in missing or delayed data, leading to poor gameplay experience, increased lag, and potential disconnections.")

        analysisReport.AppendLine("5. Reconnect: Reconnect events indicate instances where your client disconnected from the game server and then reconnected. Frequent or prolonged reconnect events may indicate instability in your network connection or disruptions in the game server.")

        ' Display analysis report
        MessageBox.Show(analysisReport.ToString(), "Network Analysis Report", MessageBoxButtons.OK, MessageBoxIcon.Information)


    End Sub

End Class