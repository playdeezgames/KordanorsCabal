Public Class ItemTypeUIDescriptor
    Sub New(pattern As Pattern?, xy As (Integer, Integer)?, hue As Hue?)
        DisplayPattern = pattern
        DisplayXY = xy
        DisplayHue = hue
    End Sub
    ReadOnly Property DisplayPattern As Pattern?
    ReadOnly Property DisplayXY As (Integer, Integer)?
    Overridable ReadOnly Property DisplayHue As Hue?
End Class
Module ItemTypeUIDescriptorUtility
    Friend ReadOnly ItemTypeUIDescriptors As IReadOnlyDictionary(Of Long, ItemTypeUIDescriptor) =
        New Dictionary(Of Long, ItemTypeUIDescriptor) From
        {
            {53L, New ItemTypeUIDescriptor(Pattern.OpenParenthesis, (10, 15), Hue.Cyan)},
            {14L, New ItemTypeUIDescriptor(Pattern.A, (10, 15), Hue.Cyan)},
            {45L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Red)},
            {48L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Blue)},
            {49L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Green)},
            {50L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Purple)},
            {51L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Orange)},
            {52L, New ItemTypeUIDescriptor(Pattern.Y, (8, 17), Hue.Orange)},
            {40L, New ItemTypeUIDescriptor(Pattern.W, (6, 15), Hue.Black)},
            {26L, New ItemTypeUIDescriptor(Pattern.B, (6, 17), Hue.Orange)},
            {33L, New ItemTypeUIDescriptor(Pattern.Pound, (17, 17), Hue.Cyan)},
            {31L, New ItemTypeUIDescriptor(Pattern.Vertical1234, (15, 16), Hue.Orange)},
            {47L, New ItemTypeUIDescriptor(Pattern.Vertical1234, (15, 16), Hue.Blue)},
            {30L, New ItemTypeUIDescriptor(Pattern.Ampersand, (9, 15), Hue.Black)},
            {19L, New ItemTypeUIDescriptor(Pattern.Slash, (2, 17), Hue.Black)},
            {17L, New ItemTypeUIDescriptor(Pattern.Dither, (12, 15), Hue.Black)},
            {2L, New ItemTypeUIDescriptor(Pattern.K, (12, 16), Hue.Green)},
            {10L, New ItemTypeUIDescriptor(Pattern.Apostrophe, (8, 16), Hue.Black)},
            {11L, New ItemTypeUIDescriptor(Pattern.E, (4, 17), Hue.Green)},
            {6L, New ElementalOrbUIDescriptor},
            {13L, New ItemTypeUIDescriptor(Pattern.F, (17, 16), Hue.Red)},
            {24L, New ItemTypeUIDescriptor(Pattern.F, (5, 15), Hue.Orange)},
            {8L, New ItemTypeUIDescriptor(Pattern.At, (18, 16), Hue.Green)},
            {4L, New ItemTypeUIDescriptor(Pattern.K, (14, 15), Hue.Yellow)},
            {16L, New ItemTypeUIDescriptor(Pattern.Spade, (6, 16), Hue.Black)},
            {34L, New ItemTypeUIDescriptor(Pattern.Asterisk, (7, 16), Hue.Green)},
            {22L, New ItemTypeUIDescriptor(Pattern.H, (16, 15), Hue.Cyan)},
            {44L, New ItemTypeUIDescriptor(Pattern.LessThan, (10, 17), Hue.Red)},
            {1L, New ItemTypeUIDescriptor(Pattern.K, (5, 16), Hue.Black)},
            {39L, New ItemTypeUIDescriptor(Pattern.L, (17, 15), Hue.Cyan)},
            {25L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 15), Hue.Black)},
            {32L, New ItemTypeUIDescriptor(Pattern.Club, (13, 13), Hue.Orange)},
            {29L, New ItemTypeUIDescriptor(Pattern.M, (10, 16), Hue.Purple)},
            {36L, New ItemTypeUIDescriptor(Pattern.Spade, (3, 17), Hue.Orange)},
            {20L, New ItemTypeUIDescriptor(Pattern.Dither, (4, 16), Hue.Cyan)},
            {5L, New ItemTypeUIDescriptor(Pattern.K, (7, 15), Hue.Cyan)},
            {7L, New ItemTypeUIDescriptor(Pattern.Ampersand, (14, 16), Hue.Red)},
            {28L, New ItemTypeUIDescriptor(Pattern.CrossDiagonals, (15, 17), Hue.Black)},
            {21L, New ItemTypeUIDescriptor(Pattern.S, (11, 16), Hue.Black)},
            {46L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (14, 17), Hue.Orange)},
            {37L, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 15), Hue.Green)},
            {35L, New ItemTypeUIDescriptor(Pattern.F, (5, 15), Hue.Orange)},
            {15L, New ItemTypeUIDescriptor(Pattern.FilledCircle, (16, 17), Hue.Black)},
            {42L, New ItemTypeUIDescriptor(Pattern.S, (11, 15), Hue.Orange)},
            {18L, New ItemTypeUIDescriptor(Pattern.Minus, (15, 15), Hue.Black)},
            {3L, New ItemTypeUIDescriptor(Pattern.K, (9, 16), Hue.Purple)},
            {9L, New ItemTypeUIDescriptor(Pattern.Octothorpe, (4, 15), Hue.Blue)},
            {41L, New ItemTypeUIDescriptor(Pattern.Comma, (5, 17), Hue.Green)},
            {43L, New ItemTypeUIDescriptor(Pattern.LeftArrow, (19, 17), Hue.Purple)},
            {23L, New ItemTypeUIDescriptor(Pattern.T, (18, 17), Hue.Purple)},
            {27L, New ItemTypeUIDescriptor(Pattern.Pi, (3, 16), Hue.Blue)},
            {12L, New ItemTypeUIDescriptor(Pattern.W, (13, 16), Hue.Blue)},
            {38L, New ItemTypeUIDescriptor(Pattern.Octothorpe, (4, 15), Hue.Purple)}
        }
End Module