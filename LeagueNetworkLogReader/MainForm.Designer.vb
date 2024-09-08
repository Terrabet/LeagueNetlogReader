<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(MainForm))
        DataGridView1 = New DataGridView()
        OptionContextMenu = New ContextMenuStrip(components)
        ViewToolStripMenuItem = New ToolStripMenuItem()
        ShowLogFilesToolStripMenuItem = New ToolStripMenuItem()
        GlueWindowsToolStripMenuItem = New ToolStripMenuItem()
        ShowOverviewAnalysisToolStripMenuItem = New ToolStripMenuItem()
        AnalyzeNowToolStripMenuItem = New ToolStripMenuItem()
        dropcap = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        OptionContextMenu.SuspendLayout()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.BackgroundColor = Color.DimGray
        DataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        DataGridView1.ContextMenuStrip = OptionContextMenu
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = Color.Gainsboro
        DataGridViewCellStyle1.Font = New Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle1.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(0, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowHeadersVisible = False
        DataGridView1.RowTemplate.Height = 25
        DataGridView1.Size = New Size(1084, 436)
        DataGridView1.TabIndex = 0
        ' 
        ' OptionContextMenu
        ' 
        OptionContextMenu.Items.AddRange(New ToolStripItem() {ViewToolStripMenuItem, AnalyzeNowToolStripMenuItem})
        OptionContextMenu.Name = "ContextMenu"
        OptionContextMenu.Size = New Size(181, 70)
        ' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ShowLogFilesToolStripMenuItem, GlueWindowsToolStripMenuItem, ShowOverviewAnalysisToolStripMenuItem})
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(180, 22)
        ViewToolStripMenuItem.Text = "View"
        ' 
        ' ShowLogFilesToolStripMenuItem
        ' 
        ShowLogFilesToolStripMenuItem.Checked = True
        ShowLogFilesToolStripMenuItem.CheckState = CheckState.Checked
        ShowLogFilesToolStripMenuItem.Name = "ShowLogFilesToolStripMenuItem"
        ShowLogFilesToolStripMenuItem.Size = New Size(201, 22)
        ShowLogFilesToolStripMenuItem.Text = "Show Log Files"
        ' 
        ' GlueWindowsToolStripMenuItem
        ' 
        GlueWindowsToolStripMenuItem.Checked = True
        GlueWindowsToolStripMenuItem.CheckState = CheckState.Checked
        GlueWindowsToolStripMenuItem.Name = "GlueWindowsToolStripMenuItem"
        GlueWindowsToolStripMenuItem.Size = New Size(201, 22)
        GlueWindowsToolStripMenuItem.Text = "Glue Windows"
        ' 
        ' ShowOverviewAnalysisToolStripMenuItem
        ' 
        ShowOverviewAnalysisToolStripMenuItem.Checked = True
        ShowOverviewAnalysisToolStripMenuItem.CheckState = CheckState.Checked
        ShowOverviewAnalysisToolStripMenuItem.Name = "ShowOverviewAnalysisToolStripMenuItem"
        ShowOverviewAnalysisToolStripMenuItem.Size = New Size(201, 22)
        ShowOverviewAnalysisToolStripMenuItem.Text = "Show Overview Analysis"
        ' 
        ' AnalyzeNowToolStripMenuItem
        ' 
        AnalyzeNowToolStripMenuItem.Name = "AnalyzeNowToolStripMenuItem"
        AnalyzeNowToolStripMenuItem.Size = New Size(180, 22)
        AnalyzeNowToolStripMenuItem.Text = "Analyze Now"
        ' 
        ' dropcap
        ' 
        dropcap.BackColor = Color.DimGray
        dropcap.ContextMenuStrip = OptionContextMenu
        dropcap.Dock = DockStyle.Fill
        dropcap.Font = New Font("Consolas", 48F, FontStyle.Regular, GraphicsUnit.Point)
        dropcap.ForeColor = Color.Gray
        dropcap.Location = New Point(0, 0)
        dropcap.Name = "dropcap"
        dropcap.Size = New Size(1084, 436)
        dropcap.TabIndex = 1
        dropcap.Text = "DROP A FILE HERE" & vbCrLf & "(_netlog.txt)"
        dropcap.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' MainForm
        ' 
        AllowDrop = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1084, 436)
        Controls.Add(dropcap)
        Controls.Add(DataGridView1)
        DoubleBuffered = True
        Font = New Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "MainForm"
        Text = "Analyze Leauge of Legnds NetLog"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        OptionContextMenu.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents dropcap As Label
    Friend WithEvents OptionContextMenu As ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowLogFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GlueWindowsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowOverviewAnalysisToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AnalyzeNowToolStripMenuItem As ToolStripMenuItem
End Class
