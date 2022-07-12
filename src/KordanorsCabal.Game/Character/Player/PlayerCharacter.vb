﻿Public Class PlayerCharacter
    Inherits Character
    Shared ReadOnly Property Messages As New Queue(Of Message)
    Public Overrides Sub EnqueueMessage(ParamArray lines() As String)
        EnqueueMessage(Nothing, lines)
    End Sub
    Public Overrides Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
        If sfx.HasValue Then
            Messages.Enqueue(Message.Create(sfx.Value, lines))
            Return
        End If
        Messages.Enqueue(Message.Create(lines))
    End Sub
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

    Public ReadOnly Property CanGamble As Boolean
        Get
            Return Money >= 5
        End Get
    End Property


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

    Public Sub Gamble()
        If Not CanGamble Then
            EnqueueMessage("You cannot gamble at this time!")
            Return
        End If
        Dim lines As New List(Of String)
        lines.Add("You flip the two coins!")
        Dim firstCoin = RNG.FromRange(0, 1)
        lines.Add($"The first coin comes up {If(firstCoin > 0, "heads", "tails")}!")
        Dim secondCoin = RNG.FromRange(0, 1)
        lines.Add($"The second coin comes up {If(secondCoin > 0, "heads", "tails")}!")
        Dim winner = firstCoin > 0 AndAlso secondCoin > 0
        If winner Then
            lines.Add("You win and receive 15 money!")
            Money += 15
        Else
            lines.Add("You lose and must pay 5 money!")
            Money -= 5
        End If
        'TODO: sound effect
        EnqueueMessage(lines.ToArray)
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
            EnqueueMessage($"You equip {item.Name} to {equipSlot.Name}.")
            Return
        End If
        EnqueueMessage($"You cannot equip {item.Name}!")
    End Sub

    Public Function CanAcceptQuest(quest As Quest) As Boolean
        Return Not HasQuest(quest) AndAlso quest.CanAccept(Me)
    End Function

    Public Sub UseItem(item As Item)
        If item.CanUse(Me) Then
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

    Public Sub CompleteQuest(quest As Quest)
        quest.Complete(Me)
    End Sub

    Public Sub AcceptQuest(quest As Quest)
        quest.Accept(Me)
    End Sub

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

    ReadOnly Property CanDoIntimidation() As Boolean
        Get
            If If(GetStatistic(CharacterStatisticType.Influence), 0) <= 0 Then
                Return False
            End If
            Dim enemy = Location.Enemies(Me).FirstOrDefault
            If enemy Is Nothing Then
                Return False
            End If
            Return enemy.CanIntimidate
        End Get
    End Property

    Public Sub DoIntimidation()
        If CanDoIntimidation Then
            Dim lines As New List(Of String)
            Dim enemy = Location.Enemies(Me).First
            Dim influenceRoll = RollInfluence()
            lines.Add($"You roll {influenceRoll} influence.")
            Dim willpowerRoll = enemy.RollWillpower()
            lines.Add($"{enemy.Name} rolls {willpowerRoll} willpower.")
            If influenceRoll > willpowerRoll Then
                enemy.AddStress(1)
                lines.Add($"{enemy.Name} loses 1 MP!")
                If enemy.IsDemoralized Then
                    lines.Add($"{enemy.Name} runs away!")
                    enemy.Destroy()
                End If
            Else
                lines.Add($"{enemy.Name} is not intimidated.")
            End If
            EnqueueMessage(lines.ToArray)
            DoCounterAttacks()
            Return
        End If
        EnqueueMessage("You cannot intimidate at this time!")
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
                EnqueueMessage("You successfully ran!") 'TODO: sfx
                Exit Sub
            End If
            EnqueueMessage("You fail to run!") 'TODO: shucks!
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


    Private Sub DoAttack()
        Dim lines As New List(Of String)
        Dim attackRoll = RollAttack()
        Dim enemy = Location.Enemies(Me).First
        lines.Add($"You roll an attack of {attackRoll}.")
        Dim defendRoll = enemy.RollDefend()
        lines.Add($"{enemy.Name} rolls a defend of {defendRoll}.")
        Dim result = attackRoll - defendRoll
        Dim sfx As Sfx?
        enemy.DoArmorWear(attackRoll)
        Select Case result
            Case Is <= 0
                lines.Add("You miss!")
                sfx = Game.Sfx.Miss
            Case Else
                Dim damage = DetermineDamage(result)
                lines.Add($"You do {damage} damage!")
                enemy.DoDamage(damage)
                For Each brokenItemType In DoWeaponWear(damage)
                    lines.Add($"Yer {brokenItemType.Name} breaks!")
                Next
                If enemy.IsDead Then
                    Dim killResult = enemy.Kill(Me)
                    sfx = If(killResult.Item1, sfx)
                    lines.AddRange(killResult.Item2)
                    Exit Select
                End If
                sfx = Game.Sfx.EnemyHit
                lines.Add($"{enemy.Name} has {enemy.CurrentHP} HP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub


    Public ReadOnly Property HasEquipment As Boolean
        Get
            Return Equipment.Any
        End Get
    End Property
End Class
