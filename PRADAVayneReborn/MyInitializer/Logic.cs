using EnsoulSharp;
using EnsoulSharp.SDK;
using PRADA_Vayne.MyLogic.E;
using PRADA_Vayne.MyLogic.Others;
using Events = PRADA_Vayne.MyLogic.Q.Events;

namespace PRADA_Vayne.MyInitializer
{
    public static partial class PRADALoader
    {
        public static void LoadLogic()
        {
            #region Q

            Orbwalker.OrbwalkerType.AfterAttack += Events.AfterAttack;
            Orbwalker.OrbwalkerType.BeforeAttack += Events.BeforeAttack;
            Orbwalker.OrbwalkerType.OnAttack += Events.OnAttack;
            Spellbook.OnCastSpell += Events.OnCastSpell;
            Gapcloser.OnGapcloser += Events.OnGapcloser;
            AIBaseClient.OnProcessSpellCast += Events.OnProcessSpellCast;
            Game.OnUpdate += Events.OnUpdate;

            #endregion Q

            #region E

            GameObject.OnCreate += AntiAssasins.OnCreateGameObject;
            AIBaseClient.OnProcessSpellCast += MyLogic.E.Events.OnProcessSpellCast;
            Game.OnUpdate += MyLogic.E.Events.OnUpdate;
            Interrupter.OnInterrupterSpell += MyLogic.E.Events.OnPossibleToInterrupt;
            Game.OnUpdate += MyLogic.E.Events.JungleUsage;

            #endregion E

            #region R

            Spellbook.OnCastSpell += MyLogic.R.Events.OnCastSpell;

            #endregion R

            #region Others

            Game.OnUpdate += MyLogic.Others.Events.OnUpdate;
            AIBaseClient.OnProcessSpellCast += MyLogic.Others.Events.OnProcessSpellcast;
            Drawing.OnDraw += MyLogic.Others.Events.OnDraw;

            #endregion Others
        }
    }
}