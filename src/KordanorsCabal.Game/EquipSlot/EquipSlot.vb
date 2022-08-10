Imports System.Runtime.CompilerServices

Public Enum EquipSlot
    None
    Weapon
    Shield
    Head
    Torso
    Legs
    Neck
    LeftHand
    RightHand
End Enum
Public Module EquipSlotUtility
    <Extension>
    Public Function ToDescriptor(equipSlot As EquipSlot) As EquipSlotDescriptor
        Return New EquipSlotDescriptor(equipSlot)
    End Function
    <Extension>
    Public Function ToEquipSlot(descriptor As EquipSlotDescriptor) As EquipSlot
        Return CType(descriptor.Id, EquipSlot)
    End Function
    Public Const Weapon = "Weapon"
    Public Const Shield = "Shield"
    Public Const Head = "Head"
    Public Const Torso = "Torso"
    Public Const Legs = "Legs"
    Public Const Neck = "Neck"
    Public Const LeftHand = "LHand"
    Public Const RightHand = "RHand"
End Module