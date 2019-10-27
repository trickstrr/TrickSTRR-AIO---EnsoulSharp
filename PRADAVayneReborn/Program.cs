using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Utility;
using EnsoulSharp.SDK.Utils;
using PRADA_Vayne.MyInitializer;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;

namespace PRADA_Vayne
{
    public class Program
    {
        public static void VayneMain()
        {
            GameEvent.OnGameLoad += GameEvent_OnGameLoad; PRADALoader.Init();
        }

        private static void GameEvent_OnGameLoad()
        {
            throw new System.NotImplementedException();
        }

        #region Fields and Objects
        public static void Orbwalking()
        {
            Orbwalker.OnAction += Orbwalker_OnAction;
        }

        public static void Orbwalker_OnAction(object sender, OrbwalkerActionArgs args)
        {
            throw new System.NotImplementedException();
        }
        #region Menu

        public static Menu MainMenu { get; set; }
        public static Menu ComboMenu { get; set; }
        public static Menu LaneClearMenu { get; set; }
        public static Menu EscapeMenu { get; set; }
        public static Menu DrawingsMenu { get; set; }
        public static Menu OrbwalkerMenu { get; set; }

        #endregion Menu

        #region Spells

        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;

        #endregion Spells

        #endregion Fields and Objects
    }
}