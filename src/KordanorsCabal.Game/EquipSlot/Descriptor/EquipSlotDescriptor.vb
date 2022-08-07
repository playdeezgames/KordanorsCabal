Public Class EquipSlotDescriptor
    ReadOnly Property Name As String
    Sub New(name As String)
        Me.Name = name
    End Sub
End Class
Public Module EquipSlotDescriptorUtility
    Friend EquipSlotDescriptors As IReadOnlyDictionary(Of EquipSlot, EquipSlotDescriptor) =
        New Dictionary(Of EquipSlot, EquipSlotDescriptor) From
        {
            {EquipSlot.Head, New EquipSlotDescriptor("Head")},
            {EquipSlot.LeftHand, New EquipSlotDescriptor("LHand")},
            {EquipSlot.Legs, New EquipSlotDescriptor("Legs")},
            {EquipSlot.Neck, New EquipSlotDescriptor("Neck")},
            {EquipSlot.RightHand, New EquipSlotDescriptor("RHand")},
            {EquipSlot.Shield, New EquipSlotDescriptor("Shield")},
            {EquipSlot.Torso, New EquipSlotDescriptor("Torso")},
            {EquipSlot.Weapon, New EquipSlotDescriptor("Weapon")}
        }
End Module
