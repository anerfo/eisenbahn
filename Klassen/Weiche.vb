<Serializable()> Public Class Weiche
    Implements ICloneable

    Public Nummer As Integer
    Public Richtung As WeichenRichtung

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim result As New Weiche

        result.Nummer = Nummer
        result.Richtung = Richtung

        Return result
    End Function
End Class
