<Serializable()> Public Class Lok
    Implements ICloneable

    Public Nummer As Integer
    Public Geschwindigkeit As Integer
    Public SollGeschwindigkeit As Integer
    Public Hauptfunktion As Boolean
    Public Funktion1 As Boolean
    Public Funktion2 As Boolean
    Public Funktion3 As Boolean
    Public Funktion4 As Boolean

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim result As New Lok
        result.Nummer = Nummer
        result.Geschwindigkeit = Geschwindigkeit
        result.Hauptfunktion = Hauptfunktion
        result.Funktion1 = Funktion1
        result.Funktion2 = Funktion2
        result.Funktion3 = Funktion3
        result.Funktion4 = Funktion4

        Return result
    End Function
End Class
