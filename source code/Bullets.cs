using RimWorld;
using VanillaWeaponsExpandedLaser;
using Verse;

namespace DestinyMod
{
    public class ProjectileDestinyDef : ThingDef
    {
        public HediffDef hediffToAdd;
        public float hediffChance;
    }

    public class LaserBeamDestinyDef : LaserBeamDef
    {
        public HediffDef hediffToAdd;
        public float hediffChance;
    }

    public class ProjectileDestiny : Bullet
    {
        private new ProjectileDestinyDef def => base.def as ProjectileDestinyDef;
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            // Logic for status effects
            if (Rand.Chance(def.hediffChance))
            {
                if (hitThing is Pawn pawn)
                {
                    HediffDef hediffDef = def.hediffToAdd;
                    Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
                    pawn.health.AddHediff(hediff);
                    //Log.Message("Added hediff '" + def.hediffToAdd.defName + "' to pawn named " + pawn.Name);
                }
            }

            // Do the actual impact
            base.Impact(hitThing, blockedByShield);
        }
    }

    public class LaserBeamDestiny : LaserBeam
    {
        private new LaserBeamDestinyDef def => base.def as LaserBeamDestinyDef;
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            // Logic for status effects
            if (Rand.Chance(def.hediffChance))
            {
                if (hitThing is Pawn pawn)
                {
                    HediffDef hediffDef = def.hediffToAdd;
                    Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
                    pawn.health.AddHediff(hediff);
                    //Log.Message("Added hediff '" + def.hediffToAdd.defName + "' to pawn named " + pawn.Name);
                }
            }

            // Do the actual impact
            base.Impact(hitThing, blockedByShield);
        }
    }
}