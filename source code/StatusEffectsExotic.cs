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
}
