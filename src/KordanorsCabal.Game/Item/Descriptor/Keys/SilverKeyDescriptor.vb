Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "AG KEY",,
            MakeDictionary(
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
