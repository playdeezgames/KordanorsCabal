Imports System.Runtime.CompilerServices

Module ItemTypeUIExtensions
    <Extension>
    Function DisplayPattern(itemType As OldItemType) As Pattern?
        Return ItemTypeUIDescriptors(itemType).DisplayPattern
    End Function
    <Extension>
    Function DisplayXY(itemType As OldItemType) As (Integer, Integer)?
        Return ItemTypeUIDescriptors(itemType).DisplayXY
    End Function
    <Extension>
    Function DisplayHue(itemType As OldItemType) As Hue?
        Return ItemTypeUIDescriptors(itemType).DisplayHue
    End Function
End Module
