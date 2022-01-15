Imports System.Windows.Forms

''' <summary>
''' Verwaltet das Laden, starten, anzeigen und stoppen von Plugins
''' </summary>
''' <remarks>Klasse die zum Verwenden von Plugins instanziiert werden muss</remarks>
Public Class PluginManager
    Inherits PluginServiceObjectBaseClass

    Implements InterfaceFuerPlugins

    'handelt die Anzeige der Steuerelement die von den Plugins geladen werden
    Private theControlManager As New ControlManager()

    'behandlet die Aufrufe die die Plugins machen können (getReference, registerXControl)
    Private thePluginHandler As New PluginHandler(theControlManager)

    'handelt das Laden, starten und stoppen der Plugins
    Private thePluginManagement As New PluginManagment()

    'stellt Informationen über die Plugins dar
    Private thePluginInformation As PluginInformation

    'stellt ein Control zur Verfügung, welches die Anzeige der Controls verwaltet
    Private theControlOrganisation As GUIControlOrgansiation

    ''' <summary>
    ''' Initialisiert den Plugin Manager. Muss nach dem laden aller benötigten Steuerelemente geschehen.
    ''' </summary>
    ''' <param name="GUIControls">Controls, die zum Anzeigen von Plugin-Steuerelementen verwendet werden können</param>
    ''' <param name="ConfigControls">Controls, die zum Anzeigen von Configurations-Steuerelementen verwendet werden können</param>
    ''' <remarks></remarks>
    Public Sub initialize(ByVal GUIControls() As GUIControlContainer, ByVal ConfigControls() As GUIControlContainer, ByVal StorageLocation As String)
        theControlManager.setControls(GUIControls, ConfigControls)
        thePluginManagement.Initialize(StorageLocation)
        xmlFileLocation = StorageLocation
    End Sub

    ''' <summary>
    ''' Lädt Plugins
    ''' </summary>
    ''' <param name="pluginPaths">Pfade von denen die Plugins geladen werden sollen</param>
    ''' <remarks></remarks>
    Public Sub loadPlugins(ByVal pluginPaths() As String)
        thePluginManagement.Load(pluginPaths)
    End Sub

    ''' <summary>
    ''' startet die Plugins
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub startPlugins()
        thePluginManagement.PluginsStarten(thePluginHandler)
    End Sub

    ''' <summary>
    ''' stoppt die Ausführung der Plugins
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub stopPlugins()
        If Not theControlOrganisation Is Nothing Then
            theControlOrganisation.store(xmlFileLocation)
        End If
        thePluginManagement.PluginsStoppen()
    End Sub

    ''' <summary>
    ''' das Hauptprogramm kann ein Service Objekt anmelden, dass genauso behandelt wird wie Steuerelemente der Plugins.
    ''' </summary>
    ''' <param name="obj">obj muss mindestens ein Interface implementieren!</param>
    ''' <remarks>kann z.B. für Tastatureingaben genutzt werden</remarks>
    Public Sub addServiceObject(ByVal obj As Object)
        thePluginManagement.addServiceObject(obj, Nothing)
    End Sub

    ''' <summary>
    ''' das Hauptprogramm kann die Referenz auf ein ServiceObjekt bekommen
    ''' </summary>
    ''' <remarks>wird delegiert an thePluginHandler</remarks>
    Public Function getReferenceToObject(ByVal InterfaceName As String, ByVal thisPlugin As PluginInterface) As Object Implements InterfaceFuerPlugins.getReferenceToObject
        Return thePluginHandler.getReferenceToObject(InterfaceName, thisPlugin)
    End Function

    ''' <summary>
    ''' das Hauptprogramm kann ein Konfigurations-Steuerelement anmelden, dass genauso behandelt wird wie Steuerelemente der Plugins
    ''' </summary>
    ''' <remarks>kann z.B. für Tastatureingaben genutzt werden</remarks>
    Public Sub registerConfigControl(ByVal ConfigControl As System.Windows.Forms.Control) Implements InterfaceFuerPlugins.registerConfigControl
        thePluginHandler.registerConfigControl(ConfigControl)
    End Sub

    ''' <summary>
    ''' das Hauptprogramm kann ein Steuerelement anmelden, dass genauso behandelt wird wie Steuerelemente der Plugins
    ''' </summary>
    ''' <remarks>kann z.B. für Tastatureingaben genutzt werden</remarks>
    Public Sub registerGUIControl(ByVal GUIControl As System.Windows.Forms.Control) Implements InterfaceFuerPlugins.registerGUIControl
        thePluginHandler.registerGUIControl(GUIControl)
    End Sub

    ''' <summary>
    ''' Fügt einem KonfigurationsControl ein Control hinzu, welches Informationen über die Plugins liefert
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub viewSettings()
        If thePluginInformation Is Nothing Then
            thePluginInformation = New PluginInformation(Plugin, serviceObjects)
            registerConfigControl(thePluginInformation)
        End If
    End Sub

    ''' <summary>
    ''' Fügt einem KonfigurationsControl ein Control hinzu, welches Informationen über die Plugins liefert
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub controlOrganisation()
        If theControlOrganisation Is Nothing Then
            theControlOrganisation = New GUIControlOrgansiation(theControlManager)
            registerConfigControl(theControlOrganisation)
        End If
    End Sub

    ''' <summary>
    ''' returns the Reference to the PluginHandler
    ''' </summary>
    ''' <returns>thePluginHandler</returns>
    ''' <remarks></remarks>
    Public Function getPluginHandler() As InterfaceFuerPlugins
        Return thePluginHandler
    End Function

    ''' <summary>
    ''' returns all plugins that implement the passed interface
    ''' </summary>
    ''' <param name="InterfaceName">name of interface to search for</param>
    ''' <returns>all plugins that implement the passed interface</returns>
    ''' <remarks></remarks>
    Public Function getReferencesToObjects(ByVal InterfaceName As String) As Object() Implements InterfaceFuerPlugins.getReferencesToObjects
        Return thePluginHandler.getReferencesToObjects(InterfaceName)
    End Function
End Class
