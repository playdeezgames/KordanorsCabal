Public Class CharacterStatisticTypeData
    Inherits NameCacheData
    Implements ICharacterStatisticTypeData
    Friend Const TableName = "CharacterStatisticTypes"
    Friend Const CharacterStatisticTypeIdColumn = "CharacterStatisticTypeId"
    Friend Const CharacterStatisticTypeNameColumn = "CharacterStatisticTypeName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const MinimumValueColumn = "MinimumValue"
    Friend Const DefaultValueColumn = "DefaultValue"
    Friend Const MaximumValueColumn = "MaximumValue"

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadDefaultValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DefaultValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function


    Public Function ReadName(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            CharacterStatisticTypeNameColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{CharacterStatisticTypeIdColumn}],
                    [{CharacterStatisticTypeNameColumn}],
                    [{AbbreviationColumn}],
                    [{MinimumValueColumn}],
                    [{DefaultValueColumn}],
                    [{MaximumValueColumn}]) AS
                (VALUES
                    (1,'Strength','STR',0,NULL,99999),
                    (2,'Dexterity','DEX',0,NULL,99999),
                    (3,'Influence','INF',0,NULL,99999),
                    (4,'Willpower','WIL',0,NULL,99999),
                    (5,'Power','POW',0,NULL,99999),
                    (6,'HP','HP',0,NULL,99999),
                    (7,'MP','MP',0,NULL,99999),
                    (8,'Mana','Mana',0,NULL,99999),
                    (9,'Unassigned','Unassigned',0,NULL,99999),
                    (10,'Unarmed Maximum Damage','MAXDMG',0,1,99999),
                    (11,'Base Maximum Defend','MAXDEF',0,1,99999),
                    (12,'Wounds','Wounds',0,NULL,99999),
                    (13,'Stress','Stress',0,NULL,99999),
                    (14,'Money','$',0,NULL,99999),
                    (15,'Fatigue','Fatigue',0,NULL,99999),
                    (16,'XP','XP',0,NULL,99999),
                    (17,'XP Goal','XP Goal',0,NULL,99999),
                    (18,'Drunkenness','Drunkenness',0,0,99999),
                    (19,'Highness','Highness',0,NULL,99999),
                    (20,'Hunger','Hunger',0,0,100),
                    (21,'Food Poisoning','Food Poisoning',0,NULL,99999),
                    (22,'Chafing','Chafing',0,0,99999),
                    (23,'Immobilization','Immobilization',0,NULL,99999),
                    (24,'Base Lift','Base Lift',0,0,99999),
                    (25,'Bonus Lift','Bonus Lift',0,0,99999)
                )
                SELECT 
                    [{CharacterStatisticTypeIdColumn}],
                    [{CharacterStatisticTypeNameColumn}],
                    [{AbbreviationColumn}],
                    [{MinimumValueColumn}],
                    [{DefaultValueColumn}],
                    [{MaximumValueColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadMaximumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMaximumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            MaximumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadMinimumValue(statisticTypeId As Long) As Long? Implements ICharacterStatisticTypeData.ReadMinimumValue
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            MinimumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadAbbreviation(statisticTypeId As Long) As String Implements ICharacterStatisticTypeData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            AbbreviationColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
        lookUpByName = Function(name) store.Column.ReadValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            CharacterStatisticTypeIdColumn,
            (CharacterStatisticTypeNameColumn, name))
    End Sub
End Class
