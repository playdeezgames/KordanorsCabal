Public Class EquipSlotData
    Inherits BaseData
    Implements IEquipSlotData
    Friend Const TableName = "EquipSlots"
    Friend Const EquipSlotIdColumn = "EquipSlotId"
    Friend Const EquipSlotNameColumn = "EquipSlotName"

    Public Function ReadName(equipSlotId As Long) As String Implements IEquipSlotData.ReadName
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            EquipSlotNameColumn,
            (EquipSlotIdColumn, equipSlotId))
    End Function
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{EquipSlotIdColumn}],
                    [{EquipSlotNameColumn}]) AS
                (VALUES
                    (1,'Weapon'),
                    (2,'Shield' ),
                    (3,'Head'),
                    (4,'Torso' ),
                    (5,'Legs'),
                    (6,'Neck'),
                    (7,'LHand'),
                    (8,'RHand'))
                SELECT 
                    [{EquipSlotIdColumn}],
                    [{EquipSlotNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub
End Class
