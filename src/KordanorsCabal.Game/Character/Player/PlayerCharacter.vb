﻿Public Class PlayerCharacter
    Inherits Character
    Shared ReadOnly Property Messages As New Queue(Of Message)
    Sub New()
        MyBase.New(PlayerData.Read().Value)
    End Sub

    ReadOnly Property IsFullyAssigned As Boolean
        Get
            Return If(GetStatistic(CharacterStatisticType.Unassigned), 0) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As CharacterStatisticType)
        If Not IsFullyAssigned Then
            ChangeStatistic(statisticType, 1)
            ChangeStatistic(CharacterStatisticType.Unassigned, -1)
        End If
    End Sub
    Public Property Direction As Direction
        Get
            Return CType(PlayerData.ReadDirection().Value, Direction)
        End Get
        Set(value As Direction)
            PlayerData.WriteDirection(value)
        End Set
    End Property

    Public Sub Unequip(equipSlot As EquipSlot)
        If Equipment.ContainsKey(equipSlot) Then
            Dim item = Equipment(equipSlot)
            Inventory.Add(item)
            CharacterEquipSlotData.Clear(Id, equipSlot)
        End If
    End Sub

    Public Sub Equip(item As Item)
        If item.CanEquip Then
            InventoryItemData.ClearForItem(item.Id)
            Dim equipSlot = item.EquipSlot.Value
            If Equipment.ContainsKey(equipSlot) Then
                Dim oldItem = Equipment(equipSlot)
                Inventory.Add(oldItem)
            End If
            CharacterEquipSlotData.Write(Id, equipSlot, item.Id)
            Messages.Enqueue(Message.Create($"You equip {item.Name} to {equipSlot.Name}."))
            Return
        End If
        Messages.Enqueue(Message.Create($"You cannot equip {item.Name}!"))
    End Sub

    Public Sub UseItem(item As Item)
        If item.CanUse Then
            item.Use(Me)
            item.Destroy()
        End If
    End Sub

    Public Property Mode As PlayerMode
        Get
            Return CType(PlayerData.ReadMode().Value, PlayerMode)
        End Get
        Set(value As PlayerMode)
            PlayerData.WriteMode(value)
        End Set
    End Property

    Public ReadOnly Property CanInteract As Boolean
        Get
            Return If(Location?.Feature?.CanInteract(Me), False)
        End Get
    End Property

    Public Function GetItemTypeCount(itemType As ItemType) As Integer
        Return Inventory.Items.Where(Function(x) x.ItemType = itemType).Count
    End Function

    Public Function CanMoveLeft() As Boolean
        Return CanMove(Direction.PreviousDirection.Value)
    End Function

    Public Function CanMoveRight() As Boolean
        Return CanMove(Direction.NextDirection.Value)
    End Function

    Public Function CanMoveForward() As Boolean
        Return CanMove(Direction)
    End Function

    Public Sub Heal()
        SetStatistic(CharacterStatisticType.Wounds, 0)
    End Sub

    Public Function CanMoveBackward() As Boolean
        Return CanMove(Direction.Opposite)
    End Function

    Public Sub Interact()
        If CanInteract Then
            Mode = Location.Feature.InteractionMode(Me)
        End If
    End Sub

    Public Sub Buy(itemType As ItemType)
        If CanBuy(itemType) Then
            Dim price = itemType.PurchasePrice.Value
            Money -= price
            Inventory.Add(Item.Create(itemType))
        End If
    End Sub

    Private Function CanBuy(itemType As ItemType) As Boolean
        If itemType.PurchasePrice Is Nothing Then
            Return False
        End If
        Return Money >= itemType.PurchasePrice.Value
    End Function

    Public Function CanMove(direction As Direction) As Boolean
        If IsEncumbered Then
            Return False
        End If
        If Not Location.HasRoute(direction) Then
            Return False
        End If
        Return Location.Routes(direction).CanMove(Me)
    End Function

    Private Shared ReadOnly RunDirections As IReadOnlyList(Of Direction) =
        New List(Of Direction) From
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        }

    Public Sub Run()
        If CanFight Then
            Direction = RNG.FromEnumerable(RunDirections)
            If CanMove(Direction) Then
                Move(Direction)
                Messages.Enqueue(New Message(New List(Of String) From {"You successfully ran!"})) 'TODO: sfx
                Exit Sub
            End If
            Messages.Enqueue(New Message(New List(Of String) From {"You fail to run!"})) 'TODO: shucks!
            DoCounterAttacks()
        End If
    End Sub

    Friend Function HasItemType(itemType As ItemType) As Boolean
        Return Inventory.Items.Any(Function(x) x.ItemType = itemType)
    End Function

    Public Sub Move(direction As Direction)
        If CanMove(direction) Then
            Location = Location.Routes(direction).Move(Me)
        End If
    End Sub

    Public Sub Fight()
        If CanFight Then
            DoAttack()
            DoCounterAttacks()
        End If
    End Sub

    Private Sub DoCounterAttacks()
        Dim enemies = Location.Enemies(Me)
        Dim enemyCount = enemies.Count
        Dim enemyIndex = 1
        For Each enemy In enemies
            DoCounterAttack(enemy, enemyIndex, enemyCount)
            enemyIndex += 1
        Next
    End Sub

    Private Sub DoCounterAttack(enemy As Character, enemyIndex As Integer, enemyCount As Integer)
        If IsDead Then
            Return
        End If
        Dim lines As New List(Of String)
        Dim sfx As Sfx? = Nothing
        lines.Add($"Counter-atttack {enemyIndex}/{enemyCount}:")
        Dim attackRoll = enemy.RollAttack()
        lines.Add($"{enemy.Name} rolls an attack of {attackRoll}.")
        Dim defendRoll = RollDefend()
        lines.Add($"You roll a defend of {defendRoll}.")
        Dim result = attackRoll - defendRoll
        Select Case result
            Case Is <= 0
                lines.Add($"{enemy.Name} misses!")
                sfx = Game.Sfx.Miss
            Case Else
                Dim damage = enemy.DetermineDamage(result)
                lines.Add($"{enemy.Name} does {damage} damage!")
                DoDamage(damage)
                enemy.DoWeaponWear(damage)
                If IsDead Then
                    sfx = Game.Sfx.PlayerDeath
                    lines.Add($"{enemy.Name} kills you!")
                    Dim partingShot = enemy.PartingShot
                    If Not String.IsNullOrEmpty(partingShot) Then
                        lines.Add($"{enemy.Name} says ""{partingShot}""")
                    End If
                    Exit Select
                End If
                sfx = Game.Sfx.PlayerHit
                lines.Add($"You have {CurrentHP} HP left.")
        End Select
        Messages.Enqueue(New Message(lines, sfx))
    End Sub

    Private Sub DoAttack()
        Dim lines As New List(Of String)
        Dim attackRoll = RollAttack()
        Dim enemy = Location.Enemies(Me).First
        lines.Add($"You roll an attack of {attackRoll}.")
        Dim defendRoll = enemy.RollDefend()
        lines.Add($"{enemy.Name} rolls a defend of {defendRoll}.")
        Dim result = attackRoll - defendRoll
        Dim sfx As Sfx?
        Select Case result
            Case Is <= 0
                lines.Add("You miss!")
                sfx = Game.Sfx.Miss
            Case Else
                Dim damage = DetermineDamage(result)
                lines.Add($"You do {damage} damage!")
                enemy.DoDamage(damage)
                DoWeaponWear(damage)
                If enemy.IsDead Then
                    lines.Add($"You kill {enemy.Name}!")
                    sfx = Game.Sfx.EnemyDeath
                    Dim money As Long = enemy.RollMoneyDrop
                    If money > 0 Then
                        lines.Add($"You get {money} money!")
                        ChangeStatistic(CharacterStatisticType.Money, money)
                    End If
                    Dim xp As Long = enemy.XPValue
                    If xp > 0 Then
                        lines.Add($"You get {xp} XP!")
                        If AddXP(xp) Then
                            lines.Add($"You level up!")
                        End If
                    End If
                    enemy.DropLoot()
                    enemy.Destroy()
                    Exit Select
                End If
                sfx = Game.Sfx.EnemyHit
                lines.Add($"{enemy.Name} has {enemy.CurrentHP} HP left.")
        End Select
        Messages.Enqueue(New Message(lines, Sfx))
    End Sub

    Private Function AddXP(xp As Long) As Boolean
        ChangeStatistic(CharacterStatisticType.XP, xp)
        Dim xpGoal = GetStatistic(CharacterStatisticType.XPGoal).Value
        If GetStatistic(CharacterStatisticType.XP).Value >= xpGoal Then
            ChangeStatistic(CharacterStatisticType.XP, -xpGoal)
            ChangeStatistic(CharacterStatisticType.XPGoal, xpGoal)
            ChangeStatistic(CharacterStatisticType.Unassigned, 1)
            SetStatistic(CharacterStatisticType.Wounds, 0)
            SetStatistic(CharacterStatisticType.Stress, 0)
            SetStatistic(CharacterStatisticType.Fatigue, 0)
            Return True
        End If
        Return False
    End Function

    Property Money As Long
        Get
            Return GetStatistic(CharacterStatisticType.Money).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Money, value)
        End Set
    End Property

    Public ReadOnly Property HasEquipment As Boolean
        Get
            Return Equipment.Any
        End Get
    End Property
End Class
