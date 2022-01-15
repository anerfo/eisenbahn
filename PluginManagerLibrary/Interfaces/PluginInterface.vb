''' <summary>
''' Interface, das jedes Plugin implementieren muss
''' </summary>
''' <remarks></remarks>
Public Interface PluginInterface

    ''' <summary>
    ''' Funktion die Aufgerufen wird um das Plugin zu initialisieren
    ''' </summary>
    ''' <returns>Es können beliebig viele Objekte zurückgegeben werden. Diese müssen ein Interface implementieren auf das andere Objekte zugreifen können.</returns>
    ''' <remarks></remarks>
    Function pluginInitalisieren() As Object()

    ''' <summary>
    ''' Funktion die aufgerufen wird um das Plugin zu starten
    ''' </summary>
    ''' <param name="Referenz">Es wird eine Referenz auf ein Objekt im Hauptprogramm mitgeliefert, dass die InterfacefuerPlugins-Schnittstelle implementiert</param>
    ''' <remarks></remarks>
    Sub pluginStarten(ByVal Referenz As InterfaceFuerPlugins)

    ''' <summary>
    ''' Funktion die aufgerufen wird um das Plugin zu stoppen
    ''' </summary>
    ''' <remarks></remarks>
    Sub pluginStoppen()

    ''' <summary>
    ''' Gibt den Namen des Plugins zurück
    ''' </summary>
    ''' <value>Name des Plugins als String</value>
    ''' <returns>Name des Plugins als String</returns>
    ''' <remarks></remarks>
    ReadOnly Property name() As String

    ''' <summary>
    ''' Kurze Beschreibung des Plugins
    ''' </summary>
    ''' <value>Kurze Beschreibung des Plugins</value>
    ''' <returns>Kurze Beschreibung des Plugins</returns>
    ''' <remarks></remarks>
    ReadOnly Property beschreibung() As String

End Interface