<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogListForm
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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(LogListForm))
        ListBoxLogs = New ListBox()
        LogContext = New ContextMenuStrip(components)
        AnalyseAllToolStripMenuItem = New ToolStripMenuItem()
        LogContext.SuspendLayout()
        SuspendLayout()
        ' 
        ' ListBoxLogs
        ' 
        ListBoxLogs.ContextMenuStrip = LogContext
        ListBoxLogs.Dock = DockStyle.Fill
        ListBoxLogs.FormattingEnabled = True
        ListBoxLogs.ItemHeight = 15
        ListBoxLogs.Location = New Point(0, 0)
        ListBoxLogs.Name = "ListBoxLogs"
        ListBoxLogs.Size = New Size(284, 461)
        ListBoxLogs.TabIndex = 0
        ' 
        ' LogContext
        ' 
        LogContext.Items.AddRange(New ToolStripItem() {AnalyseAllToolStripMenuItem})
        LogContext.Name = "LogContext"
        LogContext.Size = New Size(276, 26)
        ' 
        ' AnalyseAllToolStripMenuItem
        ' 
        AnalyseAllToolStripMenuItem.Name = "AnalyseAllToolStripMenuItem"
        AnalyseAllToolStripMenuItem.Size = New Size(275, 22)
        AnalyseAllToolStripMenuItem.Text = "Analyse All Files (May Take Sometime)"
        ' 
        ' LogListForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(284, 461)
        Controls.Add(ListBoxLogs)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "LogListForm"
        Text = "List of Recent Logs"
        LogContext.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents ListBoxLogs As ListBox
    Friend WithEvents LogContext As ContextMenuStrip
    Friend WithEvents AnalyseAllToolStripMenuItem As ToolStripMenuItem
End Class
