﻿Public Class LocationTypeDataTests
    Inherits WorldDataSubobjectTests(Of ILocationTypeData)
    Sub New()
        MyBase.New(Function(x) x.LocationType)
    End Sub

    <Fact>
    Sub location_types_have_names_fetched_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const locationTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(locationTypeId).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadString(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.LocationTypes,
                                 Columns.LocationTypeNameColumn,
                                 (Columns.LocationTypeIdColumn, locationTypeId)))
            End Sub)
    End Sub

    <Fact>
    Sub location_types_have_can_map_flag_fetched_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const locationTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadCanMap(locationTypeId).ShouldBeFalse
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.LocationTypes,
                                 Columns.CanMapColumn,
                                 (Columns.LocationTypeIdColumn, locationTypeId)))
            End Sub)
    End Sub

    <Fact>
    Sub location_types_have_is_dungeon_flag_fetched_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const locationTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadIsDungeon(locationTypeId).ShouldBeFalse
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.LocationTypes,
                                 Columns.IsDungeonColumn,
                                 (Columns.LocationTypeIdColumn, locationTypeId)))
            End Sub)
    End Sub

    <Fact>
    Sub location_types_have_requires_mp_flag_fetched_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const locationTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadRequiresMP(locationTypeId).ShouldBeFalse
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.LocationTypes,
                                 Columns.RequiresMPColumn,
                                 (Columns.LocationTypeIdColumn, locationTypeId)))
            End Sub)
    End Sub
End Class
