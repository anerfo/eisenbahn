''' <summary>
''' Base-Klasse für alle Klassen, die mit Service-Objekten arbeiten
''' </summary>
''' <remarks></remarks>
Public MustInherit Class PluginServiceObjectBaseClass
    'speichert die ServiceObjeckte, die die Plugins anbieten
    Protected Shared serviceObjects() As ServiceObject

    'speichert Instanzen der Plugins
    Protected Shared Plugin() As PluginClass

    'Speichert die Reihenfolge in denen die Plugins gestartet wurden (werden rückwärts beendet)
    Protected Shared StartList As New List(Of PluginClass)

    'Speichert die Reihenfolge in denen die Plugins gestartet wurden (werden rückwärts beendet)
    Protected Shared xmlFile As String

    'speichert den Ort, wo die XML-Dateien gespeichert werden sollen
    Protected Shared xmlFileLocation As String

    'Startet ein bestimmtes Plugin
    Protected Shared Sub StartPlugin(ByRef pluginToStart As PluginClass, ByRef thePluginHandler As PluginHandler)
        If pluginToStart.state = PluginClass.StartState.stopped Then
            Try
                pluginToStart.state = PluginClass.StartState.starting
                pluginToStart.PluginReference.pluginStarten(thePluginHandler)
                pluginToStart.state = PluginClass.StartState.started
                StartList.Add(pluginToStart)
            Catch
                pluginToStart.state = PluginClass.StartState.failure
                MsgBox("Fehler beim Starten eines Plugins '" & pluginToStart.PluginReference.name & "' (" & Err.Source & " line: " & Err.Erl & "): " & vbNewLine & Err.Description)
            End Try
        End If
    End Sub
End Class
