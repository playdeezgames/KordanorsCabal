Imports System.Runtime.CompilerServices

Public Enum ItemType
    None
    IronKey
    CopperKey
    SilverKey
    GoldKey
    PlatinumKey
    ElementalOrb
    Potion
    GoblinEar
    SkullFragment
    Dagger
    EarthShard
    WaterShard
    FireShard
    AirShard
    Shield
End Enum
Public Module ItemTypeExtensions
    <Extension>
    Function SpawnLocationTypes(itemType As ItemType, level As Long) As HashSet(Of LocationType)
        Return ItemTypeDescriptors(itemType).SpawnLocationTypes(level)
    End Function
    <Extension>
    Function Name(itemType As ItemType) As String
        Return ItemTypeDescriptors(itemType).Name
    End Function
    <Extension>
    Function CanUse(itemType As ItemType) As Boolean
        Return ItemTypeDescriptors(itemType).CanUse
    End Function
    <Extension>
    Function Encumbrance(itemType As ItemType) As Single
        Return ItemTypeDescriptors(itemType).Encumbrance
    End Function
    <Extension>
    Public Function PurchasePrice(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).PurchasePrice
    End Function
    <Extension>
    Public Sub Use(itemType As ItemType, character As Character)
        ItemTypeDescriptors(itemType).Use(character)
    End Sub
    <Extension>
    Public Function RollSpawnCount(itemType As ItemType, level As Long) As Long
        Return ItemTypeDescriptors(itemType).RollSpawnCount(level)
    End Function
    <Extension>
    Public Function EquipSlot(itemType As ItemType) As EquipSlot?
        Return ItemTypeDescriptors(itemType).EquipSlot
    End Function
    <Extension>
    Public Function AttackDice(itemType As ItemType) As Long
        Return ItemTypeDescriptors(itemType).AttackDice
    End Function
    <Extension>
    Public Function MaximumDamage(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDamage
    End Function
    <Extension>
    Public Function MaximumDurability(itemType As ItemType) As Long?
        Return ItemTypeDescriptors(itemType).MaximumDurability
    End Function
End Module
