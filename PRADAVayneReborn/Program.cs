using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.Utility;
using EnsoulSharp.SDK.Utils;
using PRADA_Vayne.MyInitializer;

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
        //public static Orbwalking.Orbwalker Orbwalker;
        #region Menu

        public static Menu MainMenu;
        public static Menu ComboMenu;
        public static Menu LaneClearMenu;
        public static Menu EscapeMenu;
        public static Menu DrawingsMenu;
        public static Menu OrbwalkerMenu;

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