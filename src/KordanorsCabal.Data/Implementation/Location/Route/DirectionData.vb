Public Class DirectionData
    Inherits BaseData
    Implements IDirectionData
    Friend Const TableName = "Directions"
    Friend Const DirectionIdColumn = "DirectionId"
    Friend Const DirectionNameColumn = "DirectionName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const IsCardinalColumn = "IsCardinal"
    Friend Const PreviousDirectionIdColumn = "PreviousDirectionId"
    Friend Const OppositeDirectionIdColumn = "OppositeDirectionId"
    Friend Const NextDirectionIdColumn = "NextDirectionId"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(directionId As Long) As String Implements IDirectionData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            DirectionNameColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadAbbreviation(directionId As Long) As String Implements IDirectionData.ReadAbbreviation
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            AbbreviationColumn,
            (DirectionIdColumn, directionId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements IDirectionData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            TableName,
            DirectionIdColumn)
    End Function

    Public Function ReadIsCardinal(directionId As Long) As Boolean Implements IDirectionData.ReadIsCardinal
        Return If(Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            IsCardinalColumn,
            (DirectionIdColumn, directionId)), 0) > 0
    End Function
    Public Function ReadOpposite(directionId As Long) As Long? Implements IDirectionData.ReadOpposite
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            OppositeDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadNext(directionId As Long) As Long? Implements IDirectionData.ReadNext
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            NextDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadPrevious(directionId As Long) As Long? Implements IDirectionData.ReadPrevious
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            PreviousDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
End Class
