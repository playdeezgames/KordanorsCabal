Friend MustInherit Class DirectionDescriptor
    MustOverride ReadOnly Property Opposite As Direction
End Class
Friend Module DirectionDescriptorUtility
    Friend DirectionDescriptors As IReadOnlyDictionary(Of Direction, DirectionDescriptor) =
        New Dictionary(Of Direction, DirectionDescriptor) From
        {
            {Direction.Down, New DownDescriptor},
            {Direction.East, New EastDescriptor},
            {Direction.Inward, New InwardDescriptor},
            {Direction.North, New NorthDescriptor},
            {Direction.Outward, New OutwardDescriptor},
            {Direction.South, New SouthDescriptor},
            {Direction.Up, New UpDescriptor},
            {Direction.West, New WestDescriptor}
        }
End Module
