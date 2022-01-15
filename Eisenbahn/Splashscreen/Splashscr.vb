Imports System.Reflection

Public Class Splashscr

    Private labelsCount As Integer = 6
    Private labels() As Label

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Me.Opacity = 0


        Label1.Text = "Version " & Assembly.GetExecutingAssembly.GetName.Version.ToString

        Dim sSRegion As New Drawing2D.GraphicsPath
        sSRegion.FillMode = Drawing2D.FillMode.Winding
        sSRegion.AddEllipse(New Rectangle(0, 0, 100, 100))
        sSRegion.AddRectangle(New Rectangle(50, 0, 400, 250))
        sSRegion.AddRectangle(New Rectangle(0, 50, 50, 200))

        Me.Region = New Region(sSRegion)

        ' Add any initialization after the InitializeComponent() call.
        Me.Show()
        Me.Update()

        ReDim labels(labelsCount)
        For i As Integer = 0 To labelsCount - 1
            labels(i) = New Label
            Dim col As Integer = i * 40 + 20
            With labels(i)
                .TextAlign = ContentAlignment.MiddleRight
                .Width = Me.Width / 2
                .Left = Me.Width - .Width
                .Height = .Font.Height
                .Top = Me.Height - (i + 1) * .Height - 10
                .BackColor = Color.Transparent
                .ForeColor = Color.FromArgb(col, col, col)
                .Text = ""
                .Parent = Me
                .Name = "Label" & i
            End With
        Next

        labels(0).ForeColor = Color.FromArgb(0, 0, 0)
    End Sub

    Public Sub progress(ByVal desc As String)
        'Blendet den Splashscreen langsam ein. Verzögert dabei jedoch Programmstart um 10*40ms
        While Me.Opacity < 1
            Me.Opacity = Me.Opacity + 0.1
            Me.Update()
            System.Threading.Thread.Sleep(40)
        End While

        For i As Integer = labelsCount - 1 To 1 Step -1
            labels(i).Text = labels(i - 1).Text
            labels(i).Update()
        Next
        labels(0).Text = desc
        labels(0).Update()
    End Sub

    Public Sub switches(ByVal count As Integer)
        ProgressBar1.Visible = True
        If count > ProgressBar1.Maximum Then
            ProgressBar1.Maximum = count
        End If
        ProgressBar1.Value = ProgressBar1.Maximum - count
        ProgressBar1.Update()
    End Sub

    Public Sub finish()
        ProgressBar1.Value = ProgressBar1.Maximum
        ProgressBar1.Update()
        Timer1.Stop()
        Timer1.Interval = 1000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Hide()
        Timer1.Stop()
    End Sub
End Class