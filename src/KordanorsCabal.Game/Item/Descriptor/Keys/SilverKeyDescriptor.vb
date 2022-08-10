Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SilverKey,
            "AG KEY",,
            MakeDictionary(
                (2L, MakeHashSet(OldLocationType.DungeonDeadEnd))))
    End Sub
End Class
