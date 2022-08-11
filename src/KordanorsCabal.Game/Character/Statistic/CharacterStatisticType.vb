Imports System.Runtime.CompilerServices

Public Class CharacterStatisticType
    ReadOnly Property Id As Long
    Sub New(characterStatisticTypeId As Long)
        Id = characterStatisticTypeId
    End Sub
    Private Sub New(characterStatisticTypeName As String)
        Me.New(StaticWorldData.World.CharacterStatisticType.ReadForName(characterStatisticTypeName).Value)
    End Sub
    Public Shared Function FromName(characterStatisticTypeName As String) As CharacterStatisticType
        Return New CharacterStatisticType(characterStatisticTypeName)
    End Function
    ReadOnly Property DefaultValue As Long?
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadDefaultValue(Id)
        End Get
    End Property

    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MinimumValue() As Long
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadMinimumValue(Id).Value
        End Get
    End Property

    ReadOnly Property MaximumValue As Long
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadMaximumValue(Id).Value
        End Get
    End Property

    ReadOnly Property Abbreviation As String
        Get
            Return StaticWorldData.World.CharacterStatisticType.ReadAbbreviation(Id)
        End Get
    End Property
End Class
Public Module CharacterStatisticTypeUtility
    <Extension>
    Public Function ToOld(characterStatisticType As CharacterStatisticType) As OldCharacterStatisticType
        Return CType(characterStatisticType.Id, OldCharacterStatisticType)
    End Function
    <Extension>
    Public Function ToNew(oldCharacterStatisticType As OldCharacterStatisticType) As CharacterStatisticType
        Return New CharacterStatisticType(oldCharacterStatisticType)
    End Function
    Friend ReadOnly CharacterStatisticTypeDescriptors As IReadOnlyDictionary(Of OldCharacterStatisticType, CharacterStatisticType) =
        New Dictionary(Of OldCharacterStatisticType, CharacterStatisticType) From
        {
            {OldCharacterStatisticType.BaseMaximumDefend, New CharacterStatisticType(OldCharacterStatisticType.BaseMaximumDefend)},
            {OldCharacterStatisticType.Chafing, New CharacterStatisticType(OldCharacterStatisticType.Chafing)},
            {OldCharacterStatisticType.Dexterity, New CharacterStatisticType(OldCharacterStatisticType.Dexterity)},
            {OldCharacterStatisticType.Drunkenness, New CharacterStatisticType(OldCharacterStatisticType.Drunkenness)},
            {OldCharacterStatisticType.Fatigue, New CharacterStatisticType(OldCharacterStatisticType.Fatigue)},
            {OldCharacterStatisticType.FoodPoisoning, New CharacterStatisticType(OldCharacterStatisticType.FoodPoisoning)},
            {OldCharacterStatisticType.Highness, New CharacterStatisticType(OldCharacterStatisticType.Highness)},
            {OldCharacterStatisticType.HP, New CharacterStatisticType(OldCharacterStatisticType.HP)},
            {OldCharacterStatisticType.Hunger, New CharacterStatisticType(OldCharacterStatisticType.Hunger)},
            {OldCharacterStatisticType.Immobilization, New CharacterStatisticType(OldCharacterStatisticType.Immobilization)},
            {OldCharacterStatisticType.Influence, New CharacterStatisticType(OldCharacterStatisticType.Influence)},
            {OldCharacterStatisticType.Mana, New CharacterStatisticType(OldCharacterStatisticType.Mana)},
            {OldCharacterStatisticType.Money, New CharacterStatisticType(OldCharacterStatisticType.Money)},
            {OldCharacterStatisticType.MP, New CharacterStatisticType(OldCharacterStatisticType.MP)},
            {OldCharacterStatisticType.Power, New CharacterStatisticType(OldCharacterStatisticType.Power)},
            {OldCharacterStatisticType.Strength, New CharacterStatisticType(OldCharacterStatisticType.Strength)},
            {OldCharacterStatisticType.Stress, New CharacterStatisticType(OldCharacterStatisticType.Stress)},
            {OldCharacterStatisticType.UnarmedMaximumDamage, New CharacterStatisticType(OldCharacterStatisticType.UnarmedMaximumDamage)},
            {OldCharacterStatisticType.Unassigned, New CharacterStatisticType(OldCharacterStatisticType.Unassigned)},
            {OldCharacterStatisticType.Willpower, New CharacterStatisticType(OldCharacterStatisticType.Willpower)},
            {OldCharacterStatisticType.Wounds, New CharacterStatisticType(OldCharacterStatisticType.Wounds)},
            {OldCharacterStatisticType.XP, New CharacterStatisticType(OldCharacterStatisticType.XP)},
            {OldCharacterStatisticType.XPGoal, New CharacterStatisticType(OldCharacterStatisticType.XPGoal)}
        }
End Module
