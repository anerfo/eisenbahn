Public Class WindowController
    Implements PluginManagerLibrary.GUIControlContainer

    Private windows As New List(Of ControlForm)
    Private controls As New List(Of ControlLocation)

    Private dieWindowAuswahl As New WindowAuswahl

    Private showWindowsOnLoad As Boolean = False
    Private startingUp As Boolean = True
    Private controlsToCome As New List(Of String)
    Private startupLocation As New Hashtable

    Private windowNode As Xml.XmlNode

    Public Function addElement(ByVal con As System.Windows.Forms.Control) As Boolean Implements PluginManagerLibrary.GUIControlContainer.addElement
        Dim win As ControlForm = Nothing
        Dim targetWindowNode As Xml.XmlNode = Nothing

        If controlsToCome.Contains(con.Name) Then
            controlsToCome.Remove(con.Name)

            For i As Integer = 0 To windows.Count - 1
                If startupLocation(con.Name) = windows(i).Name Then
                    win = windows(i)
                    Exit For
                End If
            Next
            If win Is Nothing Then
                win = New ControlForm
                windows.Add(win)
                If startupLocation(con.Name) Is Nothing Then
                    win.Name = "Window" & windows.Count
                Else
                    win.Name = startupLocation(con.Name)
                End If
                win.Text = ""
                win.StartPosition = FormStartPosition.Manual
                targetWindowNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromNode(windowNode, win.Name)
                If Not targetWindowNode Is Nothing Then
                    win.Left = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "Left")
                    win.Top = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "Top")
                    win.Height = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "Height")
                    win.Width = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "Width")
                    If PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "WindowState") = FormWindowState.Maximized Then
                        win.WindowState = FormWindowState.Maximized
                    Else
                        win.WindowState = FormWindowState.Normal
                    End If
                End If
                AddHandler win.FormClosing, AddressOf controlWindowClosing
                If showWindowsOnLoad Then
                    win.Show(MainWindow)
                End If
            End If
        Else
            If dieWindowAuswahl.ShowDialog(KonfigurationForm) = vbOK Then
                win = dieWindowAuswahl.gewaehltesWindow
                If Not windows.Contains(win) Then
                    For i As Integer = 1 To windows.Count + 1
                        win.Name = "Window" & windows.Count
                        If Not windows.Contains(win) Then
                            Exit For
                        End If
                    Next
                    windows.Add(win)

                    win.Text = ""
                    AddHandler win.FormClosing, AddressOf controlWindowClosing

                    If showWindowsOnLoad Then
                        win.Show(MainWindow)
                    End If
                End If
            Else
                Return False
            End If
        End If

        If win Is Nothing Then
            Return False
        End If

        win.addControl(con)
        Dim cl As New ControlLocation
        cl.Con = con
        cl.window = win
        controls.Add(cl)

        If Not windowNode Is Nothing Then
            targetWindowNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromNode(windowNode, win.Name)
        End If
        If Not targetWindowNode Is Nothing Then
            Dim selectedIndex As Integer = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(targetWindowNode, "SelectedIndex")
            If selectedIndex < win.TabControl1.TabPages.Count Then
                win.TabControl1.SelectedIndex = selectedIndex
            End If
        End If
        Return True
    End Function

    Public Function getNameOfContainer() As String Implements PluginManagerLibrary.GUIControlContainer.getNameOfContainer
        Return "WindowController"
    End Function

    Public Function removeElement(ByVal con As System.Windows.Forms.Control) As Boolean Implements PluginManagerLibrary.GUIControlContainer.removeElement
        For i As Integer = 0 To controls.Count - 1
            If con Is controls(i).Con Then
                If controls(i).window.removeControl(con) = 0 Then
                    controls(i).window.Dispose()
                    windows.Remove(controls(i).window)
                End If
                controls.RemoveAt(i)
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub New()
        dieWindowAuswahl.windows = windows
    End Sub

    Private Sub controlWindowClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        If e.CloseReason = CloseReason.FormOwnerClosing Then
            'ansonsten wird das Control beim Schließen des Hauptforms entfernt und lädt beim nächsten mal nicht mehr
            Return
        End If
        Dim window As ControlForm = sender
        For Each tb As TabPage In window.TabControl1.TabPages
            For Each con As Control In tb.Controls
                con.Parent = Nothing
                For i As Integer = 0 To controls.Count - 1
                    If controls(i).Con Is con Then
                        controls.RemoveAt(i)
                        Exit For
                    End If
                Next
            Next
        Next
        windows.Remove(window)
    End Sub

    Public Property sessionState As System.Xml.XmlNode Implements PluginManagerLibrary.GUIControlContainer.sessionState
        Get
            Dim xmldoc As Xml.XmlDocument = PluginManagerLibrary.XmlFunctions.createXMLDocument
            Dim xmlRoot As Xml.XmlNode = PluginManagerLibrary.XmlFunctions.addRootNode(xmldoc, "WindowControllerSessionState")
            Dim currentNode, localNode As Xml.XmlNode

            currentNode = PluginManagerLibrary.XmlFunctions.addXmlNode(xmlRoot, "Windows")
            For Each window As ControlForm In windows
                localNode = PluginManagerLibrary.XmlFunctions.addXmlNode(currentNode, window.Name)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "Left", window.Left)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "Top", window.Top)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "Height", window.Height)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "Width", window.Width)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "WindowState", window.WindowState)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "SelectedIndex", window.TabControl1.SelectedIndex)
            Next
            currentNode = PluginManagerLibrary.XmlFunctions.addAttributeToNode(xmlRoot, "WindowCount", windows.Count)

            currentNode = PluginManagerLibrary.XmlFunctions.addXmlNode(currentNode, "Controls")

            For Each con As ControlLocation In controls
                localNode = PluginManagerLibrary.XmlFunctions.addXmlNode(currentNode, con.Con.Name)
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "Window", con.window.Name)
            Next

            Return xmlRoot
        End Get
        Set(ByVal value As System.Xml.XmlNode)

            If Not startingUp Then
                Return
            End If
            startingUp = False

            Dim currentNode As Xml.XmlNode

            currentNode = PluginManagerLibrary.XmlFunctions.getSubnodeFromNode(value, "WindowControllerSessionState")

            windowNode = PluginManagerLibrary.XmlFunctions.getSubnodeFromNode(currentNode, "Windows")

            If currentNode Is Nothing Then
                Return
            End If

            Dim windowCount As Integer = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(currentNode, "WindowCount")

            Dim controlsNode As Xml.XmlNode = PluginManagerLibrary.XmlFunctions.getSubnodeFromNode(currentNode, "Controls")

            If controlsNode.HasChildNodes Then
                For i As Integer = 0 To controlsNode.ChildNodes.Count - 1
                    controlsToCome.Add(controlsNode.ChildNodes(i).Name)
                    startupLocation.Add(controlsNode.ChildNodes(i).Name, PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(controlsNode.ChildNodes(i), "Window"))
                Next
            End If

        End Set
    End Property

    Public Sub showAllWindows()
        For Each win As ControlForm In windows
            win.Show(MainWindow)
        Next
        showWindowsOnLoad = True
    End Sub
End Class
