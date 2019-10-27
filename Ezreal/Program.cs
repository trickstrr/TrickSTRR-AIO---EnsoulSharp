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

namespace TrickSTRR.AIO.Ezreal
{
    class Program
    {
        public static Menu Config;
        public static List<Spell> SpellList = new List<Spell>();
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static Spell R1;
        public static float QMANA;
        public static float WMANA;
        public static float EMANA;
        public static float RMANA;
        public static Items.Item Potion = new Items.Item(2003, 0);
        private static AIHeroClient Player;
        public static void Main()
        {
            GameEvent.OnGameLoad += OnGameLoad;
        }
        private static void OnGameLoad()
        {
            Player = ObjectManager.Player;
            if (Player.CharacterName != "Ezreal")
            {
                return;
            }
            CreateSpells();
            CreateMenu();
            CreateEvents();
        }
        public static void CreateEvents()
        {
            Drawing.OnDraw += DrawingOnOnDraw;
            Gapcloser.OnGapcloser += GapcloserOnOnGapcloser;
            Game.Print("<font color=\"#008aff\">E</font>zrealist ver 1.0 by <font color=\"#FF0000\"> xDreams</font> - <font color=\"#00BFFF\">Loaded</font>");
        }



        private static void GapcloserOnOnGapcloser(AIHeroClient sender, Gapcloser.GapcloserArgs args)
        {
            if (Helper.GetMenuBoolValue(Config, "EConfig", "AGC") && E.IsReady() && ObjectManager.Player.Mana > RMANA + EMANA && ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range).CountEnemyHeroesInRange(400) < 3 && sender.IsEnemy)
            {
                var Target = sender;
                if (Target.IsValidTarget(E.Range))
                {
                    E.Cast(ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range), true);
                }
            }
            return;
        }


        private static bool Farm
        {
            get { return Orbwalker.ActiveMode == OrbwalkerMode.LaneClear || Orbwalker.ActiveMode == OrbwalkerMode.Harass || Orbwalker.ActiveMode == OrbwalkerMode.LastHit; }
        }
        public static void ManaManager()
        {
            QMANA = Q.Instance.ManaCost;
            WMANA = W.Instance.ManaCost;
            EMANA = E.Instance.ManaCost;

            if (!R.IsReady())
                RMANA = QMANA - ObjectManager.Player.Level * 2;
            else
                RMANA = R.Instance.ManaCost; ;

            if (Farm)
                RMANA = RMANA + ObjectManager.Player.CountEnemyHeroesInRange(2500) * 20;

            if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.2)
            {
                QMANA = 0;
                WMANA = 0;
                EMANA = 0;
                RMANA = 0;
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
                Clear();
            }
            if (Orbwalker.ActiveMode == OrbwalkerMode.Harass)
            {
                Harass();
            }
            if (Helper.GetMenuKeyBindValue(Config, "Combo", "UseRmanuel"))
            {
                ManuelUlt();
            }
            PotionManager();
            QStack();
        }

        private static void Harass()
        {
            if (Q.IsReady() && Helper.GetMenuBoolValue(Config, "Harass", "HarassUseQ"))
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                Q.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                return;
            }
            if (W.IsReady() && Helper.GetMenuBoolValue(Config, "Harass", "HarassUseW"))
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                W.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                return;
            }
        }

        public static void PotionManager()
        {
            if (Helper.GetMenuBoolValue(Config, "Misc", "usepotions") && !ObjectManager.Player.InFountain() && !ObjectManager.Player.HasBuff("Recall"))
            {
                if (Potion.IsReady && !ObjectManager.Player.HasBuff("RegenerationPotion"))
                {
                    if (ObjectManager.Player.CountEnemyHeroesInRange(700) > 0 && ObjectManager.Player.Health + 200 < ObjectManager.Player.MaxHealth)
                        Potion.Cast();
                    else if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.6)
                        Potion.Cast();
                }
            }
        }


        private static void QStack()
        {
            if (!Farm && Helper.GetMenuBoolValue(Config, "Misc", "stack") && !ObjectManager.Player.HasBuff("Recall") && ObjectManager.Player.Mana > ObjectManager.Player.MaxMana * 0.95 && Orbwalker.ActiveMode != OrbwalkerMode.Combo)
            {
                if (Player.HasItem(3070) || Player.HasItem(3004))
                {
                    Q.Cast(Player.Position);
                }
            }
        }

        public static void ManuelUlt()
        {
            Orbwalker.Move(Game.CursorPos);

            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (R.IsReady() && target.IsValidTarget())
            {
                R.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }
        }
        private static bool ValidUlt(AIHeroClient target)
        {
            if (target.HasBuffOfType(BuffType.PhysicalImmunity)
                || target.HasBuffOfType(BuffType.SpellImmunity)
                || target.HasBuffOfType(BuffType.Invulnerability)
                || target.HasBuffOfType(BuffType.SpellShield)
            )
                return false;
            else
                return true;
        }
        private static void Clear()
        {
            var minions = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range) && x.IsMinion());
            if (minions.Count() > 0)
            {
                var mob = minions.FirstOrDefault();
                if (Helper.GetMenuBoolValue(Config, "LaneClear", "UseQ") && Q.IsReady())
                {
                    Q.Cast(mob);
                    return;
                }
                if (Helper.GetMenuBoolValue(Config, "LaneClear", "UseW") && W.IsReady())
                {
                    W.Cast(mob);
                    return;
                }
            }
            // get Mob
            var mobs = GameObjects.Jungle.Where(x => x.IsValidTarget(Q.Range));
            if (mobs.Count() > 0)
            {
                var mob = mobs.FirstOrDefault();
                if (Helper.GetMenuBoolValue(Config, "JungleClear", "UseQ") && Q.IsReady())
                {
                    Q.Cast(mob);
                    return;
                }
                if (Helper.GetMenuBoolValue(Config, "JungleClear", "UseW") && W.IsReady())
                {
                    W.Cast(mob);
                    return;
                }
            }

        }

        private static void Combo()
        {
            if (Q.IsReady() && Helper.GetMenuBoolValue(Config, "Combo", "UseQ"))
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                Q.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                return;
            }
            if (W.IsReady() && Helper.GetMenuBoolValue(Config, "Combo", "UseW"))
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                W.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                return;

            }
            if (R.IsReady() && Helper.GetMenuBoolValue(Config, "Combo", "UseR"))
            {
                var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                if (ValidUlt(target) && Player.Distance(target) >= 700)
                {
                    float predictedHealth = target.Health + target.HPRegenRate * 2;
                    double Rdmg = R.GetDamage(target);
                    if (Rdmg > predictedHealth)
                    {
                        R.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                        return;
                    }
                }
            }
            if (E.IsReady() && Helper.GetMenuBoolValue(Config, "Combo", "UseE"))
            {
                ManaManager();

                if (ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range)
                        .CountEnemyHeroesInRange(500) < 4)
                {
                    E.Cast(ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range), true);
                    return;
                }

            }
        }

        private static void DrawingOnOnDraw(EventArgs args)
        {
            if (Helper.GetMenuBoolValue(Config, "Drawings", "DrawQ"))
            {
                // Draw Circle
                Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.Red);
            }
            if (Helper.GetMenuBoolValue(Config, "Drawings", "DrawW"))
            {
                // Draw Circle
                Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.DarkRed);
            }
            if (Helper.GetMenuBoolValue(Config, "Drawings", "DrawE"))
            {
                // Draw Circle
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.Yellow);
            }

        }

        public static void CreateSpells()
        {
            Q = new Spell(SpellSlot.Q, 1200);
            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 475);
            R = new Spell(SpellSlot.R, 3000f);
            R1 = new Spell(SpellSlot.R, 3000f);
            Q.SetSkillshot(0.25f, 50f, 2000f, true, false, SkillshotType.Line);
            W.SetSkillshot(0.25f, 80f, 1600f, false, false, SkillshotType.Line);
            R.SetSkillshot(1.2f, 160f, 2000f, false, false, SkillshotType.Line);
            R1.SetSkillshot(1.2f, 200f, 2000f, false, false, SkillshotType.Circle);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);
            SpellList.Add(R1);
        }
        public static void CreateMenu()
        {
            Config = new Menu("Ezreal", "Ezreal", true);

            Menu comboMenu = new Menu("Combo", "Combo");
            Helper.AddMenuBool(comboMenu, "UseQ", "Use Q", true);
            Helper.AddMenuBool(comboMenu, "UseW", "Use W", true);
            Helper.AddMenuBool(comboMenu, "UseE", "Use E", false);
            Helper.AddMenuBool(comboMenu, "UseR", "Use R", true);
            Helper.AddMenuKeyBind(comboMenu, "UseRmanuel", "Use R manuel", Keys.T, KeyBindType.Press);
            Config.Add(comboMenu);

            Menu laneClearMenu = new Menu("LaneClear", "Lane Clear");
            Helper.AddMenuBool(laneClearMenu, "UseQ", "Use Q", true);
            Helper.AddMenuBool(laneClearMenu, "UseW", "Use W", false);
            Config.Add(laneClearMenu);



            Menu jungleClearMenu = new Menu("JungleClear", "Jungle Clear");
            Helper.AddMenuBool(jungleClearMenu, "UseQ", "Use Q", true);
            Helper.AddMenuBool(jungleClearMenu, "UseW", "Use W", false);
            Config.Add(jungleClearMenu);

            Menu miscMenu = new Menu("Misc", "Misc");
            Helper.AddMenuBool(miscMenu, "AntiGapcloserE", "AntiGapcloserE", true);
            Helper.AddMenuBool(miscMenu, "stack", "Stack Q", true);
            Helper.AddMenuBool(miscMenu, "usepotions", "Use Potions", true);
            Helper.AddMenuBool(miscMenu, "autoQ", "Auto Q", false);
            Config.Add(miscMenu);

            Menu harassMenu = new Menu("Harass", "Harass");
            Helper.AddMenuBool(harassMenu, "HarassUseQ", "Use Q", true);
            Helper.AddMenuBool(harassMenu, "HarassUseW", "Use W", true);
            Config.Add(harassMenu);

            Menu drawMenu = new Menu("Drawings", "Drawings");
            Helper.AddMenuBool(drawMenu, "DrawQ", "Draw Q", true);
            Helper.AddMenuBool(drawMenu, "DrawW", "Draw W", true);
            Helper.AddMenuBool(drawMenu, "DrawE", "Draw E", true);
            Config.Add(drawMenu);


            Config.Attach();

        }
    }
}
