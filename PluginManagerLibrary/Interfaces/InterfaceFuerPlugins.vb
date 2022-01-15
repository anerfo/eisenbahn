Imports System.Reflection
Imports System.Windows.Forms

''' <summary>
''' Interface, das Funktionen definiert, auf welche Plugins zugreifen können
''' </summary>
''' <remarks></remarks>
Public Interface InterfaceFuerPlugins

    ''' <summary>
    ''' Referenz auf ein Objekt holen, welches ein bestimmtes Interface implementiert
    ''' </summary>
    ''' <param name="InterfaceName">Name des gesuchten Interfaces</param>
    ''' <param name="thisPlugin">Referenz auf das Objekt, welches diese Funktion aufruft.</param>
    ''' <returns>liefert alle Service Objekte die angegebenes Interface implementieren</returns>
    ''' <remarks></remarks>
    Function getReferenceToObject(ByVal InterfaceName As String, ByVal thisPlugin As PluginInterface) As Object

    Function getReferencesToObjects(ByVal InterfaceName As String) As Object()

    ''' <summary>
    ''' ein Steuerelement auf der Programmoberfläche registrieren
    ''' </summary>
    ''' <param name="GUIControl">das anzuzeigende Steuerelement</param>
    ''' <remarks></remarks>
    Sub registerGUIControl(ByVal GUIControl As Control)

    ''' <summary>
    ''' ein Steuerelement für die Konfiguration registrieren
    ''' </summary>
    ''' <param name="ConfigControl">das anzuzeigende Steuerelement</param>
    ''' <remarks></remarks>
    Sub registerConfigControl(ByVal ConfigControl As Control)

End Interface
