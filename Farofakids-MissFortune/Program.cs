using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using Color = System.Drawing.Color;

namespace Farofakids_MissFortune
{
    internal class Program
    {
        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.BaseSkinName != "MissFortune") return;
            SPELLS.Initialize();
            MENUS.Initialize();
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += MODES.Gapcloser_OnGapcloser;
            Obj_AI_Base.OnProcessSpellCast += MODES.Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += MODES.Obj_AI_Base_OnSpellCas;
            Orbwalker.OnPostAttack += MODES.Orbwalker_OnPostAttack;
            Game.OnUpdate += MODES.Game_OnUpdate;
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            if (MENUS.QRange && SPELLS.Q.Handle.IsLearned)
                Drawing.DrawCircle(Player.Instance.Position, SPELLS.Q.Range, Color.Red);
            if (MENUS.WRange && SPELLS.W.Handle.IsLearned)
                Drawing.DrawCircle(Player.Instance.Position, SPELLS.W.Range, Color.Red);
            if (MENUS.ERange && SPELLS.E.Handle.IsLearned)
                Drawing.DrawCircle(Player.Instance.Position, SPELLS.E.Range, Color.Red);
            if (MENUS.RRange && SPELLS.R.Handle.IsLearned)
                Drawing.DrawCircle(Player.Instance.Position, SPELLS.R.Range, Color.Red);
        }

    }
}