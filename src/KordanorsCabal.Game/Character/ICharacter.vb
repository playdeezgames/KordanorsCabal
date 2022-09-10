﻿Public Interface ICharacter
    Inherits IBaseThingie
    ReadOnly Property CanBeBribedWith(itemType As OldItemType) As Boolean
    ReadOnly Property CanIntimidate As Boolean
    ReadOnly Property CharacterType As ICharacterType
    Property CurrentHP As Long
    ReadOnly Property IsDead As Boolean
    ReadOnly Property IsEnemy(character As ICharacter) As Boolean
    ReadOnly Property IsDemoralized As Boolean
    ReadOnly Property IsUndead As Boolean
    Property Location As Location
    ReadOnly Property MaximumHP As Long
    ReadOnly Property Name As String
    ReadOnly Property PartingShot As String
    ReadOnly Property MaximumMana As Long
    Property Statistic(statisticType As CharacterStatisticType) As Long
    ReadOnly Property HasStatistic(statisticType As CharacterStatisticType) As Boolean
    ReadOnly Property Inventory As Inventory
    Property Hunger As Long
    Property Highness As Long
    Property FoodPoisoning As Long
    Property Drunkenness As Long
    Property CurrentMP As Long
    Property CurrentMana As Long
    Property Chafing As Long
    Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As CharacterStatisticType) As Long?
    Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)


    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String))
    Sub Destroy()

    Sub AddStress(delta As Long)
    Function AddXP(xp As Long) As Boolean

    Function DetermineDamage(value As Long) As Long
    Sub DoDamage(damage As Long)
    Function DoArmorWear(wear As Long) As IEnumerable(Of OldItemType)
    Sub DoImmobilization(delta As Long)
    Function DoWeaponWear(wear As Long) As IEnumerable(Of OldItemType)
    Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
    Sub EnqueueMessage(ParamArray lines() As String)
    Sub Learn(spellType As SpellType)
    Sub Heal()
    Sub DoFatigue(fatigue As Long)
    Sub DoCounterAttacks()

    Function RollWillpower() As Long
    Function RollDefend() As Long
    Function RollAttack() As Long
    Function RollInfluence() As Long
    Function RollSpellDice(spellType As SpellType) As Long
End Interface
