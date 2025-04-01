using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace DestinyMod
{
    public class MapComponent_ExoticsManager : MapComponent
    {
        // list of all exotic weapons (including equipped ones) so we can tick em
        private List<ThingWithExoticBehavior> exotics;

        // ensure initialization runs only once
        private bool isInitialized = false;

        public MapComponent_ExoticsManager(Map map) : base(map)
        {
            exotics = new List<ThingWithExoticBehavior>();
        }

        public void InitializeEquippedExotics()
        {
            // Search the map for all the exotic weapons that are equipped by pawns
            // exotic weapons on the ground already get saved AND ticked
            foreach (Pawn pawn in Find.CurrentMap.mapPawns.AllPawnsSpawned)
            {
                if (pawn.equipment != null && pawn.equipment.Primary is ThingWithExoticBehavior exoticEquipped)
                {
                    exotics.Add(exoticEquipped);
                }
            }

            // print how many exotics are on the map
            Log.Message("Number of exotics on the map: " + exotics.Count);
            foreach (var thing in exotics)
            {
                Log.Message($"Exotic: {thing.def.defName}, ParentHolder: {thing.ParentHolder?.ToString() ?? "null"}");
            }
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // only tick exotics when the map is ready
            if (isInitialized)
            {
                // do the exotic ticks
                foreach (var exotic in exotics)
                {
                    exotic.ExoticTick();
                }
            }
            else
            {
                // wait until map is ready
                if (Find.CurrentMap == null) return;

                // perform a magic trick
                isInitialized = true;
                InitializeEquippedExotics();
            }
        }

        public void RegisterExotic(ThingWithExoticBehavior exotic)
        {
            if (!exotics.Contains(exotic))
            {
                exotics.Add(exotic);
            }
        }

        public void DeregisterExotic(ThingWithExoticBehavior exotic)
        {
            if (exotics.Contains(exotic))
            {
                exotics.Remove(exotic);
            }
        }
    }
}
