using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Clipper;
using EnsoulSharp.SDK.Utility;
using EnsoulSharp.SDK.Utils;
using PRADA_Vayne.MyUtils;
using System;

namespace PRADA_Vayne.MyLogic.Others
{
    public static partial class Events
    {
        private static AIHeroClient Player;
        public static void OnUpdate(EventArgs args)
        {
            
            if (Heroes.Player.HasBuff("rengarralertsound"))
            {
                if (Items.HasItem((int)ItemId.Oracle_Alteration, Heroes.Player) &&
                    Items.CanUseItem((int)ItemId.Oracle_Alteration))
                    Items.UseItem((int)ItemId.Oracle_Alteration, Heroes.Player.Position);
                else if (Items.HasItem((int)ItemId.Control_Ward, Heroes.Player))
                    Items.UseItem((int)ItemId.Control_Ward, Heroes.Player.Position.Randomize(0, 125));
            }
        }
    }
}