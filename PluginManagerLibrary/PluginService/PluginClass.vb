Public Class PluginClass
    'speichert Instanzen der Plugins
    Public PluginReference As PluginInterface

    Public Enum StartState
        stopped
        starting
        started
        failure
    End Enum

    'speichert ob das Plugin gestartet wurde
    Public state As StartState = StartState.stopped

    Public fullPath As String

    'speichert die Plugins von denen diese Plugin abhängt
    Public dependencies() As List(Of ServiceObject)

    'Speichert alle angefragten Interfaces
    Public requestedInterfaces As New List(Of String)

    'Speichert die Objekte, auf welche das Objekt zugreift
    Public objects() As ServiceObject
End Class
