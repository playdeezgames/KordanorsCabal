Public Class Character
    ReadOnly Property Id As Long
    Sub New(characterId As Long)
        Id = characterId
    End Sub
    ReadOnly Property CharacterType As CharacterType
        Get
            Return CType(CharacterData.ReadCharacterType(Id).Value, CharacterType)
        End Get
    End Property

    Friend Shared Function Create(characterType As CharacterType, location As Location) As Character
        Dim character = FromId(CharacterData.Create(characterType, location.Id))
        For Each entry In characterType.InitialStatistics
            character.SetStatistic(entry.Key, entry.Value)
        Next
        Return character
    End Function

    Friend Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
        CharacterStatisticData.Write(Id, statisticType, statisticValue)
    End Sub

    Friend Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    Property Location As Location
        Get
            Return Location.FromId(CharacterData.ReadLocation(Id).Value)
        End Get
        Set(value As Location)
            CharacterData.WriteLocation(Id, value.Id)
        End Set
    End Property
    Shared Function FromId(characterId As Long) As Character
        Return New Character(characterId)
    End Function

    Public Function GetStatistic(statisticType As CharacterStatisticType) As Long
        Return If(CharacterStatisticData.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = CharacterInventoryData.Read(Id)
            If Not inventoryId.HasValue Then
                inventoryId = InventoryData.Create()
                CharacterInventoryData.Write(Id, inventoryId.Value)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property

    ReadOnly Property IsEncumbered As Boolean
        Get
            Return Encumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property Encumbrance As Single
        Get
            Return Inventory.TotalEncumbrance
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Single
        Get
            Return CharacterType.MaximumEncumbrance(Me)
        End Get
    End Property
End Class
