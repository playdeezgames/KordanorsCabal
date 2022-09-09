﻿Public Class LocationDungeonLevelDataTests
    Inherits WorldDataSubobjectTests(Of ILocationDungeonLevelData)
    Sub New()
        MyBase.New(Function(x) x.LocationDungeonLevel)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheDungeonLevelOfAGivenLocation()
        WithSubobject(
            Sub(store, subject)
                Dim locationId = 1L
                subject.Read(locationId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.LocationDungeonLevels,
                    Columns.DungeonLevelIdColumn,
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheLocationsWithinAGivenDungeonLevel()
        WithSubobject(
            Sub(store, subject)
                Dim dungeonLevel = 1L
                subject.ReadForDungeonLevel(dungeonLevel).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.LocationDungeonLevels,
                    Columns.LocationIdColumn,
                    (Columns.DungeonLevelIdColumn, dungeonLevel)))
            End Sub)
    End Sub
End Class
