Public Class KonfigurationForm

    Private Sub KonfigurationForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Visible = False
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
        End If
    End Sub

    Private Sub KonfigurationForm_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        EnhancedTabControl1.SelectedIndex = 0
    End Sub

    Private Sub KonfigurationForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = "Version " & Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
    End Sub
End Class