Public Class CharacterEncumbrance
    Inherits SubcharacterBase
    Implements ICharacterEncumbrance

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
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
    ReadOnly Property MaximumEncumbrance As Long Implements ICharacterEncumbrance.MaximumEncumbrance
        Get
            Return If(
                Character.Statistics.GetStatistic(StatisticType.FromId(WorldData, StatisticTypeBaseLift)), 0) +
                If(
                    Character.Statistics.GetStatistic(StatisticType.FromId(WorldData, StatisticTypeBonusLift)), 0) *
                    If(
                        Character.Statistics.GetStatistic(StatisticType.FromId(WorldData, StatisticTypeStrength)), 0)
        End Get
    End Property
End Class
