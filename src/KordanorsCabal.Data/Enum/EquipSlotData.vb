Public Class EquipSlotData
    Inherits BaseData
    Friend Const TableName = "EquipSlots"
    Friend Const EquipSlotIdColumn = "EquipSlotId"
    Friend Const EquipSlotNameColumn = "EquipSlotName"

    Public Function ReadName(equipSlotId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            EquipSlotNameColumn,
            (EquipSlotIdColumn, equipSlotId))
    End Function

    Public Function ReadForName(equipSlotName As String) As Long?
        Return Store.ReadColumnValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            EquipSlotIdColumn,
            (EquipSlotNameColumn, equipSlotName))
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

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub
End Class
