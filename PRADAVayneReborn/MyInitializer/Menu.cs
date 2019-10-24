using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Utils;
using EnsoulSharp.SDK.Utility;
using PRADA_Vayne.MyLogic.Others;
using PRADA_Vayne.MyUtils;
using System.Drawing;
using System.Linq;
using Color = SharpDX.Color;

namespace PRADA_Vayne.MyInitializer
{
    public static partial class PRADALoader
    {
        public static void LoadMenu()
        {
            ConstructMenu();
            InitOrbwalker();
            FinishMenuInit();
        }

        public static void ConstructMenu()
        {
            Program.MainMenu = new Menu("PRADA Vayne", "pradamenu", true).SetFontStyle(FontStyle.Bold, Color.Gold);
            Program.ComboMenu = new Menu("Combo Settings", "combomenu").SetFontStyle(FontStyle.Bold, Color.Red);
            Program.LaneClearMenu =
                new Menu("Laneclear Settings", "laneclearmenu").SetFontStyle(FontStyle.Bold, Color.Red);
            Program.EscapeMenu = new Menu("Escape Settings", "escapemenu").SetFontStyle(FontStyle.Bold, Color.Red);

            Program.DrawingsMenu =
                new Menu("Drawing Settings", "drawingsmenu").SetFontStyle(FontStyle.Regular, Color.Turquoise);
            Program.DrawingsMenu.Add(new MenuItem("streamingmode", "Disable All Drawings").SetValue(false));
            Program.DrawingsMenu.Add(new MenuItem("drawenemywaypoints", "Draw Enemy Waypoints").SetValue(true));
            Program.OrbwalkerMenu =
                new Menu("Orbwalker", "orbwalkermenu").SetFontStyle(FontStyle.Regular, Color.Turquoise);
            Program.ComboMenu.AddItem(new MenuItem("QCombo", "Auto Tumble").SetValue(true));
            Program.ComboMenu
                .AddItem(new MenuItem("QMode", "Q Mode: ").SetValue(new MenuList(new[] { "PRADA", "TO MOUSE" })))
                .SetFontStyle(FontStyle.Bold, Color.Red);
            Program.ComboMenu.Add(
                new MenuItem("QMinDist", "Min dist from enemies").SetValue(new Slider(375, 325, 525)));
            Program.ComboMenu.Add(
                new MenuItem("QOrderBy", "Q to position").SetValue(new MenuList(new[]
                    {"CLOSETOMOUSE", "CLOSETOTARGET"})));
            Program.ComboMenu.Add(new MenuItem("QChecks", "Q Safety Checks").SetValue(true));
            Program.ComboMenu.Add(new MenuItem("EQ", "Q After E").SetValue(false));
            Program.ComboMenu.Add(new MenuItem("QR", "Q after Ult").SetValue(true));
            Program.ComboMenu.Add(new MenuItem("OnlyQinCombo", "Only Q in COMBO").SetValue(false));
            Program.ComboMenu.Add(new MenuItem("FocusTwoW", "Focus 2 W Stacks").SetValue(true));
            Program.ComboMenu.Add(new MenuItem("ECombo", "Auto Condemn").SetValue(true));
            Program.ComboMenu
                .Add(new MenuItem("ManualE", "Semi-Manual Condemn").SetValue(new KeyBind('E', KeyBindType.Press)))
                .SetFontStyle(FontStyle.Bold, Color.Gold);
            Program.ComboMenu.Add(new MenuItem("EMode", "E Mode").SetValue(new MenuList(new[]
            {
                "PRADASMART", "PRADAPERFECT", "MARKSMAN", "SHARPSHOOTER", "GOSU", "VHR", "PRADALEGACY", "FASTEST",
                "OLDPRADA"
            }))).SetFontStyle(FontStyle.Bold, Color.Red);
            Program.ComboMenu.Add(new MenuItem("EPushDist", "E Push Distance").SetValue(new Slider(475, 300, 475)));
            Program.ComboMenu.Add(new MenuItem("EHitchance", "E % Hitchance").SetValue(new Slider(50)));
            Program.ComboMenu.Add(new MenuItem("RCombo", "Auto Ult").SetValue(false));
            Program.EscapeMenu.Add(new MenuItem("QUlt", "Smart Q-Ult").SetValue(true));
            Program.EscapeMenu.Add(new MenuItem("EInterrupt", "Use E to Interrupt").SetValue(true));

            var antigcmenu = Program.EscapeMenu.AddSubMenu(new Menu("Anti-Gapcloser", "antigapcloser"))
                .SetFontStyle(FontStyle.Bold, Color.Red);
            foreach (var hero in Heroes.EnemyHeroes)
            {
                var championName = hero.CharacterName;
                antigcmenu.AddItem(
                    new MenuItem("antigc" + championName, championName).SetValue(
                        Lists.CancerChamps.Any(entry => championName == entry)));
            }

            Program.LaneClearMenu.Add(new MenuItem("QLastHit", "Use Q to Lasthit").SetValue(true))
                .SetFontStyle(FontStyle.Bold, Color.Red);
            Program.LaneClearMenu.Add(
                new MenuItem("QLastHitMana", "Min Mana% for Q Lasthit").SetValue(new Slider(45)));
            Program.LaneClearMenu.Add(new MenuItem("EJungleMobs", "Use E on Jungle Mobs").SetValue(true))
                .SetFontStyle(FontStyle.Bold, Color.Red);
        }

        public static void InitOrbwalker()
        {
            Program.Orbwalker = new Orbwalker.Orbwalk(Program.OrbwalkerMenu);
        }

        public static void FinishMenuInit()
        {
            Program.MainMenu.AddSubMenu(Program.ComboMenu);
            Program.MainMenu.AddSubMenu(Program.LaneClearMenu);
            Program.MainMenu.AddSubMenu(Program.EscapeMenu);
            Program.MainMenu.AddSubMenu(Program.DrawingsMenu);
            Program.MainMenu.AddSubMenu(Program.OrbwalkerMenu);
        }
    }
}