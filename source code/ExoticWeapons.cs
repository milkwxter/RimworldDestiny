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
            base.SpawnSetup(map, respawningAfterLoad);

            if (map.GetComponent<MapComponent_ExoticsManager>() is MapComponent_ExoticsManager manager)
            {
                Log.Message("Spawned EXOTIC -->  " + this.def.defName);
                manager.RegisterExotic(this);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            if (this.Map == null)
            {
                if (this.Map.GetComponent<MapComponent_ExoticsManager>() is MapComponent_ExoticsManager manager)
                {
                    Log.Message("Despawned EXOTIC -->  " + this.def.defName);
                    manager.DeregisterExotic(this);
                }
            }
            base.DeSpawn(mode);
        }
    }

    // RISKRUNNER!!!!!!!!!
    public class ThingExoticRiskrunner : ThingWithExoticBehavior
    {
        public override void ExoticTick()
        {
            Log.Message("EXOTIC TICK INCOMING!!!");

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