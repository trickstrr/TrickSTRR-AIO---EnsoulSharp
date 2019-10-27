using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;
using SharpDX;

namespace TrickSTRR.AIO.Sylas
{
    class Program
    {
        private static Spell Q, W, E, E2, R, R2;

        private static Menu Config;
        private static SpellSlot Ignite;


        private static AIHeroClient Player = ObjectManager.Player;
        public static void Main()
        {
            GameEvent.OnGameLoad += GameEventOnOnGameLoad;
        }

        private static void GameEventOnOnGameLoad()
        {
            if (Player.CharacterName != "Sylas") return;
            CreateSpells();
            CreateMenu();
            CreateEvents();
        }

        private static void CreateEvents()
        {
            Drawing.OnDraw += DrawingOnOnDraw;
        }

        private static void DrawingOnOnDraw(EventArgs args)
        {
            if (DrawQ)
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Red);
            }
            if (DrawW)
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, System.Drawing.Color.Yellow);
            }
            if (DrawE)
            {
                Render.Circle.DrawCircle(Player.Position, E.Range + E2.Range, System.Drawing.Color.Blue);
            }
        }

        private static void GameOnOnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode == OrbwalkerMode.Combo)
            {
                Combo();
            }
            if (Orbwalker.ActiveMode == OrbwalkerMode.Harass)
            {
                Harass();
            }
            if (Orbwalker.ActiveMode == OrbwalkerMode.LaneClear)
            {
                LaneClear();
                JugnleClear();
            }
            KillSteal();
        }

        private static void KillSteal()
        {
            if (MiscIgnite && Ignite.IsReady())
            {
                var target = TargetSelector.GetTarget(600, DamageType.True);
                if (target != null && CastIgnite(target))
                {
                    return;
                }
            }
            if (MiscKillStealQ && Q.IsReady())
            {
                var target = GameObjects.EnemyHeroes.Where(x =>
                    x.IsValidTarget(Q.Range) && Player.GetSpellDamage(x, SpellSlot.Q) > x.Health);
                if (target.Any())
                {
                    Q.Cast(target.FirstOrDefault());
                }
            }
            if (MiscKillStealW && W.IsReady())
            {
                var target = GameObjects.EnemyHeroes.Where(x =>
                    x.IsValidTarget(W.Range) && Player.GetSpellDamage(x, SpellSlot.W) > x.Health);
                if (target.Any())
                {
                    W.CastOnUnit(target.FirstOrDefault());
                }
            }
            if (MiscKillStealE && E.IsReady())
            {
                var target = GameObjects.EnemyHeroes.Where(x =>
                    x.IsValidTarget(E.Range + E2.Range) && Player.GetSpellDamage(x, SpellSlot.Q) > x.Health);
                if (target.Any())
                {
                    var pos = target.FirstOrDefault().Position.Extend(Player.Position, -500);
                    if (!IsSecondE() && E.IsReady())
                    {
                        E.Cast(pos);
                    }
                    else if (IsSecondE() && E.IsReady())
                    {
                        E.Cast(target.FirstOrDefault().Position);
                    }
                }
            }
        }
        public static bool CastIgnite(AIHeroClient target)
        {
            return Ignite.IsReady() && target.IsValidTarget(600)
                   && target.Health + 5 < Player.GetSummonerSpellDamage(target, SummonerSpell.Ignite)
                   && Player.Spellbook.CastSpell(Ignite, target);
        }


        private static void JugnleClear()
        {
            var mobs = GameObjects.Jungle.Where(x => x.IsValidTarget(Q.Range) && x.Team == GameObjectTeam.Neutral);
            if (mobs == null) return;
            if (JungleClearQ && Q.IsReady())
            {
                Q.Cast(mobs.FirstOrDefault());
            }
            if (JungleClearW && W.IsReady() && mobs.FirstOrDefault().IsValidTarget(W.Range))
            {
                W.CastOnUnit(mobs.FirstOrDefault());
            }
            if (JungleClearE && E.IsReady())
            {
                var pos = mobs.FirstOrDefault().Position.Extend(Player.Position, -500);
                if (!IsSecondE() && E.IsReady())
                {
                    E.Cast(pos);
                }
                else if (IsSecondE() && E.IsReady())
                {
                    E.Cast(mobs.FirstOrDefault().Position);
                }
            }
        }

        private static void LaneClear()
        {
            var minios = GameObjects.EnemyMinions.Where(x => x.IsValidTarget(Q.Range) && x.IsEnemy).ToList();
            if (minios == null) return;
            if (LaneClearQ && Q.IsReady())
            {
                Q.Cast(minios.FirstOrDefault());
            }
            if (LaneClearW && W.IsReady() && minios.FirstOrDefault().IsValidTarget(W.Range))
            {
                W.Cast(minios.FirstOrDefault());
            }
        }

        private static void Harass()
        {
            if (HarassQ && Q.IsReady())
            {
                var target = GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(Q.Range) && x.IsEnemy).FirstOrDefault();
                if (target != null)
                {
                    Q.Cast(target);
                }
            }

        }

        private static void Combo()
        {
            var target = GameObjects.EnemyHeroes.Where(x => x.Distance(Player.Position) < E.Range + E2.Range && x.IsEnemy).FirstOrDefault();
            if (target == null) return;
            Vector3 pos = target.Position.Extend(Player.Position, -500);

            if (ComboQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                Q.Cast(target);
            }
            if (ComboE && E.IsReady())
            {
                if (!IsSecondE() && target.Distance(Player) > DontEEnemyRange)
                {
                    E.Cast(pos);
                }
                if (IsSecondE())
                {
                    //E2.UpdateSourcePosition(pos,Player.Position);
                    if (ComboEMode == "Only target")
                    {
                        E2.Cast(target);
                    }
                    else
                    {
                        E2.Cast(target.Position);
                    }
                }
            }

            if (ComboW && W.IsReady() && target.IsValidTarget(W.Range))
            {
                if (ComboWHealth && Player.HealthPercent < 40)
                {
                    W.CastOnUnit(target);
                }
                else if (!ComboWHealth)
                {
                    W.CastOnUnit(target);
                }
                
            }
            if (R.IsReady() && NotStolenR())
            {
                var Rtarget = GameObjects.EnemyHeroes
                    .Where(x => x.IsValidTarget(R.Range) && x.IsEnemy && TargetIsReady(x) && !Config["Combo"]["DontUlt" + x.Spellbook.GetSpell(SpellSlot.R).Name].GetValue<MenuBool>().Enabled).FirstOrDefault();
                if (Rtarget != null && R.CastOnUnit(target) && TargetIsReady(Rtarget))
                {
                    return;
                }

            }
            if (!NotStolenR())
            {
                switch (R2.Name)
                {
                    case "UFSlash":
                        MalphiteUlt();
                        break;
                }
            }

        }

        private static void MalphiteUlt()
        {
            R2 = new Spell(SpellSlot.R, 1000);
            R2.SetSkillshot(0, 50, 1500, true, false, SkillshotType.Circle);
            var target = GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(1000));
            foreach (var enemy in target)
            {
                var pred = R2.GetPrediction(enemy, true);
                if (pred.Hitchance >= HitChance.VeryHigh)
                {
                    R2.Cast(pred.CastPosition);
                    return;
                }
            }

        }

        public static bool TargetIsReady(AIHeroClient target)
        {
            return !target.HasBuff("SylasR");
        }
        public static bool IsSecondE()
        {
            return E2.Name == "SylasE2";
        }
        public static bool NotStolenR()
        {
            return R.Name == "SylasR";
        }
        private static void CreateSpells()
        {
            Q = new Spell(SpellSlot.Q, 800);
            W = new Spell(SpellSlot.W, 400);
            E = new Spell(SpellSlot.E, 400);
            E2 = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 950);
            R2 = new Spell(SpellSlot.R);
            Ignite = Player.GetSpellSlot("summonerdot");

            Q.SetSkillshot(0.4f, 70, 1800, false, true, SkillshotType.Line);
            E2.SetSkillshot(0.25f, 60, 1600, true, true, SkillshotType.Line);
            R.SetTargetted(250, 1000);
        }

        private static void CreateMenu()
        {
            Config = new Menu("xDreammsSylas", "xDreamms Sylas", true);

            //Combo Menu
            var comboMenu = new Menu("Combo", "Combo");
            Helper.AddMenuBool(comboMenu, "ComboQ", "Use Q");
            Helper.AddMenuBool(comboMenu, "ComboW", "Use W");
            Helper.AddMenuBool(comboMenu,"ComboWHealth","Use W only Sylas has %40 HP",false);
            Helper.AddMenuBool(comboMenu, "ComboE", "Use E");
            Helper.AddMenuList(comboMenu, "ComboEMode", "E mode: ", new[] { "Only target", "Closer target" });
            Helper.AddMenuSlider(comboMenu, "DontEEnemyRange", "Use use E If Enemy in > ", 400, 0, 1200);
            Helper.AddMenuBool(comboMenu, "ComboR", "Use R to steal enemies's ultimates.");
            comboMenu.Add(new MenuSeparator("DontSteal", "Dont Steal these ultimates."));
            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                Helper.AddMenuBool(comboMenu, "DontUlt" + enemy.Spellbook.GetSpell(SpellSlot.R).Name, enemy.CharacterName + " " + enemy.Spellbook.GetSpell(SpellSlot.R).Name, false);
            }
            Config.Add(comboMenu);

            //Harass Menu
            var harassMenu = new Menu("Harass", "Harass");
            Helper.AddMenuBool(harassMenu, "HarassQ", "Use Q");
            Config.Add(harassMenu);

            //LaneClear Menu
            var laneClearMenu = new Menu("LaneClear", "LaneClear");
            Helper.AddMenuBool(laneClearMenu, "LaneClearQ", "Use Q");
            Helper.AddMenuBool(laneClearMenu, "LaneClearW", "Use W");
            Helper.AddMenuBool(laneClearMenu, "LaneClearE", "Use E");
            Config.Add(laneClearMenu);

            //JungleClear Menu
            var jungleClearMenu = new Menu("JungleClear", "JungleClear");
            Helper.AddMenuBool(jungleClearMenu, "JungleClearQ", "Use Q");
            Helper.AddMenuBool(jungleClearMenu, "JungleClearW", "Use W");
            Helper.AddMenuBool(jungleClearMenu, "JungleClearE", "Use E");
            Config.Add(jungleClearMenu);

            //Misc Menu
            var miscMenu = new Menu("Misc", "Misc");
            Helper.AddMenuBool(miscMenu, "MiscIgnite", "KillSteal with Ignite");
            Helper.AddMenuBool(miscMenu, "MiscKillStealQ", "KillSteal with Q");
            Helper.AddMenuBool(miscMenu, "MiscKillStealW", "KillSteal with W");
            Helper.AddMenuBool(miscMenu, "MiscKillStealE", "KillSteal with E");

            Config.Add(miscMenu);

            //Draw Menu
            var drawMenu = new Menu("Drawings", "Drawings");
            Helper.AddMenuBool(drawMenu, "DrawQ", "Draw Q");
            Helper.AddMenuBool(drawMenu, "DrawW", "Draw W");
            Helper.AddMenuBool(drawMenu, "DrawE", "Draw E");
            Config.Add(drawMenu);



            Config.Attach();
        }
        public static bool MiscIgnite { get { return Helper.GetMenuBoolValue(Config, "Misc", "MiscIgnite"); } }
        public static bool MiscKillStealQ { get { return Helper.GetMenuBoolValue(Config, "Misc", "MiscKillStealQ"); } }
        public static bool MiscKillStealW { get { return Helper.GetMenuBoolValue(Config, "Misc", "MiscKillStealW"); } }
        public static bool MiscKillStealE { get { return Helper.GetMenuBoolValue(Config, "Misc", "MiscKillStealE"); } }



        public static bool LaneClearQ { get { return Helper.GetMenuBoolValue(Config, "LaneClear", "LaneClearQ"); } }
        public static bool LaneClearW { get { return Helper.GetMenuBoolValue(Config, "LaneClear", "LaneClearW"); } }
        public static bool LaneClearE { get { return Helper.GetMenuBoolValue(Config, "LaneClear", "LaneClearE"); } }


        public static bool JungleClearQ { get { return Helper.GetMenuBoolValue(Config, "JungleClear", "JungleClearQ"); } }
        public static bool JungleClearW { get { return Helper.GetMenuBoolValue(Config, "JungleClear", "JungleClearW"); } }
        public static bool JungleClearE { get { return Helper.GetMenuBoolValue(Config, "JungleClear", "JungleClearE"); } }


        public static bool ComboQ { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboQ"); } }
        public static bool ComboW { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboW"); } }
        
        public static bool ComboWHealth { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboWHealth"); } }

        public static bool ComboE { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboE"); } }
        public static bool ComboR { get { return Helper.GetMenuBoolValue(Config, "Combo", "ComboR"); } }
        public static int DontEEnemyRange { get { return Helper.GetMenuSliderValue(Config, "Combo", "DontEEnemyRange"); } }

        public static string ComboEMode { get { return Helper.GetMenuList(Config, "Combo", "ComboEMode"); } }


        public static bool HarassQ { get { return Helper.GetMenuBoolValue(Config, "Harass", "HarassQ"); } }


        public static bool DrawQ { get { return Helper.GetMenuBoolValue(Config, "Drawings", "DrawQ"); } }
        public static bool DrawW { get { return Helper.GetMenuBoolValue(Config, "Drawings", "DrawW"); } }
        public static bool DrawE { get { return Helper.GetMenuBoolValue(Config, "Drawings", "DrawE"); } }

    }
}
