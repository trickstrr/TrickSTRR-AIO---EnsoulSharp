using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrickSTRR.AIO.Dual_Port
{
    class Misc
    {
        public static Menu menu, info;
        public static void Load()
        {

            info = new Menu("TrickSTRR.AIO.Info", "[~] EnsoulAIO - Info", true);
            info.Add(new MenuSeparator("aioBerb", "TrickSTRR.AIO - By TrickSTRR Based on PortAIO"));
            info.Add(new MenuSeparator("aioVersion", "Version : " + Game.BuildVersion));
            info.Add(new MenuSeparator("aioNote", "Note : Make sure you're in Borderless!"));
            info.Attach();

            menu = new Menu("TAIOMisc", "[~] TrickSTRR.AIO - Ports", true);
            var dualPort = new Menu("DualPAIOPort", "Dual-Port");
            dualPort.Add(new MenuSeparator("note", "When using any option, use f5 to reload after your choice."));
            menu.Add(dualPort);
            menu.Attach();

            var hasDualPort = true;
            var champ = new string[] { };
            switch (ObjectManager.Player.CharacterName)
            {
                /*case "ChampName":
                     champ = new[] { "ChampName#" };
                    break;
                case "ChampName":
                    champ = new[] { "ChampName#" };
                    break;
                case "ChampName":
                    champ = new[] { " ChampName# "};
                    break;
                default:
                    hasDualPort = false;
                    dualPort.Add(new MenuBool("info1", "There are no dual-port for this champion."));
                    dualPort.Add(new MenuBool("info2", "Feel free to request one."));
                    break;*/

            }
            if (hasDualPort)
            {
                dualPort.Add(new MenuList(ObjectManager.Player.CharacterName, "Which dual-port?", champ ));
            }


            //menu.Add(new MenuBool("UtilityOnly", "Utility Only?",false));
            //menu.Add(new MenuBool("ChampsOnly", "Champs Only?", false));



        }


    }
}
