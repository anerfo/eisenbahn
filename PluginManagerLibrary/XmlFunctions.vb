''' <summary>
''' Vereinfacht die Nutzung von Xml-Dateien und stellt Funktionen bereit
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class XmlFunctions
    ''' <summary>
    ''' Erstellt ein neues XML-Dokument
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function createXMLDocument() As Xml.XmlDocument
        Dim xmlDoc As New Xml.XmlDocument
        Dim xmlDeclaration As Xml.XmlDeclaration

        xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-16", Nothing)
        xmlDoc.AppendChild(xmlDeclaration)
        Return xmlDoc
    End Function

    ''' <summary>
    ''' Fügt dem XML-Dokument einen Rootnode hinzu
    ''' </summary>
    ''' <param name="xmlDoc">Dokument bei dem der Rootnode eingefügt werden soll</param>
    ''' <param name="name">Name des Rootnodes</param>
    ''' <returns>den Rootnode</returns>
    ''' <remarks></remarks>
    Public Shared Function addRootNode(ByRef xmlDoc As Xml.XmlDocument, ByVal name As String) As Xml.XmlNode
        Dim xmlroot As Xml.XmlNode = xmlDoc.CreateElement(name)
        If xmlDoc.ChildNodes.Count > 1 Then
            Return xmlDoc.ChildNodes(1)
        End If
        xmlDoc.AppendChild(xmlroot)
        Return xmlroot
    End Function

    ''' <summary>
    ''' Fügt einem Node einen neuen Node hinzu
    ''' </summary>
    ''' <param name="xmlNode">Node, dem der Node hinzugefügt werden soll</param>
    ''' <param name="name">Name des Nodes</param>
    ''' <returns>den neu erstellten Node</returns>
    ''' <remarks></remarks>
    Public Shared Function addXmlNode(ByRef xmlNode As Xml.XmlNode, ByVal name As String) As Xml.XmlNode
        Dim xmlnode2 As Xml.XmlNode = xmlNode.OwnerDocument.CreateElement(name)
        xmlNode.AppendChild(xmlnode2)
        Return xmlnode2
    End Function

    ''' <summary>
    ''' Kopiert alle Nodes eines Xml-Dokumentes unter einen Node eines anderen Dokumentes
    ''' </summary>
    ''' <param name="DocumentToCopy">Dokument aus dem die Nodes kopiert werden sollen</param>
    ''' <param name="xmlNode">Node unter welchem die Nodes des Dokumentes hinzugefügt werden sollen</param>
    ''' <returns>den Node unter dem die Nodes hinzuefügt wurden</returns>
    ''' <remarks></remarks>
    Public Shared Function copyXmlDocumentNodesToXmlNode(ByVal DocumentToCopy As Xml.XmlDocument, ByRef xmlNode As Xml.XmlNode) As Xml.XmlNode
        Dim xmldoc As Xml.XmlDocument = xmlNode.OwnerDocument
        Dim xmlNode2 As Xml.XmlNode
        Dim nodelist As Xml.XmlNodeList

        nodelist = DocumentToCopy.ChildNodes
        For Each node As Xml.XmlNode In nodelist
            If Not TypeOf node Is Xml.XmlDeclaration Then
                xmlNode2 = xmldoc.ImportNode(node, True)
                xmlNode.AppendChild(xmlNode2)
            End If
        Next
        Return xmlNode
    End Function

    ''' <summary>
    ''' Fügt einem Node ein Attribut hinzu
    ''' </summary>
    ''' <param name="Node">Node dem ein Attribut hinzugefügt werden soll</param>
    ''' <param name="AttributeName">Name des Attributes</param>
    ''' <param name="Value">Wert des Attributes</param>
    ''' <returns>Node dem das Attribut hinzugefügt wurde</returns>
    ''' <remarks></remarks>
    Public Shared Function addAttributeToNode(ByRef Node As Xml.XmlNode, ByVal AttributeName As String, ByVal Value As String)
        Dim xmldoc As Xml.XmlDocument = Node.OwnerDocument
        Dim xmlattr As Xml.XmlAttribute = xmldoc.CreateAttribute(AttributeName)
        xmlattr.Value = Value
        Node.Attributes.Append(xmlattr)
        Return Node
    End Function

    ''' <summary>
    ''' gibt den Wert des Attributes zurück
    ''' </summary>
    ''' <param name="Node">Node von dem der Wert geholt werden soll</param>
    ''' <param name="name">Name des Attributes</param>
    ''' <returns>Wert des gesuchten Attributes</returns>
    ''' <remarks></remarks>
    Public Shared Function getValueOfAttribute(ByVal Node As Xml.XmlNode, ByVal name As String) As String
        If Node Is Nothing Then
            Return ""
        End If
        Return Node.Attributes(name).Value
    End Function

    ''' <summary>
    ''' Datenstruktur die saveDataInXml übergeben wird
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure parameterStuct
        ''' <summary>
        ''' Name des RootNodes
        ''' </summary>
        ''' <remarks></remarks>
        Public NodeName As String

        ''' <summary>
        ''' Namen der zu speichernden Parameter
        ''' </summary>
        ''' <remarks></remarks>
        Public parameterName() As String

        ''' <summary>
        ''' Werte der zu speichernden Parameter
        ''' </summary>
        ''' <remarks></remarks>
        Public parameterValue() As String
    End Structure

    ''' <summary>
    ''' Funktion die Dokument erzeugt und übergebene Daten in die XML schreibt
    ''' </summary>
    ''' <param name="rootNodeName">Name des Root Nodes</param>
    ''' <param name="parameters">ParameterStuct, die die zu speichernden Daten enthält</param>
    ''' <returns>Xml-Dokument</returns>
    ''' <remarks></remarks>
    Public Shared Function saveDataInXml(ByVal rootNodeName As String, ByVal parameters() As parameterStuct) As Xml.XmlDocument
        Dim xmldoc As Xml.XmlDocument = createXMLDocument()
        Dim xmlroot As Xml.XmlNode = addRootNode(xmldoc, rootNodeName)
        Dim xmlnode As Xml.XmlNode
        For i As Integer = 0 To parameters.Length - 1
            If parameters(i).NodeName Is Nothing Then
                Continue For
            End If
            xmlnode = addXmlNode(xmlroot, parameters(i).NodeName)
            For q As Integer = 0 To Math.Min(parameters(i).parameterName.Length, parameters(i).parameterValue.Length) - 1
                xmlnode = addAttributeToNode(xmlnode, parameters(i).parameterName(q), parameters(i).parameterValue(q))
            Next
        Next
        Return xmldoc
    End Function

    ''' <summary>
    ''' Erstellt ein Parameterfeld mit der angegebenen Länge
    ''' </summary>
    ''' <param name="parameters">leeres ParameterStruct-feld </param>
    ''' <param name="length">Dimension des Parameterfeldes</param>
    ''' <returns>Prameterfeld</returns>
    ''' <remarks></remarks>
    Public Shared Function createParameters(ByRef parameters() As parameterStuct, ByVal length As Integer) As parameterStuct()
        ReDim parameters(length)
        For i As Integer = 0 To length - 1
            parameters(i) = New parameterStuct
        Next
        Return parameters
    End Function

    ''' <summary>
    ''' Kopiert eine Liste von XmlNodes unter einen Node in einem anderen Dokument
    ''' </summary>
    ''' <param name="xmlNodesToCopy">Nodes die kopiert werden sollen</param>
    ''' <param name="xmlNode">Node unter den die Nodes kopiert werden sollen</param>
    ''' <returns>den Node unter dem die Nodes hinzugefügt wurden</returns>
    ''' <remarks></remarks>
    Public Shared Function copyXmlNodesToXmlNode(ByVal xmlNodesToCopy As Xml.XmlNodeList, ByRef xmlNode As Xml.XmlNode) As Xml.XmlNode
        If xmlNodesToCopy Is Nothing Then
            Return xmlNode
        End If
        Dim xmldoc As Xml.XmlDocument = xmlNode.OwnerDocument
        Dim xmlNode2 As Xml.XmlNode

        For Each node As Xml.XmlNode In xmlNodesToCopy
            xmlNode2 = xmldoc.ImportNode(node, True)
            xmlNode.AppendChild(xmlNode2)
        Next
        Return xmlNode
    End Function

    ''' <summary>
    ''' Kopiert einen XmlNode unter einen Node in einem anderen Dokument
    ''' </summary>
    ''' <param name="xmlNodeToCopy">Node der kopiert werden soll</param>
    ''' <param name="xmlNode">Node unter den der Node kopiert werden sollen</param>
    ''' <returns>den Node unter dem der Node hinzugefügt wurde</returns>
    ''' <remarks></remarks>
    Public Shared Function copyXmlNodeToXmlNode(ByVal xmlNodeToCopy As Xml.XmlNode, ByRef xmlNode As Xml.XmlNode) As Xml.XmlNode
        If xmlNodeToCopy Is Nothing Then
            Return xmlNode
        End If
        Dim xmldoc As Xml.XmlDocument = xmlNode.OwnerDocument
        Dim xmlNode2 As Xml.XmlNode

        xmlNode2 = xmldoc.ImportNode(xmlNodeToCopy, True)
        xmlNode.AppendChild(xmlNode2)
        Return xmlNode
    End Function

    ''' <summary>
    ''' gibt ein Node zurück
    ''' </summary>
    ''' <param name="xmlDoc">Dokument in dem gesucht werden soll</param>
    ''' <param name="nodeName">Name des Elements</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getChildNodeFromDocument(ByVal xmlDoc As Xml.XmlDocument, ByVal nodeName As String) As Xml.XmlNode
        Dim nodelist As Xml.XmlNodeList = xmlDoc.ChildNodes
        For Each node As Xml.XmlNode In nodelist
            If node.Name = nodeName Then
                Return node
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' gibt ein Childnode zurück
    ''' </summary>
    ''' <param name="xmlNode">Node aus dem gelesen werden soll</param>
    ''' <param name="nodeName">Name des Nodes</param>
    ''' <returns>den Node wenn er gefunden wurde</returns>
    ''' <remarks></remarks>
    Public Shared Function getChildNodeFromNode(ByVal xmlNode As Xml.XmlNode, ByVal nodeName As String) As Xml.XmlNode
        If xmlNode Is Nothing Then
            Return Nothing
        End If
        For Each node As Xml.XmlNode In xmlNode.ChildNodes
            If node.Name = nodeName Then
                Return node
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Gibt den Wert eines Attributes zurück
    ''' </summary>
    ''' <param name="xmlNode">Node in dem gesucht werden soll</param>
    ''' <param name="AttributName">Name des Attributes</param>
    ''' <returns>Wert des Attributes</returns>
    ''' <remarks></remarks>
    Public Shared Function getAttributValueFromNode(ByVal xmlNode As Xml.XmlNode, ByVal AttributName As String) As String
        Return xmlNode.Attributes(AttributName)?.Value
    End Function

    ''' <summary>
    ''' gibt ein Subnode zurück
    ''' </summary>
    ''' <param name="xmlNode">Node in dem gesucht werden soll</param>
    ''' <param name="nodeName">Name des Nodes. Die Nodes die dazwischen liegen müssen durch '/' oder '\' getrennt sein</param>
    ''' <returns>den gesuchten Node</returns>
    ''' <remarks></remarks>
    Public Shared Function getSubnodeFromNode(ByVal xmlNode As Xml.XmlNode, ByVal nodeName As String) As Xml.XmlNode
        Dim result As Xml.XmlNode = Nothing
        Dim pos As Integer = getMinPosition(nodeName)
        While pos
            xmlNode = getChildNodeFromNode(xmlNode, Left(nodeName, pos - 1))

            nodeName = Right(nodeName, nodeName.Length - pos)

            pos = getMinPosition(nodeName)
        End While
        result = getChildNodeFromNode(xmlNode, nodeName)
        Return result
    End Function

    ''' <summary>
    ''' Hilfsfunktion um Position des ersten '/' oder '\' zu finden, das nicht null ist
    ''' </summary>
    ''' <param name="text">Text in dem gesucht werden soll</param>
    ''' <returns>Position des ersten '/' oder '\' oder '0' wenn nichts gefunden wird</returns>
    ''' <remarks></remarks>
    Private Shared Function getMinPosition(ByVal text As String) As Integer
        Dim pos1 As Integer = InStr(text, "/")
        Dim pos2 As Integer = InStr(text, "\")
        If pos1 <> 0 And pos2 <> 0 Then
            Return Math.Min(pos1, pos2)
        Else
            If pos1 = 0 Then
                Return pos2
            Else
                Return pos1
            End If
        End If
    End Function
End Class
