Public Class ControlForm
    Public Sub addControl(ByRef con As Control)
        Dim newTabPage As New TabPage(con.Name)
        newTabPage.Controls.Add(con)
        TabControl1.TabPages.Add(newTabPage)
    End Sub

    Public Function removeControl(ByRef con As Control) As Integer
        If Not TabControl1.TabPages Is Nothing Then
            Dim i As Integer = 0
            While i < TabControl1.TabPages.Count
                If TabControl1.TabPages(i).Controls Is Nothing Then
                    TabControl1.TabPages.RemoveAt(i)
                End If
                For q As Integer = 0 To TabControl1.TabPages(i).Controls.Count - 1
                    If TabControl1.TabPages(i).Controls(q) Is con Then
                        TabControl1.TabPages.RemoveAt(i)
                    End If
                Next
                i = i + 1
            End While
        End If
        Return TabControl1.TabPages.Count
    End Function
End Class