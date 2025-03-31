using RimWorld;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using VanillaWeaponsExpandedLaser;
using Verse;

namespace DestinyMod
{
    public class StatWorker_HediffAdder : StatWorker
    {
        public bool ValidReq(StatRequest req)
        {
            if(req.Thing != null && req.Thing.def != null)
            {
                return true;
            }
            return false;
        }

        public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
        {
            if (ValidReq(req))
            {
                // get the projectile
                ThingDef projectile = req.Thing.def.Verbs[0].defaultProjectile;

                if (projectile is ProjectileDestinyDef projectileDestinyDef)
                {
                    return $"Inflicts {projectileDestinyDef.hediffToAdd.label} with a chance of {projectileDestinyDef.hediffChance * 100}% on hit.";
                }
                else if (projectile is LaserBeamDestinyDef laserBeamDestinyDef)
                {
                    return $"Inflicts {laserBeamDestinyDef.hediffToAdd.label} with a chance of {laserBeamDestinyDef.hediffChance * 100}% on hit.";
                }
            }
            return base.GetExplanationFinalizePart(req, numberSense, finalVal);
        }

        public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
        {
            // get the projectile
            ThingDef projectile = optionalReq.Thing.def.Verbs[0].defaultProjectile;

            if (projectile is ProjectileDestinyDef projectileDestinyDef)
            {
                return projectileDestinyDef.hediffToAdd.label;
            }
            else if (projectile is LaserBeamDestinyDef laserBeamDestinyDef)
            {
                return laserBeamDestinyDef.hediffToAdd.label;
            }
            else
            {
                return "None";
            }
        }

        public override bool ShouldShowFor(StatRequest req)
        {
            if (ValidReq(req))
            {
                if (req.Thing.def.Verbs[0].defaultProjectile is ProjectileDestinyDef || req.Thing.def.Verbs[0].defaultProjectile is LaserBeamDestinyDef)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}