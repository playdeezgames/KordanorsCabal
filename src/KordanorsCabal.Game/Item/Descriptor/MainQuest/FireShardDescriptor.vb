﻿Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonBoss}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Fire Shard"
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level3
                Return 1
            Case Else
                Return 0
        End Select
    End Function
End Class
