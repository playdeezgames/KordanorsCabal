Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SilverKey,
            "AG KEY",,
            MakeDictionary(
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
