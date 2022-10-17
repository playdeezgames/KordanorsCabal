Public Class CharacterTypeData
    Inherits BaseData
    Implements ICharacterTypeData
    Friend Const CharacterTypeIdColumn = "CharacterTypeId"
    Friend Const CharacterTypeNameColumn = "CharacterTypeName"
    Friend Const XPValueColumn = "XPValue"
    Friend Const MoneyDropDiceColumn = "MoneyDropDice"
    Friend Const IsUndeadColumn = "IsUndead"

    Public Function ReadIsUndead(characterTypeId As Long) As Long? Implements ICharacterTypeData.ReadIsUndead
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterTypes,
            IsUndeadColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadXPValue(characterTypeId As Long) As Long? Implements ICharacterTypeData.ReadXPValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            CharacterTypes,
            XPValueColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadMoneyDropDice(characterTypeId As Long) As String Implements ICharacterTypeData.ReadMoneyDropDice
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            CharacterTypes,
            MoneyDropDiceColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadName(characterTypeId As Long) As String Implements ICharacterTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            CharacterTypes,
            CharacterTypeNameColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements ICharacterTypeData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            CharacterTypes,
            CharacterTypeIdColumn)
    End Function
End Class
