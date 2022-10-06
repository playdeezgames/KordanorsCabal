Public Interface IEventData
    Function Check(worldData As IWorldData, checkType As String, id As Long) As Boolean
    Sub Act(worldData As IWorldData, actionType As String, id As Long)
End Interface
