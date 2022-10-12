﻿Public Class LocationShould
    Inherits ThingieShould(Of ILocation)

    Public Sub New()
        MyBase.New(AddressOf Location.FromId)
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
    Sub have_has_feature()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Feature.ReadForLocation(It.IsAny(Of Long)))
                subject.HasFeature.ShouldBeFalse
                worldData.Verify(Function(x) x.Feature.ReadForLocation(id))
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
    Sub have_friends()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Allies(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
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
    Sub has_routes_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Routes.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub has_statistics_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statistics.ShouldNotBeNull
            End Sub)
    End Sub
End Class
