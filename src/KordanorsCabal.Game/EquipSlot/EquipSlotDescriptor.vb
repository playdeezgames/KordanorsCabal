Public Class EquipSlotDescriptor
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
    Sub New(equipSlotId As Long, name As String)
        Me.Id = equipSlotId
        Me.Name = name
    End Sub
End Class
Public Module EquipSlotDescriptorUtility
    Friend EquipSlotDescriptors As IReadOnlyDictionary(Of EquipSlot, EquipSlotDescriptor) =
        New Dictionary(Of EquipSlot, EquipSlotDescriptor) From
        {
            {EquipSlot.Head, New EquipSlotDescriptor(EquipSlot.Head, "Head")},
            {EquipSlot.LeftHand, New EquipSlotDescriptor(EquipSlot.LeftHand, "LHand")},
            {EquipSlot.Legs, New EquipSlotDescriptor(EquipSlot.Legs, "Legs")},
            {EquipSlot.Neck, New EquipSlotDescriptor(EquipSlot.Neck, "Neck")},
            {EquipSlot.RightHand, New EquipSlotDescriptor(EquipSlot.RightHand, "RHand")},
            {EquipSlot.Shield, New EquipSlotDescriptor(EquipSlot.Shield, "Shield")},
            {EquipSlot.Torso, New EquipSlotDescriptor(EquipSlot.Torso, "Torso")},
            {EquipSlot.Weapon, New EquipSlotDescriptor(EquipSlot.Weapon, "Weapon")}
        }
End Module
