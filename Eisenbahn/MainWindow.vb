Imports System.Reflection

Public Class MainWindow

    Private splashscreen As New Splashscr
    Private Plugins As New PluginManagerLibrary.PluginManager()
    Private theWindowController As New WindowController

    Private Sub MainWindow_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Plugins.stopPlugins()

        Dim sessionXML As Xml.XmlDocument
        Dim sessionNode As Xml.XmlNode

        sessionXML = PluginManagerLibrary.XmlFunctions.createXMLDocument()

        sessionNode = PluginManagerLibrary.XmlFunctions.addRootNode(sessionXML, "SessionState")
        sessionNode = PluginManagerLibrary.XmlFunctions.addXmlNode(sessionNode, "WindowState")

        PluginManagerLibrary.XmlFunctions.addAttributeToNode(sessionNode, "WindowState", Me.WindowState)
        PluginManagerLibrary.XmlFunctions.addAttributeToNode(sessionNode, "Left", Me.Left)
        PluginManagerLibrary.XmlFunctions.addAttributeToNode(sessionNode, "Height", Me.Height)
        PluginManagerLibrary.XmlFunctions.addAttributeToNode(sessionNode, "Top", Me.Top)
        PluginManagerLibrary.XmlFunctions.addAttributeToNode(sessionNode, "Width", Me.Width)

        sessionXML.Save(Klassen.Konstanten.Speicherpfad & "\mainWindowState.xml")
    End Sub

    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False
        splashscreen.progress("Lade Zustand")

        Klassen.Konstanten.Speicherpfad = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Andreas Ernst\Eisenbahn 6"

        If Not IO.Directory.Exists(Klassen.Konstanten.Speicherpfad) Then
            IO.Directory.CreateDirectory(Klassen.Konstanten.Speicherpfad)
        End If

        If IO.File.Exists(Klassen.Konstanten.Speicherpfad & "\mainWindowState.xml") Then
            Dim sessionXML As New Xml.XmlDocument
            Dim sessionNode As Xml.XmlNode

            sessionXML.Load(Klassen.Konstanten.Speicherpfad & "\mainWindowState.xml")

            sessionNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromDocument(sessionXML, "SessionState")
            sessionNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromNode(sessionNode, "WindowState")
            Me.WindowState = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(sessionNode, "WindowState")
            Me.Left = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(sessionNode, "Left")
            Me.Height = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(sessionNode, "Height")
            Me.Top = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(sessionNode, "Top")
            Me.Width = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(sessionNode, "Width")
        End If

        splashscreen.progress("Initialisiere Plugins")

        Dim GUIControls() As PluginManagerLibrary.GUIControlContainer = {EnhancedTabControl1, theWindowController}
        Dim ConfigControl() As PluginManagerLibrary.GUIControlContainer = {KonfigurationForm.EnhancedTabControl1}

        Plugins.initialize(GUIControls, ConfigControl, Klassen.Konstanten.Speicherpfad)

        System.Threading.Thread.CurrentThread.Name = "MainThread"
#If DEBUG Then
        Dim pluginPaths() As String = { _
                "D:\Programmieren\VB Projekte\EisenbahnV4\V5PluginKompaitibilitaet\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Communication\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Plugin2\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Stellwerke\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Steuerpulte\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Daten\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Stellwerk\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\KontaktSimulator\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\V5PluginLoader\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Weichensteuerung\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\KlausEBspecific\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\driversCabSim\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\EisenbahnServer\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\DMXServer\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\DMXController\bin\Debug", _
                "D:\Programmieren\VB Projekte\EisenbahnV4\Plugins\Plugin1\bin\Debug"}
#Else
        Dim pluginPaths() As String = {Application.StartupPath & "\Plugins\Controls"}
#End If
        splashscreen.progress("Lade Plugins")

        Plugins.loadPlugins(pluginPaths)

        splashscreen.progress("Starte Plugins")

        Plugins.startPlugins()

        Plugins.viewSettings()
        Plugins.controlOrganisation()

        splashscreen.progress("Plugins fertig")
        splashscreen.progress("Warte auf Eisenbahn...")
        splashscreen.progress("Weichen werden geschaltet")

        'Wartet bis im Kernel alle Befehle abgearbeitet sind. Hoffe das klappt immer!
        While Klassen.GlobaleVariablen.AnzahlBefehle > 0
            splashscreen.switches(Klassen.GlobaleVariablen.AnzahlBefehle)
        End While

        splashscreen.finish()

        splashscreen.progress("Laden beendet")

        Me.Visible = True
        theWindowController.showAllWindows()
        splashscreen.TopMost = True
    End Sub

    Private Sub KonfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KonfigurationToolStripMenuItem.Click
        KonfigurationForm.Show(Me)
    End Sub

    Private Sub BeendenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BeendenToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class