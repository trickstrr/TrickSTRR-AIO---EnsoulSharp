using EnsoulSharp;
using EnsoulSharp.SDK;
using PRADA_Vayne.MyLogic.Q;
using PRADA_Vayne.MyUtils;
using PRADA_Vayne.Utils;

namespace PRADA_Vayne.MyLogic.Others
{
    public static partial class Events
    {
        public static void OnProcessSpellcast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            if (args.Target == null) return;

            #region ward brush after condemn

            if (sender.IsMe && args.SData.Name.ToLower().Contains("condemn") && args.Target.IsValid<AIHeroClient>())
            {
                var target = (AIHeroClient)args.Target;
                if (Program.ComboMenu.Item("EQ").GetValue<bool>() && target.IsVisible &&
                    !target.HasBuffOfType(BuffType.Stun) && Program.Q.IsReady()) //#TODO: fix
                {
                    var tumblePos = target.GetTumblePos();
                    Tumble.Cast(tumblePos);
                }

                if (NavMesh.IsWallOfGrass(args.End, 100))
                {
                    var blueTrinket = ItemId.Farsight_Alteration;
                    if (Items.HasItem((int)ItemId.Farsight_Alteration, Heroes.Player) &&
                        Items.CanUseItem((int)ItemId.Farsight_Alteration))
                        blueTrinket = ItemId.Farsight_Alteration;

                    var yellowTrinket = ItemId.Warding_Totem;
                    if (Items.HasItem((int)ItemId.Greater_Stealth_Totem_Trinket, Heroes.Player))
                        yellowTrinket = ItemId.Greater_Stealth_Totem_Trinket;

                    if (Items.CanUseItem((int)blueTrinket))
                        Items.UseItem((int)blueTrinket, args.End.Randomize(0, 100));
                    if (Items.CanUseItem((int)yellowTrinket))
                        Items.UseItem((int)yellowTrinket, args.End.Randomize(0, 100));
                }
            }

            #endregion ward brush after condemn

            #region Anti-Stealth

            if (args.SData.Name.ToLower().Contains("talonshadow")) //#TODO get the actual buff name
            {
                if (Items.HasItem((int)ItemId.Oracle_Alteration) &&
                    Items.CanUseItem((int)ItemId.Oracle_Alteration))
                    Items.UseItem((int)ItemId.Oracle_Alteration, Heroes.Player.Position);
                else if (Items.HasItem((int)ItemId.Control_Ward, Heroes.Player))
                    Items.UseItem((int)ItemId.Control_Ward, Heroes.Player.Position.Randomize(0, 125));
            }

            #endregion Anti-Stealth

            if (MyWizard.ShouldSaveCondemn()) return;
            if (sender.Distance(Heroes.Player) > 1500 || !args.Target.IsMe || args.SData == null)
                return;
            //how to milk alistar/thresh/everytoplaner
            var spellData = SpellDb.GetByName(args.SData.Name);
            if (spellData != null && !Heroes.Player.UnderTurret(true) &&
                !Lists.UselessChamps.Contains(sender.CharacterName))
                if (spellData.CcType == CcType.Knockup || spellData.CcType == CcType.Stun ||
                    spellData.CcType == CcType.Knockback || spellData.CcType == CcType.Suppression)
                    Program.E.Cast(sender);
        }
    }
}