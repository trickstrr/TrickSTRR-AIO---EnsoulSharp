using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Clipper;
using EnsoulSharp.SDK.Utility;
using EnsoulSharp.SDK.Utils;
using PRADA_Vayne.MyLogic.Q;
using PRADA_Vayne.MyUtils;

namespace PRADA_Vayne.MyLogic.R
{
    public static partial class Events
    {
        public static void OnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            /*if (spellbook.Owner.IsMe)
               // if (args.Slot == SpellSlot.R && Program.ComboMenu.Item("QR").GetValue<bool>())
                {
                    var target = TargetSelector.GetTarget(300, DamageType.Physical);
                    var tumblePos = target != null ? target.GetTumblePos() : Game.CursorPos;
                    Tumble.Cast(tumblePos);
                }*/
        }
    }
}