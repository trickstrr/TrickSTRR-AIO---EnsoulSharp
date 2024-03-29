using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Clipper;
using EnsoulSharp.SDK.Utility;
using EnsoulSharp.SDK.Utils;
using SharpDX;
using System.Linq;
using static PRADA_Vayne.Program;

namespace PRADA_Vayne.MyLogic.Q
{
    public static class Tumble
    {
        public static Vector3 TumbleOrderPos = Vector3.Zero;

        public static void Cast(Vector3 position)
        {
           // if (!ComboMenu.Item("QCombo").GetValue<bool>()) return;
            TumbleOrderPos = position;
            if (position != Vector3.Zero) Program.Q.Cast(TumbleOrderPos);
            if (position == Vector3.Zero &&
                ObjectManager.Player.Buffs.Any(b => b.Name.ToLower().Contains("vayneinquisition")))
                Program.Q.Cast(Game.CursorPos);
        }
    }
}