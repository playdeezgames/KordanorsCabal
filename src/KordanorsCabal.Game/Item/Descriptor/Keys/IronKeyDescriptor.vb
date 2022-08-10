Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.IronKey,
            "FE Key",,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))))
    End Sub
End Class
