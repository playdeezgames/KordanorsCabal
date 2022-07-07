﻿Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Shield"
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As Long) As Long
        Select Case level
            Case 1
                Return RNG.RollDice("3d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlot As EquipSlot?
        Get
            Return Game.EquipSlot.Shield
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 5.0!
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 10
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 2
        End Get
    End Property
End Class
