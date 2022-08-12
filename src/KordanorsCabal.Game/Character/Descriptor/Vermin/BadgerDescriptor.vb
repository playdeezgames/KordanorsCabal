﻿Friend Class BadgerDescriptor
    Inherits CharacterType

    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 1},
                    {1, 4},
                    {2, 2},
                    {6, 2},
                    {23, 0},
                    {3, 0},
                    {7, 3},
                    {13, 0},
                    {10, 2},
                    {4, 3},
                    {12, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Select Case level.Id
            Case 1
                Return location.LocationType = LocationType.FromName(DungeonDeadEnd)
            Case Else
                Return True
        End Select
    End Function

    Public Overrides Sub DropLoot(location As Location)
        If RNG.RollDice("1d3") = 1 Then
            location.Inventory.Add(Item.Create(ItemType.Mushroom))
        End If
    End Sub

    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return character.CharacterType = OldCharacterType.N00b
    End Function

    Private ReadOnly bribes As IReadOnlyList(Of ItemType) =
        New List(Of ItemType) From {ItemType.RottenEgg}

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub

    Public Overrides Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return bribes.Contains(itemType)
    End Function
End Class
