Public Interface IEventData
    Function Test(worldData As IWorldData, eventName As String, ParamArray parms As Long()) As Boolean
    Sub Perform(worldData As IWorldData, eventName As String, ParamArray parms As Long())
End Interface
