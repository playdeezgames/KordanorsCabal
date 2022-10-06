Imports System.Runtime.CompilerServices

Module ItemTypeUIExtensions
    <Extension>
    Function DisplayPattern(itemType As Long) As Pattern?
        Return ItemTypeUIDescriptors(itemType).DisplayPattern
    End Function
    <Extension>
    Function DisplayXY(itemType As Long) As (Integer, Integer)?
        Return ItemTypeUIDescriptors(itemType).DisplayXY
    End Function
    <Extension>
    Function DisplayHue(itemType As Long) As Hue?
        Return ItemTypeUIDescriptors(itemType).DisplayHue
    End Function
End Module
