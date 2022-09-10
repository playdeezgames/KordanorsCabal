Public Interface ICharacter
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

    Function RollWillpower() As Long
    Function RollDefend() As Long
    Function RollAttack() As Long
    Function RollInfluence() As Long
End Interface
