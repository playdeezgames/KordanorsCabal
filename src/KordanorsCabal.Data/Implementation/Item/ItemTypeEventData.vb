Public Class ItemTypeEventData
    Inherits BaseData
    Implements IItemTypeEventData
    Friend Const TableName = "ItemTypeEvents"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const EventIdColumn = "EventId"
    Friend Const EventNameColumn = "EventName"

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{EventIdColumn}],
                    [{EventNameColumn}]) AS
                (VALUES
                    (7,3,'DrinkPotion'),
                    (12,3,'UseWaterShard'),
                    (11,3,'UseEarthShard'),
                    (13,3,'UseFireShard'),
                    (14,3,'UseAirShard'),
                    (22,3,'UseHolyWater'),
                    (23,3,'UseTownPortal'),
                    (24,3,'EatFood'),
                    (25,3,'UseMagicEgg'),
                    (26,3,'UseBeer'),
                    (28,3,'UsePr0n'),
                    (29,3,'UseMoonPortal'),
                    (30,3,'UseBottle'),
                    (31,3,'LearnHolyBolt'),
                    (34,3,'UseHerb'),
                    (35,3,'UseRottenFood'),
                    (37,3,'UseRottenEgg'),
                    (47,3,'LearnPurify')
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{EventIdColumn}],
                    [{EventNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(itemTypeId As Long, eventId As Long) As String Implements IItemTypeEventData.Read
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            EventNameColumn,
            (ItemTypeIdColumn, itemTypeId),
            (EventIdColumn, eventId))
    End Function
End Class
