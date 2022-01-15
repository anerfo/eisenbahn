''' <summary>
''' Ein Service Objekt, ist ein Objekt, das anderen Plugins Services anbieten kann.
''' </summary>
''' <remarks></remarks>
Public Class ServiceObject

    ''' <summary>
    ''' speichert die ServiceObjeckte, die die Plugins anbieten
    ''' </summary>
    ''' <remarks></remarks>
    Public ServiceObj As Object

    ''' <summary>
    ''' Das Plugin aus dem das ServiceObjekt stammt
    ''' </summary>
    ''' <remarks></remarks>
    Public SourcePlugin As PluginClass

End Class
