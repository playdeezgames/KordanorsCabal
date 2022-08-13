Friend Class MalcontentDescriptor
    Inherits CharacterType
    Public Overrides ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of Long, Long)
        Get
            Return New Dictionary(Of Long, Long) From
                {
                    {11, 2},
                    {1, 6},
                    {2, 4},
                    {6, 2},
                    {23, 0},
                    {3, 1},
                    {7, 1},
                    {13, 0},
                    {10, 3},
                    {4, 2},
                    {12, 0}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumEncumbrance(character As Character) As Long
        Get
            Return 0
        End Get
    End Property


    Public Overrides Function IsEnemy(character As Character) As Boolean
        Return True
    End Function

    Private ReadOnly foodTable As IReadOnlyDictionary(Of ItemType, Integer) =
        New Dictionary(Of ItemType, Integer) From
        {
            {ItemType.None, 2},
            {ItemType.Food, 1},
            {ItemType.RottenFood, 2}
        }

    Public Overrides Sub DropLoot(location As Location)
        location.Inventory.Add(Item.Create(ItemType.MembershipCard))
        Dim foodType = RNG.FromGenerator(foodTable)
        If foodType <> ItemType.None Then
            location.Inventory.Add(Item.Create(foodType))
        End If
    End Sub

    Public Overrides Function CanSpawn(location As Location, level As DungeonLevel) As Boolean
        Return level.Id <> 1 OrElse location.LocationType = LocationType.FromName(DungeonDeadEnd)
    End Function

    Public Sub New(characterTypeId As Long)
        MyBase.New(characterTypeId)
    End Sub
End Class
