Public Class ItemTypeData
    Inherits BaseData
    Friend Const TableName = "ItemTypes"
    Friend Const ItemTypeIdColumn = "ItemTypeId"
    Friend Const ItemTypeNameColumn = "ItemTypeName"
    Friend Const IsConsumedColumn = "IsConsumed"

    Public Sub New(store As Store, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{ItemTypeNameColumn}],
                    [{IsConsumedColumn}]) AS
                (VALUES
                    (1,'FE Key',0),
                    (2,'CU Key',0),
                    (3,'AG Key',0),
                    (4,'AU Key',0),
                    (5,'PT Key',0),
                    (6,'Elemental Orb',0),
                    (7,'Potion',0),
                    (8,'Goblin Ear',0),
                    (9,'Skull Fragment',0),
                    (10,'Dagger',0),
                    (11,'Earth Shard',0),
                    (12,'Water Shard',0),
                    (13,'Fire Shard',0),
                    (14,'Air Shard',0),
                    (15,'Shield',0),
                    (16,'Helmet',0),
                    (17,'Chainmail',0),
                    (18,'Shortsword',0),
                    (19,'Brodesode',0),
                    (20,'Platemail',0),
                    (21,'Rat Tail',0),
                    (22,'Holy ""Water""',0),
                    (23,'Town Portal',0),
                    (24,'Food',0),
                    (25,'Magic Egg',0),
                    (26,'Beer',0),
                    (27,'Trousers',0),
                    (28,'Pr0n Scroll',0),
                    (29,'Moon Portal',0),
                    (30,'Empty Bottle',0),
                    (31,'Book of Holy Bold',0),
                    (32,'Membership Card',0),
                    (33,'Bong',0),
                    (34,'""Herb""',0),
                    (35,'Food',0),
                    (36,'Mushroom',0),
                    (37,'Rotten Egg',0),
                    (38,'Zombie Taint',0),
                    (39,'Lotion',0),
                    (40,'Bat Wing',0),
                    (41,'Snake Fang',0),
                    (42,'Shoe Laces',0),
                    (43,'Spacesord',0),
                    (44,'Horns of Kordanor',0),
                    (45,'Amulet of HP',0),
                    (46,'Ring of HP',0),
                    (47,'Book of Purify',0),
                    (48,'Amulet of STR',0),
                    (49,'Amulet of DEX',0),
                    (50,'Amulet of POW',0),
                    (51,'Amulet of Mana',0),
                    (52,'Amulet of Yendor',0)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{ItemTypeNameColumn}],
                    [{IsConsumedColumn}]
                FROM [cte];")
    End Sub
    Public Function ReadName(itemTypeId As Long) As String
        Return Store.ReadColumnString(AddressOf Initialize, TableName, ItemTypeNameColumn, (ItemTypeIdColumn, itemTypeId))
    End Function

End Class
