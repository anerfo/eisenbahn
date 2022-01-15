Imports System.Windows.Forms
Imports System.Xml

''' <summary>
''' Interface für Container die Plugin-Steuerelemente anzeigen können
''' </summary>
''' <remarks></remarks>
Public Interface GUIControlContainer

    ''' <summary>
    ''' Funktion die ein Control hinzufügt
    ''' </summary>
    ''' <param name="con">das hinzuzufügende Control</param>
    ''' <returns>'true' wenn erfolgreich hinzugefügt</returns>
    ''' <remarks></remarks>
    Function addElement(ByVal con As Control) As Boolean

    ''' <summary>
    ''' Funktion die ein Control entfernt
    ''' </summary>
    ''' <param name="con">das zu entfernende Control</param>
    ''' <returns>'true' wenn erfolgreich entfernt</returns>
    ''' <remarks></remarks>
    Function removeElement(ByVal con As Control) As Boolean

    ''' <summary>
    ''' Name des Containers
    ''' </summary>
    ''' <returns>Name des Containers</returns>
    ''' <remarks></remarks>
    Function getNameOfContainer() As String

    Property sessionState() As Xml.XmlNode
End Interface
