﻿Public Class ItemTypeSpawnLocationTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeSpawnLocationTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeSpawnLocationType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAListOfLocationTypesSuitableForSpawningAGivenItemTypeOnAGivenDungeonLevel()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeId = 1L
                Dim dungeonLevelId = 2L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll(itemTypeId, dungeonLevelId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.WithValues(Of Long, Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypeSpawnLocationTypes,
                    Columns.LocationTypeIdColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class
