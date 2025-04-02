using RimWorld;
using System.Collections.Generic;
using VanillaWeaponsExpandedLaser;
using Verse;

namespace DestinyMod
{
    // BASE CLASS!!!!!!!!!!!!
    public abstract class ThingWithExoticBehavior : ThingWithComps
    {
        public abstract void ExoticTick();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            // do the base spawn setup
            base.SpawnSetup(map, respawningAfterLoad);

            // do my custom spawn setup
            if (map.GetComponent<MapComponent_ExoticsManager>() is MapComponent_ExoticsManager manager)
            {
                Log.Message("Spawned EXOTIC -->  " + this.def.defName);
                manager.RegisterExotic(this);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            // make sure to NOT include weapons that are equipped on pawns
            if (this.Map == null)
            {
                // do my custom despawn
                if (this.Map.GetComponent<MapComponent_ExoticsManager>() is MapComponent_ExoticsManager manager)
                {
                    Log.Message("Despawned EXOTIC -->  " + this.def.defName);
                    manager.DeregisterExotic(this);
                }
            }

            // do the base despawn
            base.DeSpawn(mode);
        }
    }

    // RISKRUNNER!!!!!!!!!
    public class ThingExoticRiskrunner : ThingWithExoticBehavior
    {
        public override void ExoticTick()
        {
            if (this.ParentHolder is Pawn_EquipmentTracker equipmentTracker)
            {
                Pawn pawn = equipmentTracker.pawn;

                // if pawn has the arc proc hediff and doesnt have the supercharged effect
                if (CodeHelpers.PawnHasHediff(pawn, "DS_Arc") && !CodeHelpers.PawnHasHediff(pawn, "DS_RiskrunnerEffect"))
                {
                    Hediff hediff = HediffMaker.MakeHediff(HediffDef.Named("DS_RiskrunnerEffect"), pawn);
                    pawn.health.AddHediff(hediff);
                }
                // if the pawn is no longer arc proc'd and still has the hediff
                else if (!CodeHelpers.PawnHasHediff(pawn, "DS_Arc") && CodeHelpers.PawnHasHediff(pawn, "DS_RiskrunnerEffect"))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("DS_RiskrunnerEffect"));
                    pawn.health.RemoveHediff(hediff);
                }
            }
        }
    }

    // RAT KING!!!!!!!!!!!!
    public class ThingExoticRatKing : ThingWithExoticBehavior
    {
        public override void ExoticTick()
        {
            if (this.ParentHolder is Pawn_EquipmentTracker equipmentTracker)
            {
                // my variables
                Pawn pawn = equipmentTracker.pawn;
                HediffDef hediffRatKing = HediffDef.Named("DS_RatKingEffect");
                int nearbyRatKingUsers = 0;

                // get nearby pawns that are also holding rat king
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(pawn.Position, 5f, true))
                {
                    if (!cell.InBounds(pawn.Map)) continue;
                    List<Thing> things = cell.GetThingList(pawn.Map);
                    foreach (Thing thing in things)
                    {
                        if (thing is Pawn potentialRat && potentialRat.def.race.Humanlike)
                        {
                            // skip guys who arent holding the rat weapon
                            if (!(potentialRat.equipment.Primary is ThingExoticRatKing)) continue;

                            // skip our selves
                            if (potentialRat == pawn) continue;


                            // increment the numbar
                            Log.Message("Potential rat acquired.");
                            nearbyRatKingUsers++;
                        }
                    }
                }

                // clean up the old hediff
                Hediff hediffPrevious = pawn.health.hediffSet.GetFirstHediffOfDef(hediffRatKing);
                pawn.health.hediffSet.hediffs.Remove(hediffPrevious);

                // depending on how many rats are there are do stuff
                Log.Message("We found this many rats! --> " + nearbyRatKingUsers);
                if (nearbyRatKingUsers <= 0) return;

                // do the new hediff
                Hediff hediff = HediffMaker.MakeHediff(hediffRatKing, pawn);
                Hediff_DM_RatKingEffect ratHediff = hediff as Hediff_DM_RatKingEffect;
                pawn.health.AddHediff(ratHediff);
                ratHediff.nearbyRats = nearbyRatKingUsers;
            }
        }
    }
}