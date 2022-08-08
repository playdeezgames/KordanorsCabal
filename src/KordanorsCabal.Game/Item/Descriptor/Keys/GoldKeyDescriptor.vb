Friend Class GoldKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "AU Key",,
            MakeDictionary(
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
