Imports Microsoft.Xna.Framework

Public Enum Hue
    Black
    White
    Red
    Cyan
    Purple
    Green
    Blue
    Yellow
    Orange
    LightOrange
    Pink
    LightCyan
    LightPurple
    LightGreen
    LightBlue
    LightYellow
End Enum
Module HueUtility
    Friend ReadOnly HueColors As IReadOnlyDictionary(Of Hue, Color) =
        New Dictionary(Of Hue, Color) From
        {
            {Hue.Black, New Color(0, 0, 0)},
            {Hue.White, New Color(255, 255, 255)},
            {Hue.Red, New Color(&H77, &H2D, &H26)},
            {Hue.Cyan, New Color(&H85, &HD4, &HDC)},
            {Hue.Purple, New Color(&HA8, &H5F, &HB4)},
            {Hue.Green, New Color(&H55, &H9E, &H4A)},
            {Hue.Blue, New Color(&H42, &H34, &H8B)},
            {Hue.Yellow, New Color(&HBD, &HCC, &H71)},
            {Hue.Orange, New Color(&HA8, &H73, &H4A)},
            {Hue.LightOrange, New Color(&HE9, &HB2, &H87)},
            {Hue.Pink, New Color(&HB6, &H68, &H62)},
            {Hue.LightCyan, New Color(&HC5, &HFF, &HFF)},
            {Hue.LightPurple, New Color(&HE9, &H9D, &HF5)},
            {Hue.LightGreen, New Color(&H92, &HDF, &H87)},
            {Hue.LightBlue, New Color(&H7E, &H70, &HCA)},
            {Hue.LightYellow, New Color(&HFF, &HFF, &HB0)}
        }
End Module
