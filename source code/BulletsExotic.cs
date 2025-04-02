using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using VanillaWeaponsExpandedLaser;
using Verse;
using Verse.Sound;

namespace DestinyMod
{
    public class LaserBeamRiskrunner : LaserBeamDestiny
    {
        private new LaserBeamDestinyDef def => base.def as LaserBeamDestinyDef;
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            // make sure launcher pawn has the hediff
            if (CodeHelpers.PawnHasHediff(this.launcher as Pawn, "DS_RiskrunnerEffect"))
            {
                // save the launcher for ease of use
                Pawn pawn = this.launcher as Pawn;

                // if we hit something that is not a pawn
                if (!(hitThing is Pawn))
                {
                    // Do the actual impact
                    base.Impact(hitThing, blockedByShield);

                    // return early
                    return;
                }

                // create a damage info
                DamageInfo zapped = new DamageInfo(DamageDefOf.Burn, 7f, 1f, -1f, null);
                List<Pawn> pawnsToZap = new List<Pawn>();

                // get nearby pawns of same faction
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(hitThing.Position, 5f, true))
                {
                    if (!cell.InBounds(hitThing.Map)) continue;
                    List<Thing> things = cell.GetThingList(hitThing.Map);
                    foreach (Thing thing in things)
                    {
                        if (thing is Pawn zapee)
                        {
                            // make sure only target only OTHER LIVING FRIENDLIES (to the guy who got zapped)
                            if (zapee.Faction != null && zapee.Faction.HostileTo(hitThing.Faction) && zapee != hitThing) continue;

                            // null map checker... apparently i need this
                            if (zapee.Map == null) continue;

                            // add them to the zap list
                            pawnsToZap.Add(zapee);
                        }
                    }
                }

                // zapping time
                foreach (Pawn pawnToZap in pawnsToZap)
                {
                    pawnToZap.TakeDamage(zapped);
                    DefDatabase<SoundDef>.GetNamed("DS_Zap").PlayOneShot(new TargetInfo(pawnToZap.Position, pawnToZap.Map, false));

                    // le epic effectz
                    Vector3 start = pawn.DrawPos;
                    Vector3 end = pawnToZap.DrawPos;
                    Material zapMat = MaterialPool.MatFrom("Things/Projectile/Shot_ArcLightning");
                    GenDraw.DrawLineBetween(start, end, zapMat, 5f);
                }
            }

            // Do the actual impact
            base.Impact(hitThing, blockedByShield);
        }
    }

    public class BulletRatKing : ProjectileDestiny
    {
        private new ProjectileDestinyDef def => base.def as ProjectileDestinyDef;
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            // make sure launcher pawn has the hediff
            if (CodeHelpers.PawnHasHediff(this.launcher as Pawn, "DS_RatKingEffect"))
            {
                // important variables
                Pawn ratKingOwner = this.launcher as Pawn;
                HediffDef hediffDefRatKing = HediffDef.Named("DS_RatKingEffect");
                Hediff hediffRatKing = ratKingOwner.health.hediffSet.GetFirstHediffOfDef(hediffDefRatKing);

                // get size of rat pack
                int ratPackSize = (hediffRatKing as Hediff_DM_RatKingEffect).nearbyRats;

                // increase the damage
                this.weaponDamageMultiplier += (0.1f * ratPackSize);
            }

            // default behavior
            base.Impact(hitThing, blockedByShield);
        }
    }
}