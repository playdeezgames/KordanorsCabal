Imports System.Runtime.CompilerServices

Module ItemTypeUIExtensions
    <Extension>
    Function DisplayPattern(itemType As ItemType) As Pattern?
        Return ItemTypeUIDescriptors(itemType).DisplayPattern
    End Function
    <Extension>
    Function DisplayXY(itemType As ItemType) As (Integer, Integer)?
        Return ItemTypeUIDescriptors(itemType).DisplayXY
    End Function
    <Extension>
    Function DisplayHue(itemType As ItemType) As Hue?
        Return ItemTypeUIDescriptors(itemType).DisplayHue
    End Function
End Module
