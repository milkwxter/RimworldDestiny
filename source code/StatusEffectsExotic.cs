﻿using RimWorld;
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
        public int nearbyRats;
        public override string LabelInBrackets
        {
            get
            {
                return $"x{this.nearbyRats} stacks";
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            // initialize variable
            this.nearbyRats = 0;
        }

        public override void Tick()
        {
            base.Tick();

            if (base.pawn.IsHashIntervalTick(180)) // Check every 180 ticks
            {
                // save a single var
                HediffDef hediffRatKing = HediffDef.Named("DS_RatKingEffect");

                // if the rat king is unequipped
                if (!(pawn.equipment.Primary is ThingExoticRatKing) || nearbyRats == 0)
                {
                    // remove the hediff and reset our counter
                    nearbyRats = 0;
                    pawn.health.hediffSet.hediffs.RemoveAll(hediff => hediff.def == hediffRatKing);
                }
            }
        }
    }
}
