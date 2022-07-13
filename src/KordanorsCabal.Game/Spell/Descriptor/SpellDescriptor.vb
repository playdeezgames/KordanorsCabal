Public MustInherit Class SpellDescriptor
    MustOverride ReadOnly Property Name As String

    Friend Function MaximumLevel() As Long
        Return 1
    End Function

    Friend Function RequiredPower(level As Long) As Long
        Select Case level
            Case 0
                Return 0
            Case 1
                Return 1
            Case Else
                Return Long.MaxValue
        End Select
    End Function
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of SpellType, SpellDescriptor) =
        New Dictionary(Of SpellType, SpellDescriptor) From
        {
            {SpellType.HolyBolt, New HolyBoltDescriptor}
        }
End Module
