Public MustInherit Class EquipSlotDescriptor
    MustOverride ReadOnly Property Name As String
End Class
Public Module EquipSlotDescriptorUtility
    Friend EquipSlotDescriptors As IReadOnlyDictionary(Of EquipSlot, EquipSlotDescriptor) =
        New Dictionary(Of EquipSlot, EquipSlotDescriptor) From
        {
            {EquipSlot.Head, New HeadEquipSlotDescriptor},
            {EquipSlot.LeftHand, New LeftHandEquipSlotDescriptor},
            {EquipSlot.Legs, New LegsEquipSlotDescriptor},
            {EquipSlot.Neck, New NeckEquipSlotDescriptor},
            {EquipSlot.RightHand, New RightHandEquipSlotDescriptor},
            {EquipSlot.Shield, New ShieldEquipSlotDescriptor},
            {EquipSlot.Torso, New TorsoEquipSlotDescriptor},
            {EquipSlot.Weapon, New WeaponEquipSlotDescriptor}
        }
End Module
