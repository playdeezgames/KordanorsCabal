Public Class LocationShould
    Inherits ThingieShould(Of ILocation)

    Public Sub New()
        MyBase.New(AddressOf Location.FromId)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationTypeId = 2L
                worldData.Setup(Function(x) x.Location.ReadLocationType(It.IsAny(Of Long))).Returns(locationTypeId)
                worldData.Setup(Function(x) x.LocationType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.Location.ReadLocationType(id))
                worldData.Verify(Function(x) x.LocationType.ReadName(locationTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_routes()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                subject.Routes(Direction.FromId(worldData.Object, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_inventory()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForLocation(It.IsAny(Of Long)))
                subject.Inventory.ShouldNotBeNull
                worldData.Verify(Function(x) x.Inventory.CreateForLocation(id))
                worldData.Verify(Function(x) x.Inventory.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub set_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticType = LocationStatisticType.DungeonRow
                Const statisticValue = 2L
                worldData.Setup(Sub(x) x.LocationStatistic.Write(It.IsAny(Of Long), It.IsAny(Of Long), It.IsAny(Of Long?)))
                subject.SetStatistic(statisticType, statisticValue)
                worldData.Verify(Sub(x) x.LocationStatistic.Write(id, statisticType, statisticValue))
            End Sub)
    End Sub
    <Fact>
    Sub get_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticType = LocationStatisticType.DungeonRow
                worldData.Setup(Function(x) x.LocationStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.GetStatistic(statisticType).ShouldBeNull
                worldData.Verify(Function(x) x.LocationStatistic.Read(id, statisticType))
            End Sub)
    End Sub
    <Fact>
    Sub have_requires_mp()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationTypeId = 2L
                worldData.Setup(Function(x) x.Location.ReadLocationType(It.IsAny(Of Long))).Returns(locationTypeId)
                worldData.Setup(Function(x) x.LocationType.ReadRequiresMP(It.IsAny(Of Long)))
                subject.RequiresMP.ShouldBeFalse
                worldData.Verify(Function(x) x.Location.ReadLocationType(id))
                worldData.Verify(Function(x) x.LocationType.ReadRequiresMP(locationTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_map()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationTypeId = 2L
                worldData.Setup(Function(x) x.Location.ReadLocationType(It.IsAny(Of Long))).Returns(locationTypeId)
                worldData.Setup(Function(x) x.LocationType.ReadCanMap(It.IsAny(Of Long)))
                subject.CanMap.ShouldBeFalse
                worldData.Verify(Function(x) x.Location.ReadLocationType(id))
                worldData.Verify(Function(x) x.LocationType.ReadCanMap(locationTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_stairs()
        WithSubject(
            Sub(worldData, id, subject)
                subject.HasStairs.ShouldBeFalse
            End Sub)
    End Sub
    <Fact>
    Sub have_is_dungeon()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationTypeId = 2L
                worldData.Setup(Function(x) x.Location.ReadLocationType(It.IsAny(Of Long))).Returns(locationTypeId)
                worldData.Setup(Function(x) x.LocationType.ReadIsDungeon(It.IsAny(Of Long)))
                subject.IsDungeon.ShouldBeFalse
                worldData.Verify(Function(x) x.Location.ReadLocationType(id))
                worldData.Verify(Function(x) x.LocationType.ReadIsDungeon(locationTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_feature()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Feature.ReadForLocation(It.IsAny(Of Long)))
                subject.HasFeature.ShouldBeFalse
                worldData.Verify(Function(x) x.Feature.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_route_count()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Route.ReadCountForLocation(It.IsAny(Of Long)))
                subject.RouteCount.ShouldBe(0)
                worldData.Verify(Function(x) x.Route.ReadCountForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_enemies()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Enemies(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_route_types()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RouteTypes.ShouldBeEmpty
            End Sub)
    End Sub
    <Fact>
    Sub destroys_route()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DestroyRoute(Direction.FromId(worldData.Object, directionid))
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
    <Fact>
    Sub have_friends()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Friends(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_enemy()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Enemy(Character.FromId(worldData.Object, characterId)).ShouldBeNull
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_route_directions()
        WithSubject(
            Sub(worldData, id, subject)
                subject.RouteDirections.ShouldBeEmpty
            End Sub)
    End Sub
    <Fact>
    Sub have_a_location_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationTypeId = 2L
                worldData.Setup(Function(x) x.Location.ReadLocationType(It.IsAny(Of Long))).Returns(locationTypeId)
                subject.LocationType.ShouldNotBeNull
                worldData.Verify(Function(x) x.Location.ReadLocationType(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_feature()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Feature.ReadForLocation(It.IsAny(Of Long)))
                subject.Feature.ShouldBeNull
                worldData.Verify(Function(x) x.Feature.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_dungeon_level()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.LocationDungeonLevel.Read(It.IsAny(Of Long)))
                subject.DungeonLevel.ShouldBeNull
                worldData.Verify(Function(x) x.LocationDungeonLevel.Read(id))
            End Sub)
    End Sub
    <Fact>
    Sub has_route()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Route.ReadForLocationDirection(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasRoute(Direction.FromId(worldData.Object, directionId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Route.ReadForLocationDirection(id, directionId))
            End Sub)
    End Sub
End Class
