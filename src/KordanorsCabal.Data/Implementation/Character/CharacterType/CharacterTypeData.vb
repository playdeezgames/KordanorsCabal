Public Class CharacterTypeData
    Inherits BaseData
    Implements ICharacterTypeData
    Friend Const TableName = "CharacterTypes"
    Friend Const CharacterTypeIdColumn = "CharacterTypeId"
    Friend Const CharacterTypeNameColumn = "CharacterTypeName"
    Friend Const XPValueColumn = "XPValue"
    Friend Const MoneyDropDiceColumn = "MoneyDropDice"
    Friend Const IsUndeadColumn = "IsUndead"

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterTypeIdColumn}],
                    [{CharacterTypeNameColumn}],
                    [{XPValueColumn}],
                    [{MoneyDropDiceColumn}],
                    [{IsUndeadColumn}]) AS
                (VALUES
                        (1,'Acolyte',5,'2d8',0),
                        (2,'Badger',2,'0d1',0),
                        (3,'Bat',1,'0d1',0),
                        (4,'Bishop',5,'4d8',0),
                        (5,'Cabal Leader',5,'5d8',0),
                        (6,'Goblin',1,'2d6',0),
                        (7,'Goblin Elite',2,'3d6',0),
                        (8,'Kordanor',5,'0d1',0),
                        (9,'Malcontent',2,'3d6',0),
                        (10,'MoonPerson',3,'0d1',0),
                        (11,'N00b',0,'0d1',0),
                        (12,'Priest', 5,'3d8',0),
                        (13,'Rat',1,'0d1',0),
                        (14,'Skeleton',1,'2d3',1),
                        (15,'Snake',1,'0d1',0),
                        (16,'Zombie',1,'2d4',1)
                )
                SELECT 
                    [{CharacterTypeIdColumn}],
                    [{CharacterTypeNameColumn}],
                    [{XPValueColumn}],
                    [{MoneyDropDiceColumn}],
                    [{IsUndeadColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadIsUndead(characterTypeId As Long) As Long? Implements ICharacterTypeData.ReadIsUndead
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            IsUndeadColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadXPValue(characterTypeId As Long) As Long? Implements ICharacterTypeData.ReadXPValue
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            XPValueColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadMoneyDropDice(characterTypeId As Long) As String Implements ICharacterTypeData.ReadMoneyDropDice
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            MoneyDropDiceColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadName(characterTypeId As Long) As String Implements ICharacterTypeData.ReadName
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            CharacterTypeNameColumn,
            (CharacterTypeIdColumn, characterTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements ICharacterTypeData.ReadAll
        Return Store.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            CharacterTypeIdColumn)
    End Function
End Class
