﻿Imports System.Runtime.CompilerServices

Public Enum EquipSlot
    None
    Weapon
End Enum
Public Module EquipSlotExtensions
    <Extension>
    Public Function Name(equipSlot As EquipSlot) As String
        Return EquipSlotDescriptors(equipSlot).Name
    End Function
End Module