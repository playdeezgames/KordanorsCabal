Imports Microsoft.Xna.Framework.Input

Module CommandUtility
    Friend ReadOnly KeyCommands As IReadOnlyDictionary(Of Keys, Command) =
        New Dictionary(Of Keys, Command) From
        {
            {Keys.Up, Command.Up},
            {Keys.Right, Command.Right},
            {Keys.Down, Command.Down},
            {Keys.Left, Command.Left},
            {Keys.Escape, Command.Red},
            {Keys.Space, Command.Green},
            {Keys.Enter, Command.Blue},
            {Keys.Tab, Command.Yellow},
            {Keys.Back, Command.Back},
            {Keys.F10, Command.Start},
            {Keys.OemComma, Command.PreviousItem},
            {Keys.OemPeriod, Command.NextItem}
        }
End Module
