''' <summary>
''' Zeigt eine Oberfläche an, mit der die Controls verwaltet werden können
''' </summary>
''' <remarks></remarks>
Public Class GUIControlOrgansiation
    Private iControlManager As ControlManager

    Private active As System.Windows.Forms.ListBox

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="theControlManager">Zeiger auf ein ControlManager-Objekt, wird benötigt um an Listen zu kommen</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal theControlManager As ControlManager)
        InitializeComponent()
        Me.Dock = Windows.Forms.DockStyle.Fill

        iControlManager = theControlManager
        listControls()
        listConfigs()
        listGUIControls()
        listConfigControls()
    End Sub

    Private Sub listControls()
        ListBox1.Items.Clear()
        If iControlManager.GUIPanels Is Nothing Then
            Return
        End If
        For i As Integer = 0 To iControlManager.GUIPanels.Length - 1
            ListBox1.Items.Add(iControlManager.GUIPanels(i).getNameOfContainer)
        Next
    End Sub

    Private Sub listConfigs()
        ListBox3.Items.Clear()
        If iControlManager.ConfigPanels Is Nothing Then
            Return
        End If
        For i As Integer = 0 To iControlManager.ConfigPanels.Length - 1
            ListBox3.Items.Add(iControlManager.ConfigPanels(i).getNameOfContainer)
        Next
    End Sub

    Private Sub listGUIControls()
        ListBox2.Items.Clear()
        If iControlManager.controlList.Count = 0 Then
            Return
        End If
        For i As Integer = 0 To iControlManager.controlList.Count - 1
            ListBox2.Items.Add(iControlManager.controlList(i).Name)
        Next
    End Sub

    Private Sub listConfigControls()
        ListBox4.Items.Clear()
        If iControlManager.configList.Count = 0 Then
            Return
        End If
        For i As Integer = 0 To iControlManager.configList.Count - 1
            ListBox4.Items.Add(iControlManager.configList(i).Name)
        Next
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ListBox1.SelectedIndex < 0 Then
            Return
        End If
        iControlManager.moveElement(selectCorrectControl, iControlManager.GUIPanels(ListBox1.SelectedIndex))
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ListBox3.SelectedIndex < 0 Then
            Return
        End If
        iControlManager.moveElement(selectCorrectControl, iControlManager.ConfigPanels(ListBox3.SelectedIndex))
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        iControlManager.moveElement(selectCorrectControl, Nothing)
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        active = ListBox2

        Dim guiCC As GUIControlContainer = iControlManager.locationsOfControls(iControlManager.controlList(ListBox2.SelectedIndex).Handle)

        markParent(guiCC)
    End Sub

    Private Sub ListBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox4.SelectedIndexChanged
        active = ListBox4

        Dim guiCC As GUIControlContainer = iControlManager.locationsOfControls(iControlManager.configList(ListBox4.SelectedIndex).Handle)

        markParent(guiCC)
    End Sub

    Private Sub markParent(ByVal guiCC As GUIControlContainer)

        ListBox1.SelectedIndex = -1
        ListBox3.SelectedIndex = -1

        If Not guiCC Is Nothing Then
            For i As Integer = 0 To ListBox1.Items.Count - 1
                If guiCC.getNameOfContainer = ListBox1.Items(i) Then
                    ListBox1.SelectedIndex = i
                    Exit For
                End If
            Next
            For i As Integer = 0 To ListBox3.Items.Count - 1
                If guiCC.getNameOfContainer = ListBox3.Items(i) Then
                    ListBox3.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function selectCorrectControl() As Windows.Forms.Control
        'gibt das Control der aktuell angewählten Liste zurück
        If active Is Nothing Then
            Return Nothing
        End If
        If active.SelectedIndex < 0 Then
            Return Nothing
        End If
        If active Is ListBox2 Then
            Return iControlManager.controlList.Item(active.SelectedIndex)
        Else
            Return iControlManager.configList.Item(active.SelectedIndex)
        End If
    End Function

    Public Sub store(ByVal storageLocation As String)
        iControlManager.StoreElements(storageLocation)
    End Sub

End Class
