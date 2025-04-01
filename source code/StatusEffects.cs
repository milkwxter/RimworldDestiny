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
    public class Hediff_DM_Arc : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();

            // every 180 ticks (3 seconds) do the main thing
            if (base.pawn.IsHashIntervalTick(180))
            {
                // make sure pawn is still living
                if (base.pawn.Dead)
                {
                    base.pawn.health.RemoveHediff(this);
                    return;
                }

                // do a fleck
                FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Arc_Fleck"), new Vector3(0f, 0f, 0f));

                // create a damage info
                DamageInfo zapped = new DamageInfo(DamageDefOf.Burn, 10f, 1f, -1f, null);
                List<Pawn> pawnsToZap = new List<Pawn>();

                // get nearby pawns of same faction
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(pawn.Position, 5f, true))
                {
                    if (!cell.InBounds(pawn.Map)) continue;
                    List<Thing> things = cell.GetThingList(pawn.Map);
                    foreach (Thing thing in things)
                    {
                        if (thing is Pawn zapee)
                        {
                            // make sure only target only OTHER LIVING FRIENDLIES (to the guy who got zapped)
                            if (zapee.Faction != null && zapee.Faction.HostileTo(pawn.Faction) && zapee != pawn) continue;

                            // add them to the zap list
                            pawnsToZap.Add(zapee);
                        }
                    }
                }

                // zapping time
                int zapperTicksRemaining = 2 * 60;
                foreach (Pawn pawnToZap in pawnsToZap)
                {
                    pawnToZap.TakeDamage(zapped);
                    DefDatabase<SoundDef>.GetNamed("DS_Zap").PlayOneShot(new TargetInfo(pawnToZap.Position, pawnToZap.Map, false));

                    // le epic effectz
                    Vector3 start = pawn.DrawPos;
                    Vector3 end = pawnToZap.DrawPos;
                    Material zapMat = MaterialPool.MatFrom("Things/Projectile/Shot_ArcLightning");
                    if (zapperTicksRemaining > 0)
                    {
                        zapperTicksRemaining--;
                        GenDraw.DrawLineBetween(start, end, zapMat, 5f);
                    }
                    zapperTicksRemaining = 2 * 60;
                }
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            // Throw a mote
            MoteMaker.ThrowText(base.pawn.DrawPos, base.pawn.Map, "Arc proc", Color.cyan, 3f);

            // do a fleck
            FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Arc_Fleck"), new Vector3(0f, 0f, 0f));
        }
    }
}
