<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainWindow))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ProgrammToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.KonfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeendenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnhancedTabControl1 = New Eisenbahn.enhancedTabControl()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgrammToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(712, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ProgrammToolStripMenuItem
        '
        Me.ProgrammToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.KonfigurationToolStripMenuItem, Me.BeendenToolStripMenuItem})
        Me.ProgrammToolStripMenuItem.Name = "ProgrammToolStripMenuItem"
        Me.ProgrammToolStripMenuItem.Size = New System.Drawing.Size(76, 20)
        Me.ProgrammToolStripMenuItem.Text = "Programm"
        '
        'KonfigurationToolStripMenuItem
        '
        Me.KonfigurationToolStripMenuItem.Name = "KonfigurationToolStripMenuItem"
        Me.KonfigurationToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.KonfigurationToolStripMenuItem.Text = "Konfiguration"
        '
        'BeendenToolStripMenuItem
        '
        Me.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem"
        Me.BeendenToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.BeendenToolStripMenuItem.Text = "Beenden"
        '
        'EnhancedTabControl1
        '
        Me.EnhancedTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EnhancedTabControl1.Location = New System.Drawing.Point(0, 24)
        Me.EnhancedTabControl1.Name = "EnhancedTabControl1"
        Me.EnhancedTabControl1.SelectedIndex = 0
        Me.EnhancedTabControl1.Size = New System.Drawing.Size(712, 520)
        Me.EnhancedTabControl1.TabIndex = 2
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(712, 544)
        Me.Controls.Add(Me.EnhancedTabControl1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainWindow"
        Me.Text = "Eisenbahnprogramm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ProgrammToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents KonfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BeendenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnhancedTabControl1 As Eisenbahn.enhancedTabControl
End Class
