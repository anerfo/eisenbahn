Imports System.Windows.Forms
Imports System.Xml

''' <summary>
''' platziert Elemente auf einem Form
''' </summary>
''' <remarks></remarks>
Public Class ControlManager
    Inherits PluginServiceObjectBaseClass

    Protected Friend GUIPanels() As GUIControlContainer
    Protected Friend ConfigPanels() As GUIControlContainer

    Protected Friend controlList As New List(Of Control)
    Protected Friend configList As New List(Of Control)

    Protected Friend locationsOfControls As New Hashtable

    Friend GUIControlOrgansiation

    Private xmlFileName As String = "\controls.xml"

    ''' <summary>
    ''' Controls auf denen Steuerelemente angezeigt werden können anmelden
    ''' </summary>
    ''' <param name="UserInterfacePanels">Steuerelemente für die Anzeige von normalen Steuerelementen</param>
    ''' <param name="ConfigurationPanels">Steuerelemente für die Anzeige von Konfigurationselementen</param>
    ''' <remarks></remarks>
    Public Sub setControls(ByVal UserInterfacePanels() As GUIControlContainer, ByVal ConfigurationPanels() As GUIControlContainer)
        If UserInterfacePanels Is Nothing Then
            MsgBox("Error: GUI kann nicht erstellt werden!")
        Else
            If UserInterfacePanels.Length = 0 Then
                MsgBox("Error: GUI kann nicht erstellt werden!")
            End If
        End If
        If ConfigurationPanels Is Nothing Then
            MsgBox("Error: Konfigurationselemente können nicht geladen werden!")
        Else
            If UserInterfacePanels.Length = 0 Then
                MsgBox("Error: Konfigurationselemente können nicht geladen werden!")
            End If
        End If
        GUIPanels = UserInterfacePanels
        ConfigPanels = ConfigurationPanels
    End Sub

    ''' <summary>
    ''' Ein Konfigurations-Steuerelement zur Verwaltung hinzufügen
    ''' </summary>
    ''' <param name="Element">das hinzuzufügende Steuerelement</param>
    ''' <remarks></remarks>
    Public Sub addConfigControl(ByVal Element As System.Windows.Forms.Control)
        If Element Is Nothing Then
            Return
        End If
        ConfigPanels(0).addElement(Element)
        configList.Add(Element)
        locationsOfControls.Add(Element.Handle, ConfigPanels(0))
    End Sub

    ''' <summary>
    ''' Ein Steuerelement zur Programmoberfläche hinzufügen
    ''' </summary>
    ''' <param name="Element">das hinzuzufügende Steuerelement</param>
    ''' <remarks></remarks>
    Public Sub addControl(ByVal Element As System.Windows.Forms.Control)
        If Element Is Nothing Then
            Return
        End If

        'Falls mehrere Elemente mit dem selben Namen erscheinen
        Dim count As Integer = 0
        For Each con As Control In controlList
            If con.Name = Element.Name Then
                count += 1
            End If
        Next
        'wird noch die Nummer angehängt
        If count > 0 Then
            Element.Name &= count
        End If

        Dim panel As GUIControlContainer = GUIPanels(0)
        Dim xmlDoc As New XmlDocument
        Dim xmlRootNode As XmlNode

        controlList.Add(Element)

        If IO.File.Exists(xmlFileLocation & xmlFileName) Then
            xmlDoc.Load(xmlFileLocation & xmlFileName)
            xmlRootNode = XmlFunctions.getChildNodeFromDocument(xmlDoc, "Controls")

            panel = choosePanel(Element, GUIPanels, xmlRootNode)

            If panel Is Nothing Then
                Return
            End If

            panel.sessionState = XmlFunctions.getSubnodeFromNode(xmlRootNode, "GUIControls/" & panel.getNameOfContainer)
        End If
        panel.addElement(Element)
        locationsOfControls.Add(Element.Handle, panel)
    End Sub

    ''' <summary>
    ''' Verschiebt ein Steuerelement auf ein anderes Parent Control
    ''' </summary>
    ''' <param name="Element">das zu verschiebende Element</param>
    ''' <param name="destination">das Ziel, auf dem das Element angezeigt werden soll</param>
    ''' <remarks></remarks>
    Public Sub moveElement(ByVal Element As System.Windows.Forms.Control, ByVal destination As GUIControlContainer)
        'Wenn kein Element angegeben ist kann keines entfernt werden
        If Element Is Nothing Then
            Return
        End If
        'Wenn das Element schon angezeigt wird, muss es erst entfernt werden
        If Not locationsOfControls.Item(Element.Handle) Is Nothing Then
            CType(locationsOfControls.Item(Element.Handle), GUIControlContainer).removeElement(Element)
        End If
        'Element aus Liste löschen
        locationsOfControls.Remove(Element.Handle)
        'Wenn Element zu neuem Contol hinzugefügt werden soll -> hinzufügen
        If destination Is Nothing Then
            Return
        End If
        'Element hinzufügen
        destination.addElement(Element)
        'In Liste eintragen
        locationsOfControls.Add(Element.Handle, destination)
    End Sub

    ''' <summary>
    ''' speichert die locations der Elemente
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StoreElements(ByVal storageLocation As String)

        Dim xmlDoc As XmlDocument = XmlFunctions.createXMLDocument
        Dim rootNode As XmlNode
        Dim currentNode As XmlNode

        rootNode = XmlFunctions.addRootNode(xmlDoc, "Controls")
        currentNode = XmlFunctions.addXmlNode(rootNode, "ControlManager")

        For Each con As Control In controlList
            Dim localNode As XmlNode
            localNode = XmlFunctions.addXmlNode(currentNode, con.Name)
            If con.Parent Is Nothing Or locationsOfControls(con.Handle) Is Nothing Then
                XmlFunctions.addAttributeToNode(localNode, "Location", "none")
            Else
                XmlFunctions.addAttributeToNode(localNode, "Location", CType(locationsOfControls(con.Handle), GUIControlContainer).getNameOfContainer)
            End If
        Next

        For Each con As Control In configList
            Dim localNode As XmlNode
            localNode = XmlFunctions.addXmlNode(currentNode, con.Name)
            XmlFunctions.addAttributeToNode(localNode, "Location", CType(locationsOfControls(con.Handle), GUIControlContainer).getNameOfContainer)
        Next

        currentNode = XmlFunctions.addXmlNode(rootNode, "GUIControls")
        For Each GUIPanel In GUIPanels
            XmlFunctions.copyXmlNodeToXmlNode(GUIPanel.sessionState, _
                                              XmlFunctions.addXmlNode(currentNode, GUIPanel.getNameOfContainer))
        Next

        currentNode = XmlFunctions.addXmlNode(rootNode, "ConfigControls")
        For Each ConfigPanel In ConfigPanels
            XmlFunctions.copyXmlNodeToXmlNode(ConfigPanel.sessionState, _
                                              XmlFunctions.addXmlNode(currentNode, ConfigPanel.getNameOfContainer))
        Next

        xmlDoc.Save(storageLocation & xmlFileName)
    End Sub

    ''' <summary>
    ''' wählt den GUIContainer in den das Control übergeben werden soll entsprechend der XML
    ''' </summary>
    ''' <param name="con">Control, das hinzugefügt werden soll</param>
    ''' <param name="panels">Panel-Array aus dem ein Container ausgewählt werden soll</param>
    ''' <returns>den Container zu dem das Control hinzugefügt weredn soll</returns>
    ''' <remarks></remarks>
    Private Function choosePanel(ByVal con As Control, ByVal panels() As GUIControlContainer, ByVal nodeWithControlInfo As XmlNode) As GUIControlContainer
        Dim result As GUIControlContainer = panels(0)
        Dim currentNode As XmlNode
        Dim eTCName As String

        currentNode = XmlFunctions.getChildNodeFromNode(nodeWithControlInfo, "ControlManager")
        currentNode = XmlFunctions.getChildNodeFromNode(currentNode, con.Name)
        eTCName = XmlFunctions.getValueOfAttribute(currentNode, "Location")

        If eTCName <> "" Then
            For i As Integer = 0 To panels.Length - 1
                If eTCName = panels(i).getNameOfContainer Then
                    result = panels(i)
                End If
            Next
        End If

        If eTCName = "none" Then
            Return Nothing
        End If

        Return result
    End Function
End Class
