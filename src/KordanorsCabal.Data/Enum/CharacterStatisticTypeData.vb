﻿Public Class CharacterStatisticTypeData
    Inherits NameCacheData
    Friend Const TableName = "CharacterStatisticTypes"
    Friend Const CharacterStatisticTypeIdColumn = "CharacterStatisticTypeId"
    Friend Const CharacterStatisticTypeNameColumn = "CharacterStatisticTypeName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const MinimumValueColumn = "MinimumValue"
    Friend Const DefaultValueColumn = "DefaultValue"
    Friend Const MaximumValueColumn = "MaximumValue"

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DefaultValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function


    Public Function ReadName(statisticTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            CharacterStatisticTypeNameColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
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
                    (18,'Drunkenness','',0,0,99999),
                    (19,'Highness','',0,NULL,99999),
                    (20,'Hunger','',0,0,100),
                    (21,'Food Poisoning','',0,NULL,99999),
                    (22,'Chafing','',0,0,99999),
                    (23,'Immobilization','',0,NULL,99999))
                SELECT 
                    [{CharacterStatisticTypeIdColumn}],
                    [{CharacterStatisticTypeNameColumn}],
                    [{AbbreviationColumn}],
                    [{MinimumValueColumn}],
                    [{DefaultValueColumn}],
                    [{MaximumValueColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadMaximumValue(statisticTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            MaximumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadMinimumValue(statisticTypeId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            MinimumValueColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Function ReadAbbreviation(statisticTypeId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            AbbreviationColumn,
            (CharacterStatisticTypeIdColumn, statisticTypeId))
    End Function

    Public Sub New(store As Store)
        MyBase.New(store)
        lookUpByName = Function(name) store.ReadColumnValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            CharacterStatisticTypeIdColumn,
            (CharacterStatisticTypeNameColumn, name))
    End Sub
End Class
