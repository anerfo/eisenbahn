Public Class GlobaleVariablen
    Private Shared anzahl As Integer
    Public Shared Property AnzahlBefehle
        Set(ByVal value)
            anzahl = value
        End Set
        Get
            Return anzahl
        End Get
    End Property
End Class
