using System;
using System.Collections.Generic;
using System.Linq;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI.Values;
using TrickSTRR.AIO.Dual_Port;


namespace TrickSTRR.AIO
{
        public static class Init
        {
            public static bool loaded = false;
            public static int moduleNum = 1;

            public static void Initialize()
            {
                Console.WriteLine("[TrickSTRR.AIO] Core loading : Module " + moduleNum + " - Common Loaded");
                moduleNum++;

                Misc.Load();
                Console.WriteLine("[TrickSTRR.AIO] Core loading : Module " + moduleNum + " - Misc Loaded");
                moduleNum++;
                LoadChampion();
                Console.WriteLine("[TrickSTRR.AIO] Core loading : Module " + moduleNum + " - Champion Script Loaded");
                moduleNum++;
                Game.OnUpdate += Game_OnUpdate;
                Console.WriteLine("TrickSTRR.AIO] Core loading : Module " + moduleNum + " - Champion Load Detected, Disabling EB Orbwalker");
                moduleNum++;

              
            }

            private static void LoadChampion()
            {
            switch (ObjectManager.Player.CharacterName)
            {

                case "Vayne":
                    PRADA_Vayne.Program.VayneMain();
                    break;
                /*case "...":
                    Darius.main.Game_OnGameLoad();
                    break;
                case "LeeSin":
                    switch (Misc.menu["DualPAIOPort"][ObjectManager.Player.CharacterName].GetValue<MenuList>().Index)
                    {
                    case 0:
                    LeeSin.Program.Game_OnGameLoad();
                    break;
                        case 1:
                    Console.WriteLine("Hi Lee");
                                break;
                    }
                  break;*/
                }
            }

                private static void Game_OnUpdate(EventArgs args)
            {
                Orbwalker.AttackState = true;
                Orbwalker.MovementState = true;
            }


        }
}
  