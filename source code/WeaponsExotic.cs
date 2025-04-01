using RimWorld;
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
}