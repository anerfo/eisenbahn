Imports System.Diagnostics.StackTrace
Imports System.Reflection

''' <summary>
''' Bietet Funktionen des Hauptprogramms, welche Plugins verwenden können
''' </summary>
''' <remarks></remarks>
Public Class PluginHandler
    Inherits PluginServiceObjectBaseClass
    Implements InterfaceFuerPlugins

    'Speichert Referenzen auf den Control Manager der die GUI verwaltet
    Private theControlManager As ControlManager

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="ReferenceToControlManager">Ein Steuerelement, das Steuerelemente die angezeigt werden sollen verwaltet</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef ReferenceToControlManager As ControlManager)
        theControlManager = ReferenceToControlManager
    End Sub

    ''' <summary>
    ''' sucht in den geladenen Service Objekten nach einem Interface und liefert alle Objekte, die dieses implementieren zurück
    ''' </summary>
    ''' <param name="InterfaceName">String, nach dem gesucht werden soll</param>
    ''' <param name="thisPlugin">Referenz auf das Objekt, welches diese Funktion aufruft.</param>
    ''' <returns>das zuerst gefundenen Objekte, welches das Interface implementieren</returns>
    ''' <remarks></remarks>
    Public Function getReferenceToObject(ByVal InterfaceName As String, ByVal thisPlugin As PluginInterface) As Object Implements InterfaceFuerPlugins.getReferenceToObject
        'Public Function getReferenceToObject(ByVal InterfaceName As String) As Object() Implements InterfaceFuerPlugins.getReferenceToObject
        'sucht eine Referenz auf ein Objekt in den Service Objeckten, die ein bestimmtes Interface implementiert
        If serviceObjects Is Nothing Then
            Return Nothing
        End If

        'Hier wird nach der PluginClass gesucht, welches die Referenz auf das aufrufende Plugin enthält.
        Dim originPlugin As PluginClass = Nothing

        For q As Integer = 0 To Plugin.Length - 1
            If Plugin(q).PluginReference Is thisPlugin Then
                originPlugin = Plugin(q)
                Exit For
            End If
        Next

        If originPlugin.dependencies Is Nothing Then
            ReDim originPlugin.dependencies(0)
        Else
            ReDim Preserve originPlugin.dependencies(originPlugin.dependencies.Length)
        End If
        originPlugin.dependencies(originPlugin.dependencies.Length - 1) = New List(Of ServiceObject)
        originPlugin.requestedInterfaces.Add(InterfaceName)

        Dim result() As ServiceObject = Nothing
        For Each obj As ServiceObject In serviceObjects
            If obj.SourcePlugin Is originPlugin Then
                Continue For
            End If

            Dim typ As Type = obj.ServiceObj.GetType
            'Interfaces die Objekt implementiert
            Dim ifaces() As Type = typ.GetInterfaces()
            If Not ifaces Is Nothing Then
                For i As Integer = 0 To ifaces.Length - 1
                    'Die Namen stimmen überein
                    If Strings.UCase(InterfaceName) = Strings.UCase(ifaces(i).ToString) Then
                        If result Is Nothing Then
                            ReDim result(0)
                        Else
                            ReDim Preserve result(result.Length)
                        End If
                        result(result.Length - 1) = obj '.ServiceObj

                        If Not originPlugin Is Nothing Then
                            originPlugin.dependencies(originPlugin.dependencies.Length - 1).Add(obj)
                        End If
                    End If
                Next
            End If
        Next

        If result Is Nothing Then
            MsgBox("Das Interface '" & InterfaceName & "' ist nicht vorhanden.", MsgBoxStyle.Critical, "schwerer Fehler")
            Return Nothing
        End If

        If originPlugin.objects Is Nothing Then
            ReDim originPlugin.objects(0)
        Else
            ReDim Preserve originPlugin.objects(originPlugin.objects.Length)
        End If

        originPlugin.objects(originPlugin.objects.Length - 1) = result(0)

        'Jetzt muss entschieden werden, welches Interface zurückgegeben werden soll. Muss aus XML gelesen werden
        If result.Length > 1 Then
            If System.IO.File.Exists(xmlFile) Then
                Dim fs As New Xml.XmlTextReader(xmlFile)
                fs.ReadToFollowing("Dependencies")
                fs.ReadToFollowing(HelpFunctions.elliminateIllegalChars(originPlugin.fullPath))
                Dim tp As String = fs.GetAttribute(InterfaceName & (originPlugin.objects.Length - 1).ToString)
                For i As Integer = 0 To result.Length - 1
                    If result(i).ServiceObj.GetType.ToString = tp Then

                        originPlugin.objects(originPlugin.objects.Length - 1) = result(i)
                        fs.Close()

                        ''Wenn das Plugin noch nicht gestartet wurde, wird es jetzt gestartet
                        'If originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin.state = PluginClass.StartState.stopped Then
                        '    If Not originPlugin Is originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin Then
                        '        StartPlugin(originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin, Me)
                        '    End If
                        'End If

                        If checkDependenciesForCycles() Then
                            If MsgBox("Es wurde ein Abhängigkeitskreis gefunden. Fortfahren?", MsgBoxStyle.YesNo, "Abhängigkeitskreis") = vbNo Then
                                originPlugin.objects(originPlugin.objects.Length - 1) = result(0)
                            End If
                        End If
                        'Return originPlugin.objects(originPlugin.objects.Length - 1).ServiceObj
                        Exit For
                    End If
                Next
                fs.Close()
            End If
        End If

        'Wenn das Plugin noch nicht gestartet wurde, wird es jetzt gestartet
        If originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin.state = PluginClass.StartState.stopped Then
            If Not originPlugin Is originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin Then
                StartPlugin(originPlugin.objects(originPlugin.objects.Length - 1).SourcePlugin, Me)
            End If
        End If

        'wenn keine Entscheidung getroffen wird, gib das erste Element zurück
        Return originPlugin.objects(originPlugin.objects.Length - 1).ServiceObj
    End Function


    ''' <summary>
    ''' sucht in den geladenen Service Objekten nach einem Interface und liefert alle Objekte, die dieses implementieren zurück
    ''' </summary>
    ''' <param name="InterfaceName">String, nach dem gesucht werden soll</param>
    ''' <returns>das zuerst gefundenen Objekte, welches das Interface implementieren</returns>
    ''' <remarks></remarks>
    Public Function getReferencesToObjects(ByVal InterfaceName As String) As Object() Implements InterfaceFuerPlugins.getReferencesToObjects
        'Public Function getReferenceToObject(ByVal InterfaceName As String) As Object() Implements InterfaceFuerPlugins.getReferenceToObject
        'sucht eine Referenz auf ein Objekt in den Service Objeckten, die ein bestimmtes Interface implementiert
        If serviceObjects Is Nothing Then
            Return Nothing
        End If

        Dim result() As Object = Nothing
        For Each obj As ServiceObject In serviceObjects

            Dim typ As Type = obj.ServiceObj.GetType
            'Interfaces die Objekt implementiert
            Dim ifaces() As Type = typ.GetInterfaces()
            If Not ifaces Is Nothing Then
                For i As Integer = 0 To ifaces.Length - 1
                    'Die Namen stimmen überein
                    If Strings.UCase(InterfaceName) = Strings.UCase(ifaces(i).ToString) Then
                        If result Is Nothing Then
                            ReDim result(0)
                        Else
                            ReDim Preserve result(result.Length)
                        End If
                        result(result.Length - 1) = obj.ServiceObj
                    End If
                Next
            End If
        Next

        If result Is Nothing Then
            MsgBox("Das Interface '" & InterfaceName & "' ist nicht vorhanden.", MsgBoxStyle.Critical, "schwerer Fehler")
            Return Nothing
        End If

        Return result
    End Function

    ''' <summary>
    ''' ein Steuerelement registrieren das auf der Programmoberfläche angezeigt werden soll
    ''' </summary>
    ''' <param name="ConfigControl">anzuzeigendes Steuerelement</param>
    ''' <remarks></remarks>
    Public Sub registerConfigControl(ByVal ConfigControl As System.Windows.Forms.Control) Implements InterfaceFuerPlugins.registerConfigControl
        theControlManager.addConfigControl(ConfigControl)
    End Sub

    ''' <summary>
    ''' ein Steuerelement registrieren das zur Konfiguration angezeigt werden soll
    ''' </summary>
    ''' <param name="GUIControl">anzuzeigendes Steuerelement</param>
    ''' <remarks></remarks>
    Public Sub registerGUIControl(ByVal GUIControl As System.Windows.Forms.Control) Implements InterfaceFuerPlugins.registerGUIControl
        theControlManager.addControl(GUIControl)
    End Sub

    ''' <summary>
    ''' prüft ob ein Plugin von einem zweiten Plugin abhängig ist, das von dem Plugin ursprünglichen Plugin abhängt
    ''' </summary>
    ''' <returns>ob es ein Abhängigkeitskreis gibt</returns>
    ''' <remarks>gibt noch bei unkritische Abhängigkeitskreise wahr zurück</remarks>
    Public Function checkDependenciesForCycles() As Boolean
        For i As Integer = 0 To serviceObjects.Length - 1
            Dim dependencies As New List(Of ServiceObject)
            If foundDependencycycle(dependencies, serviceObjects(i)) Then
                'MsgBox("Found")
                Return True
            End If
        Next
    End Function

    ''' <summary>
    ''' iterativ aufgerufene Funktion zum Prüfen auf Abhängigkeitskreise
    ''' </summary>
    ''' <param name="Dependencies">Liste der geprüften Abhängigkeiten</param>
    ''' <param name="currentServiceObject">aktuell geprüftes ServiceObjekt</param>
    ''' <returns>'wahr' Wenn ein Abhängigkeitskreis entdeckt wurde</returns>
    ''' <remarks></remarks>
    Private Function foundDependencycycle(ByVal Dependencies As List(Of ServiceObject), ByVal currentServiceObject As ServiceObject) As Boolean
        If currentServiceObject.SourcePlugin.dependencies Is Nothing Then
            Return False
        End If
        If currentServiceObject.SourcePlugin.dependencies.Count = 0 Then
            Return False
        End If

        For i As Integer = 0 To Dependencies.Count - 1
            If (currentServiceObject Is Dependencies(i)) Then
                Return True
            End If
        Next

        Dependencies.Add(currentServiceObject)

        If currentServiceObject.SourcePlugin.objects Is Nothing Then
            Return False
        End If

        For i As Integer = 0 To currentServiceObject.SourcePlugin.objects.Count - 1
            If foundDependencycycle(Dependencies, currentServiceObject.SourcePlugin.objects(i)) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
