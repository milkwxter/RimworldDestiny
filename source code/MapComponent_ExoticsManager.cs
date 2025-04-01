using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DestinyMod
{
    public class MapComponent_ExoticsManager : MapComponent
    {
        private List<ThingWithExoticBehavior> exotics;

        public MapComponent_ExoticsManager(Map map) : base(map)
        {
            exotics = new List<ThingWithExoticBehavior>();
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            // print how many exotics are on the map
            Log.Message("Number of exotics on the map: " + exotics.Count);
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // do the exotic ticks
            foreach (var exotic in exotics)
            {
                exotic.ExoticTick();
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
