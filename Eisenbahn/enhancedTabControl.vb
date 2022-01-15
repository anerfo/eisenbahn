Public Class enhancedTabControl
    Inherits TabControl
    Implements PluginManagerLibrary.GUIControlContainer

    Private selectedTabName As String

    Public Function addElement(ByVal con As System.Windows.Forms.Control) As Boolean Implements PluginManagerLibrary.GUIControlContainer.addElement
        TabPages.Add(con.Name, con.Name)
        TabPages(con.Name).Controls.Add(con)
        If con.Name = selectedTabName Then
            SelectedIndex = TabPages.Count - 1
        End If
        Return True
    End Function

    Public Function removeElement(ByVal con As System.Windows.Forms.Control) As Boolean Implements PluginManagerLibrary.GUIControlContainer.removeElement
        TabPages.Remove(TabPages(con.Name))
        Return True
    End Function

    Public Function getNameOfContainer() As String Implements PluginManagerLibrary.GUIControlContainer.getNameOfContainer
        Return Me.Parent.Name & "." & Me.Name
    End Function

    Public Property sessionState As System.Xml.XmlNode Implements PluginManagerLibrary.GUIControlContainer.sessionState
        Get
            Dim xmldoc As Xml.XmlDocument = PluginManagerLibrary.XmlFunctions.createXMLDocument
            Dim xmlRoot As Xml.XmlNode = PluginManagerLibrary.XmlFunctions.addRootNode(xmldoc, Me.Parent.Name & "." & Me.Name & "State")
            Dim localNode As Xml.XmlNode

            localNode = PluginManagerLibrary.XmlFunctions.addXmlNode(xmlRoot, "TabControl")
            If (Not SelectedTab Is Nothing) Then
                PluginManagerLibrary.XmlFunctions.addAttributeToNode(localNode, "SelectedName", SelectedTab.Name)
            End If


            Return xmlRoot
        End Get
        Set(ByVal value As System.Xml.XmlNode)
            Dim localNode As Xml.XmlNode
            If Not value Is Nothing Then
                localNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromNode(value, Me.Parent.Name & "." & Me.Name & "State")
                localNode = PluginManagerLibrary.XmlFunctions.getChildNodeFromNode(localNode, "TabControl")
                If Not localNode Is Nothing Then
                    selectedTabName = PluginManagerLibrary.XmlFunctions.getAttributValueFromNode(localNode, "SelectedName")
                End If
            End If
        End Set
    End Property
End Class
