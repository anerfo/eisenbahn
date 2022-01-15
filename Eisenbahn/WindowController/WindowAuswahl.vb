Public Class WindowAuswahl

    Public windows As List(Of ControlForm)

    Public gewaehltesWindow As ControlForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        gewaehltesWindow = New ControlForm
        Me.DialogResult = vbOK
    End Sub

    Private Sub WindowAuswahl_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ListBox1.Items.Clear()
        For i As Integer = 0 To windows.Count - 1
            ListBox1.Items.Add(windows(i).Name.ToString & " (" & windows(i).TabControl1.TabPages(0).Controls(0).Name & ")")
        Next
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.Items.Count > 0 Then
            If ListBox1.SelectedIndex = -1 Then
                Return
            End If
            gewaehltesWindow = windows(ListBox1.SelectedIndex)
            Me.DialogResult = vbOK
        Else
            gewaehltesWindow = Nothing
        End If
    End Sub
End Class