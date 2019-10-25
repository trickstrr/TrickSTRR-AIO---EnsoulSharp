using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Utils;
using EnsoulSharp.SDK.Utility;
using SharpDX;
using PRADA_Vayne.MyLogic.Others;
using PRADA_Vayne.MyUtils;
using PRADA_Vayne.MyCommon;
using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;
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
            Program.MainMenu = new Menu("PRADA Vayne", "pradamenu", true);
            Program.ComboMenu = new Menu("Combo Settings", "combomenu");
            Program.ComboMenu.Add();
            Program.LaneClearMenu = new Menu("Laneclear Settings", "laneclearmenu");
            Program.EscapeMenu = new Menu("Escape Settings", "escapemenu");
            Program.DrawingsMenu = new Menu("Drawing Settings", "drawingsmenu");
            Program.DrawingsMenu.Add(new MenuBool("streamingmode", "Disable All Drawings").SetValue(false));
            Program.DrawingsMenu.Add(new MenuBool("drawenemywaypoints", "Draw Enemy Waypoints").SetValue(true));
            Program.OrbwalkerMenu = new Menu("Orbwalker", "orbwalkermenu");
            Program.ComboMenu.Add(new Menu("QCombo", "Auto Tumble").SetValue(true));
            Program.ComboMenu.Add(new MenuBool("QMode", "Q Mode: ").SetValue(new MenuList(new[] { "PRADA", "TO MOUSE" })));
            Program.ComboMenu.Add(new MenuBool("QMinDist", "Min dist from enemies").SetValue(new MenuSlider(375, 325, 525)));
            Program.ComboMenu.Add(new MenuBool("QOrderBy", "Q to position").SetValue(new MenuList(new[] {"CLOSETOMOUSE", "CLOSETOTARGET"})));
            Program.ComboMenu.Add(new MenuBool("QChecks", "Q Safety Checks").SetValue(true));
            Program.ComboMenu.Add(new MenuBool("EQ", "Q After E").SetValue(false));
            Program.ComboMenu.Add(new MenuBool("QR", "Q after Ult").SetValue(true));
            Program.ComboMenu.Add(new MenuBool("OnlyQinCombo", "Only Q in COMBO").SetValue(false));
            Program.ComboMenu.Add(new MenuBool("FocusTwoW", "Focus 2 W Stacks").SetValue(true));
            Program.ComboMenu.Add(new MenuBool("ECombo", "Auto Condemn").SetValue(true));
            Program.ComboMenu.Add(new MenuBool("ManualE", "Semi-Manual Condemn").SetValue(new MenuKeyBind('E', System.Windows.Forms.Keys.E, KeyBindType.Press)));
            Program.ComboMenu.Add(new MenuBool("EMode", "E Mode").SetValue(new MenuList(new[] {"PRADASMART", "PRADAPERFECT", "MARKSMAN", "SHARPSHOOTER", "GOSU", "VHR", "PRADALEGACY", "FASTEST", "OLDPRADA"})));
            Program.ComboMenu.Add(new MenuSlider("EPushDist", "E Push Distance").SetValue(new Slider(475, 300, 475)));
            Program.ComboMenu.Add(new MenuSlider("EHitchance", "E % Hitchance").SetValue(new Slider(50)));
            Program.ComboMenu.Add(new MenuBool("RCombo", "Auto Ult").SetValue(false));
            Program.EscapeMenu.Add(new MenuBool("QUlt", "Smart Q-Ult").SetValue(true));
            Program.EscapeMenu.Add(new MenuBool("EInterrupt", "Use E to Interrupt").SetValue(true));

            var antigcmenu = Program.EscapeMenu.Attach(new MenuBool("Anti-Gapcloser", "antigapcloser"));
            foreach (var hero in Heroes.EnemyHeroes)
            {
                var championName = hero.CharacterName;
                antigcmenu.Add(
                    new MenuItem("antigc" + championName, championName).SetValue(Lists.CancerChamps.Any(entry => championName == entry)));
            }

            Program.LaneClearMenu.Add(new MenuBool("QLastHit", "Use Q to Lasthit").SetValue(true));
            Program.LaneClearMenu.Add(new MenuSlider("QLastHitMana", "Min Mana% for Q Lasthit").SetValue(new Slider(45)));
            Program.LaneClearMenu.Add(new MenuBool("EJungleMobs", "Use E on Jungle Mobs").SetValue(true));
        }

        public static void InitOrbwalker()
        {
            Orbwalker.OnAction += Orbwalker_OnAction;

        }

        public static void FinishMenuInit()
        {
            Program.MainMenu.Attach(Program.ComboMenu);
            Program.MainMenu.Attach(Program.LaneClearMenu);
            Program.MainMenu.Attach(Program.EscapeMenu);
            Program.MainMenu.Attach(Program.DrawingsMenu);
            Program.MainMenu.Attach(Program.OrbwalkerMenu);
        }
    }
}