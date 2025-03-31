using RimWorld;
using VanillaWeaponsExpandedLaser;
using Verse;

namespace DestinyMod
{
    public static class CodeHelpers
    {
        public static bool PawnHasHediff(Pawn pawn, string hediffName)
        {
            return pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named(hediffName)) != null;
        }
    }
}