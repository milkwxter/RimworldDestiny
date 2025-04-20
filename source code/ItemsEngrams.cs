using AnimalBehaviours;
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DestinyMod
{
    public class CompUseEffect_DecryptEngram : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);

            // decryption logic
            if (user.skills.GetSkill(SkillDefOf.Intellectual).Level >= 5)
            {
                // save position
                IntVec3 position = this.parent.Position;

                // delete engram
                this.parent.Destroy(DestroyMode.Vanish);

                // get random weapon from all exotic weapons
                var exoticWeaponDefs = DefDatabase<ThingDef>.AllDefs.Where(def => def.weaponTags != null && def.weaponTags.Contains("DestinyExotic")).ToList();
                ThingDef randomExoticWeapon = exoticWeaponDefs.RandomElement();
                Thing resultingWeapon = ThingMaker.MakeThing(randomExoticWeapon);
                GenPlace.TryPlaceThing(resultingWeapon, position, user.Map, ThingPlaceMode.Near);

                // message user
                Messages.Message("The engram has been successfully decrypted into a " + resultingWeapon.Label + "!", MessageTypeDefOf.PositiveEvent);

                // play fun sound
                DefDatabase<SoundDef>.GetNamed("DS_Decrypt").PlayOneShot(new TargetInfo(resultingWeapon.Position, resultingWeapon.Map, false));

                // Throw a mote
                MoteMaker.ThrowText(resultingWeapon.DrawPos, resultingWeapon.Map, resultingWeapon.Label + " get!", Color.yellow, 3f);

                // do a fleck
                FleckMaker.AttachedOverlay(resultingWeapon, DefDatabase<FleckDef>.GetNamed("DS_ExoticDecrypt_Fleck"), new Vector3(0f, 0f, 0f));
            }
            else
            {
                Messages.Message("The pawn's intellectual skill is too low to decrypt an engram.", MessageTypeDefOf.RejectInput);
            }
        }
    }

}