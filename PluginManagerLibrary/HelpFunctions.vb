''' <summary>
''' Bietet grundlegende Funktionen an
''' </summary>
''' <remarks></remarks>
Public Class HelpFunctions

    ''' <summary>
    ''' Entfernt bestimmte Charaktere aus einem String
    ''' </summary>
    ''' <param name="text">Text aus dem illegale Zeichen entfernt werden sollen</param>
    ''' <returns>Text ohne illegale Zeichen</returns>
    ''' <remarks>Illegale Zeichen: ' ','!','@','#','$','%','^','*','(',')','{','}','[',']','"','"','_','+','?','/','\','.',':','MAL','größer','kleiner'</remarks>
    Public Shared Function elliminateIllegalChars(ByVal text As String) As String
        Dim illegalChars As Char() = " !@#$%^&*(){}[]""_+<>?/\.:".ToCharArray()
        Dim str As String = text
        For Each ch In illegalChars
            str = Strings.Replace(str, ch, "")
        Next
        Return str
    End Function

End Class
