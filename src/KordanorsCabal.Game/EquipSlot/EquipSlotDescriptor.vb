Public Class EquipSlotDescriptor
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.EquipSlot.ReadName(Id)
        End Get
    End Property
    Sub New(equipSlotId As Long)
        Me.Id = equipSlotId
    End Sub
End Class
Public Module EquipSlotDescriptorUtility
    Friend EquipSlotDescriptors As IReadOnlyDictionary(Of EquipSlot, EquipSlotDescriptor) =
        New Dictionary(Of EquipSlot, EquipSlotDescriptor) From
        {
            {EquipSlot.Head, New EquipSlotDescriptor(EquipSlot.Head)},
            {EquipSlot.LeftHand, New EquipSlotDescriptor(EquipSlot.LeftHand)},
            {EquipSlot.Legs, New EquipSlotDescriptor(EquipSlot.Legs)},
            {EquipSlot.Neck, New EquipSlotDescriptor(EquipSlot.Neck)},
            {EquipSlot.RightHand, New EquipSlotDescriptor(EquipSlot.RightHand)},
            {EquipSlot.Shield, New EquipSlotDescriptor(EquipSlot.Shield)},
            {EquipSlot.Torso, New EquipSlotDescriptor(EquipSlot.Torso)},
            {EquipSlot.Weapon, New EquipSlotDescriptor(EquipSlot.Weapon)}
        }
End Module
