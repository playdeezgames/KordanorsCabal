// VB oracle: drives the original game's UI state machine headlessly and dumps screens as text.
// usage: dotnet run -- <workdir> [explore|capture <outdir>]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using KordanorsCabal.UI;
using SPLORR.UI;

static class Oracle
{
    public static UIState State = UIState.TitleScreen;
    static readonly Stack<UIState> Stack = new();
    public static Renderer<uint> Rend = new Renderer<uint>((16, 28), (22, 23), (8, 8), Palette());
    static PatternBuffer Buffer => Rend.PatternBuffer;
    static Dictionary<Hue, uint> Palette() => new() {
        [Hue.Black] = Rgb(0,0,0), [Hue.White] = Rgb(255,255,255), [Hue.Red] = Rgb(0x77,0x2D,0x26), [Hue.Cyan] = Rgb(0x85,0xD4,0xDC), [Hue.Purple] = Rgb(0xA8,0x5F,0xB4),
        [Hue.Green] = Rgb(0x55,0x9E,0x4A), [Hue.Blue] = Rgb(0x42,0x34,0x8B), [Hue.Yellow] = Rgb(0xBD,0xCC,0x71), [Hue.Orange] = Rgb(0xA8,0x73,0x4A), [Hue.LightOrange] = Rgb(0xE9,0xB2,0x87),
        [Hue.Pink] = Rgb(0xB6,0x68,0x62), [Hue.LightCyan] = Rgb(0xC5,0xFF,0xFF), [Hue.LightPurple] = Rgb(0xE9,0x9D,0xF5), [Hue.LightGreen] = Rgb(0x92,0xDF,0x87), [Hue.LightBlue] = Rgb(0x7E,0x70,0xCA), [Hue.LightYellow] = Rgb(0xFF,0xFF,0xB0) };
    static uint Rgb(int r, int g, int b) => (uint)(0xFF000000 | (uint)r << 16 | (uint)g << 8 | (uint)b);
    static Dictionary<Pattern, char> Glyph = new();

    static void SetStatic(string typeName, string prop, object value)
    {
        var t = typeof(MainProcessor).Assembly.GetType("KordanorsCabal.UI." + typeName, true);
        t.GetProperty(prop, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
    }

    public static void ClearStack() { Stack.Clear(); }

    public static void Init()
    {
        MainProcessor.PushUIState = s => Stack.Push(s);
        MainProcessor.PopUIState = () => State = Stack.Pop();
        SetStatic("ScreenSizerProcessor", "GetCurrentScreenSize", new Func<int>(() => 2));
        SetStatic("ScreenSizerProcessor", "SetCurrentScreenSize", new Action<int>(_ => { }));
        SetStatic("MuxVolumizerProcessor", "GetCurrentMuxVolume", new Func<float>(() => 0.5f));
        SetStatic("MuxVolumizerProcessor", "SetCurrentMuxVolume", new Action<float>(_ => { }));
        SetStatic("SfxVolumizerProcessor", "GetCurrentSfxVolume", new Func<float>(() => 0.5f));
        SetStatic("SfxVolumizerProcessor", "SetCurrentSfxVolume", new Action<float>(_ => { }));
        foreach (var kv in PatternUtility.CharacterPattern)
            if (!Glyph.ContainsKey(kv.Value) || char.IsUpper(kv.Key)) Glyph[kv.Value] = kv.Key;
    }

    public static void Send(Command c)
    {
        State = MainProcessor.ProcessCommand(KordanorsCabal.UI.StaticWorldData.WorldData, State, c);
    }
    static object CurrentProcessor()
    {
        var t = typeof(MainProcessor);
        var f = t.GetField("processors", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
        var dict = (System.Collections.IDictionary)f.GetValue(null);
        return dict[State];
    }
    // Menu screens remember their cursor between visits, so scripts address items by index: move the cursor to item n, then press Green.
    public static void Pick(int n, bool confirm = true)
    {
        var p = CurrentProcessor();
        var bt = typeof(MainProcessor).Assembly.GetType("KordanorsCabal.UI.MenuProcessor", true);
        int count = ((System.Collections.IList)bt.GetField("MenuItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(p)).Count;
        int cur = (int)bt.GetField("currentItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(p);
        for (int i = 0; i < ((n - cur) % count + count) % count; i++) Send(Command.Down);
        if (confirm) Send(Command.Green);
    }
    // In-play screens have 10 cell buttons; address one by index (0..4 left column, 5..9 right column).
    public static void Button(int n, bool confirm = true)
    {
        var mp = typeof(MainProcessor).Assembly.GetType("KordanorsCabal.UI.ModeProcessor", true);
        mp.GetProperty("CurrentButtonIndex", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(null, n);
        if (confirm) Send(Command.Green);
    }
    public static void SendAll(params Command[] cs) { foreach (var c in cs) Send(c); }

    public static PatternBuffer Draw()
    {
        MainProcessor.UpdateBuffer(KordanorsCabal.UI.StaticWorldData.WorldData, State, Buffer);
        return Buffer;
    }

    // PNG of the whole 208 x 240 frame, 2x wide for the VIC-20 pixel aspect, 3x scale
    public static void WritePng(string path)
    {
        Rend.Update();
        int w = Rend.FrameBuffer.Columns, h = Rend.FrameBuffer.Rows, sx = 6, sy = 3;
        var raw = new List<byte>();
        for (int y = 0; y < h * sy; y++)
        {
            raw.Add(0);
            for (int x = 0; x < w * sx; x++) { var p = Rend.FrameBuffer.Pixels[(y / sy) * w + x / sx]; raw.Add((byte)(p >> 16)); raw.Add((byte)(p >> 8)); raw.Add((byte)p); }
        }
        using var ms = new MemoryStream();
        void Chunk(string type, byte[] data)
        {
            var len = BitConverter.GetBytes(data.Length); Array.Reverse(len); ms.Write(len);
            var td = System.Text.Encoding.ASCII.GetBytes(type).Concat(data).ToArray(); ms.Write(td);
            uint crc = 0xFFFFFFFF; foreach (var b in td) { crc ^= b; for (int k = 0; k < 8; k++) crc = (crc & 1) != 0 ? (crc >> 1) ^ 0xEDB88320 : crc >> 1; }
            var c = BitConverter.GetBytes(~crc); Array.Reverse(c); ms.Write(c);
        }
        ms.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        var ihdr = new byte[13]; var W = BitConverter.GetBytes(w * sx); Array.Reverse(W); var H = BitConverter.GetBytes(h * sy); Array.Reverse(H);
        W.CopyTo(ihdr, 0); H.CopyTo(ihdr, 4); ihdr[8] = 8; ihdr[9] = 2;
        Chunk("IHDR", ihdr);
        using (var z = new MemoryStream()) { using (var zs = new System.IO.Compression.ZLibStream(z, System.IO.Compression.CompressionLevel.Optimal, true)) zs.Write(raw.ToArray()); Chunk("IDAT", z.ToArray()); }
        Chunk("IEND", Array.Empty<byte>());
        File.WriteAllBytes(path, ms.ToArray());
    }

    // FNV-1a over the R, G, B bytes of the 208 x 240 frame (row by row), so another renderer can be compared bit for bit
    public static ulong FrameHash()
    {
        Rend.Update();
        ulong h = 14695981039346656037UL;
        foreach (var p in Rend.FrameBuffer.Pixels) foreach (var v in new[] { (byte)(p >> 16), (byte)(p >> 8), (byte)p }) { h ^= v; h *= 1099511628211UL; }
        return h;
    }

    public static string Ascii(PatternBuffer b)
    {
        var rows = new List<string>();
        for (int y = 0; y < b.Rows; y++)
        {
            var s = new char[b.Columns];
            for (int x = 0; x < b.Columns; x++)
            {
                var c = b.get_Cell(x, y);
                char ch = Glyph.TryGetValue(c.Pattern, out var g) ? g : '#';
                s[x] = ch;
            }
            rows.Add(new string(s));
        }
        return string.Join("\n", rows);
    }

    // cells file: one line per row, "glyph.hue.inverted" triples, for exact comparison with the Odin renderer
    public static string Cells(PatternBuffer b)
    {
        var lines = new List<string>();
        for (int y = 0; y < b.Rows; y++)
            lines.Add(string.Join(" ", Enumerable.Range(0, b.Columns).Select(x => { var c = b.get_Cell(x, y); return $"{(int)c.Pattern}.{(int)c.Hue}.{(c.Inverted ? 1 : 0)}"; })));
        return string.Join("\n", lines);
    }
}

static class P
{
    static readonly Dictionary<string, Command> Cmd = new() { ["U"] = Command.Up, ["D"] = Command.Down, ["L"] = Command.Left, ["R"] = Command.Right, ["G"] = Command.Green, ["B"] = Command.Blue, ["X"] = Command.Red };
    public static Command[] Parse(string s) => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(t => Cmd[t]).ToArray();

    // Slot files must exist and contain tables, or the VB Load/Save screens throw (observed: "no such table: Players").
    static void Prep()
    {
        var wd = KordanorsCabal.UI.StaticWorldData.WorldData;
        wd.Reset();
        for (int i = 3; i <= 5; i++) wd.Save($"SaveSlot{i}.db");            // valid but empty slots
        KordanorsCabal.Game.World.FromWorldData(wd).Start();
        for (int i = 1; i <= 2; i++) wd.Save($"SaveSlot{i}.db");            // two used slots
        wd.Reset();
        Oracle.State = UIState.TitleScreen;
    }

    // Meta commands set up game state directly (the player could reach it by playing). They are listed in the capture log.
    //   !start        begin a new game (as the title menu's Start does, but skipping the UI)   !play  jump to the InPlay screen
    //   !to T [e]     move the player to a location of location type T (that has enemies when e is given)
    //   !give I       put a new item of item type I in the player's inventory       !hurt N   add N wounds
    //   !feature F    move the player to the location of feature F (1 elder, 2 innkeeper, 3 drunk, 4 chicken, 5 black market, 6 black mage, 7 blacksmith, 8 healer, 9 constable)
    //   !money N      add N money
    //   !xp N         add N experience
    //   !mode M       set the player mode (see PlayerModes)   !ground I   put an item of type I on the ground here
    static void Meta(string a)
    {
        var wd = KordanorsCabal.UI.StaticWorldData.WorldData;
        var w = KordanorsCabal.Game.World.FromWorldData(wd);
        var parts = a.Substring(1).Split(' ');
        switch (parts[0])
        {
            case "start": w.Start(); break;
            case "play": Oracle.State = UIState.InPlay; break;
            case "to":
                {
                    var type = KordanorsCabal.Game.LocationType.FromId(wd, long.Parse(parts[1]));
                    var player = w.PlayerCharacter;
                    var candidates = wd.Location.ReadForLocationType(type.Id).Select(id => KordanorsCabal.Game.Location.FromId(wd, id)).ToList();
                    if (parts.Length > 2) candidates = candidates.Where(l => l.Factions.EnemiesOf(player).Any()).ToList();
                    player.Movement.Location = candidates.First();
                    break;
                }
            case "reset-ui":
                {
                    Oracle.State = UIState.InPlay; Oracle.ClearStack();
                    KordanorsCabal.Game.PlayerCharacter.Messages.Clear();
                    w.PlayerCharacter.Mode = 1;
                    var mp = typeof(MainProcessor).Assembly.GetType("KordanorsCabal.UI.ModeProcessor", true);
                    mp.GetMethod("ResetButtonIndexStack", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                    break;
                }
            case "feature":
                {
                    long want = long.Parse(parts[1]);
                    foreach (var lt in new long[] { 1, 2, 3 })
                        foreach (var id in wd.Location.ReadForLocationType(lt))
                        {
                            var f = wd.Feature.ReadForLocation(id);
                            if (f.HasValue && wd.Feature.ReadFeatureType(f.Value) == want) { w.PlayerCharacter.Movement.Location = KordanorsCabal.Game.Location.FromId(wd, id); return; }
                        }
                    throw new Exception("no feature of type " + want);
                }
            case "give": w.PlayerCharacter.Items.Inventory.Add(KordanorsCabal.Game.Item.Create(wd, KordanorsCabal.Game.ItemType.FromId(wd, long.Parse(parts[1])))); break;
            case "ground": w.PlayerCharacter.Movement.Location.Inventory.Add(KordanorsCabal.Game.Item.Create(wd, KordanorsCabal.Game.ItemType.FromId(wd, long.Parse(parts[1])))); break;
            case "money": w.PlayerCharacter.Statistics.ChangeStatistic(KordanorsCabal.Game.StatisticType.FromId(wd, 14), long.Parse(parts[1])); break;
            case "xp": w.PlayerCharacter.Advancement.AddXP(long.Parse(parts[1])); break;
            case "hurt": w.PlayerCharacter.Statistics.ChangeStatistic(KordanorsCabal.Game.StatisticType.FromId(wd, 12), long.Parse(parts[1])); break;
            case "mode": w.PlayerCharacter.Mode = long.Parse(parts[1]); break;
            default: throw new Exception("unknown meta command " + a);
        }
    }

    static void Main(string[] args)
    {
        var work = args[0];
        Directory.SetCurrentDirectory(work);
        Oracle.Init();
        if (args.Length > 2 && args[1] == "capture")
        {
            // script directives: "# comment" | "! meta..." | ": D D G" (commands) | "snap name"
            var outDir = args[3]; Directory.CreateDirectory(outDir);
            var log = new List<string>();
            int lineNo = 0;
            foreach (var raw in File.ReadAllLines(args[2]))
            {
                lineNo++;
                var line = raw.Trim();
                try {
                if (line.Length == 0 || line.StartsWith("#")) continue;
                if (line.StartsWith("!")) { if (line == "!prep") Prep(); else Meta(line); log.Add(line); }
                else if (line.StartsWith(":")) { Oracle.SendAll(Parse(line.Substring(1))); log.Add(line); }
                else if (line == "finalize") { for (int i = 0; i < 30 && Oracle.State == UIState.FinalizeCharacter; i++) Oracle.Pick(1); log.Add(line); } // spend the unassigned points the random roll may leave
                else if (line.StartsWith("pick ")) { Oracle.Pick(int.Parse(line.Substring(5))); log.Add(line); }
                else if (line.StartsWith("button ")) { Oracle.Button(int.Parse(line.Substring(7))); log.Add(line); }
                else if (line.StartsWith("snap ") || line.StartsWith("snap-if "))
                {
                    var words = line.Split(' ');
                    if (words[0] == "snap-if" && Oracle.State.ToString() != words[1]) continue; // only when the random roll left points to assign
                    var name = words[0] == "snap-if" ? words[2] : line.Substring(5).Trim();
                    var b = Oracle.Draw();
                    File.WriteAllText(Path.Combine(outDir, name + ".txt"), $"state {Oracle.State}\n" + Oracle.Ascii(b) + "\n");
                    File.WriteAllText(Path.Combine(outDir, name + ".cells"), Oracle.Cells(b) + "\n");
                    Oracle.WritePng(Path.Combine(outDir, name + ".png"));
                    File.WriteAllText(Path.Combine(outDir, name + ".hash"), Oracle.FrameHash().ToString("x16") + "\n");
                    log.Add($"snap {name} [{Oracle.State}]");
                }
                else throw new Exception("bad script line: " + line);
                } catch (Exception e) { log.Add($"ERROR line {lineNo} '{line}' in {Oracle.State}: {e.GetType().Name} {e.Message.Split('\n')[0]}"); Console.WriteLine(log.Last()); }
            }
            File.WriteAllLines(Path.Combine(outDir, "_log.txt"), log);
            return;
        }
        if (args.Length > 1 && args[1] == "explore")
        {
            // each following arg is either "?" (show screen) or a command string like "D D G"
            foreach (var a in args.Skip(2))
            {
                if (a == "!prep") { Prep(); }
                else if (a.StartsWith("!")) { Meta(a); }
                else if (a == "?") { Console.WriteLine($"[{Oracle.State}]"); Console.WriteLine(Oracle.Ascii(Oracle.Draw())); Console.WriteLine(); }
                else { Oracle.SendAll(Parse(a)); }
            }
        }
    }
}
