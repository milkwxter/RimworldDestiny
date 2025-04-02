using RimWorld;
using System.Linq;
using Verse.AI;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using Verse.Sound;
using VanillaWeaponsExpandedLaser;
using static UnityEngine.UI.Image;

namespace DestinyMod
{
    public class Hediff_DM_RiskrunnerEffect : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();

            if (base.pawn.IsHashIntervalTick(180)) // Check every 180 ticks
            {
                if (!CodeHelpers.PawnHasHediff(pawn, "DS_Arc"))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("DS_RiskrunnerEffect"));
                    pawn.health.RemoveHediff(hediff);
                }
            }
        }
    }

    public class Hediff_DM_RatKingEffect : HediffWithComps
    {
        public override string LabelInBrackets
        {
            get
            {
                int stackCount = pawn.health.hediffSet.hediffs.Count(h => h.def == this.def);
                // show count if its above 1
                return stackCount > 1 ? $"x{stackCount}" : null;
            }
        }
        public override void Tick()
        {
            base.Tick();

            if (base.pawn.IsHashIntervalTick(180)) // Check every 180 ticks
            {
                // save a single var
                HediffDef hediffRatKing = HediffDef.Named("DS_RatKingEffect");

                // if the rat king is unequipped
                if (!(pawn.equipment.Primary is ThingExoticRatKing))
                {
                    // remove the hediff and the damage multiplier
                    pawn.health.hediffSet.hediffs.RemoveAll(hediff => hediff.def == hediffRatKing);
                }
            }
        }
    }
}
