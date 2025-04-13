using RimWorld;
using System.Linq;
using Verse.AI;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using Verse.Sound;
using VanillaWeaponsExpandedLaser;
using static UnityEngine.UI.Image;
using UnityEngine.Networking.Types;

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

    public class Hediff_DM_Kinetic : HediffWithComps
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
                FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Kinetic_Fleck"), new Vector3(0f, 0f, 0f));
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            // Throw a mote
            MoteMaker.ThrowText(base.pawn.DrawPos, base.pawn.Map, "Kinetic proc", Color.white, 3f);

            // do a fleck
            FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Kinetic_Fleck"), new Vector3(0f, 0f, 0f));
        }
    }

    public class Hediff_DM_Solar : HediffWithComps
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
                FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Solar_Fleck"), new Vector3(0f, 0f, 0f));

                // pawn fires a explosive fireball at nearest pawn
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(base.pawn.Position, 5f, true))
                {
                    if (!cell.InBounds(base.pawn.Map)) continue;
                    List<Thing> things = cell.GetThingList(base.pawn.Map);
                    foreach (Thing thing in things)
                    {
                        if (thing is Pawn fireballee)
                        {
                            // super sick early return
                            if (fireballee.Faction == null || fireballee.Faction != base.pawn.Faction || fireballee == base.pawn)
                            {
                                continue;
                            }

                            // shoot a fireball from pawn to fireballee
                            ThingDef fireballDef = ThingDef.Named("Destiny_Projectile_Fireball");
                            Projectile projectile = (Projectile)GenSpawn.Spawn(fireballDef, base.pawn.Position, base.pawn.Map);
                            projectile.Launch(base.pawn, fireballee.Position, fireballee, ProjectileHitFlags.IntendedTarget);

                            // special sound
                            DefDatabase<SoundDef>.GetNamed("DS_SolarThrow").PlayOneShot(new TargetInfo(base.pawn.Position, base.pawn.Map, false));

                            // stop after throwing a fireball ONCE cus it hurts a lot
                            return;
                        }
                    }
                }
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            // Throw a mote (ORANGE DOESNT EXIST HAHAHAHAA)
            MoteMaker.ThrowText(base.pawn.DrawPos, base.pawn.Map, "Solar proc", new Color(1f, 0.5f, 0f), 3f);

            // do a fleck
            FleckMaker.AttachedOverlay(base.pawn, DefDatabase<FleckDef>.GetNamed("DS_Solar_Fleck"), new Vector3(0f, 0f, 0f));

            // pawn lights on fire
            IntVec3 cell = base.pawn.Position;
            FireUtility.TryStartFireIn(cell, base.pawn.Map, 5f, base.pawn);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();

            // check if the pawn and its map are valid
            if (base.pawn == null || base.pawn.Map == null || base.pawn.Dead)
            {
                return;
            }

            // create variables for damage
            DamageInfo burnt = new DamageInfo(DamageDefOf.Burn, 25f, 1f, -1f, base.pawn);
            List<Pawn> pawnsToBurn = new List<Pawn>();

            // do some effects
            FleckMaker.Static(base.pawn.Position, base.pawn.Map, FleckDefOf.PsycastAreaEffect);
            FleckMaker.Static(base.pawn.Position, base.pawn.Map, DefDatabase<FleckDef>.GetNamed("DS_SolarBoom_Fleck"));
            DefDatabase<SoundDef>.GetNamed("DS_SolarBoom").PlayOneShot(new TargetInfo(base.pawn.Position, base.pawn.Map, false));
            Find.CameraDriver.shaker.DoShake(10f);

            // save position in case pawn dies from the explosion
            IEnumerable<IntVec3> cachedRadialCells = GenRadial.RadialCellsAround(base.pawn.Position, 3f, true);

            // epic loop
            foreach (IntVec3 cell in cachedRadialCells)
            {
                // check if the cell is within bounds
                if (!cell.InBounds(base.pawn.Map)) continue;

                // cool effect
                FleckMaker.Static(cell, base.pawn.Map, DefDatabase<FleckDef>.GetNamed("DS_SolarFlame_Fleck"));

                // try to spawn a fire
                if (Rand.Chance(0.5f))
                {
                    FireUtility.TryStartFireIn(cell, base.pawn.Map, 5f, base.pawn);
                }

                // epic loop for things in the cell
                Thing[] array = cell.GetThingList(base.pawn.Map).ToArray();
                foreach (Thing thing in array)
                {
                    // perform action only for pawns
                    if (thing is Pawn pawn)
                    {
                        // make sure we only target pawns of the same faction
                        if (pawn.Faction != null && pawn.Faction.HostileTo(base.pawn.Faction)) continue;

                        // add them to the zap list
                        pawnsToBurn.Add(pawn);
                    }
                }
            }

            // burning time
            foreach (Pawn pawnToBurn in pawnsToBurn)
            {
                pawnToBurn.TakeDamage(burnt);
            }
        }
    }
}
