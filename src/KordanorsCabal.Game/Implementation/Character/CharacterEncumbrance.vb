Public Class CharacterEncumbrance
    Inherits BaseThingie
    Implements ICharacterEncumbrance
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterEncumbrance
        Return If(character IsNot Nothing, New CharacterEncumbrance(worldData, character), Nothing)
    End Function
    ReadOnly Property IsEncumbered As Boolean Implements ICharacterEncumbrance.IsEncumbered
        Get
            Return CurrentEncumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property CurrentEncumbrance As Long Implements ICharacterEncumbrance.CurrentEncumbrance
        Get
            Dim result = character.Items.Inventory.TotalEncumbrance
            For Each item In EquippedItems
                result += item.ItemType.Encumbrance
            Next
            Return result
        End Get
    End Property
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Long Implements ICharacterEncumbrance.MaximumEncumbrance
        Get
            Return If(
            character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.BaseLift)), 0) +
            If(
                character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.BonusLift)), 0) *
            If(
                character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Strength)), 0)
        End Get
    End Property
End Class
