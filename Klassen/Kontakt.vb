<Serializable()> Public Class Kontakt
    Implements ICloneable

    Public Modul As Integer
    Public Adresse As Integer

    Public status As Boolean

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim result As New Kontakt

        result.Modul = Modul
        result.Adresse = Adresse
        result.status = status

        Return result
    End Function

    Public Function IsActive(ByVal kontaktModul As Integer, ByVal kontaktAdresse As Integer) As Boolean
        If kontaktModul = Modul And kontaktAdresse = Adresse Then
            Return status
        End If
        Return False
    End Function

    Public Function IsInactive(ByVal kontaktModul As Integer, ByVal kontaktAdresse As Integer) As Boolean
        Return (IsActive(kontaktModul, kontaktAdresse) = False)
    End Function
End Class
