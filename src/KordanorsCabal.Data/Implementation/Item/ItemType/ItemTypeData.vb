Public Class ItemTypeData
    Inherits BaseData
    Implements IItemTypeData
    Friend Const TableName = "ItemTypes"
    Friend Const ItemTypeIdColumn = "ItemTypeId"
    Friend Const ItemTypeNameColumn = "ItemTypeName"
    Friend Const IsConsumedColumn = "IsConsumed"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadIsConsumed(itemTypeId As Long) As Long? Implements IItemTypeData.ReadIsConsumed
        Return Store.Column.ReadColumnValue(Of Long, Long)(AddressOf Initialize, TableName, IsConsumedColumn, (ItemTypeIdColumn, itemTypeId))
    End Function

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{ItemTypeNameColumn}],
                    [{IsConsumedColumn}]) AS
                (VALUES
                    (1,'FE Key',1),
                    (2,'CU Key',1),
                    (3,'AG Key',1),
                    (4,'AU Key',1),
                    (5,'PT Key',1),
                    (6,'Elemental Orb',0),
                    (7,'Potion',1),
                    (8,'Goblin Ear',1),
                    (9,'Skull Fragment',1),
                    (10,'Dagger',1),
                    (11,'Earth Shard',0),
                    (12,'Water Shard',0),
                    (13,'Fire Shard',0),
                    (14,'Air Shard',0),
                    (15,'Shield',1),
                    (16,'Helmet',1),
                    (17,'Chainmail',1),
                    (18,'Shortsword',1),
                    (19,'Brodesode',1),
                    (20,'Platemail',1),
                    (21,'Rat Tail',1),
                    (22,'Holy ""Water""',1),
                    (23,'Town Portal',1),
                    (24,'Food',1),
                    (25,'Magic Egg',1),
                    (26,'Beer',1),
                    (27,'Trousers',1),
                    (28,'Pr0n Scroll',1),
                    (29,'Moon Portal',1),
                    (30,'Empty Bottle',1),
                    (31,'Book of Holy Bolt',1),
                    (32,'Membership Card',1),
                    (33,'Bong',1),
                    (34,'""Herb""',1),
                    (35,'Food',1),
                    (36,'Mushroom',1),
                    (37,'Rotten Egg',1),
                    (38,'Zombie Taint',1),
                    (39,'Lotion',1),
                    (40,'Bat Wing',1),
                    (41,'Snake Fang',1),
                    (42,'Shoe Laces',1),
                    (43,'Spacesord',1),
                    (44,'Horns of Kordanor',1),
                    (45,'Amulet of HP',1),
                    (46,'Ring of HP',1),
                    (47,'Book of Purify',1),
                    (48,'Amulet of STR',1),
                    (49,'Amulet of DEX',1),
                    (50,'Amulet of POW',1),
                    (51,'Amulet of Mana',1),
                    (52,'Amulet of Yendor',1)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{ItemTypeNameColumn}],
                    [{IsConsumedColumn}]
                FROM [cte];")
    End Sub
    Public Function ReadName(itemTypeId As Long) As String Implements IItemTypeData.ReadName
        Return Store.Column.ReadColumnString(
            AddressOf Initialize,
            TableName,
            ItemTypeNameColumn,
            (ItemTypeIdColumn, itemTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements IItemTypeData.ReadAll
        Return Store.Record.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            ItemTypeIdColumn)
    End Function
End Class
