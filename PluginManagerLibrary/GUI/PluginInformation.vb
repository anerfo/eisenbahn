Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Public Class PluginInformation

    Private iPlugins() As PluginClass
    Private iserviceObjects() As ServiceObject

    Public Sub New(ByVal Plugin() As PluginClass, ByVal serviceObjects() As ServiceObject)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Dock = Windows.Forms.DockStyle.Fill

        iPlugins = Plugin
        iserviceObjects = serviceObjects
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        TextBox1.Text = ""

        TextBox1.Text &= "Name:" & vbNewLine
        TextBox1.Text &= iPlugins(ListView1.FocusedItem.Index).PluginReference.name() & vbNewLine
        TextBox1.Text &= vbNewLine
        TextBox1.Text &= "Beschreibung:" & vbNewLine
        TextBox1.Text &= iPlugins(ListView1.FocusedItem.Index).PluginReference.beschreibung
        TextBox1.Text &= vbNewLine & vbNewLine
        TextBox1.Text &= "[Type = "
        TextBox1.Text &= iPlugins(ListView1.FocusedItem.Index).PluginReference.GetType.ToString
        TextBox1.Text &= "]"
        TextBox1.Text &= vbNewLine & vbNewLine
        TextBox1.Text &= "[Status = "
        TextBox1.Text &= iPlugins(ListView1.FocusedItem.Index).state.ToString
        TextBox1.Text &= "]"
        TextBox1.Text &= vbNewLine & vbNewLine
        TextBox1.Text &= "[Pfad = "
        TextBox1.Text &= iPlugins(ListView1.FocusedItem.Index).fullPath
        TextBox1.Text &= "]"
        TextBox1.Text &= vbNewLine & vbNewLine

        FlowLayoutPanel1.Controls.Clear()
        If Not iPlugins(ListView1.FocusedItem.Index).dependencies Is Nothing Then
            For i As Integer = 0 To iPlugins(ListView1.FocusedItem.Index).dependencies.Length - 1
                Dim cb As New ComboBox
                cb.Size = New Size(FlowLayoutPanel1.Width - 10, cb.Height)
                cb.Tag = i
                For q As Integer = 0 To iPlugins(ListView1.FocusedItem.Index).dependencies(i).Count - 1
                    cb.Items.Add(iPlugins(ListView1.FocusedItem.Index).dependencies(i)(q).SourcePlugin.PluginReference.name & " [" & iPlugins(ListView1.FocusedItem.Index).dependencies(i)(q).SourcePlugin.PluginReference.GetType.ToString & "]")
                    If iPlugins(ListView1.FocusedItem.Index).dependencies(i)(q).ServiceObj Is iPlugins(ListView1.FocusedItem.Index).objects(i).ServiceObj Then
                        cb.SelectedIndex = q
                    End If
                Next
                FlowLayoutPanel1.Controls.Add(cb)
                AddHandler cb.SelectedIndexChanged, AddressOf ComboBoxSelectedIndexChanged
            Next
        End If
    End Sub

    Private Sub ComboBoxSelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'iPlugins(sender.tag).dependencies(sender.selectedindex)
        Dim cb As ComboBox = sender
        iPlugins(ListView1.FocusedItem.Index).objects(cb.Tag) = iPlugins(ListView1.FocusedItem.Index).dependencies(cb.Tag)(cb.SelectedIndex)
    End Sub


    Private Sub ListPlugins()
        If Not iPlugins Is Nothing Then
            ListView1.Clear()
            For i As Integer = 0 To iPlugins.Length - 1
                ListView1.Items.Add(iPlugins(i).PluginReference.name.ToString)
            Next
        End If
    End Sub

    Private Sub ListServiceObjects()
        If Not iserviceObjects Is Nothing Then
            ListView2.Clear()
            For i As Integer = 0 To iserviceObjects.Length - 1
                ListView2.Items.Add(iserviceObjects(i).ServiceObj.ToString)
            Next
        End If
    End Sub

    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        With TextBox2
            .Text = ""
            Dim typ As Type = iserviceObjects(ListView2.FocusedItem.Index).ServiceObj.GetType

            .Text &= "Name:" & vbNewLine
            .Text &= typ.ToString & vbNewLine & vbNewLine
            .Text &= "Ursprung:" & vbNewLine
            .Text &= iserviceObjects(ListView2.FocusedItem.Index).SourcePlugin.PluginReference.name
            .Text &= vbNewLine & vbNewLine
            .Text &= "Enthält Schnittstellen:" & vbNewLine
            For i As Integer = 0 To typ.GetInterfaces.Length - 1
                .Text &= " - " & typ.GetInterfaces(i).ToString & vbNewLine
            Next
        End With
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case TabControl1.SelectedIndex
            Case 0
                ListPlugins()
            Case 1
                ListServiceObjects()
            Case 2
                populateInterfaceList()
        End Select
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    With FlowLayoutPanel1
    '        .Controls.Clear()
    '        Dim ct As New ComboBox()
    '        ct.Items.Add(1)
    '        ct.Items.Add(2)
    '        ct.Size = New Size(.Width - 5, 20)
    '        .Controls.Add(ct)
    '    End With
    'End Sub

    Private Sub PluginInformation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListPlugins()
        Label1.Text = "Plugins von denen dieses Plugin abhängt: " & vbNewLine & "Änderungen werden nach einem Neustart übernommen"
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InterfaceList.SelectedIndexChanged
        Dim t As Type = InterfaceList.SelectedItem
        Dim pic As New Bitmap(1024, 1024, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(pic)
        Dim xpos As Integer = 0
        Dim ypos As Integer = 0
        Dim rectWidth As Integer = 100
        Dim rectHeight As Integer = 50
        Dim padding As Integer = 10
        Dim levelSpacing As Integer = 30
        Dim count As Integer = 0
        Dim textsize As Integer = 8
        Dim schriftart As New Font("Arial", textsize)
        Dim level As Integer = 0

        'Liste der ServiceObjekte, die Interface implementieren
        Dim serviceObjectsImplementingIF As New List(Of ServiceObject)

        For i As Integer = 0 To iserviceObjects.Length - 1
            If Not iserviceObjects(i).ServiceObj.GetType.GetInterface(t.ToString) Is Nothing Then
                serviceObjectsImplementingIF.Add(iserviceObjects(i))
            End If
        Next

        'Baum generieren

        Dim levelElements() As List(Of PluginClass)
        Dim edges As New List(Of Point)

        ReDim levelElements(0)
        levelElements(0) = New List(Of PluginClass)

        'Wenn Plugin von Interface abhängt (Interface ist Element der Dependencies)
        For i As Integer = 0 To iPlugins.Length - 1
            For q As Integer = 0 To serviceObjectsImplementingIF.Count - 1
                If Not iPlugins(i).objects Is Nothing Then
                    If iPlugins(i).objects.Contains(serviceObjectsImplementingIF(q)) Then
                        levelElements(0).Add(iPlugins(i))
                    End If
                End If
            Next
        Next

        For i As Integer = 0 To serviceObjectsImplementingIF.Count - 1
            If Not levelElements(0).Contains(serviceObjectsImplementingIF(i).SourcePlugin) Then
                levelElements(0).Add(serviceObjectsImplementingIF(i).SourcePlugin)
            End If
        Next

        While levelElements(levelElements.Length - 1).Count > 0
            ReDim Preserve levelElements(levelElements.Length)
            levelElements(levelElements.Length - 1) = New List(Of PluginClass)

            For i As Integer = 0 To levelElements(levelElements.Length - 2).Count - 1
                For q As Integer = 0 To levelElements(levelElements.Length - 2).Count - 1
                    If i = q Then
                        Continue For
                    End If

                    If Not levelElements(levelElements.Length - 2)(i).objects Is Nothing Then
                        For s As Integer = 0 To levelElements(levelElements.Length - 2)(i).objects.Count - 1
                            If levelElements(levelElements.Length - 2)(i).objects(s).SourcePlugin Is levelElements(levelElements.Length - 2)(q) Then
                                If Not levelElements(levelElements.Length - 1).Contains(levelElements(levelElements.Length - 2)(q)) Then
                                    levelElements(levelElements.Length - 1).Add(levelElements(levelElements.Length - 2)(q))
                                End If
                            End If
                        Next
                    End If
                Next
            Next
        End While

        'In den Listen doppelt vorkommende Plugins löschen
        Dim bearbeiteteElemente As New List(Of PluginClass)
        For i As Integer = levelElements.Length - 2 To 0 Step -1
            For q As Integer = 0 To levelElements(i + 1).Count - 1
                bearbeiteteElemente.Add(levelElements(i + 1)(q))
            Next
            For q As Integer = 0 To bearbeiteteElemente.Count - 1
                If levelElements(i).Contains(bearbeiteteElemente(q)) Then
                    levelElements(i).Remove(bearbeiteteElemente(q))
                End If
            Next
        Next

        For i As Integer = 0 To iPlugins.Length - 1
            If iPlugins(i).objects Is Nothing Then
                Continue For
            End If
            For q As Integer = 0 To iPlugins(i).objects.Length - 1
                For s As Integer = 0 To serviceObjectsImplementingIF.Count - 1
                    If i = s Then
                        Continue For
                    End If

                    If iPlugins(i).objects(q) Is serviceObjectsImplementingIF(s) Then
                        For r As Integer = 0 To iPlugins.Length - 1
                            If serviceObjectsImplementingIF(s).SourcePlugin Is iPlugins(r) Then
                                edges.Add(New Point(r, i))
                            End If
                        Next
                    End If
                Next
            Next
        Next

        level = 0
        count = 0

        Dim edgesWorkCopy As New List(Of Point)
        For i As Integer = 0 To edges.Count - 1
            edgesWorkCopy.Add(edges(i))
        Next

        While edgesWorkCopy.Count > 0
            For i As Integer = 0 To iPlugins.Length - 1
                Dim foundInYValues As Boolean = False
                Dim foundInXValues As Boolean = False
                For q As Integer = 0 To edgesWorkCopy.Count - 1
                    If edgesWorkCopy(q).X = i Then
                        foundInXValues = True
                    End If
                    If edgesWorkCopy(q).Y = i Then
                        foundInYValues = True
                    End If
                Next
                If foundInXValues = True And foundInYValues = False Then
                    g.DrawRectangle(Pens.Red, xpos + (padding + rectWidth) * count, ypos + (padding + rectHeight) * level, rectWidth, rectHeight)
                    Dim text As String = iPlugins(i).PluginReference.name.ToString
                    g.DrawString(text, schriftart, Brushes.Red, New PointF(xpos + (padding + rectWidth) * count, ypos + (padding + rectHeight) * level))

                    Dim q As Integer = 0
                    While q < edgesWorkCopy.Count
                        If edgesWorkCopy(q).X = i Then
                            edgesWorkCopy.RemoveAt(q)
                        Else
                            q += 1
                        End If
                    End While
                    count = 0
                    level += 1
                End If
            Next
        End While


            PictureBox1.Image = pic

            'Dim levelElements As New List(Of ServiceObject)

            'For i As Integer = 0 To iserviceObjects.Length - 1
            '    If Not iserviceObjects(i).ServiceObj.GetType.GetInterface(t.ToString) Is Nothing Then
            '        levelElements.Add(iserviceObjects(i))

            '        g.DrawRectangle(Pens.Red, xpos + (padding + rectWidth) * count, ypos + padding, rectWidth, rectHeight)
            '        Dim text As String = iserviceObjects(i).ServiceObj.GetType.ToString
            '        Dim count2 As Integer = 0
            '        While Strings.InStr(text, ".")
            '            Dim printtext As String = Strings.Left(text, Strings.InStr(text, ".") - 1)
            '            text = Strings.Right(text, text.Length - Strings.InStr(text, "."))
            '            g.DrawString(printtext, schriftart, Brushes.Red, New PointF(xpos + (padding + rectWidth) * count, ypos + padding + count2 * (textsize + 2)))
            '            count2 += 1
            '        End While
            '        g.DrawString(text, schriftart, Brushes.Red, New PointF(xpos + (padding + rectWidth) * count, ypos + padding + count2 * (textsize + 2)))
            '        count += 1
            '    End If
            'Next

            'Dim localLevelElements As New List(Of ServiceObject)

            'For i As Integer = 0 To levelElements.Count - 1
            '    localLevelElements.Add(levelElements(i))
            'Next

            'While Not localLevelElements Is Nothing

            '    level += 1
            '    count = 0
            '    localLevelElements.Clear()

            '    For i As Integer = 0 To iPlugins.Length - 1
            '        If Not iPlugins(i).objects Is Nothing Then
            '            For q As Integer = 0 To iPlugins(i).objects.Length - 1
            '                If levelElements.Contains(iPlugins(i).objects(q)) Then
            '                    'Verbindung zeichnen

            '                    g.DrawRectangle(Pens.Red, xpos + (padding + rectWidth) * count, ypos + padding + rectHeight * level + levelSpacing, rectWidth, rectHeight)
            '                    Dim text As String = iPlugins(i).GetType.ToString
            '                    Dim count2 As Integer = 0
            '                    While Strings.InStr(text, ".")
            '                        Dim printtext As String = Strings.Left(text, Strings.InStr(text, ".") - 1)
            '                        text = Strings.Right(text, text.Length - Strings.InStr(text, "."))
            '                        g.DrawString(printtext, schriftart, Brushes.Red, New PointF(xpos + (padding + rectWidth) * count, ypos + padding + rectHeight * level + levelSpacing + count2 * (textsize + 2)))
            '                        count2 += 1
            '                    End While
            '                    g.DrawString(text, schriftart, Brushes.Red, New PointF(xpos + (padding + rectWidth) * count, ypos + padding + rectHeight * level + levelSpacing + count2 * (textsize + 2)))
            '                    count += 1


            '                    'For s As Integer = 0 to 
            '                End If
            '            Next
            '        End If
            '    Next

            '    For i As Integer = 0 To localLevelElements.Count - 1
            '        levelElements.Add(localLevelElements(i))
            '    Next

            'End While

    End Sub

    Private Sub populateInterfaceList()
        Dim interfaces As New List(Of Type)
        If iserviceObjects Is Nothing Then
            Return
        End If
        For i As Integer = 0 To iserviceObjects.Length - 1
            Dim ifs As Type = iserviceObjects(i).ServiceObj.GetType
            If Not ifs.GetInterfaces Is Nothing Then
                For q As Integer = 0 To ifs.GetInterfaces.Length - 1
                    If Not interfaces.Contains(ifs.GetInterfaces(q)) Then
                        interfaces.Add(ifs.GetInterfaces(q))
                    End If
                Next
            End If
        Next
        If interfaces Is Nothing Then
            Return
        End If
        For i As Integer = 0 To interfaces.Count - 1
            InterfaceList.Items.Add(interfaces(i)) '.ToString)
        Next
    End Sub
End Class
