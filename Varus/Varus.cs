using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;
using System.Drawing;

namespace TrickSTRR.AIO.Varus
{
    class Varus
    {
        private static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static Spell Q, W, E, R;
        private static Menu Config;
        public static void VarusMain()
        {
            GameEvent.OnGameLoad += GameEventOnOnGameLoad;
        }

        private static void GameEventOnOnGameLoad()
        {
           if(Player.CharacterName != "Varus") return;
            CreateSpells();
            CreateMenu();
            CreateEvents();
            Game.Print("<font color=\"#008aff\"> TrickSTRR Varus </font> ver 1.0 by <font color=\"#FF0000\"> TrickSTRR</font> - <font color=\"#00BFFF\">Loaded</font>");
            Game.Print("Based on xDreamms Varus - Thanks to ProDragon for Updating it!");

        }
        private static void CreateEvents()
        {
            Drawing.OnDraw += DrawingOnOnDraw;
        }

        private static void DrawingOnOnDraw(EventArgs args)
        {
            var drawOff = DrawOff;
            var drawQ = DrawQ;
            var drawW = DrawW;
            var drawE = DrawE;
            var drawR = DrawR;

            if (drawOff)
            {
                return;
            }

            if (drawQ)
            {
                if (Q.Level > 0)
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position,
                        Q.Range,
                       Q.IsReady() ? Color.Green : Color.Red);
                }
            }

            if (drawW)
            {
                if (W.Level > 0)
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position,
                        W.Range,
                        W.IsReady() ? Color.Green : Color.Red);
                }
            }

            if (drawE)
            {
                if (E.Level > 0)
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position,
                        E.Range,
                        E.IsReady() ? Color.Green : Color.Red);
                }
            }

            if (drawR)
            {
                if (R.Level > 0)
                {
                    Render.Circle.DrawCircle(
                        ObjectManager.Player.Position,
                       R.Range,
                        R.IsReady() ? Color.Green : Color.Red);
                }
            }
        }

        private static void GameOnOnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode == OrbwalkerMode.Combo)
            {
                Combo();
            }

            if (Orbwalker.ActiveMode == OrbwalkerMode.LaneClear)
            {
                LaneClear();
                JungleClear();
            }

            if (Orbwalker.ActiveMode == OrbwalkerMode.Harass)
            {
                Harass();
            }

            Killsteal();

            var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);

            if (R.IsReady() && target.IsValidTarget()
                && SemiR)
            {
                R.CastOnUnit(target);
            }
        }
        private static void Killsteal()
        {
            if (MiscKillSteal && Q.IsReady())
            {
                foreach (
                    var target in
                        GameObjects.EnemyHeroes.Where(
                            enemy =>
                                enemy.IsValidTarget() && Player.GetSpellDamage(enemy, Q.Slot) > enemy.Health &&
                                Player.Distance(enemy.Position) <= Q.ChargedMaxRange))
                {
                    if (!Q.IsCharging)
                    {
                        Q.StartCharging();
                    }

                    if (Q.IsCharging)
                    {
                        var prediction = Q.GetPrediction(target);
                        if (prediction.Hitchance >= HitChance.VeryHigh)
                        {
                            Q.ShootChargedSpell(prediction.CastPosition);
                        }
                    }
                }
            }
        }

        private static void LaneClear()
        {
            var useQ = UseQFarm;
            var useE = UseEFarm;
            var countMinions = CountMinionsQ;
            var countMinionsE = CountMinionsE;
            var minmana = Minmanaclear;

            if (Player.ManaPercent < minmana)
            {
                return;
            }

            var minions = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range)).ToList();
            if (minions.Count <= 0)
            {
                return;
            }

            if (Q.IsReady() && useQ)
            {
                var allMinions = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range)).ToList();
                {
                    foreach (var minion in
                         allMinions.Where(minion => minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q)))
                    {
                        var killcount = 0;

                        foreach (var colminion in minions)
                        {
                            if (colminion.Health <= Q.GetDamage(colminion))
                            {
                                killcount++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (killcount >= countMinions)
                        {
                            if (minion.IsValidTarget())
                            {
                                Q.Cast(minion);
                                return;
                            }
                        }
                    }
                }
            }

            if (!useE || !E.IsReady())
            {
                return;
            }

            var minionkillcount =
                minions.Count(x => E.CanCast(x) && x.Health <= E.GetDamage(x));

            if (minionkillcount >= countMinionsE)
            {
                foreach (var minion in minions.Where(x => x.Health <= E.GetDamage(x)))
                {
                    E.Cast(minion);
                }
            }
        }
        private static void JungleClear()
        {
            var useQ = UseQFarmJungle;
            var useE = UseEFarmJungle;
            var minmana = Minmanaclear;
            var minions = GameObjects.Jungle.Where(x => x.IsValidTarget(700) && x.Team == GameObjectTeam.Neutral)
                .OrderBy(x => x.MaxHealth);

            if (Player.ManaPercent >= minmana)
            {
                foreach (var minion in minions)
                {
                    if (Q.IsReady() && useQ)
                    {
                        if (!Q.IsCharging)
                        {
                            Q.StartCharging();
                        }

                        if (Q.IsCharging && Q.Range >= Q.ChargedMaxRange)
                            Q.ShootChargedSpell(minion.Position);
                    }

                    if (E.IsReady() && useE)
                    {
                        E.CastOnUnit(minion);
                    }
                }
            }
        }
        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.ChargedMaxRange, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            var harassQ = HarassQ;
            var harassE = HarassE;
            var minmana = Minmanaharass;

            if (Player.ManaPercent > minmana)
            {
                if (harassE && E.IsReady())
                {
                    E.Cast(target);
                }

                if (harassQ && Q.IsReady())
                {
                    if (!Q.IsCharging)
                    {
                        Q.StartCharging();
                    }

                    if (Q.IsCharging)
                    {
                        var prediction = Q.GetPrediction(target);
                        if (prediction.Hitchance >= HitChance.VeryHigh)
                        {
                            Q.ShootChargedSpell(prediction.CastPosition);
                        }
                    }
                }
            }
        }

        private static int GetWStacks(AIBaseClient target)
        {
            return target.GetBuffCount("varuswdebuff");
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget((Q.ChargedMaxRange + Q.Width) * 1.1f, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            var stackCount = ComboStackCount;
            var rCount = ComboRCount;
            var comboQ = ComboQ;
            var comboE = ComboE;
            var comboR = ComboR;
            var alwaysQ = ComboAlwaysQ;

            Items(target);

            if (E.IsReady() && !Q.IsCharging && comboE)
            {
                if (Player.GetSpellDamage(target, E.Slot) > target.Health || GetWStacks(target) >= 1)
                {
                    var prediction = E.GetPrediction(target);
                    if (prediction.Hitchance >= HitChance.VeryHigh)
                    {
                        E.Cast(prediction.CastPosition);
                    }
                }
            }

            if (Q.IsReady() && comboQ)
            {
                if (Q.IsCharging || alwaysQ
                    || target.Distance(Player) > target.GetRealAutoAttackRange() * 1.2f
                    || GetWStacks(target) >= stackCount
                    || Player.GetSpellDamage(target, Q.Slot) > target.Health)
                {
                    if (W.IsReady() && ComboW)
                    {
                        W.Cast();
                    }
                    if (!Q.IsCharging)
                    {
                        Q.StartCharging();
                    }

                    if (Q.IsCharging)
                    {
                        var prediction = Q.GetPrediction(target);
                        if (prediction.Hitchance >= HitChance.VeryHigh)
                        {
                            Q.ShootChargedSpell(prediction.CastPosition);
                        }
                    }
                }
            }

            if (R.IsReady() && !Q.IsCharging
                && target.IsValidTarget(R.Range) && comboR)
            {
                var pred = R.GetPrediction(target);
                if (pred.Hitchance >= HitChance.VeryHigh)
                {
                    var ultimateHits = GameObjects.EnemyHeroes.Where(x => x.Distance(target) <= 450f).ToList();
                    if (ultimateHits.Count >= rCount)
                    {
                        R.Cast(pred.CastPosition);
                    }
                }
            }
        }
        private static void Items(AIBaseClient target)
        {
            var botrk = new Items.Item(ItemId.Blade_of_the_Ruined_King, 550);
            var ghost = new Items.Item(ItemId.Youmuus_Ghostblade, float.MaxValue);
            var cutlass = new Items.Item(ItemId.Bilgewater_Cutlass, 550);

            var useYoumuu = ItemsYoumuu;
            var useCutlass = ItemsCutlass;
            var useBlade = ItemsBlade;

            var useBladeEhp = ItemsBladeEnemyEHP;
            var useBladeMhp = ItemsBladeEnemyMHP;

            if (botrk.IsReady && botrk.IsOwned() && botrk.IsInRange(target)
                && target.HealthPercent <= useBladeEhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (botrk.IsReady && botrk.IsOwned() && botrk.IsInRange(target)
                && Player.HealthPercent <= useBladeMhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (cutlass.IsReady && cutlass.IsOwned() && cutlass.IsInRange(target)
                && target.HealthPercent <= useBladeEhp && useCutlass)
            {
                cutlass.Cast(target);
            }

            if (ghost.IsReady && ghost.IsOwned() && target.IsValidTarget(Q.Range) && useYoumuu)
            {
                ghost.Cast();
            }
        }
        private static void CreateMenu()
        {
            Config = new Menu("Varus", "TrickSTRR Varus", true);

            var comboMenu = new Menu("Combo", "Combo");
            Helper.AddMenuBool(comboMenu, "ComboQ", "Use Q");
            Helper.AddMenuBool(comboMenu, "ComboAlwaysQ", "Always Q");
            Helper.AddMenuBool(comboMenu, "ComboW", "Use W before Q", false);
            Helper.AddMenuBool(comboMenu, "ComboE", "Use E");
            Helper.AddMenuBool(comboMenu, "ComboR", "Use R");
            Helper.AddMenuSlider(comboMenu, "ComboRCount", "R when enemies >= ", 1, 1, 5);
            Helper.AddMenuSlider(comboMenu, "ComboStackCount", "Q when stacks >= ", 3, 1, 3);
            Helper.AddMenuKeyBind(comboMenu, "SemiR", "Semi-manual cast R key", Keys.T, KeyBindType.Press);
            Config.Add(comboMenu);

            //Harass Menu
            var harassMenu = new Menu("Harass", "Harass");
            Helper.AddMenuBool(harassMenu, "HarassQ", "Use Q");
            Helper.AddMenuBool(harassMenu, "HarassE", "Use E");
            Helper.AddMenuSlider(harassMenu, "Minmanaharass", "Mana needed to clear ", 55);
            Config.Add(harassMenu);

            var itemsMenu = new Menu("Items", "Items");
            Helper.AddMenuBool(itemsMenu, "ItemsYoumuu", "Use Youmuu's Ghostblade");
            Helper.AddMenuBool(itemsMenu, "ItemsCutlass", "Use Cutlass");
            Helper.AddMenuBool(itemsMenu, "ItemsBlade", "Use Blade of the Ruined King");
            Helper.AddMenuSlider(itemsMenu, "ItemsBladeEnemyEHP", "Enemy HP Percentage", 80);
            Helper.AddMenuSlider(itemsMenu, "ItemsBladeEnemyMHP", "My HP Percentage", 80);
            Config.Add(itemsMenu);

            var clearMenu = new Menu("Clear", "Clear");
            Helper.AddMenuBool(clearMenu, "UseQFarm", "Use Q");
            Helper.AddMenuSlider(clearMenu, "CountMinionsQ", "Killable minions with Q >=", 2, 1, 5);
            Helper.AddMenuBool(clearMenu, "UseEFarm", "Use E");
            Helper.AddMenuSlider(clearMenu, "CountMinionsE", "Killable minions with E >=", 2, 1, 5);
            Helper.AddMenuBool(clearMenu, "UseQFarmJungle", "Use Q in jungle");
            Helper.AddMenuBool(clearMenu, "UseEFarmJungle", "Use E in jungle");
            Helper.AddMenuSlider(clearMenu, "Minmanaclear", "Mana needed to clear ", 55);
            Config.Add(clearMenu);

            var miscMenu = new Menu("Misc", "Misc");
            Helper.AddMenuBool(miscMenu, "DrawOff", "Turn drawings off", false);
            Helper.AddMenuBool(miscMenu, "DrawQ", "Draw Q");
            Helper.AddMenuBool(miscMenu, "DrawW", "Draw W");
            Helper.AddMenuBool(miscMenu, "DrawE", "Draw E");
            Helper.AddMenuBool(miscMenu, "DrawR", "Draw R");
            Helper.AddMenuBool(miscMenu, "MiscKillSteal", "Killsteal");
            Config.Add(miscMenu);
            Config.Attach();
        }

        private static void CreateSpells()
        {
            Q = new Spell(SpellSlot.Q, 925);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R, 1100);

            Q.SetSkillshot(.25f, 70f, 1650f, false, false, SkillshotType.Line);
            E.SetSkillshot(0.35f, 120, 1500, false, false, SkillshotType.Circle);
            R.SetSkillshot(.25f, 120f, 1950f, false, false, SkillshotType.Line);

            Q.SetCharged("VarusQ", "VarusQ", 250, 1600, 1.2f);

        }

        public static bool ComboW { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboW"); } }

        public static bool ComboQ { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboQ"); } }
        public static bool ComboAlwaysQ { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboAlwaysQ"); } }
        public static bool ComboE { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboE"); } }
        public static bool ComboR { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboR"); } }
        public static int ComboRCount { get { return Helper.GetMenuSliderValue(Config, "Combo", "ComboRCount"); } }
        public static int ComboStackCount { get { return Helper.GetMenuSliderValue(Config, "Combo", "ComboStackCount"); } }
        public static bool SemiR { get { return Helper.GetMenuKeyBindValue(Config, "Combo", "SemiR"); } }
        public static bool HarassQ { get { return Helper.GetMenuBoolValue(Config, "Harass", "HarassQ"); } }
        public static bool HarassE { get { return Helper.GetMenuBoolValue(Config, "Harass", "HarassE"); } }
        public static int Minmanaharass { get { return Helper.GetMenuSliderValue(Config, "Harass", "Minmanaharass"); } }
        public static bool ItemsYoumuu { get { return Helper.GetMenuBoolValue(Config, "Items", "ItemsYoumuu"); } }
        public static bool ItemsCutlass { get { return Helper.GetMenuBoolValue(Config, "Items", "ItemsCutlass"); } }
        public static bool ItemsBlade { get { return Helper.GetMenuBoolValue(Config, "Items", "ItemsBlade"); } }
        public static int ItemsBladeEnemyEHP { get { return Helper.GetMenuSliderValue(Config, "Items", "ItemsBladeEnemyEHP"); } }
        public static int ItemsBladeEnemyMHP { get { return Helper.GetMenuSliderValue(Config, "Items", "ItemsBladeEnemyMHP"); } }
        public static bool UseQFarm { get { return Helper.GetMenuBoolValue(Config, "Clear", "UseQFarm"); } }
        public static int CountMinionsQ { get { return Helper.GetMenuSliderValue(Config, "Clear", "CountMinionsQ"); } }
        public static bool UseEFarm { get { return Helper.GetMenuBoolValue(Config, "Clear", "UseEFarm"); } }
        public static int CountMinionsE { get { return Helper.GetMenuSliderValue(Config, "Clear", "CountMinionsE"); } }
        public static bool UseQFarmJungle { get { return Helper.GetMenuBoolValue(Config, "Clear", "UseQFarmJungle"); } }
        public static bool UseEFarmJungle { get { return Helper.GetMenuBoolValue(Config, "Clear", "UseEFarmJungle"); } }
        public static int Minmanaclear { get { return Helper.GetMenuSliderValue(Config, "Clear", "Minmanaclear"); } }
        public static bool DrawOff { get { return Helper.GetMenuBoolValue(Config, "Misc", "DrawOff"); } }
        public static bool DrawQ { get { return Helper.GetMenuBoolValue(Config, "Misc", "DrawQ"); } }
        public static bool DrawW { get { return Helper.GetMenuBoolValue(Config, "Misc", "DrawW"); } }
        public static bool DrawE { get { return Helper.GetMenuBoolValue(Config, "Misc", "DrawE"); } }
        public static bool DrawR { get { return Helper.GetMenuBoolValue(Config, "Misc", "DrawR"); } }

        public static bool MiscKillSteal { get { return Helper.GetMenuBoolValue(Config, "Misc", "MiscKillSteal"); } }
    }
}
