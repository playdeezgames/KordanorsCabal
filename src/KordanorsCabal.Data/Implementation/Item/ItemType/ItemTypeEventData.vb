Public Class ItemTypeEventData
    Inherits BaseData
    Implements IItemTypeEventData
    Friend Const TableName = "ItemTypeEvents"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const EventIdColumn = "EventId"
    Friend Const EventNameColumn = "EventName"

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{EventIdColumn}],
                    [{EventNameColumn}]) AS
                (VALUES
                    (7,2,'AlwaysTrue'),
                    (7,3,'DrinkPotion'),
                    (11,2,'CanUseEarthShard'),
                    (11,3,'UseEarthShard'),
                    (12,2,'CanUseWaterShard'),
                    (12,3,'UseWaterShard'),
                    (13,2,'CanUseFireShard'),
                    (13,3,'UseFireShard'),
                    (14,2,'CanUseAirShard'),
                    (14,3,'UseAirShard'),
                    (22,2,'IsFightingUndead'),
                    (22,3,'UseHolyWater'),
                    (23,2,'IsInDungeon'),
                    (23,3,'UseTownPortal'),
                    (24,2,'AlwaysTrue'),
                    (24,3,'EatFood'),
                    (25,2,'AlwaysTrue'),
                    (25,3,'UseMagicEgg'),
                    (26,2,'CanUseBeer'),
                    (26,3,'UseBeer'),
                    (28,2,'CanUsePr0n'),
                    (28,3,'UsePr0n'),
                    (29,2,'IsInDungeon'),
                    (29,3,'UseMoonPortal'),
                    (30,2,'CanUseBottle'),
                    (30,3,'UseBottle'),
                    (31,2,'CanLearnHolyBolt'),
                    (31,3,'LearnHolyBolt'),
                    (34,2,'HasBong'),
                    (34,3,'UseHerb'),
                    (35,1,'PurifyFood'),
                    (35,2,'AlwaysTrue'),
                    (35,3,'UseRottenFood'),
                    (37,2,'CanUseRottenEgg'),
                    (37,3,'UseRottenEgg'),
                    (47,2,'CanLearnPurify'),
                    (47,3,'LearnPurify')
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{EventIdColumn}],
                    [{EventNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, eventId As Long) As String Implements IItemTypeEventData.Read
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            EventNameColumn,
            (ItemTypeIdColumn, itemTypeId),
            (EventIdColumn, eventId))
    End Function
End Class
