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
    Friend ReadOnly ItemTypeUIDescriptors As IReadOnlyDictionary(Of ItemType, ItemTypeUIDescriptor) =
        New Dictionary(Of ItemType, ItemTypeUIDescriptor) From
        {
            {ItemType.AirShard, New ItemTypeUIDescriptor(Pattern.A, (10, 15), Hue.Cyan)},
            {ItemType.AmuletOfHP, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 17), Hue.Red)},
            {ItemType.BatWing, New ItemTypeUIDescriptor(Pattern.W, (6, 15), Hue.Black)},
            {ItemType.Beer, New ItemTypeUIDescriptor(Pattern.B, (6, 17), Hue.Orange)},
            {ItemType.Bong, New ItemTypeUIDescriptor(Pattern.Pound, (17, 17), Hue.Cyan)},
            {ItemType.BookOfHolyBolt, New ItemTypeUIDescriptor(Pattern.Vertical1234, (15, 16), Hue.Orange)},
            {ItemType.BookOfPurify, New ItemTypeUIDescriptor(Pattern.Vertical1234, (15, 16), Hue.Blue)},
            {ItemType.Bottle, New ItemTypeUIDescriptor(Pattern.Ampersand, (9, 15), Hue.Black)},
            {ItemType.BrodeSode, New ItemTypeUIDescriptor(Pattern.Slash, (2, 17), Hue.Black)},
            {ItemType.ChainMail, New ItemTypeUIDescriptor(Pattern.Dither, (12, 15), Hue.Black)},
            {ItemType.CopperKey, New ItemTypeUIDescriptor(Pattern.K, (12, 16), Hue.Green)},
            {ItemType.Dagger, New ItemTypeUIDescriptor(Pattern.Apostrophe, (8, 16), Hue.Black)},
            {ItemType.EarthShard, New ItemTypeUIDescriptor(Pattern.E, (4, 17), Hue.Green)},
            {ItemType.ElementalOrb, New ElementalOrbUIDescriptor},
            {ItemType.FireShard, New ItemTypeUIDescriptor(Pattern.F, (17, 16), Hue.Red)},
            {ItemType.Food, New ItemTypeUIDescriptor(Pattern.F, (5, 15), Hue.Orange)},
            {ItemType.GoblinEar, New ItemTypeUIDescriptor(Pattern.At, (18, 16), Hue.Green)},
            {ItemType.GoldKey, New ItemTypeUIDescriptor(Pattern.K, (14, 15), Hue.Yellow)},
            {ItemType.Helmet, New ItemTypeUIDescriptor(Pattern.Spade, (6, 16), Hue.Black)},
            {ItemType.Herb, New ItemTypeUIDescriptor(Pattern.Asterisk, (7, 16), Hue.Green)},
            {ItemType.HolyWater, New ItemTypeUIDescriptor(Pattern.H, (16, 15), Hue.Cyan)},
            {ItemType.HornsOfKordanor, New ItemTypeUIDescriptor(Pattern.LessThan, (10, 17), Hue.Red)},
            {ItemType.IronKey, New ItemTypeUIDescriptor(Pattern.K, (5, 16), Hue.Black)},
            {ItemType.Lotion, New ItemTypeUIDescriptor(Pattern.L, (17, 15), Hue.Cyan)},
            {ItemType.MagicEgg, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 15), Hue.Black)},
            {ItemType.MembershipCard, New ItemTypeUIDescriptor(Pattern.Club, (13, 13), Hue.Orange)},
            {ItemType.MoonPortal, New ItemTypeUIDescriptor(Pattern.M, (10, 16), Hue.Purple)},
            {ItemType.Mushroom, New ItemTypeUIDescriptor(Pattern.Spade, (3, 17), Hue.Orange)},
            {ItemType.PlateMail, New ItemTypeUIDescriptor(Pattern.Dither, (4, 16), Hue.Cyan)},
            {ItemType.PlatinumKey, New ItemTypeUIDescriptor(Pattern.K, (7, 15), Hue.Cyan)},
            {ItemType.Potion, New ItemTypeUIDescriptor(Pattern.Ampersand, (14, 16), Hue.Red)},
            {ItemType.Pr0n, New ItemTypeUIDescriptor(Pattern.CrossDiagonals, (15, 17), Hue.Black)},
            {ItemType.RatTail, New ItemTypeUIDescriptor(Pattern.S, (11, 16), Hue.Black)},
            {ItemType.RingOfHP, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (14, 17), Hue.Orange)},
            {ItemType.RottenEgg, New ItemTypeUIDescriptor(Pattern.EmptyCircle, (8, 15), Hue.Green)},
            {ItemType.RottenFood, New ItemTypeUIDescriptor(Pattern.F, (5, 15), Hue.Orange)},
            {ItemType.Shield, New ItemTypeUIDescriptor(Pattern.FilledCircle, (16, 17), Hue.Black)},
            {ItemType.ShoeLaces, New ItemTypeUIDescriptor(Pattern.S, (11, 15), Hue.Orange)},
            {ItemType.Shortsword, New ItemTypeUIDescriptor(Pattern.Minus, (15, 15), Hue.Black)},
            {ItemType.SilverKey, New ItemTypeUIDescriptor(Pattern.K, (9, 16), Hue.Purple)},
            {ItemType.SkullFragment, New ItemTypeUIDescriptor(Pattern.Octothorpe, (4, 15), Hue.Blue)},
            {ItemType.SnakeFang, New ItemTypeUIDescriptor(Pattern.Comma, (5, 17), Hue.Green)},
            {ItemType.SpaceSord, New ItemTypeUIDescriptor(Pattern.LeftArrow, (19, 17), Hue.Purple)},
            {ItemType.TownPortal, New ItemTypeUIDescriptor(Pattern.T, (18, 17), Hue.Purple)},
            {ItemType.Trousers, New ItemTypeUIDescriptor(Pattern.Pi, (3, 16), Hue.Blue)},
            {ItemType.WaterShard, New ItemTypeUIDescriptor(Pattern.W, (13, 16), Hue.Blue)},
            {ItemType.ZombieTaint, New ItemTypeUIDescriptor(Pattern.Octothorpe, (4, 15), Hue.Purple)}
        }
End Module