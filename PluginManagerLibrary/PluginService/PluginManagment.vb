Imports System.IO
Imports System.Reflection

''' <summary>
''' Klasse die Plugins lädt, startet, stoppt und die Service Objekte von ihnen holt
''' </summary>
''' <remarks></remarks>
Public Class PluginManagment
    Inherits PluginServiceObjectBaseClass

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize(ByVal StorageLocation As String)
        If StorageLocation = "" Then
            Return
        End If
        xmlFile = StorageLocation
        If Strings.Right(xmlFile, 1) <> "\" Then
            xmlFile &= "\"
        End If
        xmlFile &= "pmlDependencies.xml"
    End Sub

    ''' <summary>
    ''' Lädt Plugins von den angegebenen Pfaden und liest die Service Objekte aus
    ''' </summary>
    ''' <param name="pluginPaths">Pfade der Plugins</param>
    ''' <remarks></remarks>
    Public Sub Load(ByVal pluginPaths() As String)
        If pluginPaths Is Nothing Then
            Exit Sub
        End If
        'Laden der Plugins
        Dim plugins() As AvailablePlugin = Nothing
        Dim pluginsInPath() As AvailablePlugin
        Dim countPlugins As Integer = 0


        For i As Integer = 0 To pluginPaths.Length - 1
            pluginsInPath = PluginServices.FindPlugins(pluginPaths(i), "PluginManagerLibrary.PluginInterface")
            If pluginsInPath Is Nothing Then
                Continue For
            End If

            For q As Integer = 0 To pluginsInPath.Length - 1
                If plugins Is Nothing Then
                    ReDim plugins(countPlugins)
                Else
                    ReDim Preserve plugins(countPlugins)
                End If
                plugins(countPlugins) = pluginsInPath(q)
                countPlugins += 1
            Next
        Next

        If Not plugins Is Nothing Then
            ReDim Plugin(plugins.Length - 1)

            Dim i As Integer = 0
            While i < Plugin.Length
                'Instanziieren des Plugins
                Try
                    Plugin(i) = New PluginClass

                    Plugin(i).PluginReference = PluginServices.CreateInstance(plugins(i))
                    Plugin(i).fullPath = plugins(i).AssemblyPath
                    i += 1
                Catch ex As Exception
                    ReDim Preserve Plugin(Plugin.Length - 2)
                End Try
            End While
            For i = 0 To Plugin.Length - 1
                Dim obj() As Object = Nothing

                Try
                    'Initialisieren der Plugins
                    obj = Plugin(i).PluginReference.pluginInitalisieren()
                Catch ex As Exception
                    MsgBox("Fehler bei der Initialisierung des Plugins '" & Plugin(i).PluginReference.name & "': " & vbNewLine & Err.Description)
                End Try

                addServiceObject(obj, Plugin(i).PluginReference)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Startet die Plugins
    ''' </summary>
    ''' <param name="thePluginHandler">Referenz auf ein Objekt, das die InterfaceFuerPlugins-Schnittstelle implementiert</param>
    ''' <remarks></remarks>
    Public Sub PluginsStarten(ByRef thePluginHandler As PluginHandler)
        'startet die Plugins und übergibt eine Referenz auf sich um den Plugins zu ermöglich andere Interfaces zu nutzen
        If Not Plugin Is Nothing Then
            For i As Integer = 0 To Plugin.Length - 1
                StartPlugin(Plugin(i), thePluginHandler)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Hält die Ausführung der Plugins an
    ''' </summary>
    ''' <remarks>beendet die Plugins anderst herum wie sie gestartet wurden (wegen Abhängigkeiten)</remarks>
    Public Sub PluginsStoppen()
        If Not Plugin Is Nothing Then
            storeDependencies()
            For i As Integer = StartList.Count - 1 To 0 Step -1
                Try
                    StartList.Item(i).PluginReference.pluginStoppen()
                Catch ex As Exception
                    MsgBox("Fehler beim Stoppen eines Plugins '" & StartList.Item(i).PluginReference.name & "': " & vbNewLine & Err.Description)
                End Try
            Next
        End If
    End Sub

    ''' <summary>
    ''' Gibt zurück ob ein bestimmtes Interface implementiert ist
    ''' </summary>
    ''' <param name="obj">Objekt, welches durchsucht wird</param>
    ''' <param name="InterfaceName">Interface, welches gesucht wird</param>
    ''' <returns>true wenn Interface implementiert ist, false wenn nicht</returns>
    ''' <remarks></remarks>
    Private Function hasInterface(ByVal obj As Object, ByVal InterfaceName As String) As Boolean
        'findet heraus ob ein Objekt ein bestimmtes Interface implementiert
        If obj Is Nothing Then
            Return False
        End If
        Dim typ As Type = obj.GetType
        'Interfaces die Objekt implementiert
        Dim ifaces() As Type = typ.GetInterfaces()
        If ifaces Is Nothing Then
            Return False
        End If
        For i As Integer = 0 To ifaces.Length - 1
            'Die Namen stimmen überein
            If Strings.UCase(InterfaceName) = Strings.UCase(ifaces(i).ToString) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub addServiceObject(ByVal obj As Object, ByVal localPlugin As PluginInterface)
        'Service Objekte speichern
        If Not obj Is Nothing Then
            For q As Integer = 0 To obj.Length - 1
                Dim ifaces() As Type
                Try
                    ifaces = obj(q).GetType.GetInterfaces
                Catch ex As Exception
                    Continue For
                End Try
                If hasInterface(obj(q), "PluginManagerLibrary.PluginInterface") And ifaces.Length >= 2 Or _
                    Not hasInterface(obj(q), "PluginManagerLibrary.PluginInterface") And ifaces.Length >= 1 Then
                    If obj(q).GetType.GetInterfaces.Length > 0 Then

                        If serviceObjects Is Nothing Then
                            ReDim serviceObjects(0)
                        Else
                            ReDim Preserve serviceObjects(serviceObjects.Length)
                        End If
                        'speichern des Service Objektes und des beinhaltenden Plugins
                        serviceObjects(serviceObjects.Length - 1) = New ServiceObject
                        serviceObjects(serviceObjects.Length - 1).ServiceObj = obj(q)
                        For i As Integer = 0 To Plugin.Length - 1
                            If Plugin(i).PluginReference Is localPlugin Then
                                serviceObjects(serviceObjects.Length - 1).SourcePlugin = Plugin(i)
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub storeDependencies()
        Dim enc As New System.Text.UnicodeEncoding
        Dim fs As New Xml.XmlTextWriter(xmlFile, enc)

        fs.Formatting = Xml.Formatting.Indented
        fs.Indentation = 4

        fs.WriteStartDocument()
        fs.WriteStartElement("Dependencies")

        For i As Integer = 0 To Plugin.Length - 1
            If Not Plugin(i).objects Is Nothing Then

                fs.WriteStartElement(HelpFunctions.elliminateIllegalChars(Plugin(i).fullPath))
                For q As Integer = 0 To Plugin(i).objects.Length - 1
                    Dim tp As Type = Plugin(i).objects(q).ServiceObj.GetType
                    fs.WriteAttributeString(Plugin(i).requestedInterfaces(q) & q, tp.ToString)

                Next
                fs.WriteEndElement() 'Plugin(i)
            End If
        Next

        fs.WriteEndElement() 'Dependencies
        fs.WriteEndDocument()
        fs.Close()
    End Sub
End Class
