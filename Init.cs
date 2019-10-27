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

               /* case "Vayne":
                    PRADA_Vayne.Program.VayneMain();
                    break;*/
                case "Kalista":
                    TrickSTRR.AIO.Kalista.Program.Main();
                    Game.Print("Based on Official Kalista");
                    break;
                case "Ezreal":
                    TrickSTRR.AIO.Ezreal.Program.Main();
                    Game.Print("Based on Ezrealist - Thanks to ProDragon!");
                    break;
                case "Kassadin":
                    TrickSTRR.AIO.Kassadin.Program.Main();
                    Game.Print("Based on xDreamms Kassadin - Thanks to ProDragon!");
                    break;
                case "Sylas":
                    TrickSTRR.AIO.Sylas.Program.Main();
                    Game.Print("Based on xDreamms Sylas - Thanks to ProDragon!");
                    break;
                case "Varus":
                    TrickSTRR.AIO.Varus.Program.Main();
                    Game.Print("Based on xDreamms Varus - Thanks to ProDragon!");
                    break;
                    /* case "LeeSin":
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
  