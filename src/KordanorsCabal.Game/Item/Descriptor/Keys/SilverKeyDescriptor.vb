Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "AG KEY",,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
