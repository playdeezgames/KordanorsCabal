Public Interface ICharacter
    Inherits IBaseThingie
    Property Location As Location
    Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As CharacterStatisticType) As Long?
    Function IsEnemy(character As Character) As Boolean
    Function RollWillpower() As Long
    ReadOnly Property Name As String
    Function RollDefend() As Long
    Function Kill(killedBy As Character) As (Sfx?, List(Of String))
    Function IsDemoralized() As Boolean
    ReadOnly Property IsDead As Boolean
    Sub DoDamage(damage As Long)
    Function DoArmorWear(wear As Long) As IEnumerable(Of OldItemType)
    Sub Destroy()
    Property CurrentHP As Long
    ReadOnly Property CharacterType As ICharacterType
    ReadOnly Property CanIntimidate As Boolean
    Sub AddStress(delta As Long)
    ReadOnly Property IsUndead As Boolean
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Sub DoImmobilization(delta As Long)
    Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)
    Function RollAttack() As Long
    Function RollInfluence() As Long
    ReadOnly Property PartingShot As String
    Function DoWeaponWear(wear As Long) As IEnumerable(Of OldItemType)
    Function DetermineDamage(value As Long) As Long
    ReadOnly Property MaximumHP As Long
End Interface
